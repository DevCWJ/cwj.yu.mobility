using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using static CWJ.LinePoint_Generator;
using CWJ.YU.Mobility;
using DG.Tweening;

namespace CWJ
{
	/// <summary>
	/// CWJ - 24.08.08
	/// <para/> autoSyncRoot에 점/선 위치타겟 오브젝트의 부모오브젝트를 넣어두거나
	/// <br/> userCustomAddParent에 개별 점/선 오브젝트를 넣으면
	/// <br/>자동으로 오브젝트 이름, 위치 정보를 LinePoint_Generator에 활용해 선과 점 오브젝트로 제작해줌
	/// <para/>에디터에서 생성되어 미리보여주는 기능을 중점으로 만듬
	/// <para/> 실행중엔 자동입력 안되고, 감지되고싶으면 활성화 상태여야함
	/// </summary>
	public class LinePoint_EzDrawer : MonoBehaviour
#if UNITY_EDITOR
	                                , CWJ.AccessibleEditor.InspectorHandler.ISelectHandler, CWJ.AccessibleEditor.InspectorHandler.ICompiledHandler
#endif
	{
#if UNITY_EDITOR
		[DrawHeaderAndLine("Editor 영역")]
		[SerializeField] bool defaultIsAngleLine = true;

		[SerializeField] bool defaultIsLineLoop = false;

		[ColorUsage(true, true)]
		[SerializeField] Color defaultColor = new Color(1, 1, 1, 1);

		[DrawHeaderAndLine(nameof(autoSyncRoot) + " 자식들로 자동삽입/제거됨.")]
		[SerializeField] Transform autoSyncRoot;

		[SerializeField] PointDataContainer[] autoSyncPdcs;

		[DrawHeaderAndLine(nameof(userCustomAddParent) + " 에 넣으면 추가됨.")]
		[SerializeField] Transform userCustomAddParent = null;

		[SerializeField] PointDataContainer[] userInputPdcs;

		[SerializeField, HideInInspector] Transform autoSyncRoot_cache;
		[SerializeField]                  List<int> lineCahces_editor = new List<int>();

		private void OnValidate()
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (isCompiled)
			{
				return;
			}

			if (Can_UserCustomConfigure())
			{
				_last_userCustomAddParent = userCustomAddParent;
				userCustomAddParent = null;
				UnityEditor.EditorApplication.delayCall += () =>
				{
					DisableDraw();
					if (UserCustomConfigure())
					{
						AutoSyncConfigure();
						AutoListing();
						Draw();
					}
				};
			}
			else if (autoSyncRoot_cache != autoSyncRoot)
			{
				autoSyncRoot_cache = autoSyncRoot;

				UnityEditor.EditorApplication.delayCall += () =>
				{
					DisableDraw();
					if (autoSyncRoot != null)
					{
						CWJEditor_OnSelect(this);
					}
					else
					{
						autoSyncPdcs = null;
						OnValidate();
					}
				};
				return;
			}
		}

		static LinePoint_EzDrawer _LastSelectEditor;

		public void CWJEditor_OnSelect(MonoBehaviour target)
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (target == null) return;
			if (UnityEditor.Selection.objects.Length > 1)
			{
				return;
			}

			UnityEditor.EditorApplication.delayCall += () => //compile safe
			{
				if (_LastSelectEditor != null && _LastSelectEditor != this)
				{
					_LastSelectEditor.DisableDraw();
				}

				_LastSelectEditor = this;

				if (isCompiled)
				{
					isCompiled = false;
					return;
				}

				DisableDraw();

				if (Can_AutoSyncConfigure())
				{
					AutoSyncConfigure();
					AutoListing();
					Draw();
				}
			};
		}

		bool isCompiled = false;

		public void CWJEditor_OnCompile()
		{
			isCompiled = true;
		}

		PointDataContainer ConvertToPdc(Transform[] trfs)
		{
			if (trfs.LengthSafe() == 0) return PointDataContainer.NULL;

			trfs = trfs.Where(t => t.gameObject.hideFlags != HideFlags.DontSave).ToArray();
			bool isAngleLine = defaultIsAngleLine;
			bool isLineLoop = defaultIsLineLoop;
			if (trfs.Length < 3)
			{
				isAngleLine = false;
				isLineLoop = false;
			}

			if (trfs.Length == 1)
			{
				trfs = new Transform[2] { trfs[0], trfs[0] };
			}

			return new PointDataContainer(trfs, isAngleLine, defaultColor, isLineLoop);
		}

		bool Can_AutoSyncConfigure()
		{
			return autoSyncRoot != null && autoSyncRoot.childCount > 0;
		}

		bool AutoSyncConfigure()
		{
			Debug.Assert(Can_AutoSyncConfigure());

			List<PointDataContainer> sameList = new List<PointDataContainer>();
			List<PointDataContainer> addList = new List<PointDataContainer>();

			bool alreadyHasPdcs = autoSyncPdcs.LengthSafe() > 0;
			foreach (Transform pdcCoreTrf in autoSyncRoot)
			{
				var newPdc = ConvertToPdc(pdcCoreTrf.GetComponentsInChildren_New<Transform>(false, false));
				if (!newPdc.IsNull())
				{
					if (alreadyHasPdcs)
					{
						var existsPdc = autoSyncPdcs.FirstOrDefault(p => p.IsEqualsOther(newPdc));
						if (existsPdc.isValid) // same
							sameList.Add(existsPdc);
						else
							addList.Add(newPdc);
					}
					else
					{
						addList.Add(newPdc);
					}
				}
			}

			int addCnt = addList.Count;
			int sameCnt = sameList.Count;
			bool isChanged = false;

			if (!alreadyHasPdcs)
			{
				autoSyncPdcs = addList.ToArray();
				isChanged = addCnt > 0;
			}
			else
			{
				isChanged = addCnt > 0 || sameCnt != autoSyncPdcs.Length;
				sameList.AddRange(addList);
				autoSyncPdcs = sameList.ToArray();
			}

			if (isChanged)
			{
				Debug.Log($"[{nameof(LinePoint_EzDrawer)}] Succeed Auto Generate from {nameof(autoSyncRoot)}'s child\n" +
				          (addCnt == 0 ? "Changed pdc array " : $"Add Genrated pdc: {addCnt}"), gameObject);
			}


			return isChanged;
		}

		bool Can_UserCustomConfigure()
		{
			return userCustomAddParent;
		}


		Transform _last_userCustomAddParent;

		bool UserCustomConfigure()
		{
			if (_last_userCustomAddParent == null) return false;

			var pdc = ConvertToPdc(_last_userCustomAddParent.GetComponentsInChildren_New<Transform>(false, false));
			string objName = _last_userCustomAddParent.name;
			_last_userCustomAddParent = null;
			bool isNull = pdc.IsNull();
			if (isNull)
			{
				return false;
			}
			else
			{
				if (userInputPdcs.IsExists(p => p.IsEqualsOther(pdc)))
					Debug.Log($"이미 추가되어있는 오브젝트임 ({objName})");
				else
					ArrayUtil.Add(ref userInputPdcs, pdc);
			}

			CWJ.AccessibleEditor.AccessibleEditorUtil.PingObj(pdc.pointTrfs[0].gameObject, false);

			Debug.Log($"[{nameof(LinePoint_EzDrawer)}] Add {nameof(userCustomAddParent)} Suceed : '{objName}'");
			return !isNull;
		}

		void AutoListing()
		{
			int autoSyncLength = autoSyncPdcs.LengthSafe();
			int userInputLength = userInputPdcs.LengthSafe();

			List<PointDataContainer> list = new List<PointDataContainer>(capacity: autoSyncLength + userInputLength);
			if (autoSyncLength > 0)
				list.AddRange(autoSyncPdcs);
			if (userInputLength > 0)
				list.AddRange(userInputPdcs);
			var newArr = list.ToArray();
			bool isChanged = !ArrayUtil.ArrayEquals(pointDataContainers, newArr, (l, r) => l.IsEqualsOther(r));
			if (isChanged)
			{
				pointDataContainers = newArr;
				//Debug.LogError("?");
				//CWJ.AccessibleEditor.EditorSetDirty.SetObjectDirty(this);
			}
		}
#endif

		[DrawHeaderAndLine("Setting 영역")]
		public float arrowHeight = 0.02f;

		public float arrowWidth = 0.015f;
		public float lineWidths = 0.01f;
		public float animTime   = 1;


		/// <summary>
		/// 실행중이 아닐때는 에디터에서 자동으로 수정하기때문에 Editor상에선 수정하지말고
		/// <br/>runtime중에 code를 통해 수정할땐 이걸 쓰기
		/// </summary>
		[DrawHeaderAndLine("Runtime중에만 수정가능")]
		[Tooltip("Editor에서는 자동수정됨")]
		public PointDataContainer[] pointDataContainers;

		[Serializable]
		public struct PointDataContainer
		{
			[SerializeField]
			string editorDevComment;

			public int[] caches;
			public bool  isValid;
			public bool  isAngleLine;

			[ColorUsage(true, true)]
			public Color color;

			public bool        isLineLoop;
			public Transform[] pointTrfs;

			public static readonly PointDataContainer NULL = default(PointDataContainer);

			public PointDataContainer(Transform[] pointTrfs, bool isAngleLine, Color color, bool isLineLoop)
			{
				editorDevComment = $"\"{pointTrfs[0].name}\" ~ \"{pointTrfs[pointTrfs.Length - 1].name}\"";
				this.pointTrfs = pointTrfs;
				this.isAngleLine = isAngleLine;
				this.color = color;
				this.isLineLoop = isLineLoop;
				this.isValid = pointTrfs.LengthSafe() > 0;
				this.caches = null;
			}

			public bool IsNull() => !isValid;

			public bool IsEqualsOther(PointDataContainer other)
			{
				if (isValid != other.isValid) return false;
				return (isAngleLine == other.isAngleLine
				        && isLineLoop == other.isLineLoop
				        && ArrayUtil.ArrayEquals(pointTrfs, other.pointTrfs));
			}

			public bool IsSettingsEquals(PointDataContainer other)
			{
				return color == other.color;
			}
		}

		[InvokeButton]
		public void ChangePointWithAngleLine(Transform pointTrf, Vector3 pos, float animTime = 1, bool isUpdateName = true)
		{
			if (isUpdateName)
			{
				pointTrf.gameObject.name = Extension.UpdateNameViaPos(pointTrf.gameObject.name, pos);
			}

			ChangePos3D(pointTrf, pos, isUpdateName, animTime);
		}

		Coroutine CO_updateName = null;

		IEnumerator DO_UpdateName(Transform targetTrf, string backupFullName, float animTime)
		{
			yield return null;

			var p = pointDataContainers.FirstOrDefault(pdc => pdc.pointTrfs.IsExists(t => t == targetTrf));

			var cachePackList = p.caches.SelectMany(c => LinePoint_Generator.Instance.TryGetByCacheDic(c)).ToArray();


			float t = 0;
			do
			{
				targetTrf.gameObject.name = Extension.UpdateNameViaPos(backupFullName, targetTrf.localPosition);
				cachePackList.Do(c => c?.UpdateUIs());
				yield return null;
				t += Time.deltaTime;
			} while (animTime > t);

			CO_updateName = null;
		}

		Sequence lastSequence;

		[InvokeButton]
		public void ChangePos3D(Transform target, Vector3 localPos, bool isDoDraw, float animTime = 0.5f)
		{
			DisableDraw();

			Debug.Assert(pointDataContainers.IsExists(p => p.pointTrfs.IsExists(target)),
			             target.name + " is not include in pdcs");

			if (animTime > 0)
			{
				if (CO_updateName != null) StopCoroutine(CO_updateName);
				CO_updateName = StartCoroutine(DO_UpdateName(target, target.name, animTime));
			}
			else
			{
				target.localPosition = localPos;
			}

			if (isDoDraw)
			{
				ThreadDispatcher.Enqueue(Draw);
			}

			if (lastSequence != null)
			{
				lastSequence.Kill();
			}

			if (animTime > 0)
			{
				lastSequence = DOTween.Sequence()
				                      .SetAutoKill(true)
				                      .Append(target.DOLocalMove(localPos, animTime))
				                      .OnComplete(() => lastSequence = null);
				//lastSequence.Restart();
			}
		}

		[SerializeField] List<int> lineCahces = new List<int>();

		public void SetActiveAllPoints(bool isActive)
		{
			pointDataContainers.ForEach(p => p.pointTrfs.ForEach(t => t.gameObject.SetActive(isActive)));
		}

		[InvokeButton]
		public void Draw()
		{
			DisableDraw();
			if (Application.isPlaying)
				ThreadDispatcher.Enqueue(DrawImmediately);
			else
				DrawImmediately();
		}


		void DrawImmediately()
		{
			if (pointDataContainers.LengthSafe() == 0)
			{
				return;
			}

			var caches = lineCahces;
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				if (isCompiled || _LastSelectEditor != this) return; //이거덕에 컴파일후 캐시는 사라지고 오브젝트는 남는 버그안생김
				caches = lineCahces_editor;
			}
#endif
			List<int> cacheTmpList = new List<int>();
			for (int i = 0; i < pointDataContainers.Length; i++)
			{
				var p = pointDataContainers[i];
				if (p.isAngleLine)
				{
					var parentPoint = new PointData(p.pointTrfs[0], p.color);
					var parentKv = Generate(new PointData[2] { parentPoint, parentPoint }, false, animTime, arrowWidth, arrowHeight
					                      , isCreateToObjOnly: true, lineWidths);

					cacheTmpList.Add(parentKv.cacheID);

					for (int j = 1; j < p.pointTrfs.Length; j++)
					{
						var pointDatas = new PointData[2] { parentPoint, new PointData(p.pointTrfs[j], p.color) };
						var kv = Generate(pointDatas, false, animTime, arrowWidth, arrowHeight, isCreateToObjOnly: true, lineWidths);
						cacheTmpList.Add(kv.cacheID);
					}
				}
				else
				{
					var pointDatas = p.pointTrfs.Select(t => new PointData(t, p.color)).ToArray();
					var kv = Generate(pointDatas, p.isLineLoop, animTime, arrowWidth, arrowHeight, isCreateToObjOnly: false, lineWidths);
					cacheTmpList.Add(kv.cacheID);
				}

				caches.AddRange(cacheTmpList);
				pointDataContainers[i].caches = cacheTmpList.ToArray();
				cacheTmpList.Clear();
			}

			//return list.ToArray();
		}

		[InvokeButton]
		public void DisableDraw(bool isDestroyCache = false)
		{
			if (lineCahces.CountSafe() > 0)
			{
				var arr = lineCahces.ToArray();
				lineCahces.Clear();
				LinePoint_Generator.Instance.DisableLinePointByCache(isDestroyCache, arr);
			}
#if UNITY_EDITOR
			if (lineCahces_editor.CountSafe() > 0)
			{
				var arr = lineCahces_editor.ToArray();
				lineCahces_editor.Clear();
				LinePoint_Generator.Instance.DisableLinePointByCache(isDestroyCache, arr);
			}
#endif
		}
	}
}
