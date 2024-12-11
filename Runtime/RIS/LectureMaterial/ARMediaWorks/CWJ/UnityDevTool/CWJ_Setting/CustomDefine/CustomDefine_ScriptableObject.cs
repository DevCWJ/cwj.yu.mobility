#if UNITY_EDITOR

using System;
using System.Collections.Generic;

using UnityEditor;

/// <summary>
/// UNITY_EDITOR 필수 namespace
/// </summary>
namespace CWJ.AccessibleEditor.CustomDefine
{
    //ScriptableObject 클래스의 이름과 cs파일 이름과 같아야함 (자체자작한 WindowBehaviour때문)
    public class CustomDefine_ScriptableObject : CachedSymbol_ScriptableObject
    {
        public override bool IsAutoReset => false;

        public bool isSceneEnumSync;
        public bool isSceneEnumSyncBackup;

        public DefineSymbolStruct[] symbolStructs = new DefineSymbolStruct[0];

        public void WillAddDefine(string symbolName, bool enableDefault)
        {
            ArrayUtil.Add(ref symbolStructs, new DefineSymbolStruct(symbolName, enableDefault));
        }

        public void WillRemoveDefine(string symbolName)
        {
            int index = symbolStructs.FindIndex(d => d.symbolName.Equals(symbolName));
            if (index >= 0)
            {
                symbolStructs[index].WillRemove();
            }
        }

        public void SetDefineEnable(string symbolName, bool isEnable)
        {
            int index = symbolStructs.FindIndex(d => d.symbolName.Equals(symbolName));
            if (index >= 0)
            {
                symbolStructs[index].value = symbolStructs[index].backupValue = isEnable;
            }
        }

        public void UndoSetting()
        {
            for (int i = 0; i < symbolStructs.Length; i++)
            {
                symbolStructs[i].Undo();
            }
            isSceneEnumSyncBackup = isSceneEnumSync;
            UpdateSetting();
            EditorGUI_CWJ.RemoveFocusFromText();
        }

        public void ConfirmSetting()
        {
            for (int i = 0; i < symbolStructs.Length; i++)
            {
                symbolStructs[i].Confirm();
            }
            isSceneEnumSync = isSceneEnumSyncBackup;
            UpdateSetting();
            EditorGUI_CWJ.RemoveFocusFromText();
        }

        public bool UpdateSetting(bool isProjectInit = false)
        {
            bool isModified = false;

            var addSymbols = new List<string>();
            var removeSymbols = new List<string>();

            if (!isInitialized) //초기화, Default값
            {
                removeSymbols.AddRange(new string[]
                {
                    SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_ENABLED,
                    SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_DISABLED,
                    //MultiDisplayManager.CWJ_MULTI_DISPLAY
                });
                if (symbolStructs != null && symbolStructs.Length > 0)
                {
                    removeSymbols.AddRange(symbolStructs.ConvertAll(d => d.symbolName));
                    symbolStructs = new DefineSymbolStruct[0];
                }
                OnReset();
                isModified = true;
            }

            if (isSceneEnumSync)
            {
                addSymbols.Add(SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_ENABLED);
                removeSymbols.Add(SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_DISABLED);

                bool isSceneEnumDisabled = false;
#if CWJ_SCENEENUM_DISABLED
                isSceneEnumDisabled = true;
#endif
                if (isSceneEnumDisabled)
                {
                    ReflectionUtil.InvokeMethodForcibly(null, true, false, "CWJ.SceneHelper.Editor.AutoGenerateSceneEnum", "SyncSceneEnum");
                    AccessibleEditorUtil.OpenScriptViaPath(SceneHelper.SceneEnumDefine.PATH);
                }
            }
            else
            {
                removeSymbols.Add(SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_ENABLED);
                addSymbols.Add(SceneHelper.SceneEnumDefine.CWJ_SCENEENUM_DISABLED);
            }

            DefineSymbolStruct[] enables, disables;
            enables = symbolStructs.FindAllWithMisMatch((d) => d.value, out disables);

            addSymbols.AddRange(Array.ConvertAll(enables, (e) => e.symbolName));
            removeSymbols.AddRange(Array.ConvertAll(disables, (d) => d.symbolName));

            if (isProjectInit)
            {
                DefineSymbolUtil.RemoveSymbolsStack(removeSymbols);
                DefineSymbolUtil.AddSymbolsStack(addSymbols);
                isModified = true;
            }
            else
            {
                isModified |= DefineSymbolUtil.RemoveSymbolsFromAllTargets(true, removeSymbols);
                isModified |= DefineSymbolUtil.AddSymbolsToAllTargets(true, addSymbols);
            }

            RemoveSymbolCache(removeSymbols);
            AddSymbolCache(addSymbols);

            var deleteList = symbolStructs.FindAll(s => s.isWillDelete);

            int deleteLength = deleteList.Length;
            for (int i = 0; i < deleteLength; ++i)
            {
                ArrayUtil.Remove(ref symbolStructs, deleteList[i]);
            }

            isInitialized = true;

            SaveScriptableObj();

            return isModified;
        }

        public bool IsSettingChanged()
        {
            return isSceneEnumSync != isSceneEnumSyncBackup || symbolStructs.FindIndex(s => s.isChanged) >= 0;
        }

        public string GetSettingText()
        {
            return $"<Custom Define>\n " +
                $"{nameof(isSceneEnumSync)} : {isSceneEnumSync}\n" + string.Join("\n",
                Array.ConvertAll(symbolStructs, (d) => d.symbolName + " : " + d.value));
        }
    }

    //[Serializable]
    //public struct DefineSymbolStruct
    //{
    //    public string symbolName;
    //    public bool isEnabled;

    //    public DefineSymbolStruct(string symbolName, bool isEnabled)
    //    {
    //        this.symbolName = symbolName;
    //        this.isEnabled = isEnabled;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        DefineSymbolStruct b = (DefineSymbolStruct)obj;
    //        return symbolName.Equals(b.symbolName) && isEnabled == b.isEnabled;
    //    }

    //    public override int GetHashCode()
    //    {
    //        var hashCode = -2100008024;
    //        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(symbolName);
    //        hashCode = hashCode * -1521134295 + isEnabled.GetHashCode();
    //        return hashCode;
    //    }
    //}


    [Serializable]
    public struct DefineSymbolStruct
    {
        [UnityEngine.HideInInspector] public string symbolName;
        public bool value;
        public bool backupValue;
        [UnityEngine.SerializeField, Readonly] private bool _isWillRemove;
        public bool isWillDelete => _isWillRemove;
        public bool isChanged => value != backupValue || _isWillRemove;

        public DefineSymbolStruct(string symbolName, bool defaultValue) : this()
        {
            this.symbolName = symbolName;
            this.backupValue = this.value = defaultValue;
            _isWillRemove = false;
        }

        public void Init()
        {
            backupValue = value = false;
            _isWillRemove = false;
        }

        public void Confirm()
        {
            value = backupValue;
        }

        public void Undo()
        {
            backupValue = value;
            _isWillRemove = false;
        }

        public void WillRemove()
        {
            backupValue = value = false;
            _isWillRemove = true;
        }
    }
}

#endif