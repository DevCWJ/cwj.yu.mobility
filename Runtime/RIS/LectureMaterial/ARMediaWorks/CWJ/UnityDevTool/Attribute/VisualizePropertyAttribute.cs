using System;

namespace CWJ
{
    // TODO: 현재 Set이 없거나 자동프로퍼티의 Set이 아닌경우에 EditorDrawUtil.290라인에서 GetHashCode() 했을때 
    // 계속해서 HashCode값이 바뀌기때문에 Foldout을 유지할수없게됨 (현재는 이 경우 항시 열려있음) 그외에도 계속해서 Foldout 키값이 생기면 위험하므로 조치취해야함

    [Obsolete("Use [VisualizeProperty] instead.", error: true)]
    public class SerializePropertyAttribute : Attribute { }

    [Obsolete("Use [VisualizeProperty_All] instead.", error: true)]
    public class SerializeAllPropertyAttribute : Attribute { }

    /// <summary>
    /// <see langword="Property"/>에 사용
    /// <para><see langword="private"/>, <see langword="static"/> 등 Access Modifier에 상관없이 <see langword="Property"/>가 인스펙터에서 보임</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VisualizePropertyAttribute : Attribute { }

    /// <summary>
    /// <see langword="class"/>에 사용
    /// <para><see langword="private"/>, <see langword="static"/> 등 Access Modifier에 상관없이 <see langword="class"/>의 모든 <see langword="Property"/>가 인스펙터에서 보임</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class VisualizeProperty_AllAttribute : Attribute
    {
        public readonly bool isFindAllBaseClass = false;

        public readonly bool isReadonly = false;

        public VisualizeProperty_AllAttribute(bool isFindAllBaseClass = false, bool isReadonly = false)
        {
            this.isFindAllBaseClass = isFindAllBaseClass;
            this.isReadonly = isReadonly;
        }
    }
}