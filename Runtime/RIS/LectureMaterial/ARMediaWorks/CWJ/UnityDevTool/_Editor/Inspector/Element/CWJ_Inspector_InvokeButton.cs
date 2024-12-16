using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.CSharp;

#if CWJ_EXISTS_EDITORCOROUTINE
using CWJ.Unity.EditorCor.Editor;
#endif

using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

using CWJ.AccessibleEditor;
using UnityEngine.Events;

namespace CWJ.EditorOnly.Inspector
{
    public class CWJ_Inspector_InvokeButton : CWJ_Inspector_ElementAbstract, CWJ_Inspector_ElementAbstract.IUseMemberInfo<MethodInfo>, CWJ_Inspector_ElementAbstract.IUseMemberInfo<FieldInfo>
    {
        protected override string ElementClassName => nameof(CWJ_Inspector_InvokeButton);
        public CWJ_Inspector_InvokeButton(CWJ_Inspector_Core inspectorCore, bool isForciblyDrawAllMembers = false) : base(inspectorCore, false, isForciblyDrawAllMembers)
        {
            methodAndBtnDataList = new List<MethodAndBtnData>();
            fieldAndBtnDataList = new List<FieldAndBtnData>();
        }
        protected override bool HasMemberToDraw()
        {
            return useMethodInfo.memberInfoArray.LengthSafe() > 0 || useFieldInfo.memberInfoArray.LengthSafe() > 0;
        }


        bool IUseMemberInfo<MethodInfo>.MemberInfoClassifyPredicate(MethodInfo methodInfo)
        {
            if (isForciblyDrawAllMembers ||
                (!methodInfo.IsDefined(typeof(FoldoutAttribute)) && methodInfo.IsDefined(typeof(InvokeButtonAttribute), false)))
            {
                var invokeBtnData = GetMethodInvokeBtnData(target, methodInfo);
                bool exists = invokeBtnData != null;
                if (exists)
                {
                    methodAndBtnDataList.Add(new MethodAndBtnData(methodInfo, invokeBtnData));
                }
                return exists;
            }
            return false;
        }

        bool IUseMemberInfo<FieldInfo>.MemberInfoClassifyPredicate(FieldInfo fieldInfo)
        {
            if ((isForciblyDrawAllMembers ||
                (/*!fieldInfo.IsDefined(typeof(FoldoutAttribute)) &&*/  fieldInfo.IsDefined(typeof(InvokeButtonAttribute), true)))
                && typeof(UnityEvent).IsAssignableFrom(fieldInfo.FieldType))
            {
                var invokeBtnData = GetFieldInvokeBtnData(target, fieldInfo);
                bool exists = invokeBtnData != null;
                if (exists)
                {
                    fieldAndBtnDataList.Add(new FieldAndBtnData(fieldInfo, invokeBtnData));
                }
                return exists;
            }
            return false;
        }

        protected override void OnEndClassify()
        {
            methodAndBtnDatas_emphasize = methodAndBtnDataList.FindAllWithMisMatch(m=>m.invokeBtnData.isEmphasizeBtn, out methodAndBtnDatas);
            methodAndBtnDataList.Clear();
            methodAndBtnDataList.Capacity = methodAndBtnDatas.Length;
            fieldAndBtnDatas = fieldAndBtnDataList.ToArray();
            fieldAndBtnDataList.Clear();
            fieldAndBtnDataList.Capacity = fieldAndBtnDatas.Length;

            foldoutContent_root.text = " Invoke Buttons " + (isForciblyDrawAllMembers || GetHasMemberToDraw() ? $"[{useMethodInfo.memberInfoArray.Length + useFieldInfo.memberInfoArray.Length - methodAndBtnDatas_emphasize.Length}]" : string.Empty);
            if (!isDrawBodyPart)
            {
                foldoutContent_root.image = null;
            }
        }

        struct MethodAndBtnData
        {
            public MethodInfo methodInfo;
            public InvokeBtnData_Method invokeBtnData;

            public MethodAndBtnData(MethodInfo methodInfo, InvokeBtnData_Method invokeBtnData)
            {
                this.methodInfo = methodInfo;
                this.invokeBtnData = invokeBtnData;
            }
        }

