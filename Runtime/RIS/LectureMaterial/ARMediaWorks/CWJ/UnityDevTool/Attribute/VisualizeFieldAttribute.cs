using System;

namespace CWJ
{
    [Obsolete("Use [VisualizeField] instead.", error: true)]
    public class SerializeNonPublicAttribute : Attribute { }

    [Obsolete("Use [VisualizeField_All] instead.", error: true)]
    public class SerializeAllNonPublicAttribute : Attribute { }


    /// <summary>
    /// <see langword="Field"/>에 사용
    /// <para><see langword="private"/>, <see langword="static"/> 등 Access Modifier에 상관없이 <see langword="Field"/>가 인스펙터에서 보임</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class VisualizeFieldAttribute : Attribute { }

    /// <summary>
    /// <see langword="class"/>에 사용
    /// <para><see langword="private"/>, <see langword="static"/> 등 Access Modifier에 상관없이 <see langword="class"/>의 모든 <see langword="Field"/>가 인스펙터에서 보임</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class VisualizeField_AllAttribute : Attribute
    {
        public readonly bool isFindAllBaseClass = false;

        public readonly bool isReadonly = false;

        public VisualizeField_AllAttribute(bool isFindAllBaseClass = false, bool isReadonly = false)
        {
            this.isFindAllBaseClass = isFindAllBaseClass;
            this.isReadonly = isReadonly;
        }
    }
}