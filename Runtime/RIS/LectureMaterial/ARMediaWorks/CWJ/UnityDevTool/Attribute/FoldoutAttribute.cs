using System;

namespace CWJ
{
    /// <summary>
    /// <see cref="isSectionOpen"/> 로 감쌀 수도있음
    /// <para><see cref="isMergeParentFoldout"/> : <see langword="true"/> = 부모 클래스에 같은 name의 Foldout이 있으면 합침</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public abstract class _Root_FoldoutAttribute : Attribute
    {
        public readonly string name;
        public abstract bool isGroupFoldout { get; }
        public readonly bool isSectionOpen;
        public readonly bool isMergeParentFoldout;

        public _Root_FoldoutAttribute(string name, bool isSectionOpen, bool isMergeParentFoldout)
        {
            this.name = name.Replace("[", "").Replace("]", "");
            if (isGroupFoldout) this.isSectionOpen = isSectionOpen;
            this.isMergeParentFoldout = isMergeParentFoldout;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class FoldoutAttribute : _Root_FoldoutAttribute
    {
        public override bool isGroupFoldout => false;

        public FoldoutAttribute(string name, bool isMergeParentFoldout = true) 
            : base(name, false, isMergeParentFoldout)
        {
        }
    }

    public class FoldoutGroupAttribute : _Root_FoldoutAttribute
    {
        public override bool isGroupFoldout => true;

        public FoldoutGroupAttribute(string name, bool isSectionOpen, bool isMergeParentFoldout = true) 
            : base(name, isSectionOpen, isMergeParentFoldout)
        {
        }
    }
}