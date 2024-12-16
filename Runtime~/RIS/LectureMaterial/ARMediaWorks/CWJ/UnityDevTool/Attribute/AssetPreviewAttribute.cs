using System;

using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// GameObject, Sprite 등
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetPreviewAttribute : PropertyAttribute
    {
        public readonly int width;
        public readonly int height;

        public AssetPreviewAttribute(int width = 64, int height = 64)
        {
            this.width = width;
            this.height = height;
        }
    }
}