        List<MethodAndBtnData> methodAndBtnDataList;
        MethodAndBtnData[] methodAndBtnDatas;
        MethodAndBtnData[] methodAndBtnDatas_emphasize;


        struct FieldAndBtnData
        {
            public FieldInfo fieldInfo;
            public InvokeBtnData_Field invokeBtnData;

            public FieldAndBtnData(FieldInfo fieldInfo, InvokeBtnData_Field invokeBtnData)
            {
                this.fieldInfo = fieldInfo;
                this.invokeBtnData = invokeBtnData;
            }
        }
        List<FieldAndBtnData> fieldAndBtnDataList;
        FieldAndBtnData[] fieldAndBtnDatas;

        MethodInfo[] IUseMemberInfo<MethodInfo>.memberInfoArray { get; set; }
        FieldInfo[] IUseMemberInfo<FieldInfo>.memberInfoArray { get; set; }

        protected override void _OnDisable(bool isDestroy)
        {
            bool hasPreCondition = !isDestroy && (isDrawBodyPart || inspectorCore.isSpecialFoldoutExpand) && isRootFoldoutExpand;
            string prefsKey = PrefsKey_Foldout_Method();
            if (methodsFoldExpandDict != null)
            {
                foreach (var item in methodsFoldExpandDict)
                {
                    if (hasPreCondition && item.Value)
                        VolatileEditorPrefs.AddStackValue(prefsKey, GetPrefsValue_Foldout_Method(item.Key));
                    else
                        VolatileEditorPrefs.RemoveStackValue(prefsKey, GetPrefsValue_Foldout_Method(item.Key));
                }
            }
            methodsFoldExpandDict = null;
            invokeBtnDataDict_field = null;
            invokeBtnDataDict_method = null;

            //for (int i = 0; i < useFieldInfo.memberInfoArray.Length; ++i)
            //{
            //    VolatileEditorPrefs.SetBool(GetPrefsKey_Foldout_Field(target, i), isFieldsFoldoutOpen[i]);
            //}
        }

        public override int drawOrder => 10;

        const string VisualizeAllMethods = "Visualize All Methods";

        protected override void DrawInspector()
        {
            if (methodAndBtnDatas_emphasize.Length > 0)
            {
                EditorGUI_CWJ.DrawLineAndHeader(headerContent: new GUIContent("Invoke Button", $"Click Button to Invoke Method"));
                foreach (var item in methodAndBtnDatas_emphasize)
                {
                    _DrawInvokeButton_Method(target, item.methodInfo, item.invokeBtnData);
                    EditorGUILayout.Space(0.5f);
                }
            }

            EditorGUI_CWJ.DrawBigFoldout(ref isRootFoldoutExpand, foldoutContent_root, (isExpand) =>
            {
                if (!isExpand) return;

                ForciblyDrawAllMembersButton(VisualizeAllMethods);

                if (!GetHasMemberToDraw()) return;

                EditorGUILayout.Space(0.7f);

                foreach (var item in methodAndBtnDatas)
                {
                    _DrawInvokeButton_Method(target, item.methodInfo, item.invokeBtnData);
                    EditorGUILayout.Space(0.5f);
                }
                foreach (var item in fieldAndBtnDatas)
                {
                    _DrawInvokeButton_Field(target, item.fieldInfo, item.invokeBtnData);
                    EditorGUILayout.Space(0.5f);
                }
            });
        }

        #region Method
        private string PrefsKey_Foldout_Method() => VolatileEditorPrefs.GetVolatilePrefsKey_Child(ElementClassName, "Method");
        private string GetPrefsValue_Foldout_Method(int methodHashCode) => targetInstanceIDStr + "." + methodHashCode.ToString();
        private Dictionary<int, bool> methodsFoldExpandDict = new Dictionary<int, bool>();
        private Dictionary<int, InvokeBtnData_Method> invokeBtnDataDict_method = new Dictionary<int, InvokeBtnData_Method>();

        public struct ParameterData
        {
            public ParameterInfo paramInfo;
            public Type paramType;
            public EditorGUI_CWJ.DrawVariousTypeHandler paramDrawHandler;
            public readonly bool hasParamDrawHandler;
            public object paramValue;
            public string paramCode;

