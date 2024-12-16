using System;

using UnityEngine;

namespace CWJ
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PasswordAttribute : PropertyAttribute
    {
        //public char mask = '*';
        public readonly bool useMask = true;

        public readonly int minLength = 0;
        public readonly int maxLength = 2147483647;

        public PasswordAttribute()
        {
            useMask = true;
        }

        public PasswordAttribute(int minLength = 0, int maxLength = 2147483647, bool useMask = false)
        {
            this.useMask = useMask;
            this.minLength = minLength;
            this.maxLength = maxLength;
        }
    }
}