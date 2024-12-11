#if UNITY_EDITOR
using UnityEngine;

namespace CWJ.EditorOnly
{
    public class InvalidScope : ColorScope
    {
        public static readonly Color ValidBackground = UnityEngine.Color.white;
        public static readonly Color InvalidBackground = new Color(1.0f, 0.6f, 0.6f);

        public InvalidScope(bool valid) : base(GUI.color, valid ? ValidBackground : InvalidBackground, GUI.contentColor)
        {
        }
    }
}

#endif