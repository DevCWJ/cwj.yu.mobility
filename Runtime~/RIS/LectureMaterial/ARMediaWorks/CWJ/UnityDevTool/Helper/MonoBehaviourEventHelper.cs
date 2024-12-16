using System;
using System.Collections.Generic;
using System.Threading;
using CWJ.AccessibleEditor;
using CWJ.Singleton;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;

#else
using UnityEditor.Experimental.SceneManagement;
#endif
#endif

namespace CWJ
{
    [DefaultExecutionOrder(-32000)]
    public partial class MonoBehaviourEventHelper : SingletonBehaviourDontDestroy<MonoBehaviourEventHelper>, IDontPrecreatedInScene
    {
#if UNITY_EDITOR
        public static bool Editor_IsManagedByEditorScript = false;
        public static bool Editor_IsSilentlyCreateInstance = false;

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            CWJ_EditorEventHelper.ProjectOpenEvent += EditorEventSystem_ProjectOpenEvent;
        }

        private static void EditorEventSystem_ProjectOpenEvent()
        {
            try
            {
                ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<MonoBehaviourEventHelper>(-32000);
                ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<SingletonHelper>(-31999);
                ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<LastCloseObject>(32000);
            }
            catch
            {
            }
        }

        protected static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                IS_QUIT = true;
                IS_PLAYING = false;
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                IS_QUIT = false;
                IS_PLAYING = false;
            }
        }
#endif

        private static int _MainThreadId = -1;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnAfterAssembliesLoaded()
        {
            IS_PLAYING = true;
            _MainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static bool IsMainThread => _MainThreadId == Thread.CurrentThread.ManagedThreadId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            IS_PLAYING = true;
            _Instance = __ImmediatelyCreateForBackendIns();

            var curThread = Thread.CurrentThread;
            if (_MainThreadId == -1 || _MainThreadId != curThread.ManagedThreadId)
                _MainThreadId = curThread.ManagedThreadId;
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        // private static void OnAfterSceneLoad()
        // {
        // }

        static LastCloseObject lastCloseObject = null;


        //#if UNITY_EDITOR
        //        [UnityEditor.InitializeOnLoadMethod]
        //        public static void InitializeOnLoad()
        //        {
        //            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && UnityEditor.EditorApplication.timeSinceStartup < 15)//(play시키지않은상태이며 에디터를 켠지 15초가 안된 때)
        //            {
        //                CWJ.EditorScript.SetScriptExecutionOrder.SetOrder(typeof(ApplicationQuitEvent), 32000);
        //            }
        //        }
        //#endif


        public static event Action       AwakeEvent;
        public static event Action       StartEvent;
        public static event Action<bool> PausedEvent;

        public static event Action QuitEvent;
        public static event Action LastQuitEvent;
        void InvokeStaticQuitEvent() => QuitEvent?.Invoke();
        void InvokeStaticLastQuitEventEvent() => LastQuitEvent?.Invoke();

        public static bool IS_EDITOR =>
#if UNITY_EDITOR
                true;
#else
                false;
#endif

        /// <summary>
        /// 에디터전용 isPlaying임 runtime에서 종료된거 확인은 IS_QUIT
        /// </summary>
        public static bool IS_PLAYING { get; private set; } = false;

        //        public static bool ApplicationIsPlaying
        //        {
        //#if UNITY_EDITOR
        //            get => UnityEditor.EditorApplication.isPlaying;
        //#else
        //            get => Application.isPlaying;
        //#endif
        //        }

        /// <summary>
        /// OnDisabled, OnDestroy와 같은 곳에선 종료시에도 불릴수있기때문에 IsQuit으로 return처리 해주어야함
        /// </summary>
        public static bool IS_QUIT { get; private set; }


        /// <summary>
        /// 실행중 ~ 종료되기전까지 true
        /// DontDestroyOnLoad는 이게 true일때만 가능
        /// </summary>
        public static bool GetIsPlayingBeforeQuit() => IS_PLAYING && !IS_QUIT;

        /// <summary>
        /// 생성혹은 제거가 가능 할 때
        /// </summary>
        public static bool GetIsValidCreateObject() =>
#if UNITY_EDITOR
            (!IS_PLAYING || !IS_QUIT);
#else
        GetIsPlayingBeforeQuit();
#endif


        protected override void _OnApplicationQuit()
        {
            IS_QUIT = true;
        }

        protected override void _Awake()
        {
            if (!lastCloseObject)
            {
                lastCloseObject = new GameObject(nameof(LastCloseObject), typeof(LastCloseObject)).GetComponent<LastCloseObject>();
                lastCloseObject.transform.SetParent(transform);
                lastCloseObject.quitEvent.AddListener(InvokeStaticQuitEvent);
                lastCloseObject.lastQuitEvent.AddListener(InvokeStaticLastQuitEventEvent);
            }

            HideGameObject();
            transform.SetAsLastSibling();
            AwakeEvent?.Invoke();
        }

        protected override void _Start()
        {
            StartEvent?.Invoke();
        }

        void OnApplicationPause(bool isPause)
        {
            PausedEvent?.Invoke(isPause);
        }

        public static bool IsValidGameObject(GameObject go)
        {
            if (!go || !go.scene.IsValid())
                return false;
#if UNITY_EDITOR
            bool        isPrefabObj = false;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage)
            {
                // 현재 편집 중인 프리팹 인스턴스 루트인지 확인
                GameObject prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                isPrefabObj = (prefabInstanceRoot && prefabInstanceRoot.Equals(prefabStage.prefabContentsRoot));
            }
            else
            {
                isPrefabObj = PrefabUtility.IsPartOfAnyPrefab(go);
            }

            if (isPrefabObj)
                return go.scene.IsValid(); //현재 활성화된 씬인지 체크하면 프리팹편집씬에서 불리는일은 없게됨
#endif
            return true;
        }

        public static Coroutine StartCoroutine_Static(System.Collections.IEnumerator coroutine)
        {
            return lastCloseObject.StartCoroutine(coroutine);
        }

        public static Coroutine_New StartNewCoroutine_Static(System.Collections.IEnumerator coroutine, bool isNotStartWhenAlreadyRun,
                                                             UnityAction startAction = null, UnityAction endAction = null)
        {
            if (lastCloseObject.coroutineTrackeds == null)
                lastCloseObject.coroutineTrackeds = new List<Coroutine_New>();

            var ct = new Coroutine_New(lastCloseObject, isNotStartWhenAlreadyRun);
            endAction += () =>
            {
                lastCloseObject.coroutineTrackeds.Remove(ct);
            };
            ct.StartCoroutine(coroutine, startAction, endAction);

            lastCloseObject.coroutineTrackeds.Add(ct);
            return ct;
        }

    }
}
