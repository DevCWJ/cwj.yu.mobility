using System.Linq;
using UnityEditor;
using UnityEngine;

using CWJ.AccessibleEditor;
using System.Reflection;

namespace CWJ.EditorOnly
{
	[CustomPropertyDrawer(typeof(StringPopupAttribute))]
	public class StringPopupAttribute_Editor : PropertyDrawer_CWJ
	{
		private const string _invalidTypeWarning = "Invalid type for StringPopup on field {0}: StringPopup can only be applied to string, int type";
		private const string _invalidFieldNameError = "Invalid fieldName for StringPopup on field {0}: Check your field name in class";

        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
			bool isStringType = property.propertyType == SerializedPropertyType.String;
			if (!isStringType && property.propertyType != SerializedPropertyType.Integer)
			{
				Debug.LogErrorFormat(context: property.GetTargetObject(), _invalidTypeWarning, property.propertyPath);
				EditorGUI.PropertyField(position, property, label);
				return false;
			}
			string[] names = null;

			if (!(attribute as StringPopupAttribute).GetOptionNames(property.GetTargetObject(), out names))
			{
				Debug.LogErrorFormat(context: property.GetTargetObject(), _invalidFieldNameError, property.propertyPath, property.GetTargetObject());
				EditorGUI.PropertyField(position, property, label);
				return false;
			}

			var selectedIndex = isStringType ? names.IndexOf(property.stringValue) : property.intValue;
			var contents = names.Select(s => new GUIContent(s)).ToArray();
			var index = EditorGUI.Popup(position, label, selectedIndex, names.Select(s => new GUIContent(s)).ToArray());
			if (0 <= index && index < names.Length)
			{
				if (isStringType)
					property.stringValue = names[index];
				else
					property.intValue = index;
			}
			return false;
		}

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            //return GetPropertyHeight(property, label);
            return GetPropertyHeightDefault(property, label);
        }
    }
}
