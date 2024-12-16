using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
	public partial class LinePoint_Generator : CWJ.Singleton.SingletonBehaviour<LinePoint_Generator>
	{
#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts(0)]
		private static void OnReloadScript()
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (HasInstance)
				Instance._AllDispose(true);
		}

		[SerializeField, Foldout("Cache")]
		CWJ.Serializable.DictionaryVisualized<int, LinePointCachePack[]>
#else
        Dictionary<int, LinePointCachePack[]>
#endif
			_CacheDic =

#if UNITY_EDITOR
				new CWJ.Serializable.DictionaryVisualized<int, LinePointCachePack[]>();
#else
            new Dictionary<int, LinePointCachePack[]>();
#endif

		public LinePointCachePack[] TryGetByCacheDic(int cacheKey)
		{
			if (_CacheDic.TryGetValue(cacheKey, out var valuse))
			{
				return valuse;
			}

			return null;
		}

		[Foldout("Cache")]
		[SerializeField] UnityEvent objUpdater = new UnityEvent();

		[Foldout("Cache")]
		[SerializeField] HashSet<int> _disableReservedList = null;

		[SerializeField] bool isDisableLineWhenThisDisable;

		protected override void _Awake()
		{
			if (objUpdater == null)
				objUpdater = new UnityEvent();
			_AllDispose();
		}

		protected override void _OnDisable()
		{
			if (MonoBehaviourEventHelper.IS_QUIT) return;
			if (isDisableLineWhenThisDisable)
				_AllDispose();
		}

		protected override void _OnDestroy()
		{
			if (MonoBehaviourEventHelper.IS_QUIT) return;
			_AllDispose(true);
		}

		private void LateUpdate()
		{
			if (_disableReservedList != null)
			{
				int[] removeKeys = null;
				if (_disableReservedList.Count > 0)
				{
					removeKeys = _disableReservedList.ToArray();
				}

				_disableReservedList.Clear();
				_disableReservedList = null;

				if (removeKeys != null)
				{
					foreach (var key in removeKeys)
					{
						DisableLinePointByCache(key);
					}
				}

				return;
			}

			if (objUpdater != null)
			{
				objUpdater.Invoke();
			}
		}

		[InvokeButton]
		void _AllDispose(bool isDestroy = false)
		{
			if (_disableReservedList != null)
			{
				_disableReservedList.Clear();
				_disableReservedList = null;
			}

			if (objUpdater != null) objUpdater.RemoveAllListeners();
			if (_CacheDic.Count > 0)
			{
				var cpArr = _CacheDic.Values.ToArray();
				_CacheDic.Clear();
				if (isDestroy)
				{
					cpArr.Do(cacheP => _UnsafeDestroyLpCache(cacheP));
				}
				else
				{
					cpArr.Do(kv => _DisableLpCache(kv));
				}
			}
		}

		void _UnsafeDestroyLpCache(LinePointCachePack[] lpcpArr)
		{
			lpcpArr.ForEach(ins =>
			{
				ins.OnDisable();
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					if (ins.fromCacheRoot)
						DestroyImmediate(ins.fromCacheRoot.gameObject);
					if (ins.toCacheRoot)
						DestroyImmediate(ins.toCacheRoot.gameObject);
					return;
				}
#endif
				if (ins.fromCacheRoot)
					Destroy(ins.fromCacheRoot.gameObject);
				if (ins.toCacheRoot)
					Destroy(ins.toCacheRoot.gameObject);
			});
		}

		void _DisableLpCache(LinePointCachePack[] lpcpArr)
		{
			lpcpArr.ForEach(ins =>
			{
				ins.OnDisable();
			});
		}


#region DataContainer

		[Serializable]
		public struct LinePointSettings
		{
			public bool isLineLoop;

			public Color color;
			public float arrowWidth, arrowHeight, lineWidth;

			public bool isWidthChanged, isHeightChanged, isColorChanged, isLineWidthChanged;

			public LinePointSettings(bool isLineLoop, Color color, float arrowWidth, float arrowHeight, float lineWidth)
			{
				this.isLineLoop = isLineLoop;
				this.color = color;
				this.arrowWidth = arrowWidth;
				this.arrowHeight = arrowHeight;
				this.lineWidth = lineWidth;
				isWidthChanged = true;
				isHeightChanged = true;
				isColorChanged = true;
				isLineWidthChanged = true;
			}

			public void ChangeValue(Color newColor, float newArrowWidth, float newArrowHeight, float newLineWidth)
			{
				isColorChanged |= (color != newColor);
				color = newColor;

				isWidthChanged |= (arrowWidth != newArrowWidth);
				arrowWidth = newArrowWidth;

				isHeightChanged |= (arrowHeight != newArrowHeight);
				arrowHeight = newArrowHeight;

				isLineWidthChanged |= (lineWidth != newLineWidth);
				lineWidth = newLineWidth;
			}

			public void FixChangedValue()
			{
				isWidthChanged = false;
				isHeightChanged = false;
				isColorChanged = false;
				isLineWidthChanged = false;
			}
		}

		[Serializable]
		public class LinePointCachePack
		{
			public string displayName;
			public int cacheID;
			public bool isSubscribed = false;
			public Transform fromTrf;
			public Transform toTrf;
			public LinePointSettings settings;
			public bool isDotOnly;
			public bool isGeneratedOrAnimDone;
			public Vector3 lastFromPos;
			public Vector3 lastToPos;

			public bool isNeedToObjChild;
			public bool isCreateToObjOnly;
			public LineRenderer lineRenderer;

			public ChildUIObjSet fromChildObjSet;
			public Transform fromCacheRoot => fromChildObjSet == null ? null : fromChildObjSet.cacheRoot;

			public ChildUIObjSet toChildObjSet;
			public Transform toCacheRoot => toChildObjSet == null ? null : toChildObjSet.cacheRoot;

			public void UpdateUIs()
			{
				if (isNeedToObjChild)
				{
					SetActiveChildUIObj(toTrf, ref toChildObjSet);
					toChildObjSet.UpdateChildUISettings(settings, toTrf.name, lineRenderer);
				}

				if (!isDotOnly)
				{
					if (!isCreateToObjOnly)
					{
						SetActiveChildUIObj(fromTrf, ref fromChildObjSet);
						fromChildObjSet.UpdateChildUISettings(settings, fromTrf.name, lineRenderer);
					}
				}
			}

			[Serializable]
			public class ChildUIObjSet
			{
				public Transform cacheRoot;
				public MeshRenderer pointRndr;
				public TextMeshPro textObj;

				public ChildUIObjSet(Transform cacheRoot)
				{
					this.cacheRoot = cacheRoot;
				}

				public ChildUIObjSet(Transform cacheRoot, MeshRenderer pointRndr, TextMeshPro text)
				{
					this.cacheRoot = cacheRoot;
					this.pointRndr = pointRndr;
					this.textObj = text;
				}

				public void UpdateChildUISettings(LinePointSettings settings, string parentTrfName, LineRenderer lineRenderer)
				{
					Color color = settings.color;


					if (settings.isWidthChanged || settings.isColorChanged)
					{
						if (pointRndr && pointRndr.gameObject.activeSelf)
						{
							if (settings.isWidthChanged)
							{
								float scale = settings.arrowHeight * 2;
								pointRndr.transform.SetParent(null);
								pointRndr.transform.localScale = Vector3.one * scale;
								pointRndr.transform.SetParent(cacheRoot);
								pointRndr.transform.localPosition = Vector3.zero;
							}

							if (settings.isColorChanged)
							{
								SetMatColor(pointRndr, true, new Color(color.r, color.g, color.b, color.a));
							}
						}
					}

					if (textObj && textObj.gameObject.activeSelf)
					{
						string textContent = parentTrfName;
						Vector3? pivot = null;
						if (parentTrfName.Contains("//"))
						{
							var splits = parentTrfName.Split("//", 2);
							textContent = splits[0].Trim();
							string comment = splits[1].Trim();
							if (comment.StartsWith("("))
							{
								pivot = StringUtil.ConvertToVector3(comment);
							}
						}

						if (settings.isWidthChanged)
						{
							textObj.transform.localScale = Vector3.one * (settings.arrowWidth * 10);
							textObj.transform.SetParent(cacheRoot);
							textObj.transform.localRotation = Quaternion.identity;
						}

						if (settings.isColorChanged)
						{
							textObj.color = new Color(color.r, color.g, color.b, color.a);
						}

						if (pivot != null)
							textObj.rectTransform.pivot = pivot.Value;

						if (textObj.transform.localPosition != Vector3.zero)
							textObj.transform.localPosition = Vector3.zero;

						if (textObj.text != textContent)
							textObj.SetText(textContent);
					}


					if (settings.isLineWidthChanged)
					{
						if (lineRenderer && lineRenderer.gameObject.activeSelf)
						{
							lineRenderer.startWidth = settings.lineWidth;
							lineRenderer.endWidth = settings.lineWidth;
						}
					}
				}
			}

			public LinePointCachePack(int cacheID, LinePointSettings settings, Transform fromTrf, Transform toTrf, bool isCreateToObjOnly)
			{
				this.cacheID = cacheID;
				isGeneratedOrAnimDone = false;
				this.settings = settings;
				this.fromTrf = fromTrf;
				this.toTrf = toTrf;
				this.isCreateToObjOnly = isCreateToObjOnly;
				if (isCreateToObjOnly)
					isNeedToObjChild = true;
				isDotOnly = fromTrf == toTrf;
				this.displayName = isDotOnly ? fromTrf.name : (fromTrf.name + "->" + toTrf.name);
				lastFromPos = Vector3.zero;
				lastToPos = Vector3.zero;
				isSubscribed = false;
			}


			void DestroyReserveSelf()
			{
				OnDisable();
				var generator = Instance;
				if (generator._disableReservedList == null)
					generator._disableReservedList = new HashSet<int>();
				generator._disableReservedList.Add(cacheID);
			}

			public void OnEnable()
			{
				if (!isCreateToObjOnly)
				{
					if (fromChildObjSet == null)
						fromChildObjSet = new ChildUIObjSet(GetCacheObj(cacheID, fromTrf));
					if (!fromChildObjSet.cacheRoot)
						fromChildObjSet.cacheRoot = GetCacheObj(cacheID, fromTrf);

					if (!fromCacheRoot.gameObject.activeSelf)
						fromCacheRoot.gameObject.SetActive(true);
				}

				if (isNeedToObjChild)
				{
					if (toChildObjSet == null)
						toChildObjSet = new ChildUIObjSet(GetCacheObj(cacheID, toTrf));
					if (!toChildObjSet.cacheRoot)
						toChildObjSet.cacheRoot = GetCacheObj(cacheID, toTrf);

					if (!toCacheRoot.gameObject.activeSelf)
						toCacheRoot.gameObject.SetActive(true);
				}

				if (!isSubscribed)
				{
					isSubscribed = true;
#if UNITY_EDITOR
					if (!Application.isPlaying)
					{
						UnityEditor.EditorApplication.update += UpdateObj;
						return;
					}
#endif
					if (Instance.objUpdater == null)
						Instance.objUpdater = new UnityEvent();
					Instance.objUpdater.AddListener(UpdateObj);
				}
			}

			public void OnDisable()
			{
				isGeneratedOrAnimDone = false;

				if (isSubscribed)
				{
					isSubscribed = false;
					Instance.objUpdater?.RemoveListener(UpdateObj);
#if UNITY_EDITOR
					if (!Application.isPlaying)
					{
						UnityEditor.EditorApplication.update -= UpdateObj;
					}
#endif
				}

				//#if UNITY_EDITOR
				//                if (!Application.isPlaying)
				//                {
				//                    if (fromCacheRoot != null)
				//                        DestroyImmediate(fromCacheRoot.gameObject);
				//                    if (toCacheRoot != null)
				//                        DestroyImmediate(toCacheRoot.gameObject);

				//                    return;
				//                }
				//#endif
				if (fromCacheRoot && fromCacheRoot.gameObject.activeSelf)
					fromCacheRoot.gameObject.SetActive(false);
				if (toCacheRoot && toCacheRoot.gameObject.activeSelf)
					toCacheRoot.gameObject.SetActive(false);
			}

			void UpdateObj()
			{
				if (isCreateToObjOnly)
				{
					if (!toCacheRoot || !toCacheRoot.gameObject.activeInHierarchy)
						return;
				}
				else
				{
					if (!fromCacheRoot || !fromCacheRoot.gameObject.activeInHierarchy)
						return;
				}

				if (isGeneratedOrAnimDone && !CheckIsVerified())
				{
					//Debug.LogError("?WTF" + isGeneratedOrAnimDone);
					DestroyReserveSelf();
					return;
				}

				if (isGeneratedOrAnimDone && !isDotOnly && !EqualsCurTrfPos(out Vector3 curFromPos, out Vector3 curToPos))
				{
					UpdatePosNonAnim(curFromPos, curToPos);
				}
			}


			public void UpdatePosNonAnim(Vector3 _fromPos, Vector3 _toPos)
			{
				if (isDotOnly)
				{
					isGeneratedOrAnimDone = true;
					return;
				}

				isGeneratedOrAnimDone = false;

				lastFromPos = _fromPos;
				lastToPos = _toPos;
				//linePointIns.isGenerateOrAnimDone = false;
				float arrowHeight = settings.arrowHeight;
				float arrowWidth = settings.arrowWidth;


				var minLen = Math.Sqrt(arrowHeight * arrowHeight + arrowWidth * arrowWidth);

				var distance = Vector3.Distance(_fromPos, _toPos);

				if (distance < minLen)
				{
					arrowHeight = distance;
				}

				if (distance == 0)
				{
					distance = 0.000001f;
				}

				var pointC = _toPos + arrowHeight * (_fromPos - _toPos) / distance;

				Transform playerCamTrf = Instance.sceneObjs.playerCamTrf;
#if UNITY_EDITOR
				if (!Application.isPlaying && !playerCamTrf)
				{
					var cam = FindObjectsOfType<Camera>(true).FirstOrDefault(c => c && c.CompareTag("MainCamera"));
					playerCamTrf = cam.transform;
				}
#endif

				var camPoint = (playerCamTrf ? playerCamTrf : Camera.main.transform).position;
				var camToFrom = _fromPos - camPoint;
				var camToTo = _toPos - camPoint;

				var normal = Vector3.Cross(camToFrom, camToTo).normalized;

				var pointD = pointC + (arrowWidth * 1.2f) * normal;
				var pointE = pointC - (arrowWidth * 1.2f) * normal;

				lineRenderer.SetPosition(0, _fromPos);
				lineRenderer.SetPosition(1, pointC);

				//if (single)
				//{
				lineRenderer.SetPosition(2, pointD);
				lineRenderer.SetPosition(3, _toPos);
				lineRenderer.SetPosition(4, pointE);
				lineRenderer.SetPosition(5, pointD);
				//}
				//else
				//{
				//capRenderer.SetPosition(0, pointD);
				//capRenderer.SetPosition(1, _toPos);
				//capRenderer.SetPosition(2, pointE);
				//capRenderer.SetPosition(3, pointD);
				//}
				isGeneratedOrAnimDone = true;
			}


#if UNITY_2023_1_OR_NEWER
            public async Awaitable IE_UpdatePosWithAnim(float animationTime)
#else
			public IEnumerator IE_UpdatePosWithAnim(float animationTime)
#endif
			{
				if (isDotOnly)
				{
					isGeneratedOrAnimDone = true;
					yield break;
				}

				isGeneratedOrAnimDone = false;
				Vector3 _from = lastFromPos;
				Vector3 _to = lastToPos;

				float arrowWidth = settings.arrowWidth;
				float arrowHeight = settings.arrowHeight;

				var maxWidth = 0f;
				foreach (var k in lineRenderer.widthCurve.keys)
				{
					if (k.value > maxWidth)
						maxWidth = k.value;
				}

				maxWidth *= lineRenderer.widthMultiplier;

				var minLen = Math.Sqrt(arrowHeight * arrowHeight + arrowWidth * arrowWidth);
				var distance = Vector3.Distance(_from, _to);
				if (distance < arrowHeight)
				{
					arrowHeight = distance;
				}

				if (distance == 0)
				{
					distance = 0.000001f;
				}

				Transform playerCamTrf = Instance.sceneObjs.playerCamTrf;
#if UNITY_EDITOR
				if (!Application.isPlaying && !playerCamTrf)
				{
					var cam = FindObjectsOfType<Camera>(true).FirstOrDefault(c => c && c.CompareTag("MainCamera"));
					playerCamTrf = cam.transform;
				}
#endif
				var camPoint = (playerCamTrf ?? Camera.main.transform).position;
				var camToFrom = _from - camPoint;
				var camToTo = _to - camPoint;

				var normal = Vector3.Cross(camToFrom, camToTo).normalized;

				Vector3 from() => fromTrf.position;
				Vector3 to() => toTrf.position;

				var totalTime = 0f;
				while (totalTime < animationTime)
				{
					if (!CheckIsVerified())
					{
						yield break;
					}

					var timeRatio = totalTime / animationTime;

					var len = arrowHeight * timeRatio;

					var pointB2 = from() + timeRatio * (to() - from());
					var pointC = pointB2 + len * (from() - to()) / distance;

					var pointD = pointC + arrowWidth * normal;
					var pointE = pointC - arrowWidth * normal;

					if (Vector3.Distance(from(), pointC) >= maxWidth)
					{
						lineRenderer.SetPosition(0, from());
						lineRenderer.SetPosition(1, pointC);
						//if (single)
						//{
						lineRenderer.SetPosition(2, pointD);
						lineRenderer.SetPosition(3, pointB2);
						lineRenderer.SetPosition(4, pointE);
						lineRenderer.SetPosition(5, pointD);
						//}
						//else
						//{
						//    capRenderer.SetPosition(0, pointD);
						//    capRenderer.SetPosition(1, pointB2);
						//    capRenderer.SetPosition(2, pointE);
						//}
					}

					totalTime += Time.deltaTime;
#if UNITY_2023_1_OR_NEWER
                await Awaitable.NextFrameAsync();
#else
					yield return null;
#endif
				}

				isGeneratedOrAnimDone = true;
			}


			public bool EqualsCurTrfPos(out Vector3 curFromPos, out Vector3 curToPos)
			{
				curFromPos = fromTrf.position;
				curToPos = toTrf.position;

				if (!curToPos.Equals(lastToPos))
					return false;

				if (!isDotOnly && !curFromPos.Equals(lastFromPos))
					return false;

				return true;
			}

			public bool CheckIsVerified()
			{
				if (isDotOnly)
				{
					if (isCreateToObjOnly)
					{
						if (!toTrf || !toTrf.gameObject.activeInHierarchy)
							return false;
					}
					else
					{
						if (!fromTrf || !fromTrf.gameObject.activeInHierarchy)
							return false;
					}
				}
				else
				{
					if (!fromTrf || !toTrf)
						return false;
					if (!fromTrf.gameObject.activeInHierarchy || !toTrf.gameObject.activeInHierarchy)
						return false;
				}

				return true;
			}
		}

#endregion
	}
}
