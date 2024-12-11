using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using CWJ.AccessibleEditor;
using UnityEngine.Events;

namespace CWJ.EditorOnly.Inspector
{
    /// <summary>
    /// Foldout && Default
    /// </summary>
    public class CWJ_Inspector_BodyAndFoldout : CWJ_Inspector_ElementAbstract, CWJ_Inspector_ElementAbstract.IUseMemberInfo<MethodInfo> /*, CWJInspector_ElementAbstract.IUseMemberInfo<FieldInfo>*/
    {
        protected override string ElementClassName => nameof(CWJ_Inspector_BodyAndFoldout);
        public CWJ_Inspector_BodyAndFoldout(CWJ_Inspector_Core inspectorCore, (SerializedProperty[] serializedProperties, FieldInfo[] fieldInfos) propPackage) : base(inspectorCore, true, false)
        {
            this.serializedProperties = propPackage.serializedProperties;
            this.fieldInfos = propPackage.fieldInfos;
        }

        private SerializedProperty[] serializedProperties;
        private FieldInfo[] fieldInfos;

        protected override bool HasMemberToDraw()
        {
            InitPropCache();
            hasInvokeBtnAtt = useMethodInfo.memberInfoArray.Length > 0;
            return (hasInvokeBtnAtt || thisFoldoutAttCaches.Count > 0 || baseFoldoutAttCaches.Count > 0);
        }

        protected override void OnEndClassify()
        {
            foreach (var cache in baseFoldoutAttCaches)
            {
                cache.Value.EndClassify();
            }
            headerProps = _headerPropList.ToArray();
            hasBaseClassVariables = headerProps.Length > 0 || baseFoldoutAttCaches.Count > 0;

            foreach (var cache in thisFoldoutAttCaches)
            {
                cache.Value.EndClassify();
            }
            bodyProps = _bodyPropList.ToArray();
            hasThisClassVariables = bodyProps.Length > 0 || thisFoldoutAttCaches.Count > 0;
        }

        //Predicate<FieldInfo> IUseMemberInfo<FieldInfo>.SetMemberInfoPredicate()
        //{
        //    return (fi => !fi.IsAutoPropertyField() && fi.IsSerializeField());
        //}

        //List<FieldInfo> IUseMemberInfo<FieldInfo>._memberInfoList { get; set; }
        //Predicate<FieldInfo> IUseMemberInfo<FieldInfo>.memberInfoPredicate { get; set; }
        //FieldInfo[] IUseMemberInfo<FieldInfo>.memberInfoArray { get; set; }

        public override int drawOrder => 0;

        public bool hasInvokeBtnAtt { get; private set; } = false;
        //public bool hasVisualizeFieldAtt { get; private set; } = false; // TODO
        //public bool hasVisualizePropAtt { get; private set; } = false; // TODO
        readonly Type InvokeButtonAttType = typeof(InvokeButtonAttribute);
        readonly Type FoldoutAttType = typeof(_Root_FoldoutAttribute);
        bool IUseMemberInfo<MethodInfo>.MemberInfoClassifyPredicate(MethodInfo info)
        {
            return info.IsDefined(FoldoutAttType, true) && info.IsDefined(InvokeButtonAttType, true);
        }

        MethodInfo[] IUseMemberInfo<MethodInfo>.memberInfoArray { get; set; }

        protected override void DrawInspector()
        {
            if (hasBaseClassVariables)
            {
                EditorGUI_CWJ.DrawLineAndHeader(headerContent: new GUIContent("Base", $"Variables declared in \n<{targetType.Name}>'s Base classes"));
            }

            DrawFoldout(baseFoldoutAttCaches);

            //header prop
            if (headerProps.Length > 0)
            {
                DrawProps(headerProps);
            }

            if (hasBaseClassVariables && hasThisClassVariables)
            {
                EditorGUI_CWJ.DrawLineAndHeader(headerContent: new GUIContent("This", $"Variables declared in <{targetType.Name}>"));
            }

            DrawFoldout(thisFoldoutAttCaches);

            //body prop
            if (bodyProps.Length > 0)
            {
                DrawProps(bodyProps);
            }

            //if (inspectorCore.isElemsOccupied)
            //{
            //    EditorGUI_CWJ.DrawLine(topPadding: 1, bottomPadding: 1);
            //}
        }
        protected override void _OnEnable()
        {

        }
        protected override void _OnDisable(bool isDestroy)
        {
            WriteFoldoutCache(isDestroy);
        }

        private string GetPrefsValue_Foldout_FoldoutCache(string foldoutName) => targetInstanceIDStr + "." + foldoutName;

