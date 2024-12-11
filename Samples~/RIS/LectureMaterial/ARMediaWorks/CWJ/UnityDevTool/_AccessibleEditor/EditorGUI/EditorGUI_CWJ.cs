#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

using CWJ.EditorOnly;
using UnityObject = UnityEngine.Object;
using CWJ.Serializable;

namespace CWJ.AccessibleEditor
{


    public delegate (bool isExpanded, float height) NewPropertyDrawHandler(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren);
    public interface ICustomPropertyDrawer
    {
        (bool isExpanded, float height) Draw(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren);
    }

    public static partial class EditorGUI_CWJ
    {
        public static readonly Type TypeOfHideInInspector = typeof(HideInInspector);
        public static readonly Type TypeOfMonoBehaviour = typeof(MonoBehaviour);
        public static readonly Type TypeOfUnityEventBase = typeof(UnityEventBase);

        public static readonly Type TypeOfScriptableObject = typeof(ScriptableObject);

        public static readonly Type TypeOfDictionaryVisualized = typeof(DictionaryVisualized<,>);

        public static readonly Type TypeOfCoroutine = typeof(Coroutine);

        public static bool IsProSkin => EditorStyles.label.normal.textColor != Color.black;

        static IList lastList;
        static Type lastElemType;
        private static ObjectListControl _ReorderableListDrawer;
        public static ObjectListControl GetReorderableListDrawer(IList list, Type elemType)
        {
            if (_ReorderableListDrawer == null)
            {
                _ReorderableListDrawer = new ObjectListControl();
            }
            if (lastList != list || elemType != lastElemType)
            {
                _ReorderableListDrawer.Setup(list);
                lastList = list;
                lastElemType = elemType;
            }
            return _ReorderableListDrawer;
        }


        //public static CWJ.EditorOnly.DictionaryControl _dictionaryControl = new DictionaryControl();


        public static void RemoveFocusFromText()
        {
            EditorGUI.FocusTextInControl(null);
        }

        //public enum EFieldSpecialType
        //{
        //    Else = 0,
        //    UnityEventBase,
        //    Dictionary,
        //    SerializableInterface
        //}
        static Dictionary<Type, ICustomPropertyDrawer> TypeOfCustomPropertyDrawerDic = new Dictionary<Type, ICustomPropertyDrawer>();

        public static ICustomPropertyDrawer GetCustomPropertyDrawerByType(Type type)
        {
            if (!TypeOfCustomPropertyDrawerDic.TryGetValue(type, out ICustomPropertyDrawer propertyDrawer))
            {
                if (TypeOfUnityEventBase.IsAssignableFrom(type))
                    propertyDrawer = new UnityEventDrawerCustom();
                else if (TypeOfScriptableObject.IsAssignableFrom(type))
                    propertyDrawer = new DisplayScriptableObjectDrawer();
                else if (TypeOfDictionaryVisualized.IsAssignableFromGenericType(type))
                    propertyDrawer = new VisualizedDictionaryPropertyDrawer();
                //else if ...
                //else if ...
                if (propertyDrawer != null)
                    TypeOfCustomPropertyDrawerDic.Add(type, propertyDrawer);
            }
            return propertyDrawer;
        }

        //AttributeUtil.GetVisualizeInfo 도 FieldDataCache를 사용하는 PropertyField_New 처럼 갈아치워야함
        public struct MemberAttributeCache
        {
            public readonly bool isInit;
            public readonly string name;
            public bool isHideAlways;
            public readonly bool isReadonlyAlways;
            public readonly bool isTargetObjValid;

            //OnValueChangedAttribute
            public readonly bool hasValueChangedCallback;
            public readonly string onValueChangedCallbackName;

            //SyncValueAttribute
            public readonly bool hasSyncValueAttribute;
            public readonly Action<object, FieldInfo> whenChangedCallbackInsyncValueAtt;

            public readonly bool needChangedCheck;

            //_VisualizeConditionalAttribute
            public readonly IConditionalAttEssential visualizeConditionalInterface;
            //Root_ReadonlyAttribute
            public readonly IConditionalAttEssential readonlyConditionalInterface;


            private readonly string tooltip;
            public GUIContent GetLabelWithTooltip(GUIContent label)
            {
                label.tooltip = string.IsNullOrEmpty(label.tooltip) ? tooltip : label.tooltip + "\n" + tooltip;
                return label;
            }
            public readonly NewPropertyDrawHandler drawFunc;

