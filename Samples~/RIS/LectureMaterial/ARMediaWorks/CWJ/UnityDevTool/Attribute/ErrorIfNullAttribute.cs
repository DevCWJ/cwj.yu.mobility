using System;

using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// null이면 오류를 출력하고 빨간색으로 강조함
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ErrorIfNullAttribute : PropertyAttribute
    {
        public readonly bool isWarningOnlyActive;

        public ErrorIfNullAttribute(bool isWarningOnlyActive = false)
        {
            this.isWarningOnlyActive = isWarningOnlyActive;
        }
    }
}