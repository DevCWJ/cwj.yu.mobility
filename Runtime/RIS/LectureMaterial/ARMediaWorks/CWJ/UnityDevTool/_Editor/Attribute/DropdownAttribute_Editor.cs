using System;

using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownAttribute_Editor : PropertyDrawer
    {
        private Action<int> setValue;
        private Func<int, int> validateValue;
        private string[] dropdownNames = null;

        private string[] DropdownNames
        {
            get
            {
                if (dropdownNames == null)
                {
                    dropdownNames = new string[popupAttribute.dropdownList.Length];

                    for (int i = 0; i < dropdownNames.Length; ++i)
                    {
                        dropdownNames[i] = popupAttribute.dropdownList[i].ToString();
                    }
                }
                return dropdownNames;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupAttribute.dropdownList == null || popupAttribute.dropdownList.Length == 0)
            {
                EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "[Popup] Attribute's Parameter is Empty");
                return;
            }

            if (validateValue == null || setValue == null)
            {
                SetUp(property);
            }

            if (validateValue == null || setValue == null)
            {
                base.OnGUI(position, property, label);
                return;
            }

            for (int i = 0; i < DropdownNames.Length; ++i)
            {
                popupAttribute.selectedValue = validateValue(i);
                if (popupAttribute.selectedValue != 0)
                    break;
            }

            EditorGUI.BeginChangeCheck();
            popupAttribute.selectedValue = EditorGUI.Popup(position, label.text, popupAttribute.selectedValue, DropdownNames);
            if (EditorGUI.EndChangeCheck())
            {
                setValue(popupAttribute.selectedValue);
            }
        }

        private void SetUp(SerializedProperty property)
        {
            if (variableType == typeof(string))
            {
                validateValue = (index) =>
                {
                    return property.stringValue == DropdownNames[index] ? index : 0;
                };

                setValue = (index) =>
                {
                    property.stringValue = DropdownNames[index];
                };
            }
            else if (variableType == typeof(int))
            {
                validateValue = (index) =>
                {
                    return property.intValue == Convert.ToInt32(DropdownNames[index]) ? index : 0;
                };

                setValue = (index) =>
                {
                    property.intValue = Convert.ToInt32(DropdownNames[index]);
                };
            }
            else if (variableType == typeof(float))
            {
                validateValue = (index) =>
                {
                    return property.floatValue == Convert.ToSingle(DropdownNames[index]) ? index : 0;
                };
                setValue = (index) =>
                {
                    property.floatValue = Convert.ToSingle(DropdownNames[index]);
                };
            }
        }

        private DropdownAttribute popupAttribute => attribute as DropdownAttribute;

        private Type variableType => popupAttribute.dropdownList[0].GetType();
    }
}