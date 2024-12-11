using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace CWJ.YU.Mobility
{
	[ExecuteAlways]
	public class AnimationSyncUpdater : MonoBehaviour
	{
		public int maxUpdatesPerFrame = 50; // 한 프레임당 최대 업데이트 수
		private int currentIndex = 0; // 업데이트 시작 인덱스
		[SerializeField] private GameObject[] editor_addObjects;

		[FormerlySerializedAs("renderSyncList")]
		public List<RendererEnabledSync> transformSyncList;

#if UNITY_EDITOR
		[InvokeButton(isEmphasizeBtn: true, displayName: "Get All Child Objects")]
		void GetAllChildObjs()
		{
			this.enabled = false;
			HashSet<GameObject> goHashSet = new HashSet<GameObject>();
			transformSyncList = new List<RendererEnabledSync>();

			void InitChildObj(GameObject go)
			{
				if (!goHashSet.Add(go)) return;
				RendererEnabledSync.TryAddOrGetComp(go, this, out var rendererSyncCache);
				if (rendererSyncCache.Danger_UseTrfSync || rendererSyncCache.IsRegisteredTrfSync)
				{
					rendererSyncCache.RegisterToUpdater();
				}
				else
				{
					rendererSyncCache.isLastUseTrfSync = false;
				}
			}

			foreach (Transform childTrf in transform)
			{
				InitChildObj(childTrf.gameObject);
				foreach (var childChildR in childTrf.GetComponentsInChildren_New<Renderer>(true, true))
				{
					InitChildObj(childChildR.gameObject);
				}
			}

			this.enabled = true;
			UnityEditor.EditorUtility.SetDirty(this);
		}

		private void OnValidate()
		{
			if (editor_addObjects != null && editor_addObjects.Length > 0)
			{
				this.enabled = false;

				transformSyncList ??= new List<RendererEnabledSync>();
				var goHashSet = new HashSet<GameObject>(transformSyncList.Where(g => g).Select(c => c.gameObject));

				foreach (var item in editor_addObjects)
				{
					if (item && goHashSet.Add(item))
					{
						RendererEnabledSync.TryAddOrGetComp(item, this, out var animSyncCache);
						if (animSyncCache.Danger_UseTrfSync || animSyncCache.IsRegisteredTrfSync)
							animSyncCache.RegisterToUpdater();
						else
							Debug.LogError("Danger_UseTrfSync 부터 켜고 추가시도하길", animSyncCache);
					}
				}

				editor_addObjects = Array.Empty<GameObject>();
				this.enabled = true;
				UnityEditor.EditorUtility.SetDirty(this);
			}
		}
#endif

		public void OnRegister(RendererEnabledSync sync)
		{
			if (transformSyncList == null)
				transformSyncList = new List<RendererEnabledSync>();
			if (!transformSyncList.Contains(sync))
			{
				transformSyncList.Add(sync);
#if UNITY_EDITOR
				if (!Application.isPlaying)
					UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		private void Update()
		{
			int updatesThisFrame = 0;

			int cnt = transformSyncList.CountSafe();

			if (cnt == 0)
				return;

			// currentIndex가 리스트 범위를 벗어나지 않도록 보정
			if (currentIndex >= cnt)
				currentIndex = 0;

			int processedItems = 0;
			int i = currentIndex;

			while (updatesThisFrame < maxUpdatesPerFrame && processedItems < cnt)
			{
				var sync = transformSyncList[i];

				if (sync)
				{
					if (sync.Danger_UseTrfSync)
					{
						if (sync.isUpdateWhenRendererDisable || sync.enabled)
						{
							sync.OnUpdateTrf();
							updatesThisFrame++;
						}
					}
					else
					{
						if (sync.isLastUseTrfSync)
						{
							sync.isLastUseTrfSync = false;
							sync.OnUpdateTrf();
						}
					}
				}
				else
				{
					transformSyncList.RemoveAt(i);
					cnt--;
					if (cnt == 0)
						break;
					if (i >= cnt)
						i = 0;
					continue;
				}

				i = (i + 1) % cnt;
				processedItems++;
			}

			// 다음 프레임에서 시작할 인덱스 설정
			currentIndex = i;
		}
	}
}
