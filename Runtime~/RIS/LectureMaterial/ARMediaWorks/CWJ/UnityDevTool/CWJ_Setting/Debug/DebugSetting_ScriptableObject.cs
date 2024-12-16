#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Text;

using UnityEditor;
using System.Linq;
using UnityEngine.Events;

using CEC.ExposedUnityEvents;

/// <summary>
/// UNITY_EDITOR 필수 namespace
/// </summary>
namespace CWJ.AccessibleEditor.DebugSetting
{
    using static CWJDefineSymbols;

    [Serializable] public class ExposedBoolEvent : ExposedUnityEvent<bool> { }
    
    public enum ESettingName
    {
        Null,
        isLogEnabled,
        isLogDisabledInBuild,
        isSaveLogEnabled,
        isEditorDebugEnabled,
        isRuntimeDebuggingDisabled
    }


    [Serializable]
    public struct DefineSettingStruct
    {
        [UnityEngine.HideInInspector]
        public string name;
        public ESettingName settingName;
        public string titleName;
        /// <summary>
        /// on/ off 시 같이 추가/삭제 될 Symbols
        /// </summary>
        public string[] symbolNames;

        /// <summary>
        /// on 시 삭제될 Symbols
        /// </summary>
        public string[] oppositeSymbolNames;

        public bool value;
        public bool backupValue;
        public bool isChanged => value != backupValue;

        public string[] descriptions;
        public string description => descriptions?.Length > 0 ? (descriptions[backupValue ? 0 : 1]) : "";

        /// <summary>
        /// 작동되려면 켜져있어야하는 부모이름
        /// </summary>
        public ESettingName condition_enabledParentName;
        /// <summary>
        /// 작동되려면 비활성화돼있어야하는 부모이름
        /// </summary>
        public ESettingName condition_disabledParentName;

        public string necessarySymbol;

        //public ExposedBoolEvent confirmEvent;
        public UnityEvent_Bool confirmEvent;

        public DefineSettingStruct(ESettingName settingName, string titleName, string[] symbolNames, string[] oppositeSymbolNames, bool defaultValue, string[] descriptions, ESettingName condition_enabledParentName = ESettingName.Null, ESettingName condition_disabledParentName = ESettingName.Null, string necessarySymbol = "", UnityAction<bool> confirmAction = null)
        {
            this.name = settingName.ToString();
            this.settingName = settingName;
            this.titleName = titleName;
            this.symbolNames = symbolNames;
            this.oppositeSymbolNames = oppositeSymbolNames;
            this.backupValue = this.value = defaultValue;
            this.descriptions = descriptions;
            this.condition_enabledParentName = condition_enabledParentName;
            this.condition_disabledParentName = condition_disabledParentName;
            this.necessarySymbol = necessarySymbol;
            this.confirmEvent = null;
            if (confirmAction != null)
            {
                this.confirmEvent = new UnityEvent_Bool();
                this.confirmEvent.AddListener_New(confirmAction);
                this.confirmEvent.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
                //    this.confirmEvent = new ExposedBoolEvent();
                //    this.confirmEvent.AddPersistentListener(confirmAction, UnityEventCallState.EditorAndRuntime);
            }
        }

        public void Init()
        {
            backupValue = value = false;
            confirmEvent?.Invoke(false);
        }

        public void Confirm()
        {
            value = backupValue;
            confirmEvent?.Invoke(value);
        }

        public void Undo()
        {
            backupValue = value;
        }

    }

    //ScriptableObject 클래스의 이름과 cs파일 이름과 같아야함 (자체자작한 WindowBehaviour때문)
    public class DebugSetting_ScriptableObject : CachedSymbol_ScriptableObject
    {
        public override bool IsAutoReset => false;

        [Readonly] public DefineSettingStruct[] defineSettingStructs;

        public DefineSettingStruct? FindSettingStruct(ESettingName name)
        {
            int index = defineSettingStructs.FindIndex(s => s.settingName.Equals(name));
            return index >= 0 ? (DefineSettingStruct?)defineSettingStructs[index] : null;
        }

        public void SetSettingStructBackupValue(ESettingName name, bool backupValue)
        {
            int index = defineSettingStructs.FindIndex(s => s.settingName.Equals(name));
            if (index >= 0)
            {
                defineSettingStructs[index].backupValue = backupValue;
            }
        }

        public IEnumerable<(string title, IGrouping<string, DefineSettingStruct> settings)> GetSettingStructGroupByTitle()
        {
            return from s in defineSettingStructs
                   group s by s.titleName into g
                   select (g.Key, g);
        }

        public bool CheckIsDisableCondition(DefineSettingStruct setting)
        {
            bool isNeedDisable = false;
            if (setting.condition_enabledParentName != ESettingName.Null)
            {
                var parentSetting = FindSettingStruct(setting.condition_enabledParentName);
                isNeedDisable = parentSetting != null ? !parentSetting.Value.backupValue : false;
            }
            if (setting.condition_disabledParentName != ESettingName.Null)
            {
                var parentSetting = FindSettingStruct(setting.condition_disabledParentName);
                isNeedDisable |= parentSetting != null ? parentSetting.Value.backupValue : false;
            }
            if (!string.IsNullOrEmpty(setting.necessarySymbol))
            {
                isNeedDisable |= !DefineSymbolUtil.GetCurrentSymbols().Contains(setting.necessarySymbol);
            }
            return isNeedDisable;
        }

        public override void OnReset(bool isNeedSave = false)
        {
            defineSettingStructs = new DefineSettingStruct[EnumUtil.GetLength<ESettingName>() - 1];

            defineSettingStructs[0] =
                new DefineSettingStruct(
                    settingName: ESettingName.isLogEnabled,
                    titleName: "Debug Log",
                    symbolNames: null,
                    oppositeSymbolNames: null,
                    defaultValue: true,
                    descriptions: new string[] { "//Enable logging", "//Disable logging (Exception: 'UnityEngine.Debug.Function();')" });

            defineSettingStructs[1] =
                new DefineSettingStruct(
                    settingName: ESettingName.isLogDisabledInBuild,
                    titleName: "Debug Log",
                    symbolNames: new string[] { CWJ_LOG_DISABLED_IN_BUILD },
                    oppositeSymbolNames: new string[] { CWJ_LOG_DISABLED },
                    defaultValue: false,
                    descriptions: new string[] { "//Disabled logging only 'Release build'", "//Disable logging in both the 'Unity editor' and 'Release build'" },
                    condition_disabledParentName: ESettingName.isLogEnabled);

            defineSettingStructs[2] =
                new DefineSettingStruct(
                    settingName: ESettingName.isSaveLogEnabled,
                    titleName: "Debug Log",
                    symbolNames: new string[] { CWJ_LOG_SAVE, UNITY_ASSERTIONS },
                    oppositeSymbolNames: null,
                    defaultValue: false,
                    descriptions: new string[] { "//Enable save log to .txt file", "//Disable save log to .txt file" },
                    condition_enabledParentName: ESettingName.isLogEnabled,
                    confirmAction: OnConfirmed_SaveLogEnabled);

            defineSettingStructs[3] =
                new DefineSettingStruct(
                    settingName: ESettingName.isEditorDebugEnabled,
                    titleName: "EditorOnly",
                    symbolNames: new string[] { CWJ_EDITOR_DEBUG_ENABLED },
                    oppositeSymbolNames: null,
                    defaultValue: false,
                    descriptions: null);

            defineSettingStructs[4] =
                new DefineSettingStruct(
                    settingName: ESettingName.isRuntimeDebuggingDisabled,
                    titleName: "RuntimeDebugging",
                    symbolNames: new string[] { CWJ_RUNTIMEDEBUGGING_DISABLED },
                    oppositeSymbolNames: null,
                    defaultValue: false,
                    descriptions: null,
                    confirmAction: OnConfirmed_RuntimeDebuggingDisabled);

            base.OnReset(isNeedSave);
        }

        public bool UpdateSetting(bool isProjectInit = false)
        {
            bool isModified = false;

            var addSymbols = new List<string>();
            var removeSymbols = new List<string>();

            if (!isInitialized || defineSettingStructs.Length == 0 || defineSettingStructs[0].settingName == ESettingName.Null) //초기화, Default값
            {
                OnReset();
            }

            for (int i = 0; i < defineSettingStructs.Length; i++)
            {
                if (CheckIsDisableCondition(defineSettingStructs[i]))
                {
                    defineSettingStructs[i].Init();
                    if (defineSettingStructs[i].symbolNames != null)
                        removeSymbols.AddRange(defineSettingStructs[i].symbolNames);
                    if (defineSettingStructs[i].oppositeSymbolNames != null)
                        removeSymbols.AddRange(defineSettingStructs[i].oppositeSymbolNames);
                    continue;
                }

                if (defineSettingStructs[i].value)
                {
                    if (defineSettingStructs[i].symbolNames != null)
                        addSymbols.AddRange(defineSettingStructs[i].symbolNames);

                    if (defineSettingStructs[i].oppositeSymbolNames != null)
                        removeSymbols.AddRange(defineSettingStructs[i].oppositeSymbolNames);
                }
                else
                {
                    if (defineSettingStructs[i].symbolNames != null)
                        removeSymbols.AddRange(defineSettingStructs[i].symbolNames);

                    if (defineSettingStructs[i].oppositeSymbolNames != null)
                        addSymbols.AddRange(defineSettingStructs[i].oppositeSymbolNames);
                }
            }

            if (isProjectInit)
            {
                for (int i = 0; i < defineSettingStructs.Length; i++)
                {
                    defineSettingStructs[i].Confirm();
                }
                DefineSymbolUtil.AddSymbolsStack(addSymbols);
                DefineSymbolUtil.RemoveSymbolsStack(removeSymbols);
                isModified = true;
            }
            else
            {
                isModified |= DefineSymbolUtil.RemoveSymbolsFromAllTargets(true, removeSymbols);
                isModified |= DefineSymbolUtil.AddSymbolsToAllTargets(true, addSymbols);
            }

            RemoveSymbolCache(removeSymbols);
            AddSymbolCache(addSymbols);

            isInitialized = true;

            SaveScriptableObj();

            return isModified;
        }

        public void UndoSetting()
        {
            for (int i = 0; i < defineSettingStructs.Length; i++)
            {
                defineSettingStructs[i].Undo();
            }
            EditorGUI_CWJ.RemoveFocusFromText();

            UpdateSetting();
        }

        public void ConfirmSetting()
        {
            for (int i = 0; i < defineSettingStructs.Length; i++)
            {
                defineSettingStructs[i].Confirm();
            }
            EditorGUI_CWJ.RemoveFocusFromText();

            UpdateSetting();
        }

        public void OnConfirmed_SaveLogEnabled(bool value)
        {
            EditorUserBuildSettings.development = value;
        }

        public void OnConfirmed_RuntimeDebuggingDisabled(bool value)
        {
#if CWJ_EXISTS_RUNTIMEDEBUGGING
            if (value)
            {
                var runtimeDebuggingTool = FindUtil.FindObjectOfType_New<RuntimeDebuggingTool>(true, false);
                if (runtimeDebuggingTool != null)
                {
                    Selection.activeObject = runtimeDebuggingTool;
                }
            }
#endif
        }

        public bool IsSettingChanged()
        {
            return defineSettingStructs.FindIndex(s => s.isChanged) >= 0;
        }

        public string GetSettingText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Debug Setting>");
            foreach (var setting in defineSettingStructs)
            {
                if (setting.settingName == ESettingName.Null) return "";
                if (setting.settingName == ESettingName.isLogDisabledInBuild || setting.settingName == ESettingName.isEditorDebugEnabled) continue;
                stringBuilder.AppendLine($"{setting.settingName.ToString()} : {setting.value}");
            }
            return stringBuilder.ToString();
        }
    }
}

#endif