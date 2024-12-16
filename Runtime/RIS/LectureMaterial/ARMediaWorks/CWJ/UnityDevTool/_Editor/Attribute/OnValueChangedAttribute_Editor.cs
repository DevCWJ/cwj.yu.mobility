using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute), true)]
    public class OnValueChangedAttribute_Editor : PropertyDrawer
    {
        private float height = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var info = EditorGUI_CWJ.PropertyField_CWJ(fieldInfo, position, property, label);
            height = info.height;
            //bool isValid = false;
            //UnityEngine.Object targetObj = null;

            //if ((OnValueChangedAttribute.callbackNames?.Length ?? 0) > 0)
            //{
            //    targetObj = property.GetTargetObject();

            //    if (isValid = targetObj.GetType().IsSubclassOf(typeof(MonoBehaviour)))
            //    {
            //        EditorGUI.BeginChangeCheck();
            //    }
            //}

            //var info = EditorDrawUtil.PropertyField_New(fieldInfo, position, property, label);
            //height = info.height;
            //if (isValid)
            //{
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        MonoBehaviour mono = targetObj as MonoBehaviour;

            //        for (int i = 0; i < OnValueChangedAttribute.callbackNames.Length; i++)
            //        {
            //            mono.Invoke(OnValueChangedAttribute.callbackNames[i], 0);
            //        }
            //    }
            //}
        }
    }
}