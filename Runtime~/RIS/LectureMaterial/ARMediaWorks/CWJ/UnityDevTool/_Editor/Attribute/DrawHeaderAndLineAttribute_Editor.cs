using UnityEngine;
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(_Root_DrawHeaderAndLineAttribute), true)]
    public class DrawHeaderAndLineAttribute_Editor : DecoratorDrawer
    {
        private _Root_DrawHeaderAndLineAttribute headerAttribute => attribute as _Root_DrawHeaderAndLineAttribute;

        public override float GetHeight()
        {
            return MarginHalf * 2;
        }

        readonly float MarginHalf = EditorGUI_CWJ.LineGUILabelStyle.lineHeight * .5f;

        public override void OnGUI(Rect position)
        {
            if (headerAttribute.isNeedDefaultColor)
            {
                if (headerAttribute.lineColor == Color.clear)
                {
                    headerAttribute.lineColor = EditorGUICustomStyle.DefaultLineColor;
                }
                if (headerAttribute.textColor == Color.clear)
                {
                    headerAttribute.textColor = EditorGUICustomStyle.DefaultTextColor;
                }
                headerAttribute.isNeedDefaultColor = false;
            }

            if (headerAttribute.hasText)
            {
                EditorGUI_CWJ.DrawLineAndHeaderWithColor(new GUIContent(headerAttribute.headerTitle), headerAttribute.textColor,
                    headerAttribute.lineColor, margin: (0, 0), refRect: position);
            }
            else
            {
                EditorGUI_CWJ.DrawLineWithColor(headerAttribute.lineColor, margin: (MarginHalf, MarginHalf), refRect: position);
            }
        }
    }
}

//using UnityEngine;
//using UnityEditor;
//using CWJ.AccessibleEditor;

//namespace CWJ.EditorOnly
//{
//    [CustomPropertyDrawer(typeof(Root_DrawHeaderAndLineAttribute), true)]
//    public class DrawHeaderAndLineAttribute_Editor : DecoratorDrawer
//    {
//        private Root_DrawHeaderAndLineAttribute headerAttribute => attribute as Root_DrawHeaderAndLineAttribute;

//        public override float GetHeight()
//        {
//            return 30f;
//        }

//        GUIStyle labelStyle;
//        GUIStyle LabelStyle
//        {
//            get
//            {
//                if (labelStyle == null)
//                {
//                    labelStyle = new GUIStyle(EditorGUICustomStyle.LargeLabelStyle);
//                    labelStyle.normal.textColor = headerAttribute.textColor;
//                }
//                return labelStyle;
//            }
//        }

//        private Color defaultColor = Color.clear;
//        private Color DefaultColor
//        {
//            get
//            {
//                if (defaultColor == Color.clear)
//                {
//                    defaultColor = EditorGUICustomStyle.LargeLabelStyle.normal.textColor;
//                    float offset = AccessibleEditor.EditorGUI_CWJ.IsProSkin ? 0.15f : -0.15f;
//                    defaultColor = new Color(defaultColor.r + offset, defaultColor.g + offset, defaultColor.b + offset);

//                }
//                return defaultColor;
//            }
//        }

//        float height;

//        public override void OnGUI(Rect position)
//        {
//            if (headerAttribute.isNeedDefaultColor)
//            {
//                if (headerAttribute.lineColor == Color.clear)
//                {
//                    headerAttribute.lineColor = DefaultColor;
//                }
//                if (headerAttribute.textColor == Color.clear)
//                {
//                    headerAttribute.textColor = DefaultColor;
//                }
//                headerAttribute.isNeedDefaultColor = false;
//            }

//            position.y += 17f;

//            if (headerAttribute.hasText)
//            {
//                string text = headerAttribute.headerTitle;

//                Vector2 textSize = LabelStyle.CalcSize(new GUIContent(text));
//                float separatorWidth = (position.width - textSize.x) / 2.0f - 5.0f;

//                if (headerAttribute.hasLine) EditorGUI.DrawRect(new Rect(position.xMin, position.yMin, separatorWidth, 1), headerAttribute.lineColor);
//                EditorGUI.LabelField(new Rect(position.xMin + separatorWidth + 5.0f, position.yMin - ((LabelStyle.fontSize / 3) * 2) - 0.55f, textSize.x, textSize.y), text, LabelStyle);
//                if (headerAttribute.hasLine) EditorGUI.DrawRect(new Rect(position.xMin + separatorWidth + 10.0f + textSize.x, position.yMin, separatorWidth, 1), headerAttribute.lineColor);
//            }
//            else
//            {
//                position.height = 1;
//                EditorGUI.DrawRect(position, headerAttribute.lineColor);
//                height = 1;
//            }
//        }
//    }
//}
