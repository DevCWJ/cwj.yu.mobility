#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace CWJ.AccessibleEditor.UnityBuiltIn
{
    // Built-in PropertyDrawers

    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(RangeAttribute), true)]
    public sealed class RangeDrawer : PropertyDrawer_CWJ
    {
        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            RangeAttribute rangeAttribute = attribute as RangeAttribute;
            float min = rangeAttribute.min; float max = rangeAttribute.max;
            if (property.propertyType == SerializedPropertyType.Float)
                EditorGUI.Slider(position, property, min, max, label);
            else if (property.propertyType == SerializedPropertyType.Integer)
                EditorGUI.IntSlider(position, property, (int)min, (int)max, label);
            else
                EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
            return false;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeightDefault(property, label);
        }
    }


    public sealed class MinDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            base.OnGUI(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                MinAttribute minAttribute = (MinAttribute)attribute;
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    property.floatValue = Mathf.Max(minAttribute.min, property.floatValue);
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    property.intValue = Mathf.Max((int)minAttribute.min, property.intValue);
                }
                else if (property.propertyType == SerializedPropertyType.Vector2)
                {
                    var value = property.vector2Value;
                    property.vector2Value = new Vector2(Mathf.Max(minAttribute.min, value.x), Mathf.Max(minAttribute.min, value.y));
                }
                else if (property.propertyType == SerializedPropertyType.Vector2Int)
                {
                    var value = property.vector2IntValue;
                    property.vector2IntValue = new Vector2Int(Mathf.Max((int)minAttribute.min, value.x), Mathf.Max((int)minAttribute.min, value.y));
                }
                else if (property.propertyType == SerializedPropertyType.Vector3)
                {
                    var value = property.vector3Value;
                    property.vector3Value = new Vector3(Mathf.Max(minAttribute.min, value.x), Mathf.Max(minAttribute.min, value.y), Mathf.Max(minAttribute.min, value.z));
                }
                else if (property.propertyType == SerializedPropertyType.Vector3Int)
                {
                    var value = property.vector3IntValue;
                    property.vector3IntValue = new Vector3Int(Mathf.Max((int)minAttribute.min, value.x), Mathf.Max((int)minAttribute.min, value.y), Mathf.Max((int)minAttribute.min, value.z));
                }
                else if (property.propertyType == SerializedPropertyType.Vector4)
                {
                    var value = property.vector4Value;
                    property.vector4Value = new Vector4(Mathf.Max(minAttribute.min, value.x), Mathf.Max(minAttribute.min, value.y), Mathf.Max(minAttribute.min, value.z), Mathf.Max(minAttribute.min, value.w));
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Use Min with float, int or Vector.");
                }
            }
        }
    }

    /// <summary>
    /// 그냥 <see cref="ResizableTextAreaAttribute"/> 쓰기. Unity 자체 Attribute인데 줄체크가 제대로안됨
    /// </summary>
    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(MultilineAttribute), true)]
    public sealed class MultilineDrawer : PropertyDrawer_CWJ
    {
        private const int kLineHeight = 13;

        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                label = EditorGUI.BeginProperty(position, label, property);

                position = Restore_EditorGUI.MultiFieldPrefixLabel(position, 0, label, 1);

                EditorGUI.BeginChangeCheck();
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0; // The MultiFieldPrefixLabel already applied indent, so avoid indent of TextArea itself.
                string newValue = EditorGUI.TextArea(position, property.stringValue);
                EditorGUI.indentLevel = oldIndent;
                if (EditorGUI.EndChangeCheck())
                    property.stringValue = newValue;

                EditorGUI.EndProperty();
                return false;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use Multiline with string.");
                return false;
            }
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.wideMode ? 0 : (int)Restore_EditorGUI.kSingleLineHeight) // header
                + Restore_EditorGUI.kSingleLineHeight // first line
                + ((attribute as MultilineAttribute).lines - 1) * kLineHeight; // remaining lines
        }
    }


    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(TextAreaAttribute), true)]
    public sealed class TextAreaDrawer : PropertyDrawer_CWJ
    {
        private const int kLineHeight = 13;

        private Vector2 m_ScrollPosition = new Vector2();
        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                label = EditorGUI.BeginProperty(position, label, property);
                Rect labelPosition = EditorGUI.IndentedRect(position);
                labelPosition.height = Restore_EditorGUI.kSingleLineHeight;
                position.yMin += labelPosition.height;
                EditorGUI.HandlePrefixLabel(position, labelPosition, label);

                EditorGUI.BeginChangeCheck();
                string newValue = Restore_EditorGUI.ScrollableTextAreaInternal(position, property.stringValue, ref m_ScrollPosition, EditorStyles.textArea);
                if (EditorGUI.EndChangeCheck())
                    property.stringValue = newValue;

                EditorGUI.EndProperty();
                return true;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use TextAreaDrawer with string.");
                return false;
            }
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            TextAreaAttribute textAreaAttribute = attribute as TextAreaAttribute;
            string text = property.stringValue;

            float fullTextHeight = EditorStyles.textArea.CalcHeight(new GUIContent(text), Restore_EditorGUI.contextWidth);
            int lines = Mathf.CeilToInt(fullTextHeight / kLineHeight);

            lines = Mathf.Clamp(lines, textAreaAttribute.minLines, textAreaAttribute.maxLines);

            return Restore_EditorGUI.kSingleLineHeight // header
                + Restore_EditorGUI.kSingleLineHeight // first line
                + (lines - 1) * kLineHeight; // remaining lines
        }
    }


    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(ColorUsageAttribute), true)]
    public sealed class ColorUsageDrawer : PropertyDrawer_CWJ
    {
        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var colorUsage = attribute as ColorUsageAttribute;

            if (property.propertyType == SerializedPropertyType.Color)
            {
                label = EditorGUI.BeginProperty(position, label, property);
                EditorGUI.BeginChangeCheck();
                Color newColor = EditorGUI.ColorField(position, label, property.colorValue, true, colorUsage.showAlpha, colorUsage.hdr);
                if (EditorGUI.EndChangeCheck())
                {
                    property.colorValue = newColor;
                }
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.ColorField(position, label, property.colorValue, true, colorUsage.showAlpha, colorUsage.hdr);
            }
            return false;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeightDefault(property, label);
        }
    }


    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(GradientUsageAttribute), true)]
    public sealed class GradientUsageDrawer : PropertyDrawer_CWJ
    {
        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var colorUsage = (GradientUsageAttribute)attribute;

            EditorGUI.BeginChangeCheck();
            Gradient lastGradient = property.GetGradientValue();
            Gradient newGradient = EditorGUI.GradientField(position, label, lastGradient, colorUsage.hdr);
            if (EditorGUI.EndChangeCheck() && !lastGradient.Equals(newGradient))
            {
                property.SetGradientValue(newGradient);
            }
            return false;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeightDefault(property, label);
        }
    }

    [CWJ.AccessibleEditor.InjectablePropertyDrawer(typeof(DelayedAttribute), true)]
    public sealed class DelayedDrawer : PropertyDrawer_CWJ
    {
        public override float GetHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeightDefault(property, label);
        }

        public override bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            if (property.propertyType == SerializedPropertyType.Float)
                EditorGUI.DelayedFloatField(position, property, label);
            else if (property.propertyType == SerializedPropertyType.Integer)
                EditorGUI.DelayedIntField(position, property, label);
            else if (property.propertyType == SerializedPropertyType.String)
                EditorGUI.DelayedTextField(position, property, label);
            else
                EditorGUI.LabelField(position, label.text, "Use Delayed with float, int, or string.");
            return false;
        }
    }
} 
#endif