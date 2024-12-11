using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttribute_Editor : PropertyDrawer
    {
        float height = 0;
        bool isValidType = true;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return isValidType ? EditorGUI.GetPropertyHeight(property, label, true) : height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            isValidType = property.propertyType == SerializedPropertyType.Integer;
            if (!isValidType)
            {
                typeof(LayerAttribute).DrawHelpBox("Attribute can only be used on int fields", MessageType.Error, logToConsole: false);
                var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
                height = info.height;
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}