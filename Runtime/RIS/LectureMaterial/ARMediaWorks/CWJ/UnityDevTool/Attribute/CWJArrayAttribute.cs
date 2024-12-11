using System;
using UnityEngine;

namespace CWJ
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class UnityBuiltInDrawerAttribute : PropertyAttribute { }
}