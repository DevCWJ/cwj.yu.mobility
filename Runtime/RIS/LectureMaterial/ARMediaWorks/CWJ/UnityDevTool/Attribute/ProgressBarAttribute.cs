using System;

using CWJ.AccessibleEditor;

using UnityEngine;

namespace CWJ
{
    using static AttributeUtil;

    [AttributeUsage(AttributeTargets.Field)]
    public class ProgressBarAttribute : PropertyAttribute
    {
        public readonly string name;
        public readonly float maxValue;
        public readonly Color color;
        public readonly bool isVisibleField;

        public ProgressBarAttribute(string name = "", float maxValue = 100, EParameterColor color = EParameterColor.Gray, bool isVisibleField = false)
        {
            this.name = name;
            this.maxValue = maxValue;
            this.color = color.ToColor();
            this.isVisibleField = isVisibleField;
        }
    }
}