            public ParameterData(ParameterInfo parameterInfo, Func<Type, string> getTypeNameFunction = null)
            {
                this.paramInfo = parameterInfo;
                //paramType이 매개변수 in키워드 가 있을땐 Type이름이 바뀌어 문제생기기때문에 paramType까지 캐싱함
                this.paramType = TypeUtil.GetValidType(paramInfo.ParameterType);

                this.paramDrawHandler = EditorGUI_CWJ.GetDrawVariousTypeDelegate(paramType);
                this.hasParamDrawHandler = paramDrawHandler != null && paramDrawHandler != EditorGUI_CWJ.NULL__DrawLabel_Exception;
                string paramName = paramInfo.Name;
                var defaultValue = paramInfo.GetDefaultValue(paramType);
                this.paramValue = defaultValue;
                if (paramInfo.HasDefaultValue)
                    paramName += " = " + defaultValue?.ToString() ?? "Null";
                string typeName = getTypeNameFunction != null ? getTypeNameFunction(paramType) : paramType.Name;
                this.paramCode = (string.IsNullOrEmpty(typeName) ? paramType.Name : typeName) + " " + paramName;
            }

            public void DrawParameter(bool isOnlyButton, ref bool isParamValueChanged)
            {
                bool isValueChangedViaCode = false;
                if (hasParamDrawHandler && isOnlyButton)
                {
                    return;
                }
                var lastValue = paramValue;
                var newValue = paramDrawHandler?.Invoke(paramType, paramCode, paramValue, ref isValueChangedViaCode);

                if (isValueChangedViaCode || !TypeUtil.ObjectPowerfulEquals(paramType, lastValue, newValue))
                {
                    paramValue = newValue;
                    if (!isParamValueChanged)
                        isParamValueChanged = true;
                }
            }
        }

        public class InvokeBtnData_Method
        {
            public readonly bool hasData;
            public readonly int methodHashCode;
            public readonly EditorGUI_CWJ.MemberAttributeCache attributeCache;
            public readonly string displayName;
            public readonly string tooltip;
            readonly GUIContent guiContent;

            public readonly bool isCoroutine;
            public readonly string onMarkedBoolName;
            public readonly bool isOnlyButton;
            public readonly bool isNeedUndoNSave;
            public readonly bool isEmphasizeBtn;

            public readonly string returnTypeName;
            public readonly int paramLength;
            public readonly ParameterData[] parameterDatas;

            public InvokeBtnData_Method(UnityObject target, MethodInfo methodInfo, int methodHashCode)
            {
                hasData = false;

                if (target == null || methodInfo == null)
                {
                    return;
                }

                this.methodHashCode = methodHashCode;
                attributeCache = new EditorGUI_CWJ.MemberAttributeCache(methodInfo, null, target);

                if (attributeCache.isHideAlways)
                {
                    return;
                }

                isCoroutine = typeof(IEnumerator).IsAssignableFrom(methodInfo.ReturnType);

                var attribute = methodInfo.GetCustomAttribute<InvokeButtonAttribute>();

                displayName = attribute?.displayName ?? methodInfo.Name;

                isNeedUndoNSave = attribute?.isNeedUndoNSave ?? false;
                onMarkedBoolName = attribute?.onMarkedBoolName ?? null;
                isEmphasizeBtn = attribute?.isEmphasizeBtn ?? false;
                var parameterInfos = methodInfo.GetParameters();
                this.paramLength = parameterInfos.Length;
                this.parameterDatas = new ParameterData[paramLength];

                using (var provider = new CSharpCodeProvider())
                {
                    string GetFriendlyTypeName(Type t)
                    {
                        string typeName = t.Name;
                        try
                        {
                            if (string.Equals(t.Namespace, ReflectionUtil.SystemNameSpace))
                            {
                                string csFriendlyName = provider.GetTypeOutput(new CodeTypeReference(t));
                                if (csFriendlyName.IndexOf(ReflectionUtil.Dot) == -1)
                                {
                                    typeName = csFriendlyName;
                                }
                            }
                        }
                        catch { }

                        return typeName;
                    }

                    for (int i = 0; i < paramLength; i++)
                    {
                        parameterDatas[i] = new ParameterData(parameterInfos[i], GetFriendlyTypeName);
                    }

                    returnTypeName = GetFriendlyTypeName(methodInfo.ReturnType);
                }

                isOnlyButton = (attribute?.isOnlyButton ?? true) && (paramLength == 0 || !parameterDatas.IsExists(d => d.hasParamDrawHandler));
                guiContent = new GUIContent(displayName
                    , attribute?.tooltip ?? $"Click this button to {(isCoroutine ? "start" : "invoke")} '{methodInfo.Name}' ({(isCoroutine ? "Coroutine" : "Method")}).");

                hasData = true;
            }

