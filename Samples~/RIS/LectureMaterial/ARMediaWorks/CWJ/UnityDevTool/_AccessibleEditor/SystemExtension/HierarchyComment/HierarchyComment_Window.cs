#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

using CWJ.AccessibleEditor;

#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace CWJ.EditorOnly.Hierarchy.Comment
{
    public class HierarchyComment_Window : WindowBehaviour<HierarchyComment_Window, HierarchyComment_ScriptableObject>
    {
        public override string GetScriptableFirstName => "Hierarchy Comment";

        public static void Open(GameObject targetObj, bool hasComment, string comment , PrefabStage prefabStage)
        {
            ScriptableObj.ChangeCacheData(targetObj, hasComment, comment, prefabStage);

            if (!IsOpened)
            {
                OnlyOpen(minSize: new Vector2(300, 250));
            }
            else
            {
                Window.Repaint();
            }
        }

        protected override void _OnGUI()
        {
            if (!ScriptableObj.isInit) return;

            if (IsCompiling())
            {
                return;
            }

            if (ScriptableObj.targetObj == null)
            {
                Close();
                return;
            }

            EditorGUILayout.Space();

            GUI.enabled = true;

            BeginVerticalBox_Outer(true);
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Enter the comment of ", EditorGUICustomStyle.LargeBoldLabelStyles);
                EditorGUI.BeginChangeCheck();
                var go = EditorGUILayout.ObjectField(ScriptableObj.targetObj, typeof(GameObject), allowSceneObjects: true) as GameObject;
                if (EditorGUI.EndChangeCheck())
                {
                    if (go != null && !go.Equals(ScriptableObj.targetObj) && go.scene.isLoaded)
                    {
                        ScriptableObj.ChangeCacheData(go);
                    }
                }
                EditorGUILayout.EndHorizontal();


                ScriptableObj.newCommment = EditorGUILayout.TextArea(ScriptableObj.newCommment, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f));

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                using (new EditorGUI.DisabledScope(string.Equals(ScriptableObj.newCommment, ScriptableObj.lastComment)))
                {
                    if (GUILayout.Button("Set comment"))
                    {
                        if (!ScriptableObj.OnSetCommentBtn())
                        {
                            Close();
                            return;
                        }
                    }
                }

                using (new EditorGUI.DisabledScope(!ScriptableObj.hasComment))
                {
                    if (GUILayout.Button("Remove comment"))
                    {
                        if (!ScriptableObj.OnRemoveCommentBtn())
                        {
                            Close();
                            return;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EndVerticalBox();
        }


        protected override void _OnEnable()
        {
            CWJ_EditorEventHelper.EditorSceneClosedEvent += OnEditorSceneClosed;
            CWJ_EditorEventHelper.ReloadedScriptEvent += Close;
        }

        private void OnEditorSceneClosed(Scene scene)
        {
            if (ScriptableObj.targetObj == null || scene.Equals(ScriptableObj.targetObj.scene))
            {
                Close();
            }
        }

        protected override void _OnDisable()
        {
            CWJ_EditorEventHelper.EditorSceneOpenedEvent -= OnEditorSceneClosed;
            CWJ_EditorEventHelper.ReloadedScriptEvent -= Close;
            ScriptableObj.ClearCache();
        }
    }

}
#endif