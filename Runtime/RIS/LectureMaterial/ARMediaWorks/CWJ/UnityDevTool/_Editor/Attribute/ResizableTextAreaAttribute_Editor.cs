using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;
using System.Reflection;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(ResizableTextAreaAttribute))]
    public class ResizableTextAreaAttribute_Editor : PropertyDrawer_CWJ
    {

        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label);

                EditorGUI.BeginChangeCheck();

                string textAreaValue = EditorGUILayout.TextArea(property.stringValue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f));

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = textAreaValue;
                }
                return true;
            }
            else
            {
                EditorGUILayout.HelpBox(nameof(ResizableTextAreaAttribute) + " can only be used on string", MessageType.Warning);
                EditorGUI.PropertyField(position, property, label, true);
                return false;
            }
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return isVisible ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }
    }
}