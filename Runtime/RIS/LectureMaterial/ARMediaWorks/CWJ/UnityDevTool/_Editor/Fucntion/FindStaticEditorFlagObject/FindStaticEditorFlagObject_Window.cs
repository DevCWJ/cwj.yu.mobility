using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace CWJ.AccessibleEditor.Function
{
    public class FindStaticEditorFlagObject_Window : WindowBehaviour<FindStaticEditorFlagObject_Window, FindStaticEditorFlagObject_ScriptableObject>
    {
        public override string GetScriptableFirstName => "Find StaticEditorFlag Objects";

        public const string WindowMenuPath = nameof(CWJ) + "/" + "Find StaticEditorFlag Objects";

        [MenuItem(WindowMenuPath, priority = 99)]
        public static new void Open()
        {
            OnlyOpen(minSize: new Vector2(370, 600));
            ScriptableObj.UpdateStaticFlagObjs();
        }

        private Vector2 scrollPos;

        protected override void _OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.ExpandHeight(true));
            BeginVerticalBox_Outer(true);
            if (ScriptableObj.isVisibleAll = EditorGUILayout.Foldout(ScriptableObj.isVisibleAll, "All", true, EditorGUICustomStyle.Foldout))
            {
                for (int i = 0; i < ScriptableObj.staticFlagStructs.Length; i++)
                {
                    DrawFlagStruct(ref ScriptableObj.staticFlagStructs[i], i);
                }
            }
            EndVerticalBox();
            EditorGUILayout.Space();


            BeginVerticalBox_Outer(true);
            if (ScriptableObj.isVisibleActivate = EditorGUILayout.Foldout(ScriptableObj.isVisibleActivate, "Activate", true, EditorGUICustomStyle.Foldout))
            {
                for (int i = 0; i < ScriptableObj.activateStructs.Length; i++)
                {
                    DrawFlagStruct(ref ScriptableObj.activateStructs[i], i);
                }
            }
            EndVerticalBox();
            EditorGUILayout.Space();

            BeginVerticalBox_Outer(true);
            if (ScriptableObj.isVisibleDeactivate = EditorGUILayout.Foldout(ScriptableObj.isVisibleDeactivate, "Deactivate", true, EditorGUICustomStyle.Foldout))
            {
                for (int i = 0; i < ScriptableObj.deactivateStructs.Length; i++)
                {
                    DrawFlagStruct(ref ScriptableObj.deactivateStructs[i], i);
                }
            }
            EndVerticalBox();
            GUILayout.EndScrollView();
        }

        private void DrawFlagStruct(ref StaticFlagStruct staticFlagStruct, int index)
        {
            int objListCnt = staticFlagStruct.objects.Count;
            BeginVerticalBox_Inner(true);

            if (staticFlagStruct.isVisible = EditorGUILayout.Foldout(staticFlagStruct.isVisible, staticFlagStruct.name + $" ({objListCnt})", true, EditorGUICustomStyle.Foldout))
            {
                GUI.enabled = false;
                for (int j = 0; j < objListCnt; j++)
                {
                    EditorGUILayout.ObjectField(staticFlagStruct.objects[j], typeof(UnityEngine.Object), true);
                }
                GUI.enabled = true;
            }
            EndVerticalBox();
        }

        protected override void _OnEnable()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        protected override void _OnDisable()
        {
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        }

        private void OnHierarchyChanged()
        {
            ScriptableObj.UpdateStaticFlagObjs();
            Repaint();
        }

        protected override void _OnReloadedWhileOpened()
        {
            ScriptableObj.UpdateStaticFlagObjs();
        }

        protected override void _OnSceneOpenedWhileOpened()
        {
            ScriptableObj.UpdateStaticFlagObjs();
        }
    }
}