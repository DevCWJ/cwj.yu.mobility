using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CWJ
{
    [DisallowMultipleComponent]
    public class UpScaleSampler : MonoBehaviour
    {
        #region Singleton 

        public static UpScaleSampler Instance
        {
            get
            {
                if (instance == null) CheckExsistence();
                return instance;
            }
        }
        public static UpScaleSampler I => Instance;
        private static UpScaleSampler instance;

        private static void CheckExsistence()
        {
            instance = FindObjectOfType<UpScaleSampler>();
            if (instance == null)
            {
                GameObject container = new GameObject("Upscale Sampler");
                instance = container.AddComponent<UpScaleSampler>();
            }
        }

        public static bool IsCreated() => instance != null;

        /// <summary> 
        /// [Awake()���� ȣ��]
        /// <para/> �̱��� ��ũ��Ʈ�� �̸� ������Ʈ�� ��� ����ϴ� ��츦 ���� ����
        /// </summary>
        private void CheckInstance()
        {
            if (instance == null) instance = this;
            else if (instance != this)
            {
                Debug.Log("�̹� UpscaleSampler �̱����� �����ϹǷ� ������Ʈ�� �ı��մϴ�.");
                Destroy(this);
                var components = gameObject.GetComponents<Component>();
                if (components.Length <= 2) Destroy(gameObject);
            }
        }

        private void Awake()
        {
            CheckInstance();
        }

        #endregion // ==================================================================

        #region Upscale Sampler

        [Header("Options")]
        [SerializeField]
        [Tooltip("���� ���� �� ���� ����")]
        private bool _runOnStart = true;

        [SerializeField, Range(0.1f, 1.0f)]
        [Tooltip("���� ���� �� ������ ����")]
        private float _InitialRatio = 1.0f;

        [SerializeField]
        [Tooltip("UI�� �����ϰ� �������� ī�޶�")]
        private Camera _targetCamera;

        [SerializeField]
        [Tooltip("UI�� �������� ī�޶�")]
        private Camera _uiCamera;

        [SerializeField]
        [Tooltip("_targetCamera�� �������� ���� ���, �ڵ����� ���� ������ ī�޶� Ž������ ����")]
        private bool _autoDetectMainCamera = true;

        [SerializeField]
        [Tooltip("���� ������ ī�޶� �޶��� ���, �ڵ� Ž���Ͽ� ����")]
        private bool _autoDetectCameraChange = true;


        [Header("Target UI")]
        [SerializeField]
        [Tooltip("RawImage�� ������ ��� ĵ����")]
        private Canvas _targetCanvas;

        [Header("Editor Options")]
        [SerializeField]
        [Tooltip("����� �α� ��� ���")]
        private bool _allowDebug = true;

        [SerializeField, HideInInspector]
        [Tooltip("���̾��Ű���� �����")]
        private bool _hideInHierarchy = false;

        // Fields
        private int _currentWidth;
        private int _currentHeight;
        private float _currentRatio;
        private bool _initialized = false; // �� ���̶� ����ƴ��� ����
        private RenderTexture _currentRT;
        private UnityEngine.UI.RawImage _rawImage;

        [SerializeField, HideInInspector]
        private Shader _rawImageShader;

        // Static View
        public static float CurrentRatio { get; private set; }

        private void Log(string msg)
        {
            if (!_allowDebug) return;
            Debug.Log($"[Upscale Sampler] {msg}", gameObject);
        }

        private void Reset()
        {
            _rawImageShader = Shader.Find("Unlit/Texture");
            UpScaleSampler[] uss = FindObjectsOfType<UpScaleSampler>();
            if (uss.Length > 1)
            {
                UnityEngine.Debug.LogWarning("Upscale Sampler ������Ʈ�� ���� �� �� �̻� �����մϴ�.", this);
            }
        }

        private void Start()
        {
            _currentRatio = CurrentRatio = 1f;

            if (_runOnStart)
            {
                Run(_InitialRatio);
            }
        }
        private void OnEnable()
        {
            StopCoroutine(nameof(DetectCameraChangeRoutine));
            StartCoroutine(nameof(DetectCameraChangeRoutine));
        }

        private IEnumerator DetectCameraChangeRoutine()
        {
            while (true)
            {
                if ((_initialized && _autoDetectCameraChange) &&
                    (_targetCamera == null || _targetCamera.enabled == false || _targetCamera.gameObject.activeInHierarchy == false))
                {
                    _targetCamera = null;
                    bool flag = Run(_currentRatio, forceRun: true);
                    if (flag)
                    {
                        Log("Camera Change Auto Detected");
                    }
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        private void OnDestroy()
        {
            ReleaseRT();
        }

        // forceRun : ���� ���� ���� ���� ���� ����
        public bool Run(float ratio, bool forceRun = false)
        {
            if (ratio < 0.1f) ratio = 0.1f;
            if (ratio > 1.0f) ratio = 1.0f;

            int sourceW = Screen.width;
            int sourceH = Screen.height;
#if UNITY_EDITOR
            (sourceW, sourceH) = GetMainGameViewSize();
#endif
            int w = (int)(sourceW * ratio);
            int h = (int)(sourceH * ratio);

            if (!forceRun && _currentWidth == w && _currentHeight == h)
            {
                Log($"������ �����մϴ�. - {w}x{h} ({ratio})");
                return false;
            }

            ReleaseRT();
            if (!CreateRT(w, h)) return false;
            SetCamera();
            SetRawImage();
            HideFromHierarchy();

            _currentWidth = w;
            _currentHeight = h;
            _currentRatio = ratio;
            CurrentRatio = ratio;
            Log($"Screen: {sourceW}x{sourceH} / Sampled: {w}x{h} ({ratio * 100:F2}%)");

            _initialized = true;
            return true;
        }

        private bool CreateRT(int w, int h)
        {
            _currentRT = new RenderTexture(w, h, 24, RenderTextureFormat.DefaultHDR);
            _currentRT.Create();

            if (_autoDetectMainCamera)
            {
                if (_targetCamera == null) _targetCamera = Camera.main;
                NoUiCam();
                if (_targetCamera == null) _targetCamera = Camera.current;
                NoUiCam();
                if (_targetCamera == null) _targetCamera = FindObjectOfType<Camera>();
                NoUiCam();
            }
            if (_targetCamera == null)
            {
                Log("Ÿ�� ī�޶� ã�� �� �����ϴ�.");
                return false;
            }

            return true;

            // --
            void NoUiCam()
            {
                if (_targetCamera != null && _targetCamera == _uiCamera)
                    _targetCamera = null;
            }
        }

        /// <summary> Ÿ�� ī�޶�, UI ī�޶� ���� </summary>
        private void SetCamera()
        {
            int uiLayerMask = 1 << LayerMask.NameToLayer("UI");

            _targetCamera.targetTexture = _currentRT;
            _targetCamera.cullingMask &= ~uiLayerMask; // UI ���̾ ����

            if (_uiCamera == null)
            {
                GameObject uiCamGo = new GameObject("UI Only Camera");
                _uiCamera = uiCamGo.AddComponent<Camera>();
                _uiCamera.targetDisplay = _targetCamera.targetDisplay;
                _uiCamera.clearFlags = CameraClearFlags.Nothing;
                _uiCamera.cullingMask = uiLayerMask;
            }
        }

        /// <summary> ���� Ÿ���� RawImage�� ���� </summary>
        private void SetRawImage()
        {
            if (_targetCanvas == null)
            {
                GameObject canvasGo = new GameObject("Upscale Sample Target Canvas");
                _targetCanvas = canvasGo.AddComponent<Canvas>();
                _targetCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _targetCanvas.sortingOrder = -10000;
            }
            if (_rawImage == null)
            {
                GameObject rawImageGo = new GameObject("Upscale Sample Target RawImage");
                rawImageGo.transform.SetParent(_targetCanvas.transform);

                _rawImage = rawImageGo.AddComponent<UnityEngine.UI.RawImage>();
                _rawImage.raycastTarget = false;
                _rawImage.maskable = false;

                // �⺻ ���׸��� �Ҵ�
                _rawImage.material = new Material(_rawImageShader);

                RectTransform rect = _rawImage.rectTransform;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
#if UNITY_EDITOR
                ToggleSceneVisibility(_targetCanvas.gameObject);
#endif
            }
            _rawImage.texture = _currentRT;
        }

        private void ReleaseRT()
        {
            if (_currentRT != null)
            {
                _currentRT.Release();
            }
        }

        private void HideFromHierarchy()
        {
            if (_hideInHierarchy == false || gameObject.hideFlags == HideFlags.HideInHierarchy) return;
            gameObject.hideFlags =
            _targetCanvas.gameObject.hideFlags =
            _uiCamera.gameObject.hideFlags = HideFlags.HideInHierarchy;
            Log("���̾��Ű���� ����ó�� �Ǿ����ϴ�.");
        }

        #endregion

        #region Editor Only
#if UNITY_EDITOR
        private float _editorRatio = 1f;
        private static System.Reflection.MethodInfo GetSizeOfMainGameViewMi;

        // Ŀ���� �����Ϳ��� Screen.width, height�� �����ϸ� ���� ���� �ػ󵵸� �������� ���ϹǷ� ������ ��ũ��Ʈ Ȱ��
        private static (int x, int y) GetMainGameViewSize()
        {
            if (GetSizeOfMainGameViewMi == null)
            {
                System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
                GetSizeOfMainGameViewMi = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            }
            System.Object res = GetSizeOfMainGameViewMi.Invoke(null, null);
            Vector2 resVec = (Vector2)res;
            return ((int)resVec.x, (int)resVec.y);
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void ToggleSceneVisibility(GameObject target)
        {
            UnityEditor.SceneVisibilityManager.instance.DisablePicking(target, true);
            UnityEditor.SceneVisibilityManager.instance.Hide(target, true);
        }
#endif

        #endregion

        #region Custom Editor
#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(UpScaleSampler))]
        private class CE : UnityEditor.Editor
        {
            private UpScaleSampler t;

            private void OnEnable()
            {
                if (t == null) t = target as UpScaleSampler;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorGUI.BeginChangeCheck();
                    t._hideInHierarchy = UnityEditor.EditorGUILayout.Toggle("Hide in Hierarchy", t._hideInHierarchy);
                    if (UnityEditor.EditorGUI.EndChangeCheck())
                    {
                        UnityEditor.EditorUtility.SetDirty(t);
                    }
                }

                using (new UnityEditor.EditorGUI.DisabledGroupScope(true))
                {
                    t._rawImageShader = (Shader)UnityEditor.EditorGUILayout.ObjectField("Raw Image Shader", t._rawImageShader, typeof(Shader), allowSceneObjects: false);
                    if (t._rawImageShader == null)
                    {
                        t._rawImageShader = Shader.Find("Unlit/Texture");
                    }
                    UnityEditor.EditorGUILayout.FloatField("Current Ratio", t._currentRatio);
                }
                UnityEditor.EditorGUILayout.Space(8f);

                if (Application.isPlaying == false) return;

                t._editorRatio = Mathf.Max(0.1f, Mathf.Round(t._editorRatio * 100f) * 0.01f);
                t._editorRatio = UnityEditor.EditorGUILayout.Slider("New Ratio", t._editorRatio, 0.1f, 1f);
                if (GUILayout.Button("Change Ratio"))
                {
                    t.Run(t._editorRatio);
                }
                UnityEditor.EditorGUILayout.Space(8f);

                using (new UnityEditor.EditorGUILayout.HorizontalScope())
                {
                    DrawApplyButton(0.25f);
                    DrawApplyButton(0.50f);
                    DrawApplyButton(0.75f);
                    DrawApplyButton(1.00f);
                }
                using (new UnityEditor.EditorGUILayout.HorizontalScope())
                {
                    DrawApplyButton(0.2f);
                    DrawApplyButton(0.4f);
                    DrawApplyButton(0.6f);
                    DrawApplyButton(0.8f);
                    DrawApplyButton(1.0f);
                }
                using (new UnityEditor.EditorGUILayout.HorizontalScope())
                {
                    for (float f = 0.1f; f < 1.01f; f += 0.1f)
                        DrawApplyButton2(f);
                }
            }

            private void DrawApplyButton(float ratio)
            {
                if (GUILayout.Button($"{ratio:F2}"))
                {
                    t.Run(ratio);
                }
            }

            private void DrawApplyButton2(float ratio)
            {
                if (GUILayout.Button($"{ratio:F1}"))
                {
                    t.Run(ratio);
                }
            }
        }
#endif
        #endregion
    }
}