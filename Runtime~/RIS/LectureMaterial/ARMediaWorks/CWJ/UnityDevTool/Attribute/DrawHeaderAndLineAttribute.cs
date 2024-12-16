#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ
{
    using static CWJ.AccessibleEditor.AttributeUtil;

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public abstract class _Root_DrawHeaderAndLineAttribute : PropertyAttribute
    {
        public readonly bool hasText;
        public readonly string headerTitle;
        public Color textColor;

        public readonly bool hasLine;
        public Color lineColor;

        public bool isNeedDefaultColor;

        public _Root_DrawHeaderAndLineAttribute(string headerTitle, EParameterColor textColor, bool hasLine, EParameterColor lineColor)
        {
            this.headerTitle = headerTitle?.ExcludeRichTextFormat("size");
            isNeedDefaultColor = false;
            if (this.hasText = !string.IsNullOrEmpty(headerTitle))
            {
                if (textColor == EParameterColor.Null)
                {
                    this.textColor = Color.clear;
                    isNeedDefaultColor = true;
                }
                else
                {
                    this.textColor = textColor.ToColor();
                }
            }

            if (this.hasLine = hasLine)
            {
                if (lineColor == EParameterColor.Null)
                {
                    this.textColor = Color.clear;
                    isNeedDefaultColor = true;
                }
                else
                {
                    this.lineColor = lineColor.ToColor();
                }
            }
            else
            {
                this.lineColor = Color.clear;
            }
        }
    }

    public class DrawHeaderAttribute : _Root_DrawHeaderAndLineAttribute
    {
        public DrawHeaderAttribute(string headerTitle, EParameterColor textColor = EParameterColor.Null) : base(headerTitle, textColor, false, EParameterColor.Null) { }
    }

    public class DrawLineAttribute : _Root_DrawHeaderAndLineAttribute
    {
        public DrawLineAttribute(EParameterColor lineColor = EParameterColor.Null) : base(string.Empty, EParameterColor.Null, true, lineColor) { }
    }

    public class DrawHeaderAndLineAttribute : _Root_DrawHeaderAndLineAttribute
    {
        public DrawHeaderAndLineAttribute(string headerTitle, EParameterColor textColor = EParameterColor.Null, EParameterColor lineColor = EParameterColor.Null) : base(headerTitle, textColor, true, lineColor) { }
    }
}