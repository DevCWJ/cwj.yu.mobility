using System;

using UnityEngine;

namespace CWJ
{
    //public enum ETest
    //{
    //    [EnumLabel("a의 displayName")] a,
    //    [EnumLabel("b의 displayName")] b,
    //    [EnumLabel("c의 displayName")] c
    //}
    //[EnumLabel("testType의 displayName")]
    //public ETest testType;
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumLabelAttribute : PropertyAttribute
    {
        public readonly string label;

        public EnumLabelAttribute(string label)
        {
            this.label = label;
        }
    }
}