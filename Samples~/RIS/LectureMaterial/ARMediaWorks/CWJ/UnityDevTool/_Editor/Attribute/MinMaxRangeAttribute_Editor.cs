using System;
using UnityEngine;
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxRangeAttribute_Editor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty minProp = property.FindPropertyRelative("Min");
			SerializedProperty maxProp = property.FindPropertyRelative("Max");
			if (minProp == null || maxProp == null)
			{
				typeof(MinMaxRangeAttribute).PrintLogWithClassName("MinMaxRangeAttribute used on " +
								 property.name.SetBold() +
								 ". Must be used on types with Min and Max fields",
					obj: property.GetTargetObject(), isPreventOverlapMsg: true);

				return;
			}

			var minValid = minProp.propertyType == SerializedPropertyType.Integer || minProp.propertyType == SerializedPropertyType.Float;
			var maxValid = maxProp.propertyType == SerializedPropertyType.Integer || maxProp.propertyType == SerializedPropertyType.Float;
			if (!maxValid || !minValid || minProp.propertyType != maxProp.propertyType)
			{
				typeof(MinMaxRangeAttribute).PrintLogWithClassName("MinMaxRangeAttribute used on " +
								 property.name.SetBold() +
								 ". Min and Max fields must be of int or float type",
					obj: property.GetTargetObject(), isPreventOverlapMsg: true);

				return;
			}

			MinMaxRangeAttribute rangeAttribute = (MinMaxRangeAttribute)attribute;

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			bool isInt = minProp.propertyType == SerializedPropertyType.Integer;

			float minValue = isInt ? minProp.intValue : minProp.floatValue;
			float maxValue = isInt ? maxProp.intValue : maxProp.floatValue;
			float rangeMin = rangeAttribute.Min;
			float rangeMax = rangeAttribute.Max;


			const float rangeBoundsLabelWidth = 40f;

			var rangeBoundsLabel1Rect = new Rect(position);
			rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
			GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString(isInt ? "F0" : "F2")));
			position.xMin += rangeBoundsLabelWidth;

			var rangeBoundsLabel2Rect = new Rect(position);
			rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
			GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString(isInt ? "F0" : "F2")));
			position.xMax -= rangeBoundsLabelWidth;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);

			if (EditorGUI.EndChangeCheck())
			{
				if (isInt)
				{
					minProp.intValue = Mathf.RoundToInt(minValue);
					maxProp.intValue = Mathf.RoundToInt(maxValue);
				}
				else
				{
					minProp.floatValue = minValue;
					maxProp.floatValue = maxValue;
				}
			}

			EditorGUI.EndProperty();
		}
	}
}