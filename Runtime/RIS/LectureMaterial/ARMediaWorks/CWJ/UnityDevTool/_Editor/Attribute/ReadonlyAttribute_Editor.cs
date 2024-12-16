using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(Root_ReadonlyAttribute), true)]
    public class ReadonlyAttribute_Editor : PropertyDrawer
    {
        private float height = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
            height = info.height;
        }
    }

    [CustomPropertyDrawer(typeof(BeginReadonlyGroupAttribute))]
    public class BeginReadonlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0;

        public override void OnGUI(Rect position)
        {
            if (attribute == null) return;

            var state = (attribute as BeginReadonlyGroupAttribute).callSituation;
            EditorGUI.BeginDisabledGroup(Application.isPlaying ? state.HasFlag(EPlayMode.PlayMode) : state.HasFlag(EPlayMode.NotPlayMode));
        }
    }

    [CustomPropertyDrawer(typeof(EndReadonlyGroupAttribute))]
    public class EndReadonlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0;

        public override void OnGUI(Rect position)
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}