            public MemberAttributeCache(MemberInfo memberInfo, Type memberType, object targetObj, NewPropertyDrawHandler _drawFunc = null, bool needDraw = true)
            {
                name = null;
                isTargetObjValid = false;
                hasValueChangedCallback = false;
                onValueChangedCallbackName = null;
                hasSyncValueAttribute = false;
                whenChangedCallbackInsyncValueAtt = null;
                needChangedCheck = false;
                visualizeConditionalInterface = null;
                readonlyConditionalInterface = null;
                this.drawFunc = _drawFunc;
                tooltip = null;
                isReadonlyAlways = false;
                isInit = true;

                if (targetObj == null || memberInfo == null)
                {
                    isHideAlways = true;
                    return;
                }

                name = memberInfo.Name;
                needDraw = (needDraw && !(memberInfo is MethodInfo)) || drawFunc != null;

                if (isHideAlways = (needDraw && memberType == null) || memberInfo.IsDefined(TypeOfHideInInspector))
                {
                    return;
                }

                var hideFlags = (targetObj as MonoBehaviour)?.hideFlags ?? HideFlags.None;
                if (isHideAlways = (hideFlags.HasFlag(HideFlags.HideInInspector) || hideFlags.HasFlag(HideFlags.HideInHierarchy)))
                {
                    return;
                }

                visualizeConditionalInterface = new ConditionalAttributeData<_VisualizeConditionalAttribute>(memberInfo, targetObj);
                var visualizeResult = visualizeConditionalInterface.GetConstantResult();
                if (isHideAlways = (visualizeResult.isConstantlyMatched && !visualizeResult.isPossitive))
                {
                    visualizeConditionalInterface = null;
                    return;
                }

                isReadonlyAlways = hideFlags.HasFlag(HideFlags.NotEditable);
                if (!isReadonlyAlways)
                {
                    readonlyConditionalInterface = new ConditionalAttributeData<Root_ReadonlyAttribute>(memberInfo, targetObj);
                    if (isReadonlyAlways = readonlyConditionalInterface.GetConstantResult().isConstantlyMatched)
                    {
                        readonlyConditionalInterface = null;
                    }

                }

                if (!needDraw)
                {
                    return;
                }

                tooltip = memberInfo?.GetCustomAttribute<TooltipAttribute>()?.tooltip ?? null;

                if (!isReadonlyAlways)
                {
                    if (isTargetObjValid = TypeOfMonoBehaviour.IsAssignableFrom(targetObj.GetType()))
                    {
                        onValueChangedCallbackName = memberInfo.GetCustomAttribute<OnValueChangedAttribute>()?.callbackName ?? null;

                        var syncValueAtt = memberInfo.GetCustomAttribute<_Root_SyncValueAttribute>();
                        if (syncValueAtt != null && syncValueAtt.IsValid(targetObj as UnityObject))
                            whenChangedCallbackInsyncValueAtt = syncValueAtt.WhenChanged;
                    }
                    hasValueChangedCallback = !string.IsNullOrEmpty(onValueChangedCallbackName);
                    hasSyncValueAttribute = whenChangedCallbackInsyncValueAtt != null;
                    needChangedCheck = hasSyncValueAttribute || hasValueChangedCallback; //important
                }

                if (this.drawFunc == null)
                {
                    var customPropertyDrawer = GetCustomPropertyDrawerByType(memberType);

                    if (customPropertyDrawer != null)
                        this.drawFunc = customPropertyDrawer.Draw;
                    //else
                    //{
                    //    if (memberType.IsArrayOrList())
                    //    {
                    //        //Struct 배열은 안보이는 문제가있음
                    //        drawFunc = GetReorderableListDrawer((IList)memberInfo.GetValue(targetObj), memberType).Draw;
                    //    }
                    //}

                    if (drawFunc == null)
                        this.drawFunc = DrawNormal;
                }
            }

            public (bool isVisible, bool isReadonly) GetVisualizeState(bool isBaseReadonly)
            {
                if (isHideAlways)
                {
                    return (false, false);
                }


                var visualizeResult = visualizeConditionalInterface.GetVariableResult();

                if (visualizeResult.isEnabled)
                {
                    if (visualizeResult.isPossitive != visualizeResult.isMatched)
                        return (false, false);
                }

                if (isReadonlyAlways || isBaseReadonly)
                {
                    return (true, true);
                }

                var readonlyResult = readonlyConditionalInterface.GetVariableResult();
                return (true, readonlyResult.isMatched);
            }

