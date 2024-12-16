using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
    public class HelpBoxAttribute_Editor : DecoratorDrawer
    {
        public override float GetHeight()
        {
            HelpBoxAttribute helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null) return base.GetHeight();

            GUIStyle helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
            if (helpBoxStyle == null) return base.GetHeight();
            return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text
#if UNITY_2019_3_OR_NEWER
                + "\n"
#endif
                ), EditorGUIUtility.currentViewWidth) + 4);
        }

        public override void OnGUI(Rect position)
        {
            HelpBoxAttribute helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null) return;

            EditorGUI.HelpBox(position, helpBoxAttribute.text, helpBoxAttribute.messageType);
        }

        //PropertyDrawer
        //const int paddingHeight = 8;

        //const int marginHeight = 2;

        //float baseHeight = 0;

        //float addedHeight = 0;

        //HelpBoxAttribute helpBoxAttribute => (HelpBoxAttribute)attribute;

        //public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        //{
        //    baseHeight = base.GetPropertyHeight(prop, label);

        //    float minHeight = paddingHeight * 5;

        //    var content = new GUIContent(helpBoxAttribute.text);
        //    var style = GUI.skin.GetStyle("helpbox");

        //    var height = style.CalcHeight(content, EditorGUIUtility.currentViewWidth);

        //    height += marginHeight * 2;

        //    if (fieldInfo.GetAttribute<MultilineAttribute>() != null && prop.propertyType == SerializedPropertyType.String)
        //    {
        //        addedHeight = 48f;
        //    }

        //    return height > minHeight ? height + baseHeight + addedHeight : minHeight + baseHeight + addedHeight;
        //}

        //public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        //{
        //    EditorGUI.BeginProperty(position, label, prop);

        //    var helpPos = position;

        //    helpPos.height -= baseHeight + marginHeight;

        //    var multiline = fieldInfo.GetAttribute<MultilineAttribute>();
        //    if (multiline != null)
        //    {
        //        helpPos.height -= addedHeight;
        //    }

        //    EditorGUI.HelpBox(helpPos, helpBoxAttribute.text, helpBoxAttribute.messageType);

        //    position.y += helpPos.height + marginHeight;
        //    position.height = baseHeight;

        //    GUI.enabled = fieldInfo.GetAttribute<ReadonlyAttribute>() == null;

        //    RangeAttribute rangeAtt = fieldInfo.GetAttribute<RangeAttribute>();

        //    if (rangeAtt != null)
        //    {
        //        if (prop.propertyType == SerializedPropertyType.Float)
        //        {
        //            EditorGUI.Slider(position, prop, rangeAtt.min, rangeAtt.max, label);
        //        }
        //        else if (prop.propertyType == SerializedPropertyType.Integer)
        //        {
        //            EditorGUI.IntSlider(position, prop, (int)rangeAtt.min, (int)rangeAtt.max, label);
        //        }
        //        else
        //        {
        //            EditorGUI.PropertyField(position, prop, label);
        //        }
        //    }
        //    else if (multiline != null)
        //    {
        //        if (prop.propertyType == SerializedPropertyType.String)
        //        {
        //            var style = GUI.skin.label;
        //            var size = style.CalcHeight(label, EditorGUIUtility.currentViewWidth);

        //            EditorGUI.LabelField(position, label);

        //            position.y += size;
        //            position.height += addedHeight - size;

        //            prop.stringValue = EditorGUI.TextArea(position, prop.stringValue);
        //        }
        //        else
        //        {
        //            EditorGUI.PropertyField(position, prop, label);
        //        }
        //    }
        //    else
        //    {
        //        EditorGUI.PropertyField(position, prop, label);
        //    }
        //    GUI.enabled = true;
        //    EditorGUI.EndProperty();
        //}
    }
}