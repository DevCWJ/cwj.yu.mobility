#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        public struct FoldoutCacheKey : IEquatable<FoldoutCacheKey>
        {
            public int reflectObjID;
            public string fieldName;

            public FoldoutCacheKey(int reflectObjID, string fieldName)
            {
                this.reflectObjID = reflectObjID;
                this.fieldName = fieldName;
            }

            public bool Equals(FoldoutCacheKey other) => (reflectObjID == other.reflectObjID && string.Equals(fieldName, other.fieldName));

            public override int GetHashCode()
            {
                return HashCodeHelper.GetHashCode(reflectObjID, fieldName);
            }
        }

        private static Dictionary<FoldoutCacheKey, bool> FoldoutCacheDict = new Dictionary<FoldoutCacheKey, bool>();

        //deprecated
        /// <summary>
        /// Rename dictionary key
        /// <para>change oldKey to newKey.</para>
        /// </summary>
        /// <typeparam name="TKey">type of key</typeparam>
        /// <typeparam name="TValue">type of value</typeparam>
        /// <returns>success to change</returns>
        //private static bool ChangeHashKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
        //{
        //    TValue value;
        //    if (!dictionary.TryGetValue(oldKey, out value))
        //        return false;

        //    dictionary.Remove(oldKey);
        //    dictionary[newKey] = value;
        //    return true;
        //}
        private static Dictionary<int, MemberAttributeCache> VariousDrawerAttCacheDict = new Dictionary<int, MemberAttributeCache>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="targetObj"></param>
        /// <param name="hasReadonlySub"></param>
        /// <param name="drawVariousType"></param>
        /// <returns>isChanged</returns>
        public static (bool isVisible, bool isReadonly, bool isChanged, object value) DrawVariousFieldTypeWithAtt(FieldInfo fieldInfo, string name, object targetObj, bool hasReadonlySub, DrawVariousTypeHandler drawVariousType)
        {
            Type fieldType = fieldInfo.FieldType;
            if (drawVariousType == null)
            { //not supported
                EditorGUI_CWJ.DrawLabel_Exception(fieldType, name);
                return (false, false, false, null);
            }

            var attCache = VariousDrawerAttCacheDict.GetAttributeCacheWithoutDraw(targetObj, fieldInfo, fieldType);

            if (attCache.isHideAlways)
            {
                return (false, false, false, null);
            }

            bool isAbleToSetValue = !fieldInfo.IsReadonly() && !fieldInfo.IsConst();
            var visualizeState = attCache.GetVisualizeState(hasReadonlySub || !isAbleToSetValue);

            if (!visualizeState.isVisible) return (false, false, false, null);

            bool isReadonlyState = visualizeState.isReadonly;

            bool isGuiChanged = false;
            object value = null;
            bool isValueChangedViaCode = false;

            using (new EditorGUI.DisabledScope(isReadonlyState))
            {
                if (!isReadonlyState)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        value = drawVariousType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj), ref isValueChangedViaCode);
                        isGuiChanged = changeScope.changed;
                    }
                }
                else
                {
                    value = drawVariousType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj), ref isValueChangedViaCode);
                }
            }
            bool isValueChanged = isGuiChanged || isValueChangedViaCode;
            if (isAbleToSetValue)
            {
                if (isValueChanged)
                    fieldInfo.SetValue(targetObj, value);

                attCache.CheckChangedAndCallAttFunc(isValueChanged, targetObj, fieldInfo);
            }

            return (true, isReadonlyState, isValueChanged, value);
        }

        public static (bool isVisible, bool isReadonly, bool isChanged, object value) DrawVariousPropertyTypeWithAtt(PropertyInfo propertyInfo, string name, UnityObject targetObj, bool hasReadonlySub, DrawVariousTypeHandler drawVariousType)
        {
            Type propType = propertyInfo.PropertyType;
            if (drawVariousType == null)
            { //not supported
                EditorGUI_CWJ.DrawLabel_Exception(propType, name);
                return (false, false, false, null);
            }

            var attCache = VariousDrawerAttCacheDict.GetAttributeCacheWithoutDraw(targetObj, propertyInfo, propType);

            if (attCache.isHideAlways)
            {
                return (false, false, false, null);
            }

            bool isAbleToSetValue = propertyInfo.GetSetMethod(true) != null;

            var visualizeState = attCache.GetVisualizeState(hasReadonlySub || !isAbleToSetValue);

            if (!visualizeState.isVisible) return (false, false, false, null);

            bool isReadonlyState = visualizeState.isReadonly;

            bool isGuiChanged = false;
            object value = null;
            bool isValueChangedViaCode = false;

            using (new EditorGUI.DisabledScope(isReadonlyState))
            {
                if (!isReadonlyState)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        value = drawVariousType(propType, name, propertyInfo.GetValue(propertyInfo.IsStatic() ? null : targetObj), ref isValueChangedViaCode);
                        isGuiChanged = changeScope.changed;
                    }
                }
                else
                {
                    value = drawVariousType(propType, name, propertyInfo.GetValue(propertyInfo.IsStatic() ? null : targetObj), ref isValueChangedViaCode);
                }                
            }

            if (isAbleToSetValue)
            {
                if (isGuiChanged || isValueChangedViaCode)
                    propertyInfo.SetValue(targetObj, value, null);

                attCache.CheckChangedAndCallAttFunc(isGuiChanged, targetObj, propertyInfo);
            }

            return (true, isReadonlyState, isGuiChanged, value);
        }

        public static void DrawVariousArrayType(FieldInfo fieldInfo, string name, UnityObject targetObj, UnityObject[] targets, int targetInstanceID, DrawVariousTypeHandler drawElemVariousType)
        {
            Type fieldType = fieldInfo.FieldType;

            if (!fieldType.IsArray)
            {
                DrawLabel_Exception(fieldType, name);
                return;
            }

            var attCache = VariousDrawerAttCacheDict.GetAttributeCacheWithoutDraw(targetObj, fieldInfo, fieldType);
            if (attCache.isHideAlways)
            {
                return;
            }

            bool isAbleToSetValue = !fieldInfo.IsReadonly() && !fieldInfo.IsConst();
            var visualizeState = attCache.GetVisualizeState(!isAbleToSetValue);

            if (!visualizeState.isVisible) return;

            bool isReadonlyState = visualizeState.isReadonly;


            bool isValueChangedViaCode = false;

            if (targets.Length > 1)
            {
                Type elemType = fieldType.GetElementType();
                foreach (var t in targets)
                {
                    var v = fieldInfo.GetValue(t);
                    if (v as Array == null)
                    {
                        fieldInfo.SetValue(t, _ArrayConstructor(elemType));
                        isValueChangedViaCode = true;
                    }
                }
            }

            bool isGuiChanged = false;
            object value;
            using (new EditorGUI.DisabledScope(isReadonlyState))
            {
                Action undoRecordAction = () => Undo.RecordObjects(targets, "Inspector Array");

                if (!isReadonlyState)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        value = DrawArrayType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj),
                                    ref isValueChangedViaCode, targetInstanceID, drawElemVariousType: drawElemVariousType, undoRecord: undoRecordAction);
                        isGuiChanged = changeScope.changed;
                    }
                }
                else
                {
                    value = DrawArrayType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj),
                                ref isValueChangedViaCode, targetInstanceID, drawElemVariousType: drawElemVariousType, undoRecord: undoRecordAction);
                }
            }

            if (isAbleToSetValue)
            {
                if (isGuiChanged)
                {
                    if (targets.Length == 1)
                    {
                        fieldInfo.SetValue(targetObj, value);
                    }
                    else
                    {
                        Type elemType = fieldType.GetElementType();
                        Array newArray = value as Array;
                        int newArrayLength = newArray.Length;
                        foreach (var t in targets)
                        {
                            var arr = fieldInfo.GetValue(t) as Array;
                            if (arr.Length != newArrayLength)
                            {
                                arr = _UpdateArrayLength(elemType, newArrayLength, arr);
                                isValueChangedViaCode = true;
                            }

                            for (int i = 0; i < newArrayLength; i++)
                                arr.SetValue(newArray.GetValue(i), i);

                            fieldInfo.SetValue(t, arr);
                        }
                    }
                    attCache.CheckChangedAndCallAttFunc(isGuiChanged, targetObj, fieldInfo);
                }
                else if (isValueChangedViaCode)
                {
                    fieldInfo.SetValue(targetObj, value);
                }
            }
        }

        public static void DrawVariousListType(FieldInfo fieldInfo, string name, UnityObject targetObj, UnityObject[] targets, int targetInstanceID, DrawVariousTypeHandler drawElemVariousType)
        {
            Type fieldType = fieldInfo.FieldType;

            if (!fieldType.IsGenericList())
            {
                DrawLabel_Exception(fieldType, name);
                return;
            }

            var attCache = VariousDrawerAttCacheDict.GetAttributeCacheWithoutDraw(targetObj, fieldInfo, fieldType);

            if (attCache.isHideAlways)
            {
                return;
            }

            bool isAbleToSetValue = !fieldInfo.IsReadonly() && !fieldInfo.IsConst();
            var visualizeState = attCache.GetVisualizeState(!isAbleToSetValue);

            if (!visualizeState.isVisible) return;

            bool isReadonlyState = visualizeState.isReadonly;

            bool isValueChangedViaCode = false;

            if (targets.Length > 1)
            {
                foreach (var t in targets)
                {
                    var v = fieldInfo.GetValue(t);
                    if (v == null || v.Equals(null))
                    {
                        fieldInfo.SetValue(t, _ListConstructor(fieldType));
                        isValueChangedViaCode = true;
                    }
                }
            }

            bool isGuiChanged = false;
            object value;

            using (new EditorGUI.DisabledScope(isReadonlyState))
            {
                Action undoRecordAction = () => Undo.RecordObjects(targets, "Inspector List");

                if (!isReadonlyState)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        value = DrawListType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj),
                                            ref isValueChangedViaCode, targetInstanceID, drawElemVariousType: drawElemVariousType, undoRecord: undoRecordAction);

                        isGuiChanged = changeScope.changed;
                    }
                }
                else
                {
                    value = DrawListType(fieldType, name, fieldInfo.GetValue(fieldInfo.IsStatic ? null : targetObj),
                                        ref isValueChangedViaCode, targetInstanceID, drawElemVariousType: drawElemVariousType, undoRecord: undoRecordAction);
                }
            }

            if (isAbleToSetValue)
            {
                if (isGuiChanged)
                {
                    if (targets.Length == 1)
                    {
                        fieldInfo.SetValue(targetObj, value);
                    }
                    else
                    {
                        Type elemType = fieldType.GetGenericArguments()[0];
                        IList newList = value as IList;
                        int newListCount = newList.Count;
                        foreach (var t in targets)
                        {
                            var list = fieldInfo.GetValue(t) as IList;
                            if (list.Count != newListCount)
                            {
                                list = _UpdateListCount(elemType, newListCount, list);
                                isValueChangedViaCode = true;
                            }

                            for (int i = 0; i < newListCount; i++)
                                list[i] = newList[i];

                            fieldInfo.SetValue(t, list);
                        }
                    }
                    attCache.CheckChangedAndCallAttFunc(isGuiChanged, targetObj, fieldInfo);
                }
                else if (isValueChangedViaCode)
                {
                    fieldInfo.SetValue(targetObj, value);
                }
            }
        }

    }
}
#endif