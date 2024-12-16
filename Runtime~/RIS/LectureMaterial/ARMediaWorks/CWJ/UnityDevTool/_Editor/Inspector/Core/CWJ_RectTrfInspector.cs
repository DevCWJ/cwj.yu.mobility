using System.Reflection;

using UnityEngine;
using UnityEditor;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    [CustomEditor(typeof(RectTransform)), CanEditMultipleObjects]
    public class CWJ_RectTrfInspector : Editor
    {
        private const string TypeName_RectTransformInspector = "RectTransformEditor";
        private const string MethodName_OnSceneGUI = "OnSceneGUI";

        Editor builtInEditor;
        MethodInfo onSceneGUI;

        private void OnEnable()
        {
            builtInEditor = CreateEditor(targets, ReflectionUtil.GetUnityEditorClassType(TypeName_RectTransformInspector));
            onSceneGUI = builtInEditor.GetType().GetMethod(MethodName_OnSceneGUI, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private void OnDisable()
        {
            DestroyImmediate(builtInEditor);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI_CWJ_TransformExtension.DrawSibilingIndex(targets, target as RectTransform, ref lastSiblingTxt);

            builtInEditor.OnInspectorGUI();
        }

        string lastSiblingTxt = null;

        private void OnSceneGUI()
        {
            onSceneGUI?.Invoke(builtInEditor, null);
        }
    }
}
