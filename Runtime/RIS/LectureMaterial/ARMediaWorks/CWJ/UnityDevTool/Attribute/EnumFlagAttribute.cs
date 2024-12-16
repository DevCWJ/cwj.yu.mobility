using UnityEngine;

namespace CWJ
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumFlagAttribute : PropertyAttribute
    {
        public EnumFlagAttribute() { }

    } 
}