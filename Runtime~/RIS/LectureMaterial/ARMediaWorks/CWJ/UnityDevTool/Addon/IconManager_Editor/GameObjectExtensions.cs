#if UNITY_EDITOR
using TNRD.Utilities;

namespace UnityEngine
{
    public static class GameObjectExtensions
    {
        public static void SetLabelIcon(this GameObject gameObject, LabelIcon labelIcon)
        {
            IconManager.SetIcon(gameObject, labelIcon);
        }

        public static void SetShapeIcon(this GameObject gameObject, ShapeIcon shapeIcon)
        {
            IconManager.SetIcon(gameObject, shapeIcon);
        }

        public static void RemoveIcon(this GameObject gameObject)
        {
            IconManager.RemoveIcon(gameObject);
        }
    }
} 
#endif