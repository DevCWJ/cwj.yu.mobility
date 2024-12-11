using System;

namespace CWJ
{
    /// <summary>
    /// 실행중에 동적 생성되는 클래스에는 선언하면 작동안함
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredTagAttribute : Attribute
    {
        public readonly string[] tags;
        public readonly bool isMyTag;
        public readonly bool isRecursively;

        public RequiredTagAttribute(string tag, bool isMyTag = true, bool isRecursively = false)
        {
            this.tags = new string[] { tag };
            this.isMyTag = isMyTag;
            this.isRecursively = isRecursively;
        }

        public RequiredTagAttribute(string[] tags)
        {
            this.tags = tags;
            this.isMyTag = false;
            this.isRecursively = false;
        }
    }
}