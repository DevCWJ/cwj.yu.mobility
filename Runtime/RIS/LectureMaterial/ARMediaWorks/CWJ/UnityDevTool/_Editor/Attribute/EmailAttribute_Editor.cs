using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(EmailAttribute))]
    public class EmailAttribute_Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsSupported(property)) return;

            string email = property.stringValue;

            position.height = 16;

            property.stringValue = EditorGUI.TextField(position, label, email);

            if (!IsValid(property))
            {
                DrawHelpBox(position);
            }
        }

        private void DrawHelpBox(Rect position)
        {
            position.x += 10;
            position.y += 20;
            position.width -= 10;
            position.height += 8;
            EditorGUI.HelpBox(position, "Is invalid email address", MessageType.Error);
        }

        private bool IsSupported(SerializedProperty property) => property.propertyType == SerializedPropertyType.String;

        private bool IsValid(SerializedProperty property) => property.stringValue.IsValidEmail();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property, label);
            return (IsSupported(property) && !IsValid(property)) ? (baseHeight + 30) : baseHeight;
        }

        private EmailAttribute emailAttribute => attribute as EmailAttribute;
    }
}