            private (bool isExpanded, float height) DrawNormal(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
            {
                return (EditorGUI.PropertyField(position, property, label, includeChildren)
                    , EditorGUI.GetPropertyHeight(property, label, includeChildren));
            }

            public void CheckChangedAndCallAttFunc(bool isChanged, object targetObj, FieldInfo fieldInfo)
            {
                if (isChanged)
                {
                    if (hasValueChangedCallback)
                    {
                        var mono = targetObj as MonoBehaviour;
                        if (mono != null)
                            mono.Invoke(onValueChangedCallbackName, 0);
                    }
                    if (hasSyncValueAttribute)
                    {
                        whenChangedCallbackInsyncValueAtt(targetObj, fieldInfo);
                    }
                }
            }
            public void CheckChangedAndCallAttFunc(bool isGuiChanged, object targetObj, PropertyInfo propInfo)
            {
                if (isGuiChanged)
                {
                    if (hasValueChangedCallback)
                    {
                        var mono = targetObj as MonoBehaviour;
                        if (mono != null)
                            mono.Invoke(onValueChangedCallbackName, 0);
                    }
                    if (hasSyncValueAttribute)
                    {
                        //whenChangedCallbackInsyncValueAtt(targetObj, propInfo);
                    }
                }
            }
        }

        static Dictionary<int, MemberAttributeCache> PropertyDrawerAttCacheDic = new Dictionary<int, MemberAttributeCache>();
        public static void ClearPropertyDrawerAttCache() => PropertyDrawerAttCacheDic?.Clear();
        public static MemberAttributeCache GetAttributeCacheWithoutDraw<T>(this Dictionary<int, MemberAttributeCache> dic, object targetObj, T memberInfo, Type memberType) where T : MemberInfo
        {
            return _GetAttributeCache(dic, targetObj, memberInfo, memberType, false, null);
        }
        public static MemberAttributeCache GetAttributeCacheWithDraw<T>(this Dictionary<int, MemberAttributeCache> dic, object targetObj, T memberInfo, Type memberType, NewPropertyDrawHandler newPropertyDrawHandler) where T : MemberInfo
        {
            return _GetAttributeCache(dic, targetObj, memberInfo, memberType, true, newPropertyDrawHandler);
        }

        static MemberAttributeCache _GetAttributeCache<T>(Dictionary<int, MemberAttributeCache> dic, object targetObj, T memberInfo, Type memberType, bool needDraw, NewPropertyDrawHandler newPropertyDrawHandler) where T : MemberInfo
        {
            var hashCode = HashCodeHelper.GetHashCode(targetObj, memberInfo);

            if (dic.TryGetValue(hashCode, out MemberAttributeCache attCache))
            {
                return attCache;
            }
            if (memberInfo.Name.Equals("isLocalWifiOnly"))
            {

            }
            attCache = new MemberAttributeCache(memberInfo, memberType, targetObj, needDraw: needDraw, _drawFunc: newPropertyDrawHandler);

            dic.Add(hashCode, attCache);

            return attCache;
        }

        public static (bool isVisible, bool isReadonly, bool isExpanded, float height) PropertyField_CWJ(FieldInfo fieldInfo, Rect position, SerializedProperty property, GUIContent label, bool includeChildren = true, NewPropertyDrawHandler drawFunc = null)
        {
            bool isExpanded = false;
            float height = 0;
            bool isBaseReadonly = !property.editable || !GUI.enabled;
            if (fieldInfo == null && !string.IsNullOrEmpty(property.displayName))
            {
                isExpanded = EditorGUILayout.PropertyField(property);
                //height = EditorGUI.GetPropertyHeight(property);
                return (true, isBaseReadonly, isExpanded, height);
            }
            UnityObject targetObj = property.GetTargetObject();

            var attCache = PropertyDrawerAttCacheDic.GetAttributeCacheWithDraw(targetObj, fieldInfo, fieldInfo.FieldType, drawFunc);

            if (attCache.isHideAlways)
            {
                return (false, false, false, 0);
            }

            var visualizeState = attCache.GetVisualizeState(isBaseReadonly);

            isExpanded = false;

            if (visualizeState.isVisible)
            {
                using (var disabledScope = new EditorGUI.DisabledScope(visualizeState.isReadonly))
                {
                    if (attCache.needChangedCheck)
                    {
                        using (var changeScope = new EditorGUI.ChangeCheckScope())
                        {
                            var info = attCache.drawFunc(fieldInfo, position, property, attCache.GetLabelWithTooltip(label), true);
                            isExpanded = info.isExpanded;
                            height = info.height;
                            attCache.CheckChangedAndCallAttFunc(changeScope.changed, targetObj, fieldInfo);
                        }
                    }
                    else
                    {
                        var info = attCache.drawFunc(fieldInfo, position, property, attCache.GetLabelWithTooltip(label), true);
                        isExpanded = info.isExpanded;

                        height = info.height;
                    }
                }
            }

            return (visualizeState.isVisible, visualizeState.isReadonly, isExpanded, height);
        }




        /// <summary>
        /// 오직 CWJ 라이브러리에서만 그려지는 타입
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeOnlyDrawCWJInspector(Type type)
        {
            return type.IsInterface;
        }//아직은 인터페이스뿐. SerializableDictionary도 추가하면될듯
    }
}
#endif