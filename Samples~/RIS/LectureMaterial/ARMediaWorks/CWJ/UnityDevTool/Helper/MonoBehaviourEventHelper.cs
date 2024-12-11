using CWJ.Singleton;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    [UnityEngine.DefaultExecutionOrder(-32000)]
    public class MonoBehaviourEventHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        public static bool Editor_IsManagedByEditorScript = false;
        public static bool Editor_IsSilentlyCreateInstance = false;

        [UnityEditor.InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
            CWJ.AccessibleEditor.CWJ_EditorEventHelper.ProjectOpenEvent += EditorEventSystem_ProjectOpenEvent;
        }

        private static void EditorEventSystem_ProjectOpenEvent()
        {
            try
            {
                CWJ.AccessibleEditor.ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<MonoBehaviourEventHelper>(-32000);
                CWJ.AccessibleEditor.ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<LastCloseObject>(32000);
            }
            finally
            {
            }
        }

        protected static void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                IS_QUIT = true;
                IS_PLAYING = false;
            }
            if (state == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                IS_QUIT = false;
                IS_PLAYING = false;
            }
        }
#endif


        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnAfterAssembliesLoaded()
        {
            IS_PLAYING = true;
        }
        //private static void OnAfterSceneLoad()
        //{
        //    UpdateInstance(false);
        //}

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

        static LastCloseObject lastCloseObject = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            if (lastCloseObject == null)
            {
                lastCloseObject = new GameObject(nameof(LastCloseObject)).AddComponent<LastCloseObject>();
                lastCloseObject.quitEvent.AddListener(InvokeStaticQuitEvent);
                lastCloseObject.lastQuitEvent.AddListener(InvokeStaticLastQuitEventEvent);
            }
        }

        //public static event System.Action AwakeEvent;
        //public static event System.Action StartEvent;

        public static event System.Action QuitEvent;
        public static event System.Action LastQuitEvent;
        static void InvokeStaticQuitEvent()
        {
            IS_QUIT = true;
            QuitEvent?.Invoke();
        }
        static void InvokeStaticLastQuitEventEvent()
        {
            IS_QUIT = true;
            LastQuitEvent?.Invoke();
        }

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
        //

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
