#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    public static class EditorRestartFunction
    {
        public const int PROCESS_KILL_TIME = 5000;

        public static void EditorRestart(float delayTime = 0, bool isDisplayDialog = true)
        {
            EditorApplication.CallbackFunction callbackFunction = () =>
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                if (isDisplayDialog)
                {
                    EditorUtility.DisplayDialog(nameof(CWJ), "에디터가 종료후 자동으로 재시작됩니다", ok: "Ok");
                }
                StartCLI();
            };

            if (delayTime > 0)
            {
                //EditorApplication.delayCall += callbackFunction;
                EditorCallback.AddWaitForSecondsCallback(new System.Action(callbackFunction), delayTime);
            }
            else
            {
                callbackFunction.Invoke();
            }
        }

        private static void StartCLI()
        {
            string cmdFilePath;
            bool success = false;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                try
                {
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;

                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        cmdFilePath = System.IO.Path.GetTempPath() + "CWJRestart-" + System.Guid.NewGuid() + ".cmd";

                        System.IO.File.WriteAllText(cmdFilePath, GetCmdContents());

                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/c start  \"\" " + '"' + cmdFilePath + '"';
                    }
                    else if (Application.platform == RuntimePlatform.OSXEditor)
                    {
                        cmdFilePath = System.IO.Path.GetTempPath() + "CWJRestart-" + System.Guid.NewGuid() + ".sh";

                        System.IO.File.WriteAllText(cmdFilePath, GetCmdContents());

                        process.StartInfo.FileName = "/bin/sh";
                        process.StartInfo.Arguments = '"' + cmdFilePath + "\" &";
                    }

                    process.Start();
                    process.WaitForExit(PROCESS_KILL_TIME);
                    success = true;
                }
                catch (System.Exception e)
                {
                    typeof(EditorRestartFunction).PrintLogWithClassName(e.ToString(), LogType.Error);
                    success = false;
                }

                if (success)
                {
                    EditorApplication.Exit(0);
                }
                else
                {
                    process.Close();
                }
            }
        }

        private static string GetCmdContents()
        {
            string projectPath = PathUtil.ProjectPath;
            string editorExePath = PathUtil.EditorExePath;

            System.Text.StringBuilder strb = new System.Text.StringBuilder();
            strb.AppendLine("@echo off");
            strb.AppendLine("cls");

            strb.AppendLine("echo.");
            strb.AppendLine("echo Run Restart_Unity Command");
            strb.AppendLine("echo Help : Contact to CWJ");
            strb.AppendLine("echo.");
            strb.AppendLine("timeout /t 2 > NUL");

            // Wait Unity closed
            strb.AppendLine("echo Waiting for Unity to close...");
            strb.AppendLine(":waitClose");
            strb.AppendLine($"if not exist \"{projectPath}\\Temp\\UnityLockfile\" goto waitCloseEnd");
            strb.AppendLine("timeout /t 2 > NUL");
            strb.AppendLine("goto waitClose");
            strb.AppendLine(":waitCloseEnd");

            strb.AppendLine("echo.");
            strb.AppendLine("echo Unity is closed.");
            strb.AppendLine("echo.");

            strb.AppendLine($"start \"\" {editorExePath.WithDoubleQuotes()}" +
                            $" -projectPath {projectPath.WithDoubleQuotes()}");

            strb.AppendLine("exit");

            return strb.ToString();
        }
    }
}

#endif