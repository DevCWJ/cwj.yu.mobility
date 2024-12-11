using System;

namespace CWJ
{
    /// <summary>
    /// 실행중에 동적 생성되는 클래스에는 선언하면 작동안함
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredLayerAttribute : Attribute
    {
        public readonly string[] layers;
        public readonly bool isMyLayer;
        public readonly bool isRecursively;

        public RequiredLayerAttribute(string layer, bool isRecursively = false, bool isMyLayer = true)
        {
            this.layers = new string[] { layer };
            this.isMyLayer = isMyLayer;
            this.isRecursively = isRecursively;
        }

        public RequiredLayerAttribute(string[] layers)
        {
            this.layers = layers;
            this.isMyLayer = false;
            this.isRecursively = false;
        }
    }
}