using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(PasswordAttribute))]
    public class PasswordAttribute_Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsSupported(property))
            {
                return;
            }

            string password = property.stringValue;
            int maxLength = passwordAttribute.maxLength;

            position.height = 16;
            if (property.stringValue.Length > maxLength)
            {
                password = password.Substring(0, maxLength);
            }

            if (!passwordAttribute.useMask)
            {
                property.stringValue = EditorGUI.TextField(position, label, password);
            }
            else
            {
                property.stringValue = EditorGUI.PasswordField(position, label, password);
            }

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
            EditorGUI.HelpBox(position, string.Format("Password must contain at least {0} characters!", passwordAttribute.minLength), MessageType.Error);
        }

        private bool IsSupported(SerializedProperty property) => property.propertyType == SerializedPropertyType.String;

        private bool IsValid(SerializedProperty property) => property.stringValue.Length >= passwordAttribute.minLength;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property, label);
            return (IsSupported(property) && !IsValid(property)) ? (baseHeight + 30) : baseHeight;
        }

        private PasswordAttribute passwordAttribute => attribute as PasswordAttribute;
    }
}