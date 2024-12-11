#if UNITY_EDITOR

using System.IO;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor.DebugSetting
{
    public class DebugSetting_Window : WindowBehaviour<DebugSetting_Window, DebugSetting_ScriptableObject> //_Window라고 작명한 이유가있으니 건들지말기
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += UpdateWhenDefineSymbolDiff;
            CWJ_EditorEventHelper.UnityProjectChangedEvent += OnUnityProjectChanged;
            CWJ_EditorEventHelper.PlayModeStateChangedEvent += (p) => UpdateWhenDefineSymbolDiff();
        }

        public override string GetScriptableFirstName => "Debug setting";

        public const string WindowMenuItemPath = nameof(CWJ) + "/" + "Debug setting";

        [MenuItem(WindowMenuItemPath, priority = -300)]
        public new static void Open()
        {
            if (!IsOpened)
            {
                UpdateSetting();
            }
            OnlyOpen(minSize: new Vector2(777, 425), maxSize: new Vector2(1000, 425));
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
        public static void UpdateWhenDefineSymbolDiff()
        {
            if (ScriptableObj.CheckIsDefineSymbolDiff())
            {
                UpdateSettingAndOpen();
            }
        }

        protected override void _OnReloadedWhileOpened()
        {
            UpdateWhenDefineSymbolDiff();
        }

        public static void OnUnityProjectChanged()
        {
            CloseAllThisWindow();

            ScriptableObj.isInitialized =  !DisplayDialogUtil.DisplayDialog<DebugSetting_Window>(message:
                "Do you want to initialize the 'DebugSetting'?\n\n" + GetSettingText(), ok: "Initialize", cancel: "Keep");

            UpdateSetting(isProjectInit: true);
            Open();
        }

        public static string GetSettingText()
        {
            return ScriptableObj.GetSettingText();
        }

        private void Awake() //메뉴 열때
        {
            ScriptableObj.UndoSetting();
        }

        protected override void _OnGUI()
        {
            if (IsCompiling())
            {
                return;
            }

            //GUI.enabled = false;
            //Draw(nameof(ScriptableObj.buildPackageName));
            //EditorGUILayout.Toggle(StringUtil.GetNicifyVariableName(nameof(ScriptableObj.isInitialized)), ScriptableObj.isInitialized == 1);
            //GUI.enabled = true;

            //EditorGUILayout.Space();

            //if (GUILayout.Button("Update Setting"))
            //{
            //    DefineUtility.RemoveSymbolsFromAllTargets(true, DefineUtility.GetCurrentSymbolsToArray(true));
            //    UpdateSetting();
            //    CustomDefine.CustomDefine_Window.UpdateSetting();
            //    return;
            //}

            GUI.enabled = true;

            EditorGUILayout.Space();

            BeginVerticalBox_Outer(true);
            {
                DrawLabelField("Setting");

                var settingGroupsByTitle = ScriptableObj.GetSettingStructGroupByTitle();

                foreach (var group in settingGroupsByTitle)
                {
                    BeginVerticalBox_Outer(false);
                    {
                        DrawLabelField(group.title);
                        foreach (var setting in group.settings)
                        {
                            bool isDisableCondition = ScriptableObj.CheckIsDisableCondition(setting);
                            GUI.enabled = !isDisableCondition;

                            string toggleName = (setting.value != setting.backupValue ? "*" : "  ") + setting.settingName.ToString()
                                                + "  " + (isDisableCondition ? "" : setting.description);
                            bool value = EditorGUILayout.ToggleLeft(toggleName, setting.backupValue, EditorStyles.label, GUILayout.ExpandWidth(true));
                            if (isDisableCondition)
                            {
                                value = false;
                            }
                            if (setting.backupValue != value)
                            {
                                ScriptableObj.SetSettingStructBackupValue(setting.settingName, value);
                            }

                            if (setting.settingName == ESettingName.isSaveLogEnabled && setting.backupValue)
                            {
                                DirectoryPathButton("Saved path in editor:", DebugLogWriter.GetEditorLogFolderPath);
                                string inBuildPath = DebugLogWriter.GetLogFolderPath;
                                if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                                {
                                    inBuildPath = "/storage/emulated/0" + inBuildPath;
                                }
                                DirectoryPathButton("Saved path in build:", inBuildPath);

                                // /storage/emulated/0/Android/data/com.Rise.StreetLamp/files/CWJ_LOG/23-06-16_17-42-33 Logs.txt

                            }

                        }
                    }
                    EndVerticalBox();
                    GUI.enabled = true;
                }

                EditorGUILayout.Space();

                GUI.enabled = !IsAppPlaying(true) && ScriptableObj.IsSettingChanged();
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
            }
            EndVerticalBox();

            GUILayout.FlexibleSpace();

            BeginVerticalBox_Outer(false);
            {
                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(CustomDefine.CustomDefine_ScriptableObject))} Setting Window"))
                {
                    EditorApplication.ExecuteMenuItem(CustomDefine.CustomDefine_Window.WindowMenuItemPath);
                }

                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(PsMessage.PsMessage_ScriptableObject))} Setting Window"))
                {
                    EditorApplication.ExecuteMenuItem(PsMessage.PsMessage_Window.WindowMenuItemPath);
                }

                if (GUILayout.Button("Build Settings..."))
                {
                    AccessibleEditorUtil.OpenBuildSettings();
                }
                //if (GUILayout.Button("Safety Remove"))
                //{
                //    CloseAllCWJWindow();
                //    Resources.UnloadUnusedAssets();

                //    var cwjObjPaths = Resources.FindObjectsOfTypeAll<CWJScriptableObject>().ConvertAll(s => s.GetAssetPath());
                //    for (int i = 0; i < cwjObjPaths.Length; i++)
                //    {
                //        FileUtil.DeleteFileOrDirectory(cwjObjPaths[i]);
                //    }

                //    FileUtil.DeleteFileOrDirectory(PathUtil.GetPath(true));
                //    AssetDatabase.Refresh();
                //}
            }
            EndVerticalBox();
        }
    }
}

#endif