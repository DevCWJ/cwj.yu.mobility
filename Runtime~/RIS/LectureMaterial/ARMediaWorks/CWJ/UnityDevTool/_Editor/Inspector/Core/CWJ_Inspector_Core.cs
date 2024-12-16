using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

using CWJ.Singleton.Core;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    /// <summary>
    ///CWJ Attribute, Inspector 기능들을 위한 클래스
    /// <para/>CustomEditor(typeof(MonoBehaviour) 어트리뷰트를 가진 스크립트는 하나만있어야함
    /// <br/>(CWJ_InspectorCore외에 다른 클래스가 MonoBehaviour or UnityEngine.Object를 매개변수로 가지는 CustomEditor Attribute가 존재하면 문제생김)
    /// <para/>[20.01.11]
    /// </summary>
    [CustomEditor(typeof(UnityEngine.Object), true), CanEditMultipleObjects]
    public partial class CWJ_Inspector_Core : Editor
    {
        public delegate void AddMemberHandler<T>(T memberInfo) where T : MemberInfo;
        public delegate void EventHandler();
        public delegate void DisableHandler(bool isDestroyByUser);

        public AddMemberHandler<FieldInfo> addFieldEvent;
        public AddMemberHandler<PropertyInfo> addPropertyEvent;
        public AddMemberHandler<MethodInfo> addMethodEvent;

        public event EventHandler endClassifyEvent;

        public event EventHandler onEnableEvent;
        public event DisableHandler onDisableEvent;

        List<int> drawSpecialFoldoutOrder = new List<int>();
        private event EventHandler drawSpecialFoldoutEvent;
        List<int> drawBodyOrder = new List<int>();
        private event EventHandler drawBodyEvent;

        private void SortDrawDelegateByOrder()
        {
            if (drawBodyEvent == null && drawSpecialFoldoutEvent == null) return;

            var drawBodyEvents = drawBodyEvent.GetAllDelegateArray();
            int drawBodyLength = drawBodyEvents.Length;
            if (drawBodyLength > 1)
            {
                var actionOrderList = new (int order, EventHandler evt)[drawBodyLength];
                // unsubscribe
                for (int i = 0; i < drawBodyLength; i++)
                {
                    actionOrderList[i] = (drawBodyOrder[i], drawBodyEvents[i]);
                    drawBodyEvent -= drawBodyEvents[i];
                }

                Array.Sort(actionOrderList, (a, b) =>
                {
                    int aOrder = a.order;
                    int bOrder = b.order;
                    if (aOrder < bOrder)
                        return -1;
                    else if (aOrder > bOrder)
                        return 1;
                    else //a.Length == b.Length
                        return 0;
                });

                for (int i = 0; i < drawBodyLength; i++)
                {
                    drawBodyEvent += actionOrderList[i].evt;
                }
            }

            var drawSpecialFdActs = drawSpecialFoldoutEvent.GetAllDelegateArray();
            int drawSpecialFdLength = drawSpecialFdActs.Length;
            if (drawSpecialFdLength > 1)
            {
                (int order, EventHandler evt)[] actionOrderList = new (int order, EventHandler evt)[drawSpecialFdLength];
                // unsubscribe
                for (int i = 0; i < drawSpecialFdLength; i++)
                {
                    actionOrderList[i] = (drawSpecialFoldoutOrder[i], drawSpecialFdActs[i]);
                    drawSpecialFoldoutEvent -= drawSpecialFdActs[i];
                }

                Array.Sort(actionOrderList, (a, b) =>
                {
                    int aOrder = a.order;
                    int bOrder = b.order;
                    if (aOrder < bOrder)
                        return -1;
                    else if (aOrder > bOrder)
                        return 1;
                    else //a.Length == b.Length
                        return 0;
                });

                for (int i = 0; i < drawSpecialFdLength; i++)
                {
                    drawSpecialFoldoutEvent += actionOrderList[i].evt;
                }
            }
        }


        public void AddDrawEvent(bool isDrawBodyPart, int drawOrder, EventHandler drawAction)
        {
            if (isDrawBodyPart)
            {
                drawBodyOrder.Add(drawOrder);
                drawBodyEvent += drawAction;
            }
            else
            {
                drawSpecialFoldoutOrder.Add(drawOrder);
                drawSpecialFoldoutEvent += drawAction;
            }
        }

        public MonoBehaviour targetComp;
        public System.Type targetType { get; private set; }
        private bool isCustomComp;
        public int targetInstanceID { get; private set; } = 0;
        public string targetInstanceIDStr { get; private set; } = string.Empty;
        public int compTypeHashCode { get; private set; } = 0;

        private SerializedProperty mScriptProp = null;

        private CWJInfoBoxAttribute infoBoxAttribute;

        private System.Type infoDeclareType = null;
        private string declareTypeName = string.Empty;
        private string infoText = null;

        public CWJ_Inspector_BodyAndFoldout foldoutOrBodyElem = null;

        public CWJ_Inspector_InvokeButton invokeBtnElem = null;
        private CWJ_Inspector_VisualizeField visualizeFieldElem = null;
        private CWJ_Inspector_VisualizeProperty visualizePropertyElem = null;

        public bool isElementFoldoutExpanded;

        SingletonCore singleton = null;
        static readonly Type[] drawIgnoreComp = new Type[] { };
        private void InitTargetData(out bool isCustomComp)
        {
            targetComp = (target as MonoBehaviour);
            targetType = targetComp?.GetCustomType();

            if (targetType == null || (drawIgnoreComp.Length > 0 && drawIgnoreComp.IsExists(targetType)))
            {
                targetInstanceID = 0;
                isCustomComp = false;
                return;
            }

            targetInstanceID = target.GetInstanceID();
            targetInstanceIDStr = targetInstanceID.ToString();
            compTypeHashCode = targetType.GetHashCode();

            if ((target is CWJ.Singleton.Core.SingletonCore))
            {
                singleton = target as CWJ.Singleton.Core.SingletonCore;
            }
            isCustomComp = true;
        }

        private void Awake()
        {
            //InitTargetData(out isCustomComp);

            //if (!isCustomComp)
            //{
            //    return;
            //}

            //When Added Component, etc
        }

        //FieldCache[] fieldCache;

        FieldInfo[] allFieldInfos = null;
        PropertyInfo[] allPropertyInfos = null;
        MethodInfo[] allMethodInfos = null;
        (SerializedProperty[] serializedProperties, FieldInfo[] fieldInfos) serializedPropPackage;

        private void InitAllMemberInfos()
        {
            //Collect All of MemberInfo
            Type[] targetAllBaseTypes = ReflectionUtil.GetAllBaseClassTypes(targetType);
            BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            infoBoxAttribute = null;
            infoDeclareType = null;

            List<FieldInfo> fieldInfoList = new List<FieldInfo>();
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
            List<MethodInfo> methodInfoList = new List<MethodInfo>();


            for (int i = targetAllBaseTypes.Length - 1; i >= 0; i--)
            {
                fieldInfoList.AddRange(targetAllBaseTypes[i].GetFields(bindingFlags));
                propertyInfoList.AddRange(targetAllBaseTypes[i].GetProperties(bindingFlags));
                methodInfoList.AddRange(targetAllBaseTypes[i].GetMethods(bindingFlags));

                if (targetAllBaseTypes[i].IsDefined(typeof(CWJInfoBoxAttribute)))
                {
                    infoBoxAttribute = targetAllBaseTypes[i].GetCustomAttribute<CWJInfoBoxAttribute>();
                    if (!string.IsNullOrEmpty(infoBoxAttribute.info))
                    {
                        isSpecialFoldoutExpand = true;
                    }
                    infoText += (infoBoxAttribute.info + "\n");
                    infoDeclareType = targetAllBaseTypes[i];
                    declareTypeName = StringUtil.GetNicifyVariableName(infoDeclareType.Name);
                }
            } //모든 field, property, method들의 정보를 Reflection으로 가져옴

            allFieldInfos = fieldInfoList.ToArray();
            allPropertyInfos = propertyInfoList.ToArray();
            allMethodInfos = methodInfoList.ToArray();

            SerializedProperty rootProp = serializedObject.GetIterator();
            List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
            List<FieldInfo> serializedFieldList = new List<FieldInfo>();
            if (rootProp.NextVisible(true))
            {
                do
                {
                    SerializedProperty prop = rootProp.Copy();

                    if (prop.IsPropName_m_Script())
                    {
                        mScriptProp = prop;
                        continue;
                    }

                    serializedPropertyList.Add(prop);
                    serializedFieldList.Add(allFieldInfos.Find(x => x.Name.Equals(prop.name)));
                } while (rootProp.NextVisible(false));
            } // Editor클래스의 serializedObject를 이용해 모든 Serialized변수를 가져옴 
              // 그리고 이 SerializedProperty 들을 위에서 준비해놨던 모든 field리스트와 비교하여 같은이름의 fieldInfo를 따로 가져옴
              // (SerializedProperty를 쓰지않고 FieldInfo를 쓰기위함)

            serializedPropPackage = (serializedPropertyList.ToArray(), serializedFieldList.ToArray());

            //TODO : FieldCache
            //fieldCache = new FieldCache[serializedFields.Length];
        }

        public void InvokeInitMemberInfoEvent()
        {
            //Create Instance of Element
            if (foldoutOrBodyElem == null) foldoutOrBodyElem = new CWJ_Inspector_BodyAndFoldout(this, serializedPropPackage);
            if (invokeBtnElem == null) invokeBtnElem = new CWJ_Inspector_InvokeButton(this);
            if (visualizeFieldElem == null) visualizeFieldElem = new CWJ_Inspector_VisualizeField(this);
            if (visualizePropertyElem == null) visualizePropertyElem = new CWJ_Inspector_VisualizeProperty(this);

            //Classify AllMemeberInfos
            if (addFieldEvent != null)
            {
                foreach (var fieldInfo in allFieldInfos)
                {
                    if (fieldInfo == null) continue;
                    addFieldEvent.Invoke(fieldInfo);
                }
                foreach (var item in addFieldEvent.GetAllDelegateArray()) addFieldEvent -= item;
                addFieldEvent = null;
            }

            if (addPropertyEvent != null)
            {
                foreach (var propInfo in allPropertyInfos)
                {
                    if (propInfo == null) continue;
                    addPropertyEvent.Invoke(propInfo);
                }
                foreach (var item in addPropertyEvent.GetAllDelegateArray()) addPropertyEvent -= item;
                addPropertyEvent = null;
            }

            if (addMethodEvent != null)
            {
                foreach (var methodInfo in allMethodInfos)
                {
                    if (methodInfo == null) continue;
                    addMethodEvent.Invoke(methodInfo);
                }
                foreach (var item in addMethodEvent.GetAllDelegateArray()) addMethodEvent -= item;
                addMethodEvent = null;
            }

            //End
            if (endClassifyEvent != null)
            {
                endClassifyEvent.Invoke();
                foreach (var item in endClassifyEvent.GetAllDelegateArray()) endClassifyEvent -= item;
                endClassifyEvent = null;
            }
        }

        InspectorHandler.IDestroyHandler[] onDestroyHandlers = null;

        InspectorHandler.IOnGUIHandler[] onGUIHandlers = null;

        private TI[] GetInterfaces<TI>(MonoBehaviour comp, Type compType = null) where TI : class
        {
            if (comp is TI)
            {
                return comp.GetComponents(compType ?? comp.GetType()).ConvertAll(c => c as TI);
            }
            return new TI[0];
        }


        private void EventHandling_OnEnable()
        {
            var onDestroyHandlerList = new List<InspectorHandler.IDestroyHandler>();
            if (!isCompiled)
            {
                foreach (var t in targets)
                {
                    if (t == null) continue;
                    var m = (t as MonoBehaviour);
                    onDestroyHandlerList.AddRange(m.GetComponentsInChildren<InspectorHandler.IDestroyHandler>(true));

                    foreach (var handler in GetInterfaces<InspectorHandler.ISelectHandler>(m))
                    {
                        handler.CWJEditor_OnSelect(targetComp);
                    }
                }
                onDestroyHandlers = onDestroyHandlerList.ToArray();
                onGUIHandlers = GetInterfaces<InspectorHandler.IOnGUIHandler>(targetComp, targetType);
            }
            else
            {
                foreach (var item in GetInterfaces<InspectorHandler.ICompiledHandler>(targetComp, targetType))
                {
                    item.CWJEditor_OnCompile();
                }
                isCompiled = false;
            }
        }

        private const string HierarchyWindowTypeString = " (UnityEditor.SceneHierarchyWindow)";

        private static bool IsEnableByHierarchyClick=> AccessibleEditorUtil.IsAppFocused && 
                                                (EditorWindow.mouseOverWindow?.ToString().Equals(HierarchyWindowTypeString) ?? false);

        private void EventHandling_OnDisable(bool isDestroy)
        {
            if (isCompiled) return;

            if (isDestroy)
            {
                if (onDestroyHandlers != null)
                {
                    foreach (var handler in onDestroyHandlers)
                    {
                        handler.CWJEditor_OnDestroy();
                    }
                }
            }
            else
            {
                if (targets != null)
                {
                    foreach (var t in targets)
                    {
                        if (t == null) continue;
                        foreach (var handler in GetInterfaces<InspectorHandler.IDeselectHandler>(t as MonoBehaviour))
                        {
                            handler.CWJEditor_OnDeselect(targetComp);
                        }
                    }
                }
            }
        }


        private void EventHandling_OnGUI()
        {
            if (onGUIHandlers == null) return;

            foreach (var handler in onGUIHandlers)
            {
                handler.CWJEditor_OnGUI();
            }
        }

        partial void _OnEnable();

        private void OnEnable()
        {
            InitTargetData(out isCustomComp);

            if (!isCustomComp) return;

            CWJ_EditorEventHelper.ReloadedScriptEvent += OnEditorReloaded;

            _OnEnable();

            if (CWJ_Debug.IsIgnoreSpecifiedType && CWJ_Debug.IsExistsInIgnoreTypes(compTypeHashCode))
            {
                IsTypeIgnoreCWJ_Debug = true;
            }
            if (CWJ_Debug.IsAllowSpecifiedType && CWJ_Debug.IsExistsInAllowTypes(compTypeHashCode))
            {
                IsTypeAllowCWJ_Debug = true;
            }

            if (IsTypeIgnoreCWJ_Debug || CWJ_Debug.IsAllowSpecifiedType)
            {
                fd_debugSetting = true;
            }

            addFieldEvent = null;
            addPropertyEvent = null;
            addMethodEvent = null;
            endClassifyEvent = null;

            InitAllMemberInfos();

            InvokeInitMemberInfoEvent();

            SortDrawDelegateByOrder();

            onEnableEvent?.Invoke();

            CWJ_EditorEventHelper.CompSelectedEvent?.Invoke(targetComp, targetType);

            EventHandling_OnEnable();

        }

        private bool isCompiled = false;
        private void OnEditorReloaded()
        {
            IsTypeAllowCWJ_Debug = false;
            IsTypeIgnoreCWJ_Debug = false;
            isPaintInit = false;
            isCompiled = true;

            EventHandling_OnEnable();
        }

        public override bool RequiresConstantRepaint()
        {
            if (!isSpecialFoldoutExpand) return false;

            return ((visualizeFieldElem?.CheckIsDrawing() ?? false) || (visualizePropertyElem?.CheckIsDrawing() ?? false));
        }

        bool isPaintInit = false;
        public override void OnInspectorGUI()
        {
            if (!isCustomComp)
            {
                base.OnInspectorGUI();
                return;
            }
 
            serializedObject.Update();

            DrawSpecialFoldoutGroup();

            drawBodyEvent?.Invoke();

            try
            {
            serializedObject.ApplyModifiedProperties();

            }
            catch { }

            EventHandling_OnGUI();

            if (!isPaintInit || GUI.changed)
            {
                isPaintInit = true;
                Repaint();
            }
        }

        partial void _OnDisable(bool isDestroyByUser);

        private void OnDisable()
        {
            bool isDestroy = target == null;
            bool isDestroyByUser = isDestroy && CWJ_EditorEventHelper.PlayModeState != PlayModeStateChange.ExitingEditMode && (CWJ_EditorEventHelper.PlayModeState != PlayModeStateChange.ExitingPlayMode);

            onDisableEvent?.Invoke(isDestroyByUser);

            if (isCustomComp)
            {
                _OnDisable(isDestroyByUser);
            }

            UnSubscribeEvent();

            EventHandling_OnDisable(isDestroy);

            CWJ_EditorEventHelper.ReloadedScriptEvent -= OnEditorReloaded;
            isCustomComp = false;
            allFieldInfos = null;
            allPropertyInfos = null;
            allMethodInfos = null;

            foldoutOrBodyElem = null;
            invokeBtnElem = null;
            visualizeFieldElem = null;
            visualizePropertyElem = null;
            infoBoxAttribute = null;
            targetComp = null;
            targetType = null;
            infoDeclareType = null;
            targetInstanceID = 0;
            compTypeHashCode = 0;

            EditorGUI_CWJ.ClearPropertyDrawerAttCache();
        }

        /// <summary>
        /// <see langword="= null"/>로는 Delegate안의 event들이 dispose되지 않기때문에 반복문돌려서 일일이 <see langword="-="/> unSubscribe 해준것.
        /// <br/>더 나은 방법이 있는지 알아보기
        /// </summary>
        private void UnSubscribeEvent()
        {
            foreach (var item in drawBodyEvent.GetAllDelegateEnumerable()) drawBodyEvent -= item;
            foreach (var item in drawSpecialFoldoutEvent.GetAllDelegateEnumerable()) drawSpecialFoldoutEvent -= item;
            foreach (var item in onEnableEvent.GetAllDelegateEnumerable()) onEnableEvent -= item;
            foreach (var item in onDisableEvent.GetAllDelegateEnumerable()) onDisableEvent -= item;
        }

        private void OnDestroy()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (target == null && targetInstanceID != 0 && CWJ_EditorEventHelper.PlayModeState != PlayModeStateChange.ExitingPlayMode)
                {
                    CWJ_EditorEventHelper.CompDestroyInEditModeEvent?.Invoke(targetInstanceID);
                }
            }
        }
    }
}