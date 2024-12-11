using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(SelectableLabelAttribute), true)]
    public class SelectableLabelAttribute_Editor : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            position = EditorGUI.IndentedRect(position);
            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;

            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = selectableLabelAttribute.fontSize;
            guiStyle.fontStyle = selectableLabelAttribute.fontStyle;
            guiStyle.alignment = selectableLabelAttribute.textAnchor;
            EditorGUI.SelectableLabel(position, selectableLabelAttribute.text, guiStyle);
        }

        private SelectableLabelAttribute selectableLabelAttribute => attribute as SelectableLabelAttribute;

        public override float GetHeight()
        {
            return selectableLabelAttribute.text.Split('\n').Length * base.GetHeight();
        }
    }
}