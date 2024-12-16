using System;
using UnityEngine;
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ
{
    [CustomPropertyDrawer(typeof(_VisualizeConditionalAttribute), true)]
    public class VisualizeConditionalAttribute_Editor : PropertyDrawer
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
}