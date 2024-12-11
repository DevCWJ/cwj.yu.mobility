using System;
using UnityEditor;
using UnityEngine;
using CWJ.AccessibleEditor;
using System.Reflection;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagAttribute_Editor : PropertyDrawer,ICustomPropertyDrawer
    {
		private const string _invalidTypeWarning = "Invalid type for EnumFlagsDisplayDrawer on field {0}: EnumFlagsDisplay can only be applied to Enum fields";
        public float height { get; set; } = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Enum targetEnum = (Enum)fieldInfo.GetValue(property.serializedObject.targetObject);

            //EditorGUI.BeginProperty(position, label, property);
            ////EditorGUI.EnumMaskPopup(position, propName, targetEnum);
            //Enum enumNew = EditorGUI.EnumFlagsField(position, ObjectNames.NicifyVariableName(property.name), targetEnum);
            //property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            //EditorGUI.EndProperty();

            //property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            //EditorGUI.GetPropertyHeight(property);

            NewPropertyDrawHandler drawFunc;

            if (property.propertyType == SerializedPropertyType.Enum)
            {
                drawFunc = Draw;
            }
            else
            {
                drawFunc = null;
                Debug.LogWarningFormat(_invalidTypeWarning, property.propertyPath);
            }

            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label, drawFunc: drawFunc);
            height = info.height;
        }

        public (bool isExpanded, float height) Draw(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            return (false
                , EditorGUI.GetPropertyHeight(property, label, includeChildren));
        }
    }

}