using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    public partial class CWJ_Inspector_Core : Editor
    {
        public bool isSpecialFoldoutExpand;
        private string GetPrefsKey_SpecialFoldout() => VolatileEditorPrefs.GetVolatilePrefsKey_Root("SpecialFoldoutRoot");

        partial void _OnEnable()
        {
            isSpecialFoldoutExpand = VolatileEditorPrefs.ExistsStackValue(GetPrefsKey_SpecialFoldout(), targetInstanceID.ToString());
        }

        partial void _OnDisable(bool isDestroyByUser)
        {
            if (!isDestroyByUser && isSpecialFoldoutExpand && isElementFoldoutExpanded)
            {
                VolatileEditorPrefs.AddStackValue(GetPrefsKey_SpecialFoldout(), targetInstanceID.ToString());
            }
            else
            {
                VolatileEditorPrefs.RemoveStackValue(GetPrefsKey_SpecialFoldout(), targetInstanceID.ToString());
            }
        }

        GUIContent _foldoutContent_root = null;
        GUIContent foldoutContent_root
        {
            get
            {
                if (_foldoutContent_root == null) _foldoutContent_root = new GUIContent("Script ", /*AccessibleEditorUtil.CachedObj.IconTexture,*/ "Open CWJ's hidden tool");
                return _foldoutContent_root;
            }
        }

        GUIContent _foldoutContent_specialFoldout = null;
        GUIContent foldoutContent_specialFoldout
        {
            get
            {
                if (_foldoutContent_specialFoldout == null) _foldoutContent_specialFoldout = new GUIContent(PathUtil.MyProjectName, AccessibleEditorUtil.EditorHelperObj.IconTexture);
                return _foldoutContent_specialFoldout;
            }
        }

        private void DrawSpecialFoldoutGroup()
        {
            bool prevGUIEnabled = GUI.enabled;
            if (!prevGUIEnabled) GUI.enabled = true;

            DrawSingletonWarning();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
#if UNITY_2019_3_OR_NEWER
            isSpecialFoldoutExpand = EditorGUILayout.BeginFoldoutHeaderGroup(isSpecialFoldoutExpand, foldoutContent_root, EditorGUICustomStyle.FoldoutHeader_Big);
#else
            isSpecialFoldoutExpand = EditorGUILayout.Foldout(isSpecialFoldoutExpand, foldoutContent_root, true, EditorGUICustomStyle.FoldoutHeader_Big);
#endif
            if (mScriptProp != null)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    //EditorGUILayout.PropertyField(mScriptProp, true);
                    EditorGUILayout.ObjectField(mScriptProp.objectReferenceValue, typeof(ScriptableObject), false);
                }
            }

#if UNITY_2019_3_OR_NEWER
            EditorGUILayout.EndFoldoutHeaderGroup();
#endif
            EditorGUILayout.EndHorizontal();

            if (isSpecialFoldoutExpand)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(foldoutContent_specialFoldout, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(" Provided  by  CWJ  " + PathUtil.MyVersion, EditorStyles.miniBoldLabel, GUILayout.MinWidth(0));
                EditorGUILayout.EndHorizontal();

                if (targetComp.gameObject.name != targetType.Name)
                {
                    if (GUILayout.Button(new GUIContent("Reset Name", "오브젝트의 이름을 스크립트 이름과 동일하게 수정합니다")))
                    {
                        Undo.RecordObject(targetComp.gameObject, targetComp.gameObject.name + " name changed");
                        targetComp.gameObject.name = targetType.Name;
                    }
                    EditorGUILayout.Space();
                }
                DrawScriptInfoBox();
                DrawSingletonInfoBox();
                DrawDebugSettingBox();
                drawSpecialFoldoutEvent?.Invoke();

                EditorGUILayout.Space(
#if UNITY_2019_3_OR_NEWER
                3
#endif
                );

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(
#if UNITY_2019_3_OR_NEWER
            2
#endif
            );

            if (!prevGUIEnabled) GUI.enabled = false;
        }

        private void DrawScriptInfoBox()
        {
            if (infoBoxAttribute?.info == null) return;
            EditorGUILayout.HelpBox(new GUIContent((infoBoxAttribute.info), /*AccessibleEditorUtil.CachedObj.IconTexture,*/ "CWJ's UnityDevTool"), true);
        }

        private void DrawSingletonWarning()
        {
            if (singleton == null || (!singleton.isDontAutoCreatedWhenNull && !singleton.isDontPreCreatedInScene)) return;
            if (singleton.isDontAutoCreatedWhenNull && singleton.isDontPreCreatedInScene)
            {
                EditorGUILayout.HelpBox($"Do not override '{nameof(Singleton.IDontPrecreatedInScene)}' and '{nameof(Singleton.IDontAutoCreatedWhenNull)}' at the same time!", MessageType.Error, true);
                return;
            }

            if (singleton.isDontPreCreatedInScene)
            {
                EditorGUILayout.HelpBox($"This object cannot be saved.\nthis Singleton override '{nameof(Singleton.IDontPrecreatedInScene)}'", MessageType.Warning, true);
            }
            else if (singleton.isDontSaveInBuild)
            {
                EditorGUILayout.HelpBox($"This object cannot be saved in build.\nthis Singleton override '{nameof(Singleton.IDontSaveInBuild)}'", MessageType.Warning, true);
            }
        }

        bool b_fd_SingletonInfo = false;

        GUIContent _foldoutContent_singleton;
        protected GUIContent foldoutContent_singleton
        {
            get
            {
                if (_foldoutContent_singleton == null) _foldoutContent_singleton = new GUIContent(text: " Singleton Info ");
                return _foldoutContent_singleton;
            }
        }

        const string CheckMark = "✔";
        const string XMark = "✘";
        const string IsInstanceTrue = "이 오브젝트가 Instance로 할당됨";
        const string IsInstanceFalse = "이 오브젝트는 할당된 Instance가 아님\n(할당된 Instance가 없는상태)";
        const string IsAutoCreatedTrue = "Instance 호출을 통해 자동생성된 오브젝트임";
        const string IsAutoCreatedFalse = "이 오브젝트는 자동생성된 오브젝트가 아님\n(씬에 미리 생성해놓은 싱글톤오브젝트)";
        const string IsDontDestroyOnLoadTrue = "DontDestroyOnLoad 싱글톤";
        const string IsDontDestroyOnLoadFalse = "씬 전환시 Destroy되는 싱글톤";
        const string IsOnlyUseNewTrue = "새로운 오브젝트를 Instance로 사용하는 싱글톤";
        const string IsOnlyUseNewFalse = "최초로 생성된 오브젝트를 Instance로 사용하는 기본 싱글톤";
        const string IsDontPreCreatedInSceneTrue = "씬에 미리 생성해 놓을 수 없고, 저장도 안됨";
        const string IsDontPreCreatedInSceneFalse = "씬에 미리 생성가능, 저장가능";
        const string IsDontAutoCreatedWhenNullTrue = "호출시 씬에 존재하지 않더라도 자동생성 되지 않는 싱글톤\n(씬에 미리 생성해놔야 사용 가능)";
        const string IsDontAutoCreatedWhenNullFalse = "호출시 씬에 존재하지 않으면 자동생성됨";
        const string IsDontGetInstanceInEditorModeTrue = "실행중이 아닐땐 호출이 막히는 싱글톤";
        const string IsDontGetInstanceInEditorModeFalse = "실행중이 아닐때도 호출가능";
        const string IsDontSaveInBuildTrue = "빌드파일에 저장이 되지않음";
        const string IsDontSaveInBuildFalse = "빌드파일에 저장됨";

        private void DrawSingletonInfoBox()
        {
            if (singleton == null) return;

            if (infoBoxAttribute != null && infoBoxAttribute.info == null)
            {
                DrawScriptInfoBox();
            }
            foldoutContent_singleton.text = declareTypeName;
            EditorGUI_CWJ.DrawBigFoldout(ref b_fd_SingletonInfo, foldoutContent_singleton, (isExpand) =>
            {
                if (isExpand)
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        bool chk = singleton.isInstance;

                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isInstance), $"[{(chk ? CheckMark : XMark)}] ={(chk ? IsInstanceTrue : IsInstanceFalse)}"), singleton.isInstance);
                        if (!singleton.isDontAutoCreatedWhenNull)
                        {
                            chk = singleton.isAutoCreated;
                            EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isAutoCreated), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsAutoCreatedTrue : IsAutoCreatedFalse)}"), singleton.isAutoCreated);
                        }
                        EditorGUILayout.Space();
                        chk = singleton.isDontDestroyOnLoad;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isDontDestroyOnLoad), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsDontDestroyOnLoadTrue : IsDontDestroyOnLoadFalse)}"), chk);
                        chk = singleton.isOnlyUseNew;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isOnlyUseNew), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsOnlyUseNewTrue : IsOnlyUseNewFalse)}"), chk);
                        chk = singleton.isDontPreCreatedInScene;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isDontPreCreatedInScene), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsDontPreCreatedInSceneTrue : IsDontPreCreatedInSceneFalse)}"), chk);
                        chk = singleton.isDontAutoCreatedWhenNull;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isDontAutoCreatedWhenNull), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsDontAutoCreatedWhenNullTrue : IsDontAutoCreatedWhenNullFalse)}"), chk);
                        chk = singleton.isDontGetInstanceInEditorMode;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isDontGetInstanceInEditorMode), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsDontGetInstanceInEditorModeTrue : IsDontGetInstanceInEditorModeFalse)}"), chk);
                        chk = singleton.isDontSaveInBuild;
                        EditorGUILayout.Toggle(new GUIContent(nameof(singleton.isDontSaveInBuild), $"[{(chk ? CheckMark : XMark)}] = {(chk ? IsDontSaveInBuildTrue : IsDontSaveInBuildFalse)}"), chk);

                    }
                }
            });
        }

        //TODO: TODO!!!!!!
        #region Debug Setting 

        bool isTypeIgnoreCWJ_Debug = false;
        bool IsTypeIgnoreCWJ_Debug
        {
            get => isTypeIgnoreCWJ_Debug;
            set
            {
                if (isTypeIgnoreCWJ_Debug == value) return;
                CWJ_Debug.RegistIgnoreType(compTypeHashCode, value);
                isTypeIgnoreCWJ_Debug = value;
            }
        }

        bool isTypeAllowCWJ_Debug = false;
        bool IsTypeAllowCWJ_Debug
        {
            get => isTypeAllowCWJ_Debug;
            set
            {
                if (isTypeAllowCWJ_Debug == value) return;
                CWJ_Debug.RegistAllowType(compTypeHashCode, value);
                isTypeAllowCWJ_Debug = value;
            }
        }

        bool isObjIgnoreCWJ_Debug = false;
        bool IsObjIgnoreCWJ_Debug
        {
            get => isObjIgnoreCWJ_Debug;
            set
            {
                if (isObjIgnoreCWJ_Debug == value) return;
                CWJ_Debug.RegistIgnoreObj(targetInstanceID, value);
                isObjIgnoreCWJ_Debug = value;
            }
        }

        bool isObjAllowCWJ_Debug = false;
        bool IsObjAllowCWJ_Debug
        {
            get => isObjAllowCWJ_Debug;
            set
            {
                if (isObjAllowCWJ_Debug == value) return;
                CWJ_Debug.RegistAllowObj(targetInstanceID, value);
                isObjAllowCWJ_Debug = value;
            }
        }

        bool fd_debugSetting = false;
        GUIContent _foldoutContent_debugSetting;
        protected GUIContent foldoutContent_debugSetting
        {
            get
            {
                if (_foldoutContent_debugSetting == null) _foldoutContent_debugSetting = new GUIContent(text: " Debug Setting (공사중)");
                return _foldoutContent_debugSetting;
            }
        }
        private void DrawDebugSettingBox()
        {
            EditorGUI_CWJ.DrawBigFoldout(ref fd_debugSetting, foldoutContent_debugSetting, (isExpand) =>
            {
                if (isExpand)
                {
                    //Type
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = false;
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = true;
                    CWJ_Debug.IsIgnoreSpecifiedType_prev = CWJ_Debug.IsIgnoreSpecifiedType;
                    CWJ_Debug.IsIgnoreSpecifiedType = EditorGUILayout.ToggleLeft(new GUIContent(nameof(CWJ_Debug.IsIgnoreSpecifiedType), $"true: <{targetType.Name}> Type 에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), CWJ_Debug.IsIgnoreSpecifiedType);
                    using (new EditorGUI.DisabledScope(!CWJ_Debug.IsIgnoreSpecifiedType))
                    {
                        EditorGUI.indentLevel++;
                        IsTypeIgnoreCWJ_Debug = EditorGUILayout.ToggleLeft(new GUIContent($"ignore this component ({target.name}.<{targetType.Name}>)", $"true: 이 '{target.name}' 오브젝트의\n<{targetType.Name}>에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), IsTypeIgnoreCWJ_Debug);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();

                    GUI.enabled = false;
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = true;
                    CWJ_Debug.IsAllowSpecifiedType_prev = CWJ_Debug.IsAllowSpecifiedType;
                    CWJ_Debug.IsAllowSpecifiedType = EditorGUILayout.ToggleLeft(new GUIContent(nameof(CWJ_Debug.IsAllowSpecifiedType), $"true: allow 된 <{targetType.Name}> Type 외에는\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), CWJ_Debug.IsAllowSpecifiedType);
                    using (new EditorGUI.DisabledScope(!CWJ_Debug.IsAllowSpecifiedType))
                    {
                        EditorGUI.indentLevel++;
                        IsTypeAllowCWJ_Debug = EditorGUILayout.ToggleLeft(new GUIContent($"allow this component ({target.name}.<{targetType.Name}>)", $"true: <{targetType.Name}> Type 에서만\nCWJ_Debug.가 실행됩니다"), IsTypeAllowCWJ_Debug);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();

                    SwitchingToggle(ref CWJ_Debug.IsIgnoreSpecifiedType_prev, ref CWJ_Debug.IsIgnoreSpecifiedType,
                        ref CWJ_Debug.IsAllowSpecifiedType_prev, ref CWJ_Debug.IsAllowSpecifiedType);
                    EditorGUILayout.EndVertical();

                    //Obj
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = false;
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = true;
                    CWJ_Debug.IsIgnoreSpecifiedObj_prev = CWJ_Debug.IsIgnoreSpecifiedObj;
                    CWJ_Debug.IsIgnoreSpecifiedObj = EditorGUILayout.ToggleLeft(new GUIContent(nameof(CWJ_Debug.IsIgnoreSpecifiedObj), $"true: 이 '{target.name}' 오브젝트의\n<{targetType.Name}>에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), CWJ_Debug.IsIgnoreSpecifiedObj);
                    using (new EditorGUI.DisabledScope(!CWJ_Debug.IsIgnoreSpecifiedObj))
                    {
                        EditorGUI.indentLevel++;
                        IsObjIgnoreCWJ_Debug = EditorGUILayout.ToggleLeft(new GUIContent($"ignore this component ({target.name}.<{targetType.Name}>)", $"true: 이 '{target.name}' 오브젝트의\n<{targetType.Name}>에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), IsObjIgnoreCWJ_Debug);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();

                    GUI.enabled = false;
                    EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
                    GUI.enabled = true;
                    CWJ_Debug.IsAllowSpecifiedObj_prev = CWJ_Debug.IsAllowSpecifiedObj;
                    CWJ_Debug.IsAllowSpecifiedObj = EditorGUILayout.ToggleLeft(new GUIContent(nameof(CWJ_Debug.IsAllowSpecifiedObj), $"true: 이 '{target.name}' 오브젝트의\n<{targetType.Name}>에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), CWJ_Debug.IsAllowSpecifiedObj);
                    using (new EditorGUI.DisabledScope(!CWJ_Debug.IsAllowSpecifiedObj))
                    {
                        EditorGUI.indentLevel++;
                        IsObjAllowCWJ_Debug = EditorGUILayout.ToggleLeft(new GUIContent($"allow this component ({target.name}.<{targetType.Name}>)", $"true: 이 '{target.name}' 오브젝트의\n<{targetType.Name}>에서\nCWJ_Debug.를 사용한 함수가 실행되지 않게함"), IsObjAllowCWJ_Debug);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();

                    SwitchingToggle(ref CWJ_Debug.IsIgnoreSpecifiedObj_prev, ref CWJ_Debug.IsIgnoreSpecifiedObj,
                        ref CWJ_Debug.IsAllowSpecifiedObj_prev, ref CWJ_Debug.IsAllowSpecifiedObj);
                    EditorGUILayout.EndVertical();
                }
            });
        }

        private void SwitchingToggle(ref bool a_prevValue, ref bool a_curValue,
                               ref bool b_PrevValue, ref bool b_CurValue)
        {
            if (a_prevValue != a_curValue)
            {
                if (a_curValue)
                {
                    b_PrevValue = b_CurValue = false;
                }
                a_prevValue = a_curValue;
            }

            if (b_PrevValue != b_CurValue)
            {
                if (b_CurValue)
                {
                    a_prevValue = a_curValue = false;
                }
                b_PrevValue = b_CurValue;
            }
        }
        #endregion Debug Setting
    }
}