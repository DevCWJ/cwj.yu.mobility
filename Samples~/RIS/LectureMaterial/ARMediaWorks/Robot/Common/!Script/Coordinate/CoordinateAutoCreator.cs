using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

namespace CWJ.YU.Mobility
{
    [SelectionBase]
    public class CoordinateAutoCreator : MonoBehaviour
    {
        /// <summary>
        /// 격자 모델링 바꿀일 생기면 참조로 바꿔야할듯
        /// </summary>
        public const float UnitDistance = 0.103f;

        [DrawHeaderAndLine("Settings")]
        [SerializeField] Color[] coordinateColors;
        [SerializeField] Vector3 childAxisForward = new Vector3(0, 180, 0);
        [SerializeField] float fontSize = 1;
        [SerializeField] float lineWidth = 0.04f;

        [DrawHeaderAndLine("Coordinate Obj")]
        [SerializeField] Transform zeroPointPivot;
        [SerializeField] GameObject prefab_ChildModel;

        [DrawHeaderAndLine("x,y,z Ipf Package")]
        [SerializeField] Transform xyzIpfPackageRoot;
        [SerializeField] XyzIpfPackage prefab_xyzIpfPackage;
        [SerializeField] _Ipf_MinMaxValidator posIpfValidator;
        [SerializeField] _Ipf_MinMaxValidator rotIpfValidator;

        [DrawHeaderAndLine("Cache")]
        [SerializeField] List<ChildCoordinate> childCoordinates = new List<ChildCoordinate>();

        [System.Serializable]
        public class ChildCoordinate
        {
            public int index;
            [SerializeField] bool runtimeInit;
            public Transform mainPointTrf;
            /// <summary>
            /// 회전대상
            /// </summary>
            public MeshRenderer[] axisModelRenderers;
            public Transform axisRotTarget;
            public TextMeshPro mainPointText;
            public LineRenderer xToNormalLine;
            public LineRenderer yToNormalLine;
            public LineRenderer zToNormalLine;
            public XyzIpfPackage posIpfPackage, rotIpfPackage;

            public ChildCoordinate(int index, CoordinateAutoCreator coordinateCreator)
            {
                this.index = index;
                runtimeInit = false;
                Color mainColor;
                if (coordinateCreator.coordinateColors.Length <= index)
                {
                    Debug.LogError($"색상값 설정이 {index}개 보다 적게되어있음.");
                    mainColor = Color.cyan;
                }
                else
                {
                    mainColor = coordinateCreator.coordinateColors[index];
                }

                float rootScaleFactor = 1 / coordinateCreator.transform.lossyScale.z;
                mainPointTrf = CreateNewObj("Coordinate_" + index, coordinateCreator.zeroPointPivot).transform;
                mainPointTrf.localRotation = Quaternion.Euler(TransformUtil.NormalizeEulerAngle(coordinateCreator.childAxisForward));
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(coordinateCreator);
#endif
                string displayIndex = $"({(index + 1)})";
                posIpfPackage = Instantiate(coordinateCreator.prefab_xyzIpfPackage);
                posIpfPackage.transform.SetParentAndReset(coordinateCreator.xyzIpfPackageRoot, false);
                posIpfPackage.Init($"{displayIndex} 좌표계 원점", coordinateCreator.posIpfValidator, mainColor);

                rotIpfPackage = Instantiate(coordinateCreator.prefab_xyzIpfPackage);
                rotIpfPackage.transform.SetParentAndReset(coordinateCreator.xyzIpfPackageRoot, false);
                rotIpfPackage.Init($"{displayIndex} 좌표계 회전", coordinateCreator.rotIpfValidator, mainColor);

                var axisModelObj = Instantiate(coordinateCreator.prefab_ChildModel);
                axisRotTarget = axisModelObj.transform;
                axisRotTarget.SetParentAndReset(mainPointTrf, false);
                axisModelRenderers = axisRotTarget.GetChild(0).GetComponentsInChildren<MeshRenderer>();
                var indexTxt = axisModelObj.GetComponentInChildren<TMP_Text>();
                indexTxt.SetText(displayIndex);
                indexTxt.color = mainColor;


                var fontSize = coordinateCreator.fontSize * rootScaleFactor;
                mainPointText = CreateTextMeshPro("(x,y,z)", Vector3.zero, -0.06f, 1.2f);

                var lineWidth = coordinateCreator.lineWidth * rootScaleFactor;
                var skyBlueColor = new Color().GetOrientalBlue();
                xToNormalLine = CreateLineRenderer("xLine", skyBlueColor);
                yToNormalLine = CreateLineRenderer("yLine", skyBlueColor);
                zToNormalLine = CreateLineRenderer("zLine", skyBlueColor);

                GameObject CreateNewObj(string newObjName, Transform parent)
                {
                    var newGo = new GameObject(newObjName);
                    newGo.transform.SetParent(parent, true);
                    newGo.transform.localPosition = Vector3.zero;
                    newGo.transform.localScale = Vector3.one;
                    newGo.transform.localRotation = Quaternion.identity;
                    return newGo;
                }

                LineRenderer CreateLineRenderer(string name, Color color)
                {
                    var lineObj = CreateNewObj(name, mainPointTrf);
                    LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                    lineRenderer.startWidth = lineWidth;
                    lineRenderer.endWidth = lineWidth;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startColor = color;
                    lineRenderer.endColor = color;
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, Vector3.zero);
                    lineRenderer.useWorldSpace = false;
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.sortingOrder = 1;
                    lineRenderer.enabled = false;
                    return lineRenderer;
                }

                TextMeshPro CreateTextMeshPro(string txt, Vector3 localPosition, float pivotX, float pivotY)
                {
                    var textObj = CreateNewObj(txt, mainPointTrf);
                    var textMeshPro = textObj.AddComponent<TextMeshPro>();
                    var sizeFitter = textObj.AddComponent<ContentSizeFitter>();
                    var lookAtCam = textObj.AddComponent<LookAtCam>();
                    textMeshPro.fontSize = fontSize;
                    textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
                    textMeshPro.color = Color.black;
                    textMeshPro.SetText(txt);
                    textMeshPro.sortingOrder = 1;
                    sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    sizeFitter.SetLayoutHorizontal();
                    sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    sizeFitter.SetLayoutVertical();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        var textRectTrf = textObj.GetComponent<RectTransform>();
                        textRectTrf.pivot = new Vector2(pivotX, pivotY);
                        textRectTrf.anchoredPosition = localPosition;
                        //textRectTrf.SetPivotWithoutMoving(Vector2.one * 0.5f);
                    };
#endif
                    return textMeshPro;
                }
            }

            public void Init()
            {
                if (runtimeInit || !Application.isPlaying)
                {
                    return;
                }
                runtimeInit = true;
                posIpfPackage.SubscribeChangeVector3(SetPosition);
                rotIpfPackage.SubscribeChangeVector3(SetRotation);
            }



            void UpdateCurPosToUI()
            {
                var localPos = mainPointTrf.localPosition;
                var usersidePos = Extension.UnityPosConvertToUsersidePos(localPos, UnitDistance);
                mainPointText.SetText($"({usersidePos.x:F1}, {usersidePos.y:F1}, {usersidePos.z:F1})");
                UpdateLineRenderer(xToNormalLine, localPos.z, Vector3.forward);
                UpdateLineRenderer(yToNormalLine, localPos.x, Vector3.right);
                UpdateLineRenderer(zToNormalLine, localPos.y, -Vector3.up);

                bool UpdateLineRenderer(LineRenderer lineRenderer, float targetPos, Vector3 dir)
                {
                    if (lineRenderer.enabled = (targetPos != 0))
                    {
                        lineRenderer.SetPosition(1, dir * targetPos);
                        return true;
                    }
                    return false;
                }
            }

            void OnCompletePosUpdate(Vector3 targetLocalPos)
            {
                lastPosSequence = null;
                mainPointTrf.localPosition = targetLocalPos;
                UpdateCurPosToUI();
            }

            Sequence lastRotSequence, lastPosSequence;
            public void SetPosition(Vector3 usersidePos) { SetPosition(usersidePos, true); }
            public void SetPosition(Vector3 usersidePos, bool useTween)
            {
                lastPosSequence?.Kill();

                if (!mainPointTrf.gameObject.activeSelf)
                    mainPointTrf.gameObject.SetActive(true);

                Vector3 willLocalPosition = Extension.UsersidePosConvertToUnityPos(usersidePos, UnitDistance);

                if (useTween && Application.isPlaying)
                {
                    lastPosSequence = DOTween.Sequence().SetTarget(this)
                                             .SetAutoKill(true)
                                             .Append(mainPointTrf.DOLocalMove(willLocalPosition, 2))
                                             .SetEase(Ease.InOutQuad)
                                             .OnUpdate(UpdateCurPosToUI)
                                             .OnComplete(() =>
                                             {
                                                 OnCompletePosUpdate(willLocalPosition);
                                             });
                }
                else
                {
                    OnCompletePosUpdate(willLocalPosition);
                }

                //Debug.LogError(inputVector.ToStringByDetailed());
            }

            Vector3 UsersideRotConvertToUnityRot(Vector3 usersideRot)
            {
                float unitySpaceX = usersideRot.y;
                float unitySpaceY = usersideRot.z;
                float unitySpaceZ = usersideRot.x;

                return new Vector3(unitySpaceX, unitySpaceY, unitySpaceZ);
            }

            public void SetRotation(Vector3 usersideRot) { SetRotation(usersideRot, true); }
            public void SetRotation(Vector3 usersideRot, bool useTween)
            {
                lastRotSequence?.Kill();

                if (!mainPointTrf.gameObject.activeSelf)
                    mainPointTrf.gameObject.SetActive(true);

                usersideRot = UsersideRotConvertToUnityRot(usersideRot);

                if (useTween && Application.isPlaying)
                {
                    lastRotSequence = DOTween.Sequence().SetTarget(this)
                                             .SetAutoKill(true)
                                             .Append(axisRotTarget.DOLocalRotate(usersideRot, 1, RotateMode.Fast).SetEase(Ease.InOutQuad))
                                             //.Append(axisRotTarget.DOLocalRotate(eulerAngles, 1, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad))
                                             .OnComplete(() =>
                                             {
                                                 lastRotSequence = null;
                                             });
                }
                else
                {
                    axisRotTarget.localRotation = Quaternion.Euler(usersideRot);
                }
            }
        }

        private void Awake()
        {
            for (int i = 0; i < childCoordinates.Count; i++)
            {
                Color color = coordinateColors[i];
                var matInstance = new Material(childCoordinates[i].axisModelRenderers[0].material);
                matInstance.color = color;
                foreach (var renderer in childCoordinates[i].axisModelRenderers)
                {
                    renderer.material = matInstance;
                }
                // 기존 Material의 인스턴스를 생성하고 색상 변경
                childCoordinates[i].Init();
                childCoordinates[i].mainPointTrf.gameObject.SetActive(false);
            }
        }


        bool TryGetCoordinate(int index, out ChildCoordinate childCoordinate)
        {
            if (index < 0)
            {
                Debug.LogError($"{index} 인덱스가 잘못입력됨");
                childCoordinate = null;
                return false;
            }
            else if (index >= childCoordinates.Count)
            {
                int cnt = (index - childCoordinates.Count) + 1;
                for (int i = 0; i < cnt; i++)
                {
                    var newCoordinate = new ChildCoordinate(childCoordinates.Count, this);
                    childCoordinates.Add(newCoordinate);
                        newCoordinate.Init();
                }
            }
            childCoordinate = childCoordinates[index];
            return true;
        }

        [InvokeButton]
        public void UpdateCoordinatePosition(int index, Vector3 inputVector, bool useTween = true)
        {
            if (TryGetCoordinate(index, out var childCoordinate))
                childCoordinate.SetPosition(inputVector, useTween);
        }

        [InvokeButton]
        public void UpdateCoordinateRotation(int index, Vector3 angle, bool useTween = true)
        {
            if (TryGetCoordinate(index, out var childCoordinate))
                childCoordinate.SetRotation(angle, useTween);
        }

        private void OnEnable()
        {
            TestDummyLocation();
        }
        [InvokeButton]
        void TestDummyLocation()
        {
            UpdateCoordinatePosition(0, new Vector3(2, 8, 4));
            UpdateCoordinatePosition(1, new Vector3(3, 2, 0));
        }
        private void OnDisable()
        {
            UpdateCoordinatePosition(0, Vector3.zero);
            UpdateCoordinateRotation(0, Vector3.zero);
            UpdateCoordinatePosition(1, Vector3.zero);
            UpdateCoordinateRotation(1, Vector3.zero);
        }

        [InvokeButton]
        private void DestroyCache()
        {
            var destroyCaches = childCoordinates.ToArray();
            childCoordinates.Clear();
            for (int i = 0; i < destroyCaches.Length; i++)
            {
                if (destroyCaches[i].posIpfPackage != null)
                    DestroyImmediate(destroyCaches[i].posIpfPackage.gameObject);
                if (destroyCaches[i].rotIpfPackage != null)
                    DestroyImmediate(destroyCaches[i].rotIpfPackage.gameObject);
                DestroyImmediate(destroyCaches[i].mainPointTrf.gameObject);
                destroyCaches[i] = null;
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
