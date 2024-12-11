using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttribute_Editor : PropertyDrawer
    {
        float height;
        bool isValidType = true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return isValidType ? EditorGUI.GetPropertyHeight(property, label, true) : height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            isValidType = property.propertyType == SerializedPropertyType.String;
            if (!isValidType)
            {
                typeof(TagAttribute).DrawHelpBox("Attribute can only be used on string fields", MessageType.Error, logToConsole: false);
                var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
                height = info.height;
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}