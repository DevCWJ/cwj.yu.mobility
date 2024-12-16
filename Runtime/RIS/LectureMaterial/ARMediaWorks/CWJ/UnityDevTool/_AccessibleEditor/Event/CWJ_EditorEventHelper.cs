#if UNITY_EDITOR

using System;
using System.Linq;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using CWJ.AccessibleEditor.DebugSetting;
using CWJ.AccessibleEditor.PsMessage;
using CWJ.AccessibleEditor.CustomDefine;
using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    [InitializeOnLoad]
    public class CWJ_EditorEventHelper : UnityEditor.AssetModificationProcessor, UnityEditor.Build.IActiveBuildTargetChanged
    {
        private static CWJEditorHelper_ScriptableObject _EditorHelperObj = null;

        private static CWJEditorHelper_ScriptableObject EditorHelperObj
        {
            get
            {
                if (_EditorHelperObj == null)
                {
                    _EditorHelperObj = ScriptableObjectStore.Instanced.GetScriptableObj<CWJEditorHelper_ScriptableObject>();
                }
                return _EditorHelperObj;
            }
        }

        public static bool TryGetCurPlayModeState(out PlayModeStateChange playModeStateChange)
        {
            if (AccessibleEditorUtil.EditorHelperObj != null)
            {
                playModeStateChange = AccessibleEditorUtil.EditorHelperObj.CurPlayModeState;
                return true;
            }

            playModeStateChange = default(PlayModeStateChange);
            return false;
        }

        private static ProjectGUIDCache_ScriptableObject _ProjectGUIDCacheObj = null;

        private static ProjectGUIDCache_ScriptableObject ProjectGUIDCacheObj
        {
            get
            {
                if (_ProjectGUIDCacheObj == null)
                {
                    _ProjectGUIDCacheObj = ScriptableObjectStore.Instanced.GetScriptableObj<ProjectGUIDCache_ScriptableObject>();
                }
                return _ProjectGUIDCacheObj;
            }
        }

        /// <summary>
        /// EnteredPlayMode: Runtime first frame
        /// <br/>ExitingEditMode: Before enter playmode
        /// </summary>
        public static event Action<PlayModeStateChange> PlayModeStateChangedEvent;

        public static PlayModeStateChange PlayModeState
        {
            get => EditorHelperObj != null ? EditorHelperObj.CurPlayModeState : PlayModeStateChange.EnteredEditMode;
            private set { if (EditorHelperObj != null) EditorHelperObj.CurPlayModeState = value; }
        }

        public static event Action EditorOneFrameEvent;

        /// <summary>
        /// <br/>에디터 로딩완료 이벤트
        /// 보통 스크립트 컴파일이후
        /// </summary>
        public static event Action ReloadedScriptEvent;

        /// <summary>
        /// 빌드도중에만 실행되는 컴파일이후 이벤트
        /// </summary>
        public static event Action ReloadedScriptOnlyDuringBuildEvent;

        /// <summary>
        /// <br/>에디터 로딩완료 직후 한 프레임 뒤에 실행되는 이벤트
        /// <br/>'Exception thrown while invoking [DidReloadScripts] method' 때문
        /// </summary>
        public static event Action ReloadedScriptSafeEvent;

        public enum SaveTarget
        {
            Scene,
            Prefab
        }

        /// <summary>
        /// <br/>저장 시도 할 때 마다 실행되는 이벤트 (통상적으로 ctrl+s 누르면 실행됨)
        /// <br/><see cref="SaveTarget"/> : Scene이 저장된건지 Prefab이 저장된건지
        /// /<see cref="bool"/> : 수정되었는지 dirt여부라서 저장할게있으면 true (isModified 정도라고 생각하면됨)
        /// </summary>
        public static event Action<SaveTarget, bool> EditorWillSaveEvent;

        public static event Action<SaveTarget, string[]> EditorSaveModifiedEvent;



        public static event Action EditorQuitEvent;

        /// <summary>
        /// 프로젝트가 바뀌었을때 이벤트
        /// </summary>
        public static event Action UnityProjectChangedEvent;

        /// <summary>
        /// <br/>씬 전환 시에 실행되는 이벤트
        /// /물론 프로젝트 열때도 실행됨
        /// </summary>
        public static event Action<Scene> EditorSceneOpenedEvent;
        public static event Action<Scene> EditorSceneClosedEvent;

        public static event Action ProjectOpenEvent;

        ///// <summary>
        ///// 하이어라키 이동 뿐만아니라 인스펙터에서 값이 변경되어도 실행됨...
        ///// <br/>단순 컴포넌트/오브젝트 이동, 생성, 제거 이벤트는 아래와 같은 이벤트를 사용할것
        ///// <br/><see cref="AddComponentWithIDEvent"/>, <see cref="TransformHierarchyChangedEvent"/>, <see cref="RemoveComponentWithIDEvent"/>
        ///// </summary>
        //public static event Action HierarchyChangedEvent;

        public static event Action FolderChangedEvent;

        public static event Action UnityDevToolDeleteEvent;

        ///// <summary>
        ///// <see cref="int"/> : Added Component's InstanceID
        ///// </summary>
        //public static Action<int> AddComponentWithIDEvent;

        //public static Action AddComponentEvent;

        //public static Action TransformHierarchyChangedEvent;

        ///// <summary>
        ///// <see cref="int"/> : Removed Component's InstanceID
        ///// </summary>
        //public static Action<int> RemoveComponentWithIDEvent;

        //public static Action RemoveComponentEvent;

        public static void WaitForCompiled(Action action)
        {
            EditorCallback.AddWaitForPredicateNotSafeCallback(action, () => !EditorApplication.isCompiling, 60);
        }

        public static Action<MonoBehaviour, Type> CompSelectedEvent;

        public static Action<int> CompDestroyInEditModeEvent;

        public static Action<BuildTarget, BuildTarget> BuildTargetSwitchedEvent;

        public static string GetEditorPrefs_ProjectInitKey() => VolatileEditorPrefs.GetVolatilePrefsKey_Root("IsProjectOpened");

        public static bool IsProjectOpened => EditorPrefs.GetBool(GetEditorPrefs_ProjectInitKey(), false);

        public static bool IsImportLoading => ProjectGUIDCacheObj == null; //import or delete

        public int callbackOrder => 1;

        static CWJ_EditorEventHelper()
        {
            EditorApplication.update += OnEditorOneFrame;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.quitting += OnEditorQuit;
            EditorSceneManager.sceneOpened += OnEditorSceneOpened;
            EditorSceneManager.sceneClosed += OnEditorSceneClosed;

            //EditorApplication.hierarchyChanged += OnHierarchyChanged;

            EditorApplication.projectChanged += OnFolderChanged;

#if CWJ_EDITOR_DEBUG_ENABLED //Log
            //TransformHierarchyChangedEvent += () => typeof(Editor).PrintLogWithClassName("Transform's hierarchy is changed.", LogType.Log);
            //AddComponentWithIDEvent += (i) => typeof(Editor).PrintLogWithClassName(i + " have been added.", LogType.Log);
            //RemoveComponentWithIDEvent += (i) => typeof(Editor).PrintLogWithClassName(i + " have been removed.", LogType.Log);
            BuildTargetSwitchedEvent += (prevTarget, newTarget) => typeof(Editor).PrintLogWithClassName($"Switched build target to {newTarget}\n(Previous target : {prevTarget})", isPreventStackTrace: true);
#endif
        }

        //유니티 스크립트 컴파일 이후 실행
        [DidReloadScripts(0)]
        private static void OnReloadScript()
        {
            PathUtil.ClearCache();

            if (IsImportLoading)
            {
                if (CheckDeletedOrImporting())
                {
                    return;
                }
                EditorCallback.AddWaitForFrameCallback(OnReloadScript);
                return;
            }

            CheckUnityProjectChanged();

            if (!BuildEventSystem.IsBuilding)
                ReloadedScriptEvent?.Invoke();

            EditorCallback.AddWaitForFrameCallback(OnReloadScriptAddOneFrame);
        }

        private static void OnReloadScriptAddOneFrame()
        {
            if (!BuildEventSystem.IsBuilding)
                ReloadedScriptSafeEvent?.Invoke();
            else
                ReloadedScriptOnlyDuringBuildEvent?.Invoke();
        }

        private static void OnEditorOneFrame()
        {
            EditorApplication.update -= OnEditorOneFrame;
            if (CheckDeletedOrImporting())
            {
                return;
            }

            if (!IsProjectOpened)
            {
                EditorPrefs.SetBool(GetEditorPrefs_ProjectInitKey(), true);

                OnProjectOpened();
            }

            EditorOneFrameEvent?.Invoke();
        }

        private static void OnProjectOpened()
        {
            typeof(Editor).PrintLogWithClassName("Project is opened", LogType.Log, isPreventStackTrace: true);

            DetectFolderChanged.CheckFolderModifiy();

            ProjectOpenEvent?.Invoke();

            CheckUnityProjectChanged();

            OnEditorSceneOpened(SceneManager.GetActiveScene(), OpenSceneMode.Single);
        }

        private static void OnEditorSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (BuildEventSystem.IsBuilding) return;
            EditorSceneOpenedEvent?.Invoke(scene);
        }
        private static void OnEditorSceneClosed(Scene scene)
        {
            if (BuildEventSystem.IsBuilding) return;
            EditorSceneClosedEvent?.Invoke(scene);
        }

        private static void OnEditorQuit()
        {
            EditorQuitEvent?.Invoke();
        }

        private static bool CheckDeletedOrImporting()
        {
            if (IsImportLoading)
            {
                if (string.IsNullOrEmpty(PathUtil.MyAbsolutePath_UnityDevTool)) //deleted
                {
                    OnUnityDevToolDelete();
                }
                return true;
            }
            return false;
        }

        private static void OnFolderChanged()
        {
            PathUtil.ClearCache();

            if (CheckDeletedOrImporting())
            {
                WaitForCompiled(CheckUnityProjectChanged);
                return;
            }

            DetectFolderChanged.CheckFolderModifiy();

            CheckUnityProjectChanged();
            FolderChangedEvent?.Invoke();
        }

        //private static void OnHierarchyChanged()
        //{
        //    HierarchyChangedEvent?.Invoke();
        //}

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            PlayModeStateChangedEvent?.Invoke(state);
        }

        private static void CheckUnityProjectChanged()
        {
            if (ProjectGUIDCacheObj == null || !ProjectGUIDCacheObj.IsUnityProjectChanged())
            {
                return;
            }
            DisplayDialogUtil.DisplayDialog<UnityEditor.Editor>(message: $"Unity Project is changed."/*+"\n\nprevious: '{prevProjectGUID}'\n  current: '{curProjectGUID}'"*/);

            var initializableObjs = Resources.FindObjectsOfTypeAll<Initializable_ScriptableObject>();

            for (int i = 0; i < initializableObjs.Length; i++)
            {
                if (initializableObjs[i].IsAutoReset)
                {
                    initializableObjs[i].OnReset(true);
                }
            }

            DefineSymbolUtil.RemoveSymbolsStack(DefineSymbolUtil.GetCurrentSymbolsToArray(isOnlyCWJDefine: true));
            Resources.UnloadUnusedAssets();

            DetectFolderChanged.InitFolderCache();

            //UnityProjectChangedEvent?.Invoke();
            DefineSymbolUtil.InvokeRegistStackList();
        }

        public static void OnUnityDevToolDelete()
        {
            DefineSymbolUtil.RemoveSymbolsFromAllTargets(true, DefineSymbolUtil.GetCurrentSymbolsToArray(isOnlyCWJDefine: true));
            UnityDevToolDeleteEvent?.Invoke();
        }

        /// <summary>
        /// 이름으로 실행되는 UnityEngine 매직메소드
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private static string[] OnWillSaveAssets(string[] paths)
        {
            bool isModified = (paths != null && paths.Length > 0);

            SaveTarget saveTarget = SaveTarget.Scene;

            if (isModified)
            {
                CheckUnityProjectChanged();

                if (paths.Length == 1 && paths[0].EndsWith(".prefab"))
                {
                    saveTarget = SaveTarget.Prefab;
                }

                EditorSaveModifiedEvent?.Invoke(saveTarget, paths);
            }

            EditorWillSaveEvent?.Invoke(saveTarget, isModified);

            return paths;
        }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget) //플랫폼 타겟 스위칭 후
        {
            BuildTargetSwitchedEvent?.Invoke(previousTarget, newTarget);
        }
    }
}

#endif