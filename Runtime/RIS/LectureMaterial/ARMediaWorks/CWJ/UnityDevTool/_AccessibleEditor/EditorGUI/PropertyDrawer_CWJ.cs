#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace CWJ.AccessibleEditor
{
    public abstract class PropertyDrawer_CWJ : PropertyDrawer, ICustomPropertyDrawer
    {
        public float height { get; private set; } = 0;
        public bool isVisible { get; private set; } = true;

        /// <summary> 
        /// .
        /// <para/> Default Height 쓰고싶다면 <see cref="GetPropertyHeightDefault"/> 사용
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public abstract float GetHeight(SerializedProperty property, GUIContent label);

        public float GetPropertyHeightDefault(SerializedProperty property, GUIContent label) => base.GetPropertyHeight(property, label);

        public (bool isExpanded, float height) Draw(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            bool isExpanded = DrawGUI(fieldInfo, position, property, label, includeChildren);
            return (isExpanded, GetHeight(property, label));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <param name="includeChildren"></param>
        /// <returns>isExpanded</returns>
        public abstract bool DrawGUI(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren);

        public override sealed void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label, drawFunc: Draw);
            height = info.height;
            isVisible = info.isVisible;
        }

#pragma warning disable CS0809
        [System.Obsolete("Dont Use",true)]
        public override sealed float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
#pragma warning restore CS0809
    }
} 
#endif