            bool isParamValueChanged = false;
            public void DrawParameters()
            {
                for (int i = 0; i < paramLength; ++i)
                {
                    parameterDatas[i].DrawParameter(isOnlyButton, ref isParamValueChanged);
                }
            }

            string lastParamValueStr = null;
            public bool IsClickInvokeBtnGui(out string paramValuesStr)
            {
                if (lastParamValueStr == null || isParamValueChanged)
                {
                    lastParamValueStr = paramValuesStr = string.Join(", ", parameterDatas.ConvertAll(p => StringUtil.ToReadableString(p.paramValue)));
                    string text = $"{displayName}({paramValuesStr})";
                    if (isCoroutine) text = $"StartCoroutine({text})";
                    guiContent.text = text;
                    isParamValueChanged = false;
                }
                else
                    paramValuesStr = lastParamValueStr;

                return GUILayout.Button(guiContent, EditorGUICustomStyle.Button_TextAlignmentLeft, GUILayout.ExpandWidth(true));
            }

            public void ClickInvokeButton(MethodInfo methodInfo, UnityObject target, Type targetType, string paramValuesStr)
            {
                if (!string.IsNullOrEmpty(onMarkedBoolName))
                {
                    BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                    var fieldInfo = targetType.GetField(onMarkedBoolName, bindingFlags | BindingFlags.SetField);

                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(fieldInfo.IsStatic ? null : target, true);
                    }
                    else
                    {
                        var property = targetType.GetProperty(onMarkedBoolName, bindingFlags | BindingFlags.SetProperty);
                        if (property != null)
                            property.SetValue(property.IsStatic() ? null : target, true);
                        else
                            typeof(InvokeButtonAttribute).PrintLogWithClassName($"{onMarkedBoolName}({nameof(InvokeButtonAttribute.onMarkedBoolName)}) is not exists", LogType.Error, obj: target, isPreventOverlapMsg: true, isPreventStackTrace: true);
                    }
                }

                object returnValue = methodInfo.Invoke(target, parameterDatas.ConvertAll(p => p.paramValue));
                if (returnValue != null)
                {
                    if (isCoroutine)
                    {
                        var coroutine = returnValue as IEnumerator;
                        if (Application.isPlaying)
                            (target as MonoBehaviour).StartCoroutine(coroutine);
#if CWJ_EXISTS_EDITORCOROUTINE
                        else
                            EditorCoroutineUtil.StartCoroutine(coroutine, target);
#endif
                    }
                    else
                    {
                        typeof(InvokeButtonAttribute).PrintLogWithClassName($"{displayName}({paramValuesStr.RemoveEnd("\n")})\nreturn value: ".SetColor(new Color().GetCommentsColor()) + StringUtil.ToReadableString(returnValue), LogType.Log, obj: target, isBigFont: false, isPreventOverlapMsg: false, isPreventStackTrace: true);
                    }
                }
            }
        }

        void _DrawInvokeButton_Method(UnityObject target, MethodInfo methodInfo, InvokeBtnData_Method invokeBtnData)
        {
            if (invokeBtnData == null || methodsFoldExpandDict == null) return;

            var visualizeInfo = invokeBtnData.attributeCache.GetVisualizeState(false);
            if (!visualizeInfo.isVisible) return;

            int methodHashCode = invokeBtnData.methodHashCode;

            int paramLength = invokeBtnData.parameterDatas?.Length ?? 0;
            using (var disabledScope = new EditorGUI.DisabledScope(visualizeInfo.isReadonly))
            {
                if (!invokeBtnData.isOnlyButton)
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);

                if (!methodsFoldExpandDict.TryGetValue(methodHashCode, out bool isFoldExpand))
                {
                    isFoldExpand = VolatileEditorPrefs.ExistsStackValue(PrefsKey_Foldout_Method(), GetPrefsValue_Foldout_Method(methodHashCode));
                    methodsFoldExpandDict.Add(methodHashCode, isFoldExpand);
                }

                string foldoutName = $"({invokeBtnData.returnTypeName}) {invokeBtnData.displayName}{(isFoldExpand ? "" : ("(" + string.Join(", ", invokeBtnData.parameterDatas.ConvertAll(p => p.paramCode)) + ")"))}";

                if ((invokeBtnData.isOnlyButton) || (methodsFoldExpandDict[methodHashCode] = EditorGUILayout.Foldout(isFoldExpand, foldoutName, true, EditorGUICustomStyle.Foldout)))
                {
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                    {
                        if (paramLength > 0)
                        {
                            EditorGUILayout.BeginVertical();

                            invokeBtnData.DrawParameters();
                        }

                        bool isClickAndNotError = false;
                        try
                        {
                            if (invokeBtnData.IsClickInvokeBtnGui(out string paramValuesStr))
                            {
                                if (invokeBtnData.isNeedUndoNSave)
                                {
                                    var comps = ((MonoBehaviour)target)?.GetComponents<Component>();
                                    if (comps.LengthSafe() > 0)
                                        Undo.RecordObjects(comps, $"{methodInfo.Name}(in {target.name}) was Invoked Via InvokeButton");
                                }
                                invokeBtnData.ClickInvokeButton(methodInfo, target, targetType, paramValuesStr);
                                isClickAndNotError = true;
                            }
                        }
                        catch (System.Exception ex) when (!(ex.InnerException is ExitGUIException))
                        {
                            DebugLogUtil.PrintLogError(DebugLogUtil.GetRealErrorTrace(ex.ToString()));
                            isClickAndNotError = false;
                        }

                        if (paramLength > 0)
                            EditorGUILayout.EndVertical();

                        if (invokeBtnData.isNeedUndoNSave && isClickAndNotError)
                            SetDirty();
                    }
                }
                if (!invokeBtnData.isOnlyButton)
                    EditorGUILayout.EndVertical();
            }
        }



        InvokeBtnData_Method GetMethodInvokeBtnData(UnityObject targetObj, MethodInfo methodInfo)
        {
            if (!targetObj || methodInfo == null) return null;

            int methodHashCode = methodInfo.GetHashCode();
            if (invokeBtnDataDict_method.TryGetValue(methodHashCode, out var invokeBtnData))
            {
                return invokeBtnData;
            }

            invokeBtnData = new InvokeBtnData_Method(targetObj, methodInfo, methodHashCode);

            if (!invokeBtnData.hasData)
                invokeBtnData = null;

            invokeBtnDataDict_method.Add(methodHashCode, invokeBtnData);

            return invokeBtnData;
        }

        public void DrawInvokeButton_Method(UnityObject target, MethodInfo methodInfo)
        {
            if (invokeBtnDataDict_method == null || methodsFoldExpandDict == null) return;

            _DrawInvokeButton_Method(target, methodInfo, GetMethodInvokeBtnData(target, methodInfo));
        }

        #endregion Method

        // TODO : Delegate
        #region Field (UnityEvent, )
        //private bool[] isFieldsFoldoutOpen;
        //private string GetPrefsKey_Foldout_Field(UnityObject target, int index) => VolatileEditorPrefs.GetVolatilePrefsKey_Child(VolatileEditorPrefs.EKeyType.FoldoutCache, nameof(InvokeButtonAttribute), targetInstanceID, description: ("Field." + index));

        private Dictionary<int, InvokeBtnData_Field> invokeBtnDataDict_field = new Dictionary<int, InvokeBtnData_Field>();

        private class InvokeBtnData_Field
        {
            public readonly bool hasData;
            public readonly int fieldHashCode;
            public EditorGUI_CWJ.MemberAttributeCache attributeCache;
            public string displayName;
            public string tooltip;
            GUIContent guiContent;
            public string onMarkedBoolName;
            public readonly bool isNeedUndoNSave;

            public UnityEvent unityEvent;

            public InvokeBtnData_Field(UnityObject targetObj, FieldInfo fieldInfo, int fieldHashCode)
            {
                if (targetObj == null || fieldInfo == null)
                {
                    hasData = false;
                    return;
                }

                this.fieldHashCode = fieldHashCode;
                var attribute = fieldInfo.GetCustomAttribute<InvokeButtonAttribute>();
                attributeCache = new EditorGUI_CWJ.MemberAttributeCache(fieldInfo, null, targetObj, needDraw: false);

                if (attributeCache.isHideAlways)
                {
                    hasData = false;
                    return;
                }

                onMarkedBoolName = attribute?.onMarkedBoolName ?? null;

                displayName = attribute?.displayName ?? $"<UnityEvent> {fieldInfo.Name}.Invoke()";
                tooltip = attribute?.tooltip ?? $"Click this button to invoke '{fieldInfo.Name}' (UnityEvent).";
                isNeedUndoNSave = attribute?.isNeedUndoNSave ?? true;
                guiContent = new GUIContent(displayName, tooltip);

                unityEvent = fieldInfo.GetValue(targetObj) as UnityEvent;

                if (unityEvent == null)
                {
                    unityEvent = new UnityEvent();
                    fieldInfo.SetValue(targetObj, unityEvent);
                }
                hasData = true;
            }

            public bool DrawInvokeButton()
            {
                return GUILayout.Button(guiContent, GUILayout.ExpandWidth(true));
            }

            public void ClickInvokeButton(UnityObject target, Type targetType)
            {
                if (!string.IsNullOrEmpty(onMarkedBoolName))
                {
                    BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                    var fieldInfo = targetType.GetField(onMarkedBoolName, bindingFlags | BindingFlags.SetField);

                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(fieldInfo.IsStatic ? null : target, true);
                    }
                    else
                    {
                        var property = targetType.GetProperty(onMarkedBoolName, bindingFlags | BindingFlags.SetProperty);
                        if (property != null)
                        {
                            property.SetValue(property.IsStatic() ? null : target, true);
                        }
                        else
                        {
                            typeof(InvokeButtonAttribute).PrintLogWithClassName($"{onMarkedBoolName}({nameof(InvokeButtonAttribute.onMarkedBoolName)}) is not exists", LogType.Error, obj: target, isPreventOverlapMsg: true, isPreventStackTrace: true);
                        }
                    }
                }

                unityEvent.Invoke();
            }
        }

        InvokeBtnData_Field GetFieldInvokeBtnData(UnityObject targetObj, FieldInfo fieldInfo)
        {
            if (targetObj == null || fieldInfo == null) return null;

            int hashCode = fieldInfo.GetHashCode();

            if (invokeBtnDataDict_field.TryGetValue(hashCode, out var invokeBtnData))
            {
                return invokeBtnData;
            }

            invokeBtnData = new InvokeBtnData_Field(targetObj, fieldInfo, hashCode);

            if (!invokeBtnData.hasData)
                invokeBtnData = null;

            invokeBtnDataDict_field.Add(hashCode, invokeBtnData);
            return invokeBtnData;
        }

        void _DrawInvokeButton_Field(UnityObject targetObj, FieldInfo fieldInfo, InvokeBtnData_Field invokeBtnData)
        {
            if (invokeBtnData == null) return;

            var visualizeInfo = invokeBtnData.attributeCache.GetVisualizeState(false);
            if (!visualizeInfo.isVisible) return;

            using (var disabledScope = new EditorGUI.DisabledScope(visualizeInfo.isReadonly))
            {
                EditorGUILayout.BeginHorizontal();

                try
                {
                    if (invokeBtnData.DrawInvokeButton())
                    {
                        if (invokeBtnData.isNeedUndoNSave)
                            Undo.RecordObjects(((MonoBehaviour)target).GetComponents<Component>(), $"{fieldInfo.Name}(in {target.name}) was Invoked Via InvokeButton");
                        invokeBtnData.ClickInvokeButton(target, targetType);
                        //SetDirty();
                    }
                }
                catch (System.Exception ex)
                {
                    DebugLogUtil.PrintLogError(DebugLogUtil.GetRealErrorTrace(ex.ToString()));
                }
                finally
                {
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
            }
        }

        public void DrawInvokeButton_Field(UnityObject targetObj, FieldInfo fieldInfo)
        {
            if (invokeBtnDataDict_field == null) return;

            _DrawInvokeButton_Field(targetObj, fieldInfo, GetFieldInvokeBtnData(targetObj, fieldInfo));
        }

        #endregion
    }

}