        private void WriteFoldoutCache(bool isDestroy)
        {
            Action<bool, Dictionary<string, FoldoutAttCache>> writeAction = (isBaseFoldout, foldoutAttCaches) =>
            {
                string prefsKey = PrefsKey_Foldout_Child(isBaseFoldout);
                foreach (var c in foldoutAttCaches)
                {
                    if (!isDestroy && c.Value.expanded && c.Value?.propInfos?.Length > 0)
                    {
                        VolatileEditorPrefs.AddStackValue(prefsKey, GetPrefsValue_Foldout_FoldoutCache(c.Value.attribute.name));
                    }
                    else
                    {
                        VolatileEditorPrefs.RemoveStackValue(prefsKey, GetPrefsValue_Foldout_FoldoutCache(c.Value.attribute.name));
                    }
                    c.Value.Dispose();
                }
            };
            writeAction(true, baseFoldoutAttCaches);
            writeAction(false, thisFoldoutAttCaches);
        }

        private class FoldoutAttCache
        {
            public FoldoutAttCache(_Root_FoldoutAttribute attribute)
            {
                this.attribute = attribute;
                _fieldNames = new HashSet<string>();
                _requiredFieldInfoList = new List<FieldInfo>();
                _methodInfoList = new List<MethodInfo>();
                _propInfoList = new List<PropCache>();
            }

            public _Root_FoldoutAttribute attribute;
            public HashSet<string> _fieldNames = null;

            public List<PropCache> _propInfoList = null;
            public PropCache[] propInfos;

            public List<FieldInfo> _requiredFieldInfoList = null; //RequiredFieldAttribute 체크용도
            private FieldInfo[] requiredFieldInfos;

            public List<MethodInfo> _methodInfoList = null;
            public MethodInfo[] methodInfos;

            public bool expanded;

            public void AddRangeOtherCahceList(FoldoutAttCache other)
            {
                _fieldNames.AddRange(other._fieldNames);
                _propInfoList.AddRange(other._propInfoList);
                _requiredFieldInfoList.AddRange(other._requiredFieldInfoList);
                _methodInfoList.AddRange(other._methodInfoList);
            }

            public bool GetIsEmptyOfRequiredField(UnityEngine.Object targetObj)
            {
                for (int i = 0; i < requiredFieldInfos.Length; i++)
                {
                    if (requiredFieldInfos[i] == null) continue;

                    if (requiredFieldInfos[i].IsNull(targetObj))
                    {
                        return true;
                    }
                }
                return false;
            }

            public void EndClassify()
            {
                propInfos = _propInfoList.ToArray();
                _propInfoList = null;
                requiredFieldInfos = _requiredFieldInfoList.ToArray();
                _requiredFieldInfoList = null;
                methodInfos = _methodInfoList.ToArray();
                _methodInfoList = null;
            }

            public void Dispose()
            {
                attribute = null;
                _fieldNames = null;
                _propInfoList = null;
                _requiredFieldInfoList = null;
                _methodInfoList = null;
            }
        }

        private string PrefsKey_Foldout_Child(bool isBaseFoldout) => VolatileEditorPrefs.GetVolatilePrefsKey_Child(nameof(FoldoutAttribute) + $"({(isBaseFoldout ? "Base" : "This")})", description: "FoldoutExpanded");

        public class PropCache
        {
            public SerializedProperty prop;
            public FieldInfo fieldInfo;

            private EditorGUI_CWJ.DrawVariousTypeHandler variousTypeDrawer;
            public Action<UnityEngine.Object, UnityEngine.Object[]> DrawPropAction = null;

            public PropCache(SerializedProperty prop, FieldInfo fieldInfo, UnityEngine.Object targetObj, int targetInstanceID)
            {
                this.prop = prop;
                this.fieldInfo = fieldInfo;

                bool? isDrawArrayOrList = null;

                if (/*!fieldInfo.IsDefined(typeof(UnityBuiltInDrawerAttribute), true) ||*/ (fieldInfo.IsDefined(typeof(_VisualizeConditionalAttribute), true) || fieldInfo.IsDefined(typeof(Root_ReadonlyAttribute), true)))
                {
                    if (fieldInfo.FieldType.IsArray)
                    {
                        isDrawArrayOrList = true;
                    }
                    else if (fieldInfo.FieldType.IsGenericList())
                    {
                        isDrawArrayOrList = false;
                    }
                    else
                    { //List와 array외의 지원안되는 배열일수도..
                        isDrawArrayOrList = null;
                    }
                }

                if (isDrawArrayOrList == null)
                {
                    //#error 211019
                    //!체크해야함
                    DrawPropAction = (target, targets) => EditorGUILayout.PropertyField(this.prop, true);
                }
                else
                {
                    variousTypeDrawer = EditorGUI_CWJ.GetArrayElemDrawVariousTypeDelegate(isDrawArrayOrList.Value, this.fieldInfo.FieldType, this.fieldInfo.Name, this.fieldInfo.GetValue(this.fieldInfo.IsStatic ? null : targetObj));

                    if (isDrawArrayOrList.Value) //Array
                    {
                        DrawPropAction = (target, targets) =>
                        {
                            EditorGUI_CWJ.DrawVariousArrayType(this.fieldInfo, this.prop.displayName, target, targets, targetInstanceID, variousTypeDrawer);
                        };
                    }
                    else //List
                    {
                        DrawPropAction = (target, targets) =>
                        {
                            EditorGUI_CWJ.DrawVariousListType(this.fieldInfo, this.prop.displayName, target, targets, targetInstanceID, variousTypeDrawer);
                        };
                    }
                }
            }
        }

        private List<PropCache> _headerPropList = new List<PropCache>();
        private PropCache[] headerProps = null;
        private bool hasBaseClassVariables;

        private List<PropCache> _bodyPropList = new List<PropCache>();
        private PropCache[] bodyProps = null;
        private bool hasThisClassVariables;

        public struct FoldoutAttKey : IEquatable<FoldoutAttKey>
        {
            public bool isBaseField;
            public string key;

            public FoldoutAttKey(bool isBaseField, string key)
            {
                this.isBaseField = isBaseField;
                this.key = key;
            }

            public bool Equals(FoldoutAttKey other) => isBaseField == other.isBaseField && string.Equals(key, other.key);
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is FoldoutAttKey && Equals((FoldoutAttKey)obj);
            }

            public override int GetHashCode()
            {
                return HashCodeHelper.GetHashCode(isBaseField, key);
            }
        }

        private Dictionary<string, FoldoutAttCache> baseFoldoutAttCaches = new Dictionary<string, FoldoutAttCache>();
        private Dictionary<string, FoldoutAttCache> thisFoldoutAttCaches = new Dictionary<string, FoldoutAttCache>();

        private void InitPropCache()
        {
            Dictionary<FoldoutAttKey, FoldoutAttCache> allFoldoutCaches = new Dictionary<FoldoutAttKey, FoldoutAttCache>();
            List<string> ignoreMergeFoldoutNames = new List<string>();
            _Root_FoldoutAttribute prevFold = null;
            
            for (int i = 0; i < fieldInfos.Length; ++i)
            {
                if (fieldInfos[i] == null) continue;
                var foldoutAtt = fieldInfos[i].GetCustomAttribute<_Root_FoldoutAttribute>();

                FoldoutAttCache cache;
                string fieldName = fieldInfos[i].Name;
                if (foldoutAtt == null)
                {
                    if (prevFold != null && prevFold.isGroupFoldout && prevFold.isSectionOpen)
                    {
                        var foldoutInfoKey = new FoldoutAttKey(isBaseField: !fieldInfos[i].IsDeclaredTargetType(targetType), key: prevFold.name);

                        if (allFoldoutCaches.TryGetValue(foldoutInfoKey, out cache))
                        {
                            cache._fieldNames.Add(fieldName);
                        }
                        else
                        {
                            allFoldoutCaches.Add(foldoutInfoKey, new FoldoutAttCache(prevFold) { _fieldNames = new HashSet<string> { fieldName } });
                        }
                    }
                }
                else
                {
                    if (!foldoutAtt.isMergeParentFoldout)
                    {
                        ignoreMergeFoldoutNames.Add(foldoutAtt.name);
                    }
                    prevFold = foldoutAtt;

                    var foldoutInfoKey = new FoldoutAttKey(isBaseField: !fieldInfos[i].IsDeclaredTargetType(targetType), key: foldoutAtt.name);

                    if (!allFoldoutCaches.TryGetValue(foldoutInfoKey, out cache))
                    {
                        bool expanded = VolatileEditorPrefs.ExistsStackValue(PrefsKey_Foldout_Child(foldoutInfoKey.isBaseField), GetPrefsValue_Foldout_FoldoutCache(foldoutAtt.name));
                        allFoldoutCaches.Add(foldoutInfoKey,
                            new FoldoutAttCache(foldoutAtt) { _fieldNames = new HashSet<string> { fieldName }, expanded = expanded });
                    }
                    else cache._fieldNames.Add(fieldName);
                }
            }

            foreach (var method in useMethodInfo.memberInfoArray)
            {
                var foldoutAtt = method.GetCustomAttribute<_Root_FoldoutAttribute>();

                var foldoutInfoKey = new FoldoutAttKey(isBaseField: !method.IsDeclaredTargetType(targetType), key: foldoutAtt.name);

                if (!allFoldoutCaches.TryGetValue(foldoutInfoKey, out FoldoutAttCache cache))
                {
                    bool expanded = VolatileEditorPrefs.ExistsStackValue(PrefsKey_Foldout_Child(foldoutInfoKey.isBaseField), GetPrefsValue_Foldout_FoldoutCache(foldoutAtt.name));
                    cache = new FoldoutAttCache(foldoutAtt) { expanded = expanded };
                    cache._methodInfoList.Add(method);
                    allFoldoutCaches.Add(foldoutInfoKey, cache);
                }
                else cache._methodInfoList.Add(method);
            }

            for (int i = 0; i < serializedProperties.Length; i++)
            {
                bool hasFoldoutAttribute = false;
                bool isBaseClassField = false;

                string propName = serializedProperties[i].name;

                FieldInfo field = fieldInfos[i];
                if (field == null) { /*Debug.LogError(serializedProperties[i].propertyPath);*/ continue; }
                isBaseClassField = !field.IsDeclaredTargetType(targetType);

                //var pair = allFoldoutCaches.FirstOrDefault(x => x.Value._fieldNames.Contains(propName));
                foreach (var pair in allFoldoutCaches)
                {
                    if (pair.Value._fieldNames.Contains(propName)) //foldoutAttribute를 갖고있는 경우
                    {
                        if (pair.Key.isBaseField == isBaseClassField)
                            hasFoldoutAttribute = true;
                        pair.Value._propInfoList.Add(new PropCache(serializedProperties[i], field, target, targetInstanceID));

                        if (field?.GetCustomAttribute<ErrorIfNullAttribute>() != null)
                        {
                            pair.Value._requiredFieldInfoList.Add(field);
                        }
                        break;
                    }
                }

                if (!hasFoldoutAttribute)
                {
                    PropCache propInfo = new PropCache(serializedProperties[i], field, target, targetInstanceID);
                    if (isBaseClassField) // base 클래스
                    {
                        _headerPropList.Add(propInfo);
                    }
                    else
                    {
                        _bodyPropList.Add(propInfo);
                    }
                }
            }


            foreach (var pair in allFoldoutCaches)
            {
                if (pair.Key.isBaseField)
                {
                    baseFoldoutAttCaches.Add(pair.Key.key, pair.Value);
                }
                else
                {
                    thisFoldoutAttCaches.Add(pair.Key.key, pair.Value);
                }
            }

            foreach (var basePair in baseFoldoutAttCaches)
            {
                if (ignoreMergeFoldoutNames.IsExists(basePair.Key))
                {
                    continue;
                }

                if (thisFoldoutAttCaches.TryGetValue(basePair.Key, out var thisAttCache))
                {
                    basePair.Value.AddRangeOtherCahceList(thisAttCache);
                    thisAttCache.Dispose();
                    thisFoldoutAttCaches.Remove(basePair.Key);
                }
            }
        }

        private void DrawFoldout(Dictionary<string, FoldoutAttCache> foldoutCaches)
        {
            foreach (var pair in foldoutCaches)
            {
                if (pair.Value.propInfos?.Length == 0) continue;

                var r = EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                if (!pair.Value.expanded && pair.Value.GetIsEmptyOfRequiredField(target))
                {
                    pair.Value.expanded = true;
                }

                //pair.Value.expanded = EditorGUILayout.BeginFoldoutHeaderGroup(pair.Value.expanded, pair.Value.attribute.name, EditorGUICustomStyle.FoldoutHeader_Big);
                if (pair.Value.expanded = EditorGUILayout.Foldout(pair.Value.expanded, pair.Value.attribute.name, true, EditorGUICustomStyle.Foldout))
                {
                    EditorGUI_CWJ.DrawLine(refRect: r, isSameToRefRectY: false);

                    if (pair.Value.expanded)
                    {
                        DrawProps(pair.Value.propInfos);

                        DrawInvokeBtn(pair.Value.methodInfos);
                    }

                }
                //EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawProps(PropCache[] propCaches)
        {
            if (propCaches.Length == 0) return;

            using(var changeScope = new EditorGUI.ChangeCheckScope())
            {
                foreach (var item in propCaches)
                {
                    item.DrawPropAction(target, targets);
                }

                if (changeScope.changed)
                    serializedObject.ApplyModifiedProperties();
            }
        }


        private void DrawInvokeBtn(MethodInfo[] methods)
        {
            foreach (var m in methods)
                inspectorCore.invokeBtnElem.DrawInvokeButton_Method(target, m);
        }
    }
}