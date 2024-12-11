#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;

namespace CWJ.AccessibleEditor
{
    public static class PropertyHandler
    {
        const string TypeName_ScriptAttributeUtility = "UnityEditor.ScriptAttributeUtility, UnityEditor";
        const string TypeName_PropertyHandler = "UnityEditor.PropertyHandler, UnityEditor";

        const string MethodName_GetHandler = "GetHandler";
        private static MethodInfo getHandler;

        const string FieldName_PropertyDrawer = "m_PropertyDrawer";
        private static FieldInfo propertyDrawerFieldInfo;

        const string FieldName_DecoratorDrawers = "m_DecoratorDrawers";
        private static FieldInfo decoratorDrawersFieldInfo;

        private static object[] getHandlerParams;
        static PropertyHandler()
        {
            getHandler = Type.GetType(TypeName_ScriptAttributeUtility).GetMethod(MethodName_GetHandler, BindingFlags.NonPublic | BindingFlags.Static);
            Type propertyHandlerType = Type.GetType(TypeName_PropertyHandler);
            var bindingFlag = BindingFlags.NonPublic | BindingFlags.Instance;
            propertyDrawerFieldInfo = propertyHandlerType.GetField(FieldName_PropertyDrawer, bindingFlag);
            decoratorDrawersFieldInfo = propertyHandlerType.GetField(FieldName_DecoratorDrawers, bindingFlag);
            getHandlerParams = new object[1];
        }

        public static PropertyDrawer GetPropertyDrawer(SerializedProperty property)
        {
            return _GetPropertyDrawer(GetHandler(property));
        }

        private static PropertyDrawer _GetPropertyDrawer(object handler)
        {
            return propertyDrawerFieldInfo.GetValue(handler) as PropertyDrawer;
        }

        public static DecoratorDrawer[] GetDecoratorDrawers(SerializedProperty property)
        {
            return _GetDecoratorDrawers(GetHandler(property));
        }

        private static DecoratorDrawer[] _GetDecoratorDrawers(object handler)
        {
            return (decoratorDrawersFieldInfo.GetValue(handler) as List<DecoratorDrawer>)?.ToArray();
        }

        public static (PropertyDrawer propertyDrawer, DecoratorDrawer[] decoratorDrawers) GetPropertyAndDecoratorDrawer(SerializedProperty property)
        {
            var handler = GetHandler(property);
            return (_GetPropertyDrawer(handler), _GetDecoratorDrawers(handler));
        }
        private static object GetHandler(SerializedProperty property)
        {
            getHandlerParams[0] = property;
            return getHandler.Invoke(null, getHandlerParams);
        }

    }
}
#endif