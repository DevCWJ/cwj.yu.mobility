using System;

using UnityEngine;

namespace CWJ
{
    //public Animator animator;
    //[AnimatorParameter(AnimatorParameterAttribute.ParameterType.Trigger)]
    //public string move_AnimParm;
    /// <summary>
    /// only string
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AnimatorParameterAttribute : PropertyAttribute
    {
        public readonly ParameterType parameterType = ParameterType.None;

        public int selectedValue = 0;

        public AnimatorParameterAttribute(ParameterType parameterType = ParameterType.None)
        {
            this.parameterType = parameterType;
        }

        public enum ParameterType
        {
            Float = 1,
            Int = 3,
            Bool = 4,
            Trigger = 9,
            None = 9999,
        }
    }
}