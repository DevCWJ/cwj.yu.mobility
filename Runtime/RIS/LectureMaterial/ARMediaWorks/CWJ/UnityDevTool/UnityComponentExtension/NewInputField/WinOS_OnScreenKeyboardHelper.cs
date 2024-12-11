using System.Diagnostics;

using UnityEngine;
using UnityEngine.EventSystems;

namespace CWJ.UI
{
    [DisallowMultipleComponent, CWJInfoBox("Selectable을 상속받는 UI Component(InputField, Toggle 등)와 함께두고 클릭하면 OSK가 Open!")]
    public class WinOS_OnScreenKeyboardHelper : MonoBehaviour
#if (!UNITY_EDITOR && UNITY_STANDALONE_WIN) || (UNITY_EDITOR_WIN && CWJ_EDITOR_DEBUG_ENABLED)
        , ISelectHandler
#endif
    {
        private const string OnScreenKeyboardExe = "osk.exe";

        [GetComponent, ErrorIfNull] public UnityEngine.UI.Selectable selectable;

        [ReadonlyConditional(EPlayMode.PlayMode)] public bool isAutoReleaseFocus = true;

        private Process process { get; set; } = null;

        public void OnSelect(BaseEventData eventData)
        {
            if (selectable == null || !selectable.enabled || !selectable.IsInteractable()) return;

            //Close();

            OpenOsk();
        }

        private void OpenOsk()
        {
            if (process != null) return;

            process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo
                {
                    FileName = OnScreenKeyboardExe
                }
            };

            if (!process.Start())
            {
                UnityEngine.Debug.LogError($"[ERROR] {OnScreenKeyboardExe} 실행 안됨");
                DisposeProcess();
                return;
            }

            process.Exited += OnProcessExited;
        }

        private void OnProcessExited(object sender, System.EventArgs e)
        {
            DisposeProcess();
        }

        private void DisposeProcess()
        {
            process.Exited -= OnProcessExited;
            process.Dispose();
            process = null;

            if (isAutoReleaseFocus)
            {
                isProcessDisposed = true;
            }
        }

        bool isProcessDisposed = false;

        private void OnApplicationFocus(bool focus)
        {
            if (!isAutoReleaseFocus) return;
            if (isProcessDisposed && focus)
            {
                isProcessDisposed = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //private const string TabTipWindowClassName = "IPTip_Main_Window";
        //private const string TabTipExePath = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
        //private const string TabTipRegistryKeyName = @"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7";

        //[DllImport("user32.dll")]
        //private static extern int SendMessage(int hWnd, uint msg, int wParam, int lParam);

        //[DllImport("user32.dll")]
        //private static extern IntPtr FindWindow(String sClassName, String sAppName);

        //public static void OpenUndocked()
        //{
        //    const string TabTipDockedKey = "EdgeTargetDockedState";
        //    const string TabTipProcessName = "TabTip";

        //    int docked = (int)(Registry.GetValue(TabTipRegistryKeyName, TabTipDockedKey, 1) ?? 1);
        //    if (docked == 1)
        //    {
        //        Registry.SetValue(TabTipRegistryKeyName, TabTipDockedKey, 0);
        //        foreach (Process tabTipProcess in Process.GetProcessesByName(TabTipProcessName))
        //            tabTipProcess.Kill();
        //    }
        //    Open();
        //}

        //public static void Open()
        //{
        //    const string TabTipAutoInvokeKey = "EnableDesktopModeAutoInvoke";

        //    int EnableDesktopModeAutoInvoke = (int)(Registry.GetValue(TabTipRegistryKeyName, TabTipAutoInvokeKey, -1) ?? -1);
        //    if (EnableDesktopModeAutoInvoke != 1)
        //        Registry.SetValue(TabTipRegistryKeyName, TabTipAutoInvokeKey, 1);

        //    Process.Start(TabTipExePath);
        //}

        //public static void Close()
        //{
        //    const int WM_SYSCOMMAND = 274;
        //    const int SC_CLOSE = 61536;
        //    SendMessage(GetTabTipWindowHandle().ToInt32(), WM_SYSCOMMAND, SC_CLOSE, 0);
        //}

        //private static IntPtr GetTabTipWindowHandle()
        //{
        //    return FindWindow(TabTipWindowClassName, null);
        //}
    }
}