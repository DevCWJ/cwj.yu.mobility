#if UNITY_EDITOR

using System.Linq;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor.CustomDefine
{
    public class CustomDefine_Window : WindowBehaviour<CustomDefine_Window, CustomDefine_ScriptableObject> //_Window라고 작명한 이유가있으니 건들지말기
    {
         [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += UpdateWhenDefineSymbolDiff;
            CWJ_EditorEventHelper.UnityProjectChangedEvent += OnUnityProjectChanged;
            CWJ_EditorEventHelper.PlayModeStateChangedEvent += (p) => UpdateWhenDefineSymbolDiff();
        }

        public override string GetScriptableFirstName => "Custom Define setting";

        public const string WindowMenuItemPath = nameof(CWJ) + "/" + "Custom Define symbol setting";

        protected override void _OnReloadedWhileOpened()
        {
            UpdateWhenDefineSymbolDiff();
        }

        public static void UpdateWhenDefineSymbolDiff()
        {
            if (ScriptableObj.CheckIsDefineSymbolDiff())
            {
                UpdateSettingAndOpen();
            }
        }

        public static void OnUnityProjectChanged()
        {
            CloseAllThisWindow();

            ScriptableObj.isInitialized = !DisplayDialogUtil.DisplayDialog<CustomDefine_Window>(message:
                "Do you want to initialize the 'CustomDefine'?\n\n" + GetSettingText(), ok: "Initialize", cancel: "Keep");

            UpdateSetting(isProjectInit: true);
            Open();
        }

        [MenuItem(WindowMenuItemPath, priority = -200)]
        public static new void Open()
        {
            if (!IsOpened)
            {
                UpdateSetting();
            }
            OnlyOpen(minSize: new Vector2(400, 350));
        }

        private void Awake()//메뉴 열때
        {
            ScriptableObj.UndoSetting();
        }
        public static bool UpdateSetting(bool isProjectInit = false)
        {
            return ScriptableObj.UpdateSetting(isProjectInit);
        }

        public static void UpdateSettingAndOpen()
        {
            if (IsOpened)
            {
                UpdateSetting();
            }
            else
            {
                Open();
            }
        }

        protected override void _OnGUI()
        {
            if (IsCompiling())
            {
                return;
            }

            //GUI.enabled = false;
            //EditorGUILayout.Toggle(StringUtil.GetNicifyVariableName(nameof(ScriptableObj.isInitialized)), ScriptableObj.isInitialized == 1);
            //GUI.enabled = true;

            //EditorGUILayout.Space();

            //if (GUILayout.Button("Open Player Settings Menu"))
            //{
            //    SettingsService.OpenProjectSettings("Project/Player");
            //}

            GUI.enabled = true;

            EditorGUILayout.Space();

            BeginVerticalBox_Outer(true);
            {
                DrawLabelField("Current Scripting Define Symbols");
                BeginVerticalBox_Inner(false);
                EditorGUILayout.LabelField(DefineSymbolUtil.GetCurrentSymbols(true).Replace(";", ", "));
                EndVerticalBox();
            }
            EndVerticalBox();

            EditorGUILayout.Space();

            BeginVerticalBox_Outer(true);
            {
                DrawLabelField("Custom Define Symbols");

#if (CWJ_SCENEENUM_ENABLED || CWJ_SCENEENUM_DISABLED)
                EditorGUILayout.BeginHorizontal();
                ScriptableObj.isSceneEnumSyncBackup = EditorGUILayout.ToggleLeft(
                    (ScriptableObj.isSceneEnumSync != ScriptableObj.isSceneEnumSyncBackup ? "*" : "  ") + SceneHelper.SceneEnumDefine.IsSceneEnumSync,
                    ScriptableObj.isSceneEnumSyncBackup, GUILayout.ExpandWidth(false));

                GUI.enabled = ScriptableObj.isSceneEnumSync;
                if (GUILayout.Button($"Open SceneEnum.cs", GUILayout.ExpandWidth(false)))
                {
                    AccessibleEditorUtil.OpenScriptViaName(SceneHelper.SceneEnumDefine.ScriptName_SceneEnum);
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
#endif

                int symbolLength = ScriptableObj.symbolStructs.Length;
                for (int i = 0; i < symbolLength; ++i)
                {
                    GUI.enabled = !ScriptableObj.symbolStructs[i].isWillDelete;
                    EditorGUILayout.BeginHorizontal();
                    ScriptableObj.symbolStructs[i].backupValue = EditorGUILayout.ToggleLeft(
                        (ScriptableObj.symbolStructs[i].isChanged ? "*" : "  ") + ScriptableObj.symbolStructs[i].symbolName,
                        ScriptableObj.symbolStructs[i].backupValue, GUILayout.ExpandWidth(false));

                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        ScriptableObj.symbolStructs[i].WillRemove();
                    }
                    EditorGUILayout.EndHorizontal();

                    GUI.enabled = true;
                }

                EditorGUILayout.Space();

                GUI.enabled = !IsAppPlaying() && IsSettingChanged();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Confirm"))
                {
                    ScriptableObj.ConfirmSetting();
                }
                if (GUILayout.Button("Undo"))
                {
                    ScriptableObj.UndoSetting();
                }
                EditorGUILayout.EndHorizontal();

                GUI.enabled = true;

                if (GUILayout.Button("Copy ScriptableObject Name (to clipboard)"))
                {
                    string[] ignores = new string[] { (nameof(PsMessage.PsMessage_ScriptableObject) + ".asset"), (nameof(CustomDefine.CustomDefine_ScriptableObject) + ".asset") };

                    var paths = AssetDatabase.FindAssets("t:CWJScriptableObject")
                        .Select(g => System.IO.Path.GetFileName(AssetDatabase.GUIDToAssetPath(g)))
                        .Where(s => !ignores.IsExists(s))
                        .Select(s => $"{s}\r\n{s}.meta");
                    string.Join("\r\n", paths).CopyToClipboard();
                }
            }
            EndVerticalBox();



            GUILayout.FlexibleSpace();

            BeginVerticalBox_Outer(false);
            {
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(DebugSetting.DebugSetting_ScriptableObject))} Window"))
                {
                    EditorApplication.ExecuteMenuItem(DebugSetting.DebugSetting_Window.WindowMenuItemPath);
                }
                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(PsMessage.PsMessage_ScriptableObject))} Setting Window"))
                {
                    EditorApplication.ExecuteMenuItem(PsMessage.PsMessage_Window.WindowMenuItemPath);
                }
                if (GUILayout.Button("Build Settings..."))
                {
                    AccessibleEditorUtil.OpenBuildSettings();
                }
                EditorGUILayout.EndVertical();
            }
            EndVerticalBox();
        }

        private bool IsSettingChanged()
        {
            return ScriptableObj.IsSettingChanged();
        }

        public static string GetSettingText()
        {
            return ScriptableObj.GetSettingText();
        }
    }
}
#endif
