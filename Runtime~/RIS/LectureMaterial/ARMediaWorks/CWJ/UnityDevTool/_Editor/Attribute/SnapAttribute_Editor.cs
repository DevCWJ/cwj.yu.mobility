﻿using CWJ;
using UnityEditor;
using UnityEngine;

namespace CWJ.EditorOnly
{
	[CustomPropertyDrawer(typeof(SnapAttribute))]
	public class SnapAttribute_Editor : PropertyDrawer
	{
		private const string _invalidTypeWarning = "Invalid type for MinMaxSlider on field {0}: MinMaxSlider can only be applied to a float or int fields";

		public static float Snap(float value, float snap)
		{
			return snap > 0.0f ? Mathf.Round(value / snap) * snap : value;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label.tooltip = Label.GetTooltip(fieldInfo);

			var snap = attribute as SnapAttribute;

			EditorGUI.PropertyField(position, property, label);

			if (property.propertyType == SerializedPropertyType.Float)
				property.floatValue = Snap(property.floatValue, snap.SnapValue);
			else if (property.propertyType == SerializedPropertyType.Integer)
				property.intValue = Mathf.RoundToInt(Snap(property.intValue, snap.SnapValue));
			else
				Debug.LogWarningFormat(_invalidTypeWarning, property.propertyPath);
		}
	}
}
