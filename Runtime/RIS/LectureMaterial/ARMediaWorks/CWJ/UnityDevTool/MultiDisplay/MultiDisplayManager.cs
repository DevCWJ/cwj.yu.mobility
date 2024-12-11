using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using CWJ.Singleton;
using CWJ.AccessibleEditor;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CWJ
{
    public class MultiDisplayManager : SingletonBehaviourDontDestroy<MultiDisplayManager>
    {
        protected override void _Reset()
        {
            Debug.LogError(gameObject.name);
            Debug.LogError("isLoaded:" + gameObject.scene.isLoaded);
            Debug.LogError("isSubScene:" + gameObject.scene.isSubScene);
        }
        //        public const string CWJ_MULTI_DISPLAY = nameof(CWJ_MULTI_DISPLAY);

        //#if !CWJ_MULTI_DISPLAY
        //        [InvokeButton]
        //#endif
        //        private void SetDefineSymbol()
        //        {
        //            DefineSymbolUtil.AddCustomDefineSymbol(CWJ_MULTI_DISPLAY, true);
        //            Ping();
        //        }

        //        protected override void _Reset()
        //        {
        //#if UNITY_EDITOR
        //            Debug.LogError(gameObject.name);
        //            Debug.LogError("isLoaded:" + gameObject.scene.isLoaded);
        //            Debug.LogError("isSubScene:" + gameObject.scene.isSubScene);

        //            SetDefineSymbol();
        //            if (DisplayDialogUtil.DisplayDialog<MultiDisplayManager>("MultiDisplay를 위해 PlayerSetting에서 해상도와 디스플레이 설정을 변경 하시겠습니까?", ok: "변경", cancel: "유지"))
        //            {
        //                PlayerSettings.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        //                PlayerSettings.defaultIsNativeResolution = true;

        //                //PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;
        //                PlayerSettings.resizableWindow = true;
        //                PlayerSettings.visibleInBackground = false;
        //                PlayerSettings.allowFullscreenSwitch = true;
        //            }
        //#endif
        //        }

        //        private const string TAG_MainCamera = "MainCamera";

        //        [Header("Camera"), Tooltip("메인카메라와 서브카메라를 부모직계로 두면 같은 UI를 보길원하는것으로 간주하고, canvas를 함께 볼수있게 됨")]
        //        [Readonly] public bool isSameView = false;

        //        [Readonly] public Camera mainCamera; //mainCamera는 targetDisplay 1
        //        [Readonly] public Camera subUICamera; //subCamera는 targetDisplay 2
        //        [Readonly] [SerializeField] private Camera[] curSceneAllCameras = new Camera[0];

        //        [Header("Canvas")]
        //        [Readonly] [SerializeField] private Canvas[] curSceneAllCanvases = new Canvas[0];

        //        [Readonly] [SerializeField] private Canvas[] staticCanvases = new Canvas[0]; //DontDestroyOnLoad의 캔버스, Awake에서 넣어줘야함

        //        [System.Diagnostics.Conditional(CWJ_MULTI_DISPLAY)]
        //        public void AddStaticCanvas(Canvas canvas)
        //        {
        //            if (ArrayUtil.IsExists(staticCanvases, canvas))
        //            {
        //                return;
        //            }
        //            ArrayUtil.Add(ref staticCanvases, canvas);
        //        }

        //#if CWJ_MULTI_DISPLAY
        //        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //        public static void RuntimeInitAfterSceneLoad()
        //        {
        //            UpdateInstance(false);
        //        }
        //#endif

        //        protected override void _Start()
        //        {
        //#if CWJ_MULTI_DISPLAY
        //            if (Display.displays.Length > 1)
        //            {
        //                Display.displays[1].Activate();
        //            }
        //#endif
        //        }

        //        protected override void _OnEnable()
        //        {
        //#if CWJ_MULTI_DISPLAY

        //            SceneManager.sceneLoaded += SceneFinishedLoading;
        //#endif
        //        }

        //        protected override void _OnDisable()
        //        {
        //#if CWJ_MULTI_DISPLAY

        //            SceneManager.sceneLoaded -= SceneFinishedLoading;
        //#endif
        //        }

        //        private void SceneFinishedLoading(Scene scene, LoadSceneMode mode)
        //        {
        //            UpdateCameraSetting();
        //        }

        //        [System.Diagnostics.Conditional(CWJ_MULTI_DISPLAY)]
        //        public void UpdateCameraSetting()
        //        {
        //            curSceneAllCameras = FindUtil.FindObjectsOfType_New<Camera>(includeInactive: false, includeDontDestroyOnLoadObjs: true);

        //            mainCamera = System.Array.Find(curSceneAllCameras, (cam) => cam.gameObject.activeInHierarchy && cam.enabled && cam.CompareTag(TAG_MainCamera));
        //            Debug.Assert(mainCamera, "Main Camera is Null! (main camera must have 'MainCamera' tag)"); if (mainCamera == null) return;

        //            subUICamera = System.Array.Find(curSceneAllCameras, (cam) => cam.gameObject.activeInHierarchy && cam.enabled && !cam.CompareTag(TAG_MainCamera) && !cam.gameObject.layer.LayerEquals(LayerMask.NameToLayer("UI")));

        //            if (subUICamera == null)
        //            {
        //                mainCamera.targetDisplay = 0;
        //                return;
        //            }

        //            mainCamera.targetDisplay = PlayerPrefs.GetInt(TAG_MainCamera, 0);
        //            subUICamera.targetDisplay = (mainCamera.targetDisplay == 0) ? 1 : 0;

        //            Camera tmpUICam1 = System.Array.Find(curSceneAllCameras, (cam) => cam.gameObject.layer.LayerEquals(LayerMask.NameToLayer("UI"))) ?? mainCamera;
        //            Camera tmpUICam2 = System.Array.Find(curSceneAllCameras, (cam) => cam != tmpUICam1 && cam.gameObject.layer.LayerEquals(LayerMask.NameToLayer("UI"))) ?? subUICamera;

        //            if (tmpUICam2 == null)
        //            {
        //                isSameView = false;
        //                return;
        //            }

        //            if (IsParentAndSamePosCheck(tmpUICam1.transform, tmpUICam2.transform))
        //            {
        //                isSameView = true;

        //                curSceneAllCanvases = FindUtil.FindObjectsOfType_New<Canvas>(includeInactive: true, includeDontDestroyOnLoadObjs: true);

        //                if (curSceneAllCanvases.Length == 0 && staticCanvases.Length == 0) return;

        //                System.Action<Canvas> _OnCanvasSetting = (_canvas) =>
        //                {
        //                    if (!_canvas.gameObject.layer.LayerEquals(LayerMask.NameToLayer("UI")))
        //                    {
        //                        return;
        //                    }
        //                    _canvas.renderMode = RenderMode.ScreenSpaceCamera; // 이걸해주는 이유는 canvas위치를 카메라의 위치에 자동으로 맞춰주기때문.
        //                    _canvas.planeDistance = 0.5f;
        //                    _canvas.worldCamera = tmpUICam1;

        //                    _canvas.renderMode = RenderMode.WorldSpace;
        //                };

        //                for (int i = 0; i < staticCanvases.Length; i++)
        //                {
        //                    _OnCanvasSetting(staticCanvases[i]);
        //                }

        //                for (int i = 0; i < curSceneAllCanvases.Length; i++)
        //                {
        //                    _OnCanvasSetting(curSceneAllCanvases[i]);
        //                }
        //            }

        //            CameraReset();
        //        }

        //        private bool IsParentAndSamePosCheck(Transform trf1, Transform trf2)
        //        {
        //            //return (trf1.position == trf2.position) && (trf1.parent == trf2 || trf2.parent == trf1);
        //            return (trf1.position == trf2.position) && (trf1.rotation == trf2.rotation) && (trf1.root == trf2.root);
        //        }

        //        private void CameraReset()
        //        {
        //            int length = curSceneAllCameras.Length;

        //            foreach (Camera elemCam in (from cam in curSceneAllCameras
        //                                        orderby cam.targetDisplay
        //                                        select cam))
        //            {
        //                elemCam.enabled = false;
        //                elemCam.enabled = true;
        //            }
        //        }

        //        [System.Diagnostics.Conditional(CWJ_MULTI_DISPLAY), ContextMenu(nameof(Remote_ChangeDisplayTarget))]
        //        public void Remote_ChangeDisplayTarget()
        //        {
        //#if CWJ_MULTI_DISPLAY
        //            if (mainCamera == null) return;

        //            mainCamera.targetDisplay = mainCamera.targetDisplay == 0 ? 1 : 0;
        //            PlayerPrefs.SetInt(TAG_MainCamera, mainCamera.targetDisplay);

        //            if (subUICamera != null)
        //            {
        //                subUICamera.targetDisplay = (mainCamera.targetDisplay == 0) ? 1 : 0;
        //            }
        //#endif
        //        }

    }
}