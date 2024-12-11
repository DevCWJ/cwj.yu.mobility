using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{

    public static class CustomAttributeHandler
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += (scene) => OnEditorSceneOpened(); //씬 열 때 + 프로젝트 열 때

            CWJ_EditorEventHelper.EditorSceneClosedEvent += (scene) => OnEditorSceneClosed(); //씬 닫을 때

            CWJ_EditorEventHelper.ReloadedScriptEvent += OnReloadedScript; //스크립트 컴파일될때

            CWJ_EditorEventHelper.EditorWillSaveEvent += OnEditorWillSaveScene;//유니티에디터 저장 시도를 할 때마다 실행

            CWJ_EditorEventHelper.PlayModeStateChangedEvent += OnPlayModeStateChanged;

            BuildEventSystem.BeforeBuildEvent += OnBeforeBuild;
        }
        public delegate void AttributeEvent(MonoBehaviour component, Type type);

        public static event AttributeEvent EditorSceneClosedEvent;

        public static event AttributeEvent EditorSceneOpenedEvent;

        public static event AttributeEvent ReloadedScriptEvent;

        public static event AttributeEvent EditorWillSaveAfterModifiedEvent;

        /// <summary>
        /// Before enter playmode
        /// </summary>
        public static event AttributeEvent ExitingEditModeEvent;

        public static event AttributeEvent BeforeBuildEvent;

        private static MonoBehaviour[] AllMonoBehaviours = null;
        private static void CallbackAllMonoBehaviours(AttributeEvent callbackEvent, bool isNeedInit = false)
        {
            if (callbackEvent == null) return;

            if (!isNeedInit)
            {
                int sceneLength = UnityEngine.SceneManagement.SceneManager.sceneCount;
                for (int i = 0; i < sceneLength; i++)
                {
                    if (SceneManager.GetSceneAt(i).isDirty)
                    {
                        isNeedInit = true;
                        break;
                    }
                }
            }

            if (isNeedInit || AllMonoBehaviours == null)
            {
                AllMonoBehaviours = FindUtil.FindObjectsOfType_New<MonoBehaviour>
                    (true, (Application.isPlaying ? true : false), predicate: (m => m?.GetCustomType() != null));
            }

            for (int i = 0; i < AllMonoBehaviours.Length; i++)
            {
                if (AllMonoBehaviours[i] == null) continue;
                callbackEvent.Invoke(AllMonoBehaviours[i], AllMonoBehaviours[i].GetType());
            }
        }

        #region Callback
        private static void OnEditorSceneOpened()
        {
            CallbackAllMonoBehaviours(EditorSceneOpenedEvent, true);
        }
        private static void OnEditorSceneClosed()
        {
            CallbackAllMonoBehaviours(EditorSceneClosedEvent, true);
        }

        private static void OnReloadedScript()
        {
            CallbackAllMonoBehaviours(ReloadedScriptEvent, true);
        }

        private static void OnEditorWillSaveScene(CWJ_EditorEventHelper.SaveTarget saveTarget, bool isModified)
        {
            if (isModified)
            {
                CallbackAllMonoBehaviours(EditorWillSaveAfterModifiedEvent);
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                CallbackAllMonoBehaviours(ExitingEditModeEvent);
            }
        }

        private static void OnBeforeBuild()
        {
            CallbackAllMonoBehaviours(BeforeBuildEvent);
        }

        #endregion Callback
    }
}