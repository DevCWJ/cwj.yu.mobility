using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(ErrorIfNullAttribute))]
    public class ErrorIfNullAttribute_Editor : PropertyDrawer
    {
        float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool isError = false;

            if (property.propertyType == SerializedPropertyType.ArraySize)
            {
                isError = property.arraySize == 0;
            }
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                isError = property.boolValue == false;
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                isError = property.intValue == 0;
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                isError = property.floatValue == 0;
            }
            else if (property.propertyType == SerializedPropertyType.LayerMask)
            {
                isError = property.intValue == 0;
            }
            else if (property.propertyType == SerializedPropertyType.String)
            {
                isError = string.IsNullOrEmpty(property.stringValue);
            }
            else if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                isError = property.objectReferenceValue.IsNullOrMissing();
            }
            else
            {
                string message = nameof(ErrorIfNullAttribute) + $": {fieldInfo.Name}({fieldInfo.FieldType}) is not valid type!";
                typeof(ErrorIfNullAttribute).DrawHelpBox(message, MessageType.Error, context: property.GetTargetObject(), true);
            }

            if (isError)
                EditorGUI_CWJ.BeginErrorGUI(EditorGUI_CWJ.IsProSkin ? Color.red : new Color().GetDarkRed());

            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
            height = info.height;

            if (isError)
                EditorGUI_CWJ.EndErrorGUI();
        }
    }
}