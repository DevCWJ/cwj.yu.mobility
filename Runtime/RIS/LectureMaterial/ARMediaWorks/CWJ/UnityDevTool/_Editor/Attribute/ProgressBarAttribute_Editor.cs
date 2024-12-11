using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarAttribute_Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI_CWJ.DrawHeader(property);

            if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUILayout.HelpBox(property.name + " is not a number", MessageType.Warning);
                return;
            }

            ProgressBarAttribute progressBarAttribute = attribute as ProgressBarAttribute;
            if (progressBarAttribute.isVisibleField && !property.isArray) EditorGUILayout.PropertyField(property, label);

            float maxValue = progressBarAttribute.maxValue;
            float value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            string valueFormat = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : string.Format("{0:0.00}", value);

            float fillPercentage = value / maxValue;
            string barLabel = (!string.IsNullOrEmpty(progressBarAttribute.name) ? "[" + progressBarAttribute.name + "] " : "") + valueFormat + "/" + maxValue;

            Rect barPosition = new Rect(position.position.x, position.position.y, position.size.x, EditorGUIUtility.singleLineHeight);
            Color color = progressBarAttribute.color;
            Color color2 = Color.white;
            DrawBar(barPosition, Mathf.Clamp01(fillPercentage), barLabel, color, color2);
            EditorGUILayout.Space();
        }

        private void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Color savedColor = GUI.color;

            Rect fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);

            EditorGUI.DrawRect(position, new Color(0.13f, 0.13f, 0.13f));
            EditorGUI.DrawRect(fillRect, barColor);

            TextAnchor align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            Color c = GUI.contentColor;
            GUI.contentColor = labelColor;

            Rect labelRect = new Rect(position.x, position.y - 2, position.width, position.height);

            EditorGUI.DropShadowLabel(labelRect, label);

            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }
    }
}