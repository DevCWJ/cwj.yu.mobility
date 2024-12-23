
using CWJ.SceneHelper;
using CWJ.YU.Mobility;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// CWJ - 24.08.08
    /// <para/>점들을 선으로 이어주기위해 만든 녀석. 캐싱기능도 추가해놓음.
    /// <para/>target지점을 이동하면 점, 선, text 도 함께 움직임.
    /// <para/>target지점의 transform이 비활성화 되면 알아서 사라짐.
    /// </summary>
    public partial class LinePoint_Generator : CWJ.Singleton.SingletonBehaviour<LinePoint_Generator>, CWJ.SceneHelper.INeedSceneObj, CWJ.Singleton.IDontAutoCreatedWhenNull
    {
        #region Field
        [VisualizeProperty] public SceneObjContainer sceneObjs { get; set; }


        [SerializeField] LineRenderer linePrefab;
        [SerializeField] MeshRenderer pointPrefab;
        [SerializeField] TMPro.TextMeshPro textPrefab;

        public float defaultArrowHeight = 0.02f;
        public float defaultArrowWidth = 0.015f;
        public float defaultLineWidth = 0.0125f;

        [Serializable]
        public struct PointData
        {
            public Transform pointTrf;
            [ColorUsage(true, true)]
            public Color color;

            public PointData(Transform pointTrf, Color color)
            {
                this.pointTrf = pointTrf;
                this.color = color;
            }
        }

        static int GetCacheID(bool isLineLoop, PointData[] _points)
        {
            var instanceIDs = _points.Select(p => p.pointTrf.GetInstanceID()).ToArray();
            int hash = 17;
            hash = hash * 31 + isLineLoop.GetHashCode();
            foreach (int id in instanceIDs)
            {
                hash = hash * 31 + id;
            }
            return hash;
        }

        static MeshRenderer _PointPrefab = null;
        static MeshRenderer PointPrefab
        {
            get
            {
                if (_PointPrefab == null)
                {
                    _PointPrefab = Instance.pointPrefab;
                    _PointPrefab.gameObject.tag = DescriptonManager.LinePointObj_PointerTag;
                }
                return _PointPrefab;
            }
        }

        static TextMeshPro _TextPrefab = null;
        static TextMeshPro TextPrefab
        {
            get
            {
                if (_TextPrefab == null)
                {
                    _TextPrefab = Instance.textPrefab;
                    _TextPrefab.gameObject.tag = DescriptonManager.LinePointObj_PointerTag;
                }
                return _TextPrefab;
            }
        }

        #endregion Field


        [InvokeButton]
        public static (int cacheID, LinePointCachePack[] lpCaches)
            Generate(PointData[] pointDatas, bool isLineLoop, float animTime, float arrowWidth = -1, float arrowHeight = -1, float lineWidth = -1, bool isCreateToObjOnly = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) animTime = 0; //나중에 Editor에서도 animation되게 할거
#endif
            int pointDataLength = pointDatas.Length;
            bool hasAnimation = animTime > 0.0f && pointDataLength > 1;
            isLineLoop = isLineLoop && pointDataLength > 2;

            return _Generate(pointDatas, isLineLoop, hasAnimation, animTime, arrowWidth, arrowHeight, lineWidth, isCreateToObjOnly);
        }
        public void DisableLinePointByCache(bool isDestroyCache, int[] cacheIDs)
        {
            if (_disableReservedList != null)
            {
                _disableReservedList.RemoveRange(cacheIDs);
                if (_disableReservedList.Count == 0) _disableReservedList = null;
            }
            for (int i = 0; i < cacheIDs.Length; i++)
            {
                int c = cacheIDs[i];
                if (_CacheDic.TryGetValue(c, out var lpcpArr))
                {
                    if (isDestroyCache)
                        _UnsafeDestroyLpCache(lpcpArr);
                    else
                        _DisableLpCache(lpcpArr);
                }
            }
        }
        public void DisableLinePointByCache(params int[] cacheIDs)
        {
            DisableLinePointByCache(false, cacheIDs);
        }


        #region Generate

        static (int cacheID, LinePointCachePack[]) _Generate(PointData[] pointDatas, bool isLineLoop, bool hasAnimation, float animTime, float arrowWidth,
                                                             float arrowHeight, float lineWidth, bool isCreateToObjOnly = false)
        {
            int pointDataLength = pointDatas.LengthSafe();
            if (pointDataLength == 0) return (-1, null);
            if (pointDataLength == 1)
            {
                pointDatas = new PointData[2] { pointDatas[0], pointDatas[0] };
            }

            var cacheID = GetCacheID(isLineLoop, pointDatas);
            var helper = LinePoint_Generator.Instance;
            arrowWidth = arrowWidth < 0 ? helper.defaultArrowWidth : arrowWidth;
            arrowHeight = arrowHeight < 0 ? helper.defaultArrowHeight : arrowHeight;
            lineWidth = lineWidth < 0 ? helper.defaultLineWidth : lineWidth;


            if (!helper._CacheDic.TryGetValue(cacheID, out var lpCacheArr))
            {
                var newLpCacheList = new List<LinePointCachePack>();

                for (int i = 0; i < pointDataLength; i++)
                {
                    var next = i + 1;
                    if (next >= pointDataLength)
                    {
                        if (isLineLoop)
                        {
                            next = 0;
                        }
                        else
                        {
                            break;
                        }
                    }

                    var fromTrf = pointDatas[i].pointTrf;
                    var toTrf = pointDatas[next].pointTrf;

                    Debug.Assert(fromTrf && toTrf, "LinePoint_Generator.Create : from, to 어쨋음?");

                    var settings = new LinePointSettings(isLineLoop, pointDatas[i].color, arrowWidth, arrowHeight, lineWidth);
                    var lpc = new LinePointCachePack(cacheID, settings, fromTrf, toTrf, isCreateToObjOnly);

                    if (lpc.CheckIsVerified() == false)
                    {
#if UNITY_EDITOR
                        Debug.Log($"LinePoint 제작중 Target Transform이 없거나 비활성화상태.\n(cache:{lpc.cacheID} / name:{lpc.displayName})");
#endif
                        return (-1, null);
                    }

                    newLpCacheList.Add(lpc);
                }

                lpCacheArr = newLpCacheList.ToArray();

                helper._CacheDic.Add(cacheID, lpCacheArr);
            }

            int insLength = lpCacheArr.Length;

            bool isNeedLastToObjChild = isCreateToObjOnly || (!isLineLoop && pointDataLength >= 2);
            if (isNeedLastToObjChild)
                lpCacheArr[insLength - 1].isNeedToObjChild = true;

            for (int i = 0; i < insLength; ++i)
            {
                var lpCache = lpCacheArr[i];
                Debug.Assert(lpCache != null);
                Debug.Assert(lpCache.fromTrf);

                if (!lpCache.isCreateToObjOnly && (!isNeedLastToObjChild || !lpCache.isNeedToObjChild))
                    lpCache.isNeedToObjChild = false;

                lpCache.isGeneratedOrAnimDone = false;
                lpCache.OnEnable();

                var color = pointDatas[i].color;
                //if (color.a == 0) //실수일것
                //    color = new Color(color.r, color.g, color.b, 1f);
                lpCache.settings.ChangeValue(color, arrowWidth, arrowHeight, lineWidth);

                lpCache.UpdateUIs();

                helper.UpdateLineAndPoint(lpCache, color, hasAnimation, animTime, lineWidth);
                lpCache.settings.FixChangedValue();
            }

            return (cacheID, lpCacheArr);
        }

        const string ChildNameStart = @"\LP_Cache/";

        static Transform FindCacheObj(Transform parent, int cacheID, out string cacheObjName)
        {
            cacheObjName = ChildNameStart + cacheID;
            int childCnt = parent.childCount;
            if (childCnt > 0)
            {
                for (int i = childCnt - 1; i >= 0; --i)
                {
                    var c = parent.GetChild(i);
                    if (c && c.gameObject.name.Equals(cacheObjName))
                    {
                        return c;
                    }
                }
            }
            return null;
        }

        static Transform GetCacheObj(int cacheID, Transform parent)
        {
            Transform cacheObj = FindCacheObj(parent, cacheID, out var cacheObjName);
            if (!cacheObj)
            {
                cacheObj = new GameObject(cacheObjName).transform;
                cacheObj.SetParent(parent);
                cacheObj.Reset();
                cacheObj.gameObject.hideFlags = HideFlags.DontSave;
            }
            cacheObj.gameObject.SetActive(true);
            return cacheObj;
        }

        static void SetActiveChildUIObj(Transform parentTrf, ref LinePointCachePack.ChildUIObjSet childUIObjSet)
        {
            if (!parentTrf) return;
            string parentTrfName = parentTrf.gameObject.name.Trim();

            void CreateAndSwitchUIObj<T>(ref T uiO, string noneTag, T prefab) where T : Component
            {
                if (!parentTrfName.ToUpper().Contains(noneTag))
                {
                    if (!uiO)
                    {
                        uiO = Instantiate(prefab);
                        uiO.gameObject.hideFlags = HideFlags.DontSave;
                    }

                    if (!uiO.gameObject.activeSelf)
                        uiO.gameObject.SetActive(true);
                }
                else
                {
                    if (uiO && uiO.gameObject.activeSelf)
                        uiO.gameObject.SetActive(false);
                }
            }
            CreateAndSwitchUIObj(ref childUIObjSet.pointRndr, "[X:P]", PointPrefab);
            CreateAndSwitchUIObj(ref childUIObjSet.textObj, "[X:T]", TextPrefab);
        }

        static int _EmissionColor_ID = -1;
        static int _BaseColor_ID = -1;
        static int _Color_ID = -1;
        static UnityEngine.Rendering.LocalKeyword? _EMISSION_LocalKeyWord = null;

        public static void SetMatColor(Renderer rndr, bool isLit, Color color)
        {
            var mat =
#if UNITY_EDITOR
             !Application.isPlaying ? rndr.sharedMaterial :
#endif
             rndr.material;
            if (isLit)
            {
                if (_EMISSION_LocalKeyWord == null)
                    _EMISSION_LocalKeyWord = new UnityEngine.Rendering.LocalKeyword(mat.shader, "_EMISSION");
                if (_EmissionColor_ID == -1)
                    _EmissionColor_ID = Shader.PropertyToID("_EmissionColor");
                if (!mat.IsKeywordEnabled(_EMISSION_LocalKeyWord.Value))
                    mat.SetKeyword(_EMISSION_LocalKeyWord.Value, true);
                mat.SetColor(_EmissionColor_ID, color);
                if (_BaseColor_ID == -1)
                    _BaseColor_ID = Shader.PropertyToID("_BaseColor");
                mat.SetColor(_BaseColor_ID, color);
            }
            else
            {
                if (_Color_ID == -1)
                    _Color_ID = Shader.PropertyToID("_Color");
                mat.SetColor(_Color_ID, color);
            }
        }

        void UpdateLineAndPoint(LinePointCachePack lpCache, Color color, bool hasAnimation, float animationTime, float lineWidth)
        {
            if (lpCache.isDotOnly)
            {
                return;
            }

            if (!lpCache.lineRenderer)
            {
                lpCache.lineRenderer = Instantiate(linePrefab, lpCache.isCreateToObjOnly ? lpCache.toCacheRoot : lpCache.fromCacheRoot);
                lpCache.lineRenderer.gameObject.hideFlags = HideFlags.DontSave;
                lpCache.lineRenderer.startWidth = lineWidth;
                lpCache.lineRenderer.endWidth = lineWidth;
            }

            lpCache.isGeneratedOrAnimDone = false;

            Vector3 curFromPos = lpCache.fromTrf.position;
            Vector3 curToPos = lpCache.toTrf.position;

            if (lpCache.settings.isColorChanged)
            {
                var color1 = new Color(color.r, color.g, color.b, color.a);
                lpCache.lastFromPos = curFromPos;
                lpCache.lastToPos = curToPos;
                //if (linePointIns.capRndr != null)
                //{
                //    SetMatColor(linePointIns.capRndr, false, color1);
                //    linePointIns.capRndr.startColor = color1;
                //    linePointIns.capRndr.endColor = color1;
                //}
                SetMatColor(lpCache.lineRenderer, false, color1);
                lpCache.lineRenderer.startColor = color1;
                lpCache.lineRenderer.endColor = color1;
            }

            if (hasAnimation)
            {
#if UNITY_2023_1_OR_NEWER
                _ = lpCache.IE_UpdatePosWithAnim(animationTime);
#else
                StartCoroutine(lpCache.IE_UpdatePosWithAnim(animationTime));
#endif
                return;
            }
            else // or immediately
            {
                lpCache.UpdatePosNonAnim(curFromPos, curToPos);
            }
            return;
        }

        #endregion Generate

    }
}
