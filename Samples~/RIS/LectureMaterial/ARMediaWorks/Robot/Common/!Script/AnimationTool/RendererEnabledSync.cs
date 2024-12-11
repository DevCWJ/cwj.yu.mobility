using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
	[ExecuteAlways]
	public class RendererEnabledSync : MonoBehaviour
	{
		[ShowConditional(nameof(Danger_UseTrfSync))]
		public Vector3 localPosition;

		[ShowConditional(nameof(Danger_UseTrfSync))]
		public Vector3 localRotation;

		[ShowConditional(nameof(useWorldRotationZ))]
		public float worldRotationZ;

		[ShowConditional(nameof(Danger_UseTrfSync))]
		public Vector3 localScale;

		public AnimationSyncUpdater syncUpdater;

		[FoldoutGroup("Transform Settings", true)]
		[FormerlySerializedAs("useTrf")]
		public bool Danger_UseTrfSync = false;


		public bool useWorldRotationZ = false;

		[FormerlySerializedAs("isInitTrf")] [SerializeField, Readonly]
		bool isInitTrfLocation;


		[SerializeField, HideInInspector] public bool isLastUseTrfSync;

		[Tooltip("렌더러 꺼져있어도 위치 업데이트 시킬건지")]
		[FoldoutGroup("Transform Settings", false)]
		public bool isUpdateWhenRendererDisable = true;

		public bool IsRegisteredTrfSync
		{
			get
			{
				if (!syncUpdater)
				{
					syncUpdater = GetComponentInParent<AnimationSyncUpdater>(true);
				}

				if (!syncUpdater)
				{
					Danger_UseTrfSync = false;
					Debug.LogError(gameObject.name + "의 부모에 " + nameof(AnimationSyncUpdater) + "가 없습니다", this);
				}


				return !syncUpdater || (syncUpdater.transformSyncList.Count > 0 && syncUpdater.transformSyncList.Contains(this));
			}
		}

		[FoldoutGroup("Renderer Settings", true)]
		[SerializeField, Readonly] private bool isInitRenderer;

		[SerializeField, Readonly] private bool hasMyRenderer;


		[FoldoutGroup("Renderer Settings", false)]
		[SerializeField] private Renderer myRenderer;

#if UNITY_EDITOR
		private void SetDirtySafe(UnityEngine.Object target)
		{
			if (!target || Application.isPlaying)
			{
				return;
			}

			UnityEditor.EditorUtility.SetDirty(target);
		}

		private void Reset()
		{
			InitRenderersCache();
			SetDirtySafe(this);
		}

		private void OnValidate()
		{
			if (Danger_UseTrfSync)
			{
				if (!useWorldRotationZ && worldRotationZ != 0)
				{
					worldRotationZ = 0;
				}

				if (!IsRegisteredTrfSync)
				{
					if (Danger_UseTrfSync)
					{
						InitLocationCache();
						RegisterToUpdater();
					}

					UnityEditor.EditorUtility.SetDirty(this);
				}

				if (!isInitTrfLocation)
				{
					InitLocationCache();
				}
			}
		}

		// bool IsAnimationInRecordMode()
		// {
		// 	bool isRecord = false;
		// 	try
		// 	{
		// 		var animationWindow = UnityEditor.EditorWindow.GetWindow(typeof(UnityEditor.AnimationWindow)) as UnityEditor.AnimationWindow;
		//
		// 		isRecord = animationWindow && animationWindow.canRecord && animationWindow.recording;
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		Debug.LogError(e, this);
		// 		isRecord = false;
		// 	}
		//
		// 	return isRecord;
		// }
		//
		// bool IsAnimationInPreviewMode()
		// {
		// 	bool isPreview = false;
		// 	try
		// 	{
		// 		var animationWindow = UnityEditor.EditorWindow.GetWindow(typeof(UnityEditor.AnimationWindow)) as UnityEditor.AnimationWindow;
		//
		// 		isPreview = animationWindow && animationWindow.canPreview && animationWindow.previewing;
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		Debug.LogError(e, this);
		// 		isPreview = false;
		// 	}
		//
		// 	return isPreview;
		// }
#endif


		private void Awake()
		{
			if (!isInitRenderer)
				InitRenderersCache();
		}

		private void InitLocationCache()
		{
			isInitTrfLocation = true;
			localPosition = transform.localPosition;
			localScale = transform.localScale;
			localRotation = GetRotationLikeInspector(transform.localEulerAngles);
		}


		private void InitRenderersCache()
		{
			isInitRenderer = true;

			hasMyRenderer = TryGetComponent(out myRenderer);
		}

		// 주의: Danger_UseTrfSync가 한 번이라도 true가 되었던 오브젝트는 리스트에서 제거하지 않습니다.
		// 이는 런타임 중에 Danger_UseTrfSync의 상태 변화를 감지하기 위함입니다.
		public void RegisterToUpdater()
		{
			Debug.Assert(Danger_UseTrfSync);
			isLastUseTrfSync = true;
			if (!IsRegisteredTrfSync)
			{
				syncUpdater.OnRegister(this);
			}
		}

		bool isDestroyed = false;

		private void OnDestroy()
		{
			isDestroyed = true;
		}

		private bool _isRegisteredTrfSync;

		private void OnEnable()
		{
			if (Danger_UseTrfSync && !_isRegisteredTrfSync)
			{
				_isRegisteredTrfSync = true;
				RegisterToUpdater();
				OnUpdateTrf();
			}

			SetActiveRenderers(true);
		}

		private void OnDisable()
		{
			if (MonoBehaviourEventHelper.IS_QUIT || isDestroyed) return;
			if (Danger_UseTrfSync)
			{
				OnUpdateTrf();
			}

			SetActiveRenderers(false);
		}

		private void SetActiveRenderers(bool active)
		{
			if (!isInitRenderer || isDestroyed) return;

			if (hasMyRenderer)
			{
#if UNITY_EDITOR
				if (myRenderer.enabled != active)
				{
					SetDirtySafe(myRenderer);
				}
#endif
				myRenderer.enabled = active;
			}
		}

		/// <summary>
		/// 위치 상태 업데이트
		/// </summary>
		public void OnUpdateTrf()
		{
			if (!Danger_UseTrfSync)
			{
				return;
			}

			transform.localPosition = localPosition;
			transform.localScale = localScale;
			if (worldRotationZ != 0)
			{
				transform.rotation = Quaternion.AngleAxis(worldRotationZ, Vector3.forward) * Quaternion.Euler(localRotation);
				//로컬축은 Quaternion.Euler(localRotation) * Quaternion.AngleAxis(localRotationZ, Vector3.forward);반대로
			}
			else
			{
				transform.localEulerAngles = localRotation;
			}

			if (!isLastUseTrfSync)
			{
				isLastUseTrfSync = true;
				//TODO
			}
		}

		public static void TryAddOrGetComp(GameObject targetObj, AnimationSyncUpdater syncUpdater, out RendererEnabledSync rendererEnabledSync)
		{
#if !UNITY_EDITOR
            rendererEnabledSync = null;
            return;
#else
			if (!targetObj.TryGetComponent(out rendererEnabledSync))
			{
				rendererEnabledSync = targetObj.AddComponent<RendererEnabledSync>();

				// 컴포넌트를 맨 위로 이동
				bool flag = true;
				while (flag)
				{
					flag = UnityEditorInternal.ComponentUtility.MoveComponentUp(rendererEnabledSync);
				}

				rendererEnabledSync.InitRenderersCache();
				rendererEnabledSync.enabled = targetObj.activeSelf && (rendererEnabledSync.hasMyRenderer && rendererEnabledSync.myRenderer.enabled);

				rendererEnabledSync.isUpdateWhenRendererDisable = true;
				rendererEnabledSync.Danger_UseTrfSync = false;
				targetObj.SetActive(true);
				UnityEditor.EditorUtility.SetDirty(targetObj);
				UnityEditor.AssetDatabase.SaveAssetIfDirty(targetObj);
			}
			else
			{
				rendererEnabledSync.InitRenderersCache();
			}

			rendererEnabledSync.InitLocationCache();
			rendererEnabledSync.syncUpdater = syncUpdater;

			UnityEditor.EditorUtility.SetDirty(rendererEnabledSync);
			UnityEditor.AssetDatabase.SaveAssetIfDirty(rendererEnabledSync);
#endif
		}

		static Vector3 GetRotationLikeInspector(Vector3 angles)
		{
			angles.x = NormalizeAngle(angles.x);
			angles.y = NormalizeAngle(angles.y);
			angles.z = NormalizeAngle(angles.z);
			return angles;
		}

		static float NormalizeAngle(float angle)
		{
			angle = angle % 360f;
			if (angle > 180f)
				angle -= 360f;
			else
				if (angle < -180f)
					angle += 360f;
			return angle;
		}
	}
}
