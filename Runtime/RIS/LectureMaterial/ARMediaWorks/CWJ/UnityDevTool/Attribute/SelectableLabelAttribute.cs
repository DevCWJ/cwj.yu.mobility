using System;

using UnityEngine;

namespace CWJ
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectableLabelAttribute : PropertyAttribute
    {
        public readonly string text;

        public readonly int fontSize = 13;
        public readonly FontStyle fontStyle = FontStyle.Normal;
        public readonly TextAnchor textAnchor = TextAnchor.LowerLeft;

        public SelectableLabelAttribute(string text, int fontSize = 13, FontStyle fontStyle = FontStyle.Normal, TextAnchor textAnchor = TextAnchor.LowerLeft)
        {
            this.text = text;
            this.fontSize = fontSize;
            this.fontStyle = fontStyle;
            this.textAnchor = textAnchor;
        }
    }
}