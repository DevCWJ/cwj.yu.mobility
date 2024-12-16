#if UNITY_EDITOR && CWJ_EXISTS_RUNTIMEDEBUGGING
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.RuntimeDebugging
{
    [CanEditMultipleObjects, CustomEditor(typeof(RuntimeDebuggingTool))]
    public class RuntimeDebuggingTool_Inspector : InspectorBehaviour<RuntimeDebuggingTool>
    {
        protected override void DrawInspector()
        {
            if (Target == null) return;
            if (!Target.gameObject.scene.IsValid() || !Target.gameObject.scene.isLoaded) return;

#if CWJ_RUNTIMEDEBUGGING_DISABLED
            if (!EditorApplication.isPlaying)
            {
                if (Target.SetDebuggingEnabled(false))
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(Target);
                }
            }

            EditorGUILayout.HelpBox($"RuntimeDebugging is forced disabled!\nCheck to {nameof(CWJ)}/{nameof(CWJ.AccessibleEditor.DebugSetting)} -> 'isRuntimeDebuggingDisabled'", MessageType.Warning);
            //base.DrawInspector();
#else
            if (!EditorApplication.isPlaying)
            {
                if (Target.RollbackDebuggingEnabled())
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(Target);
                }
            }
            base.DrawInspector();
#endif
        }
    }
}
#endif