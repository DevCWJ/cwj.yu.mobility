#if UNITY_EDITOR

using System;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor.PsMessage
{
    using static PsMessage_ScriptableObject;
    public class PsMessage_Window : WindowBehaviour<PsMessage_Window, PsMessage_ScriptableObject>
    {
        public override string GetScriptableFirstName => "PS Message Setting";

        public const string WindowMenuItemPath = nameof(CWJ) + "/" + "PS message setting";

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.UnityProjectChangedEvent += OnUnityProjectChanged;
            CWJ_EditorEventHelper.ReloadedScriptEvent += PsMsgCheck;
            CWJ_EditorEventHelper.ProjectOpenEvent += PsMsgCheck;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += EditorEventSystem_EditorSceneOpenedEvent;
            CWJ_EditorEventHelper.EditorQuitEvent += OnEditorQuit;
        }

        private static void EditorEventSystem_EditorSceneOpenedEvent(UnityEngine.SceneManagement.Scene obj)
        {
            PsMsgCheck();
        }

        public static void OnUnityProjectChanged()
        {
            ScriptableObj.OnReset();
        }

        private static void OnEditorQuit()
        {
            if (ScriptableObj.psMsgState == EMsgState.Waiting)
            {
                ScriptableObj.psMsgState = EMsgState.ShowMsg;
                SaveScriptableObj();
            }
        }

        private static void PsMsgCheck()
        {
            if (ScriptableObj == null) return;

            ScriptableObj.isInitialized = true;

            if (ScriptableObj.psMsgState.ToInt() < EMsgState.ShowMsg.ToInt())
            {
                return;
            }

            string message = "P.S. ".SetColor(Color.red) + ScriptableObj.confirmedMessage;
            message += $"\n\nTo hide show the message, click the 'Clear message' button in the 'PS Message Setting' window.";

            Action openPSMessageDialog = () =>
            {
                if (!IsOpened && DisplayDialogUtil.DisplayDialog<PsMessage_Window>(message: message))
                {
                    Open();
                }
                if (ScriptableObj.psMsgState == EMsgState.ShowSetting)
                {
                    ScriptableObj.psMsgState = EMsgState.ShowMsg;
                    CustomDefine.CustomDefine_Window.Open();
                    DebugSetting.DebugSetting_Window.Open();
                }
            };

            CWJ_EditorEventHelper.WaitForCompiled(openPSMessageDialog);
        }


        [MenuItem(WindowMenuItemPath, priority = -100)]
        public new static void Open()
        {
            if (!IsOpened)
            {
                ScriptableObj.fieldTmpMessage = ScriptableObj.confirmedMessage;

                if (!CustomDefine.CustomDefine_Window.ScriptableObj.isInitialized || !DebugSetting.DebugSetting_Window.ScriptableObj.isInitialized)
                {
                    CustomDefine.CustomDefine_Window.UpdateSettingAndOpen();
                    DebugSetting.DebugSetting_Window.UpdateSettingAndOpen();
                }
            }
            OnlyOpen(minSize: new Vector2(450, 400));
        }

        protected override void _OnGUI()
        {
            if (IsCompiling())
            {
                return;
            }

            EditorGUILayout.Space();

            GUI.enabled = true;

            BeginVerticalBox_Outer(true);
            {
                DrawLabelField(ObjectNames.NicifyVariableName(nameof(PsMessage)));

                ScriptableObj.fieldTmpMessage = EditorGUILayout.TextArea(string.IsNullOrEmpty(ScriptableObj.fieldTmpMessage.RemoveAllSpaces()) ? "" : ScriptableObj.fieldTmpMessage, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2f));

                if (!string.Equals(ScriptableObj.fieldTmpMessage, ScriptableObj.confirmedMessage))
                {
                    if (string.IsNullOrEmpty(ScriptableObj.fieldTmpMessage.RemoveAllSpaces()))
                    {
                        ScriptableObj.confirmedMessage = ScriptableObj.fieldTmpMessage = "";
                        ScriptableObj.psMsgState = 0;
                        SaveScriptableObj();
                    }
                }
                else
                {
                    GUI.enabled = false;
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Submit"))
                {
                    EditorGUI_CWJ.RemoveFocusFromText();
                    ScriptableObj.confirmedMessage = ScriptableObj.fieldTmpMessage;
                    ScriptableObj.psMsgState = EMsgState.Waiting;
                    SaveScriptableObj();
                }
                GUI.enabled = ScriptableObj.psMsgState != 0;
                if (GUILayout.Button("Clear message"))
                {
                    EditorGUI_CWJ.RemoveFocusFromText();
                    ScriptableObj.confirmedMessage = ScriptableObj.fieldTmpMessage = "";
                    ScriptableObj.psMsgState = 0;
                    SaveScriptableObj();
                }
                EditorGUILayout.EndHorizontal();
            }
            EndVerticalBox();

            GUILayout.FlexibleSpace();

            BeginVerticalBox_Outer(false);
            {
                GUI.enabled = true;
                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(DebugSetting.DebugSetting_ScriptableObject))} Window"))
                {
                    EditorApplication.ExecuteMenuItem(DebugSetting.DebugSetting_Window.WindowMenuItemPath);
                }
                if (GUILayout.Button($"Open {StringUtil.GetNicifyVariableName(nameof(CustomDefine.CustomDefine_ScriptableObject))} Setting Window"))
                {
                    EditorApplication.ExecuteMenuItem(CustomDefine.CustomDefine_Window.WindowMenuItemPath);
                }
            }
            EndVerticalBox();
        }
    }
}

#endif