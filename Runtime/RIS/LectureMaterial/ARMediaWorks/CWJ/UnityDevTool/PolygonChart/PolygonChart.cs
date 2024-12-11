using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CWJ.UI
{
    /// <summary>
    /// 다각형 그래프 솔루션
    ///  /
    /// 값을 MaskableGraphic에 적용할땐 PolygonChart.OnRebuildRequested();
    /// </summary>
    [Serializable, DisallowMultipleComponent]
    public class PolygonChart : MaskableGraphic
    {
#if UNITY_EDITOR

        [UnityEditor.MenuItem("GameObject/" + nameof(CWJ) + "/UI/" + nameof(PolygonChart), false, 10)]
        public static void CreatePolygonChart()
        {
            GameObject newObj = new GameObject("CWJ_" + nameof(PolygonChart), typeof(PolygonChart));
        }

        protected override void Reset()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("GameObject/UI/Image");

            Action editorCallback = () =>
            {
                Transform tempTrf = UnityEditor.Selection.activeTransform;
                transform.SetParent(tempTrf);
                transform.Reset();

                transform.SetParent(tempTrf.parent, true);

                DestroyImmediate(tempTrf.gameObject);
                UnityEditor.Selection.activeTransform = transform;
                CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<PolygonChart>(nameof(PolygonChart) + " 생성 완료");

                UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
                gameObject.name = nameof(PolygonChart);
            };

            Func<bool> IsSelectionGetImage = () =>
            {
                return UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.GetComponent<Image>();
            };
            CWJ.AccessibleEditor.EditorCallback.AddWaitForPredicateCallback(editorCallback, IsSelectionGetImage, 5);
        }

#endif

        /// <summary>
        /// 각 갯수
        /// </summary>
        [Range(3, 360), Tooltip("각 갯수")]
        public int axisLength = 3;

        /// <summary>
        /// value배열에 값이 없을시 정해지는 초기화 값
        /// </summary>
        [Range(0, 1), Tooltip(nameof(values) + "에 값이 없을시 정해지는 초기화 값")]
        public float defaultValue = 1;

        [Range(0, 1)]
        public float[] values = new float[0];

        public bool isValueVisualized;
        public float valueOffset;
        public List<RectTransform> valueTrfList = new List<RectTransform>();

        //접근은 raycastTarget으로 하기
        public bool _raycastTarget;

        public override bool raycastTarget { get => base.raycastTarget; set => _raycastTarget = base.raycastTarget = value; }

        /// <summary>
        /// 표면 얼마나 채워넣을것인지 1이 최대
        /// </summary>
        [Range(0, 1)]
        public float fillAmount = 0.5f;

        public Texture fillTexture = null;
        public override Texture mainTexture => fillTexture == null ? s_WhiteTexture : fillTexture;

        //접근은 material로 하기
        public Material _material;

        public override Material material { get => base.material; set { _material = base.material = value; UpdateMaterial(); } }

        //접근은 color로 하기
        public Color _color = Color.black;

        public override Color color { get => base.color; set => _color = base.color = value; }

        /// <summary>
        /// 회전값 주기
        /// </summary>
        [Range(0, 359.9f)]
        public float turnOffset = 0;

        /// <summary>
        /// 라인 그려줄지
        /// </summary>
        public bool onDrawCenterLine = true;

        public Color lineColor = Color.gray;
        public float lineWidth = 1f;
        public float lineDist = 50;

        [HideInInspector] public bool prev_onValueMaxLineDist = true;

        /// <summary>
        /// 라인 길이를 value의 최대값(1)로 설정
        /// </summary>
        public bool isLineDistMaxValue = true;

        [HideInInspector] public bool prev_onSyncValueLineDist = false;

        public bool isLineDistSyncValue = false;

        public UnityEvent onValueChanged = new UnityEvent();

        //접근은 onCullStateChanged로 하기
        public CullStateChangedEvent _onCullStateChanged = new CullStateChangedEvent();

        /// <summary>
        /// 그래픽 Update
        /// SetAllDirty보다 빠름
        /// </summary>
        /// <param name="values"></param>
        /// <param name="_isMaterialChange"></param>
        public void UpdatePolygon(float[] values = null)
        {
            if (values != null)
            {
                this.values = values;
            }
            UpdateGeometry();
        }

        public void SetRaycastTarget(bool isRayCastTarget)
        {
            raycastTarget = _raycastTarget = isRayCastTarget;
        }

        protected UIVertex[] ConvertUIVertex(Vector2[] vertexs, Vector2[] uvs, bool customColor = false)
        {
            UIVertex[] returnVertex = new UIVertex[4];

            for (int i = 0; i < vertexs.Length; ++i)
            {
                var vert = UIVertex.simpleVert;
                vert.color = customColor ? lineColor : color;
                vert.position = vertexs[i];
                vert.uv0 = uvs[i];
                returnVertex[i] = vert;
            }
            return returnVertex;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 rectHalfSize = GetComponent<RectTransform>().rect.size / 2;

            Vector2[] uvs = new Vector2[] { new Vector2(0, 0),
                                                new Vector2(0, 1),
                                                new Vector2(1, 0),
                                                new Vector2(1, 1)};

            Vector2[] fillPoints = new Vector2[axisLength];
            Vector2[] width1 = new Vector2[axisLength];
            Vector2[] width2 = new Vector2[axisLength];
            Vector2[] points = new Vector2[axisLength];

            Vector2[] graphValuePoint = new Vector2[axisLength + 1];

            if (isValueVisualized) //값 위치오브젝트 생성/제거
            {
                int valueTrfListCnt = valueTrfList.Count;

                if (valueTrfListCnt > axisLength)
                {
                    for (int i = axisLength; i < valueTrfListCnt; i++)
                    {
                        if (valueTrfList[axisLength] != null)
                        {
                            DestroyImmediate(valueTrfList[axisLength].gameObject);
                        }
                        valueTrfList.RemoveAt(axisLength);
                    }
                }
                else if (valueTrfListCnt < axisLength)
                {
                    for (int i = valueTrfListCnt; i < axisLength; i++)
                    {
                        GameObject childObj = new GameObject("value_" + i, typeof(RectTransform));
#if UNITY_EDITOR
                        childObj.SetShapeIcon(TNRD.Utilities.ShapeIcon.CircleRed);
#endif
                        childObj.transform.SetParent(transform, false);
                        RectTransform rectTransform = childObj.GetComponent<RectTransform>();
                        valueTrfList.Add(rectTransform);
                    }
                }
            }
            else
            {
                int childCnt = transform.childCount;

                for (int i = 0; i < childCnt; i++)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }

                valueTrfList.Clear();
            }

            for (int i = 0; i < axisLength; i++)
            {
                float rotAngle = turnOffset + (360 / (float)axisLength) * i;
                float cos = Mathf.Cos(rotAngle * Mathf.Deg2Rad);
                float sin = Mathf.Sin(rotAngle * Mathf.Deg2Rad);
                Vector2 centerPos = new Vector2(cos * rectHalfSize.x, sin * rectHalfSize.y);

                float dist = (i >= values.Length ? defaultValue : values[i]);
                graphValuePoint[i] = centerPos * dist;

                if (isValueVisualized)
                {
                    valueTrfList[i].anchoredPosition = centerPos * (dist + valueOffset);
                }

                if (fillAmount < 1)
                {
                    float compareValue = (i >= values.Length) ? defaultValue : values[i];
                    if (fillAmount < compareValue)
                    {
                        fillPoints[i] = graphValuePoint[i] - centerPos * fillAmount;
                    }
                    else
                    {
                        fillPoints[i] = Vector2.zero;
                    }
                }

                if (onDrawCenterLine)
                {
                    if (isLineDistMaxValue)
                    {
                        points[i] = centerPos;
                    }
                    else if (isLineDistSyncValue)
                    {
                        points[i] = graphValuePoint[i];
                    }
                    else
                    {
                        points[i] = new Vector2(cos * lineDist, sin * lineDist);
                    }

                    width1[i] = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (rotAngle + 90)) * lineWidth, Mathf.Sin(Mathf.Deg2Rad * (rotAngle + 90)) * lineWidth);
                    width2[i] = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (rotAngle - 90)) * lineWidth, Mathf.Sin(Mathf.Deg2Rad * (rotAngle - 90)) * lineWidth);
                }
            }

            graphValuePoint[axisLength] = Vector2.zero;

            for (int i = 0; i < axisLength; i++)
            {
                if (axisLength != i + 1)
                {
                    if (fillAmount < 1)
                    {
                        vh.AddUIVertexQuad(ConvertUIVertex(new[] { graphValuePoint[i], graphValuePoint[i + 1], fillPoints[i + 1], fillPoints[i] }, uvs));
                    }
                    else
                    {
                        vh.AddUIVertexQuad(ConvertUIVertex(new[] { graphValuePoint[i], graphValuePoint[i + 1], graphValuePoint[axisLength], graphValuePoint[axisLength] }, uvs));
                    }
                }
                else
                {
                    if (fillAmount < 1)
                    {
                        vh.AddUIVertexQuad(ConvertUIVertex(new[] { graphValuePoint[i], graphValuePoint[0], fillPoints[0], fillPoints[i] }, uvs));
                    }
                    else
                    {
                        vh.AddUIVertexQuad(ConvertUIVertex(new[] { graphValuePoint[i], graphValuePoint[0], graphValuePoint[axisLength], graphValuePoint[axisLength] }, uvs));
                    }
                }
            }

            if (onDrawCenterLine)
            {
                //이거 위의 for문에 넣으면 안됨 (새로 for문 만드는 이유가 도형위에 라인 그릴려고하는거임)
                for (int i = 0; i < axisLength; i++)
                {
                    vh.AddUIVertexQuad(ConvertUIVertex(new[] { points[i] + width1[i], points[i] + width2[i], graphValuePoint[axisLength] + width2[i], graphValuePoint[axisLength] + width1[i] }, uvs, true));
                }
            }
            onValueChanged?.Invoke();
        }
    }
}