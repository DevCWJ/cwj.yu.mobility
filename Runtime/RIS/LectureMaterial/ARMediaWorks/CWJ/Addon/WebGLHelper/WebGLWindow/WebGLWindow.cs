using System;
using AOT;
using System.Runtime.InteropServices; // for DllImport
using UnityEngine;
using System.Collections;

namespace WebGLHelper
{
    static class WebGLWindowPlugin
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void WebGLWindowInit();
        [DllImport("__Internal")]
        public static extern void WebGLWindowOnFocus(Action cb);

        [DllImport("__Internal")]
        public static extern void WebGLWindowOnBlur(Action cb);

        [DllImport("__Internal")]
        public static extern void WebGLWindowOnResize(Action cb);

        [DllImport("__Internal")]
        public static extern void WebGLWindowInjectFullscreen();

        [DllImport("__Internal")]
        public static extern string WebGLWindowGetCanvasName();

        [DllImport("__Internal")]
        public static extern void MakeFullscreen(string str);

        [DllImport("__Internal")]
        public static extern void ExitFullscreen();

        [DllImport("__Internal")]
        public static extern bool IsFullscreen();

        [DllImport("__Internal")]
        public static extern void GoUrlWithoutHistory(string str);
        [DllImport("__Internal")]
        public static extern void GoUrlWithHistory(string str);
        [DllImport("__Internal")]
        public static extern void OpenUrlWithNewPopup(string str);
#else
        public static void WebGLWindowInit() { }
        public static void WebGLWindowOnFocus(Action cb) { }
        public static void WebGLWindowOnBlur(Action cb) { }
        public static void WebGLWindowOnResize(Action cb) { }
        public static void WebGLWindowInjectFullscreen() { }
        public static string WebGLWindowGetCanvasName() { return ""; }
        public static void MakeFullscreen(string str) { }
        public static void ExitFullscreen() { }
        public static bool IsFullscreen() { return false; }

        public static void GoUrlWithoutHistory(string str) { }
        public static void GoUrlWithHistory(string str) { }
        public static void OpenUrlWithNewPopup(string str) { }
#endif
    }

    public static class WebGLWindow
    {
        public static bool IsFocus { get; private set; }
        public static event Action OnFocusEvent = () => { };
        public static event Action OnBlurEvent = () => { };
        public static event Action OnResizeEvent = () => { };

        static string ViewportContent;

#if UNITY_WEBGL
        static WebGLWindow()
        {
            WebGLWindowPlugin.WebGLWindowInit();
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void RuntimeInitializeOnLoadMethod()
        {
            Init();
        }

        [MonoPInvokeCallback(typeof(Action))]
        static void OnWindowFocus()
        {
            IsFocus = true;
            OnFocusEvent();
        }

        [MonoPInvokeCallback(typeof(Action))]
        static void OnWindowBlur()
        {
            IsFocus = false;
            OnBlurEvent();
        }

        [MonoPInvokeCallback(typeof(Action))]
        static void OnWindowResize()
        {
            OnResizeEvent();
        }
#endif
        public static bool IsInit { get; private set; }
        public static void Init()
        {
            if (IsInit)
            {
                return;
            }
            IsInit = true;
#if UNITY_WEBGL
            WebGLWindowPlugin.WebGLWindowOnFocus(OnWindowFocus);
            WebGLWindowPlugin.WebGLWindowOnBlur(OnWindowBlur);
            WebGLWindowPlugin.WebGLWindowOnResize(OnWindowResize);
            if (IsFocus = Application.isFocused)
                OnWindowFocus();
            else
                OnWindowBlur();
#endif
            WebGLWindowPlugin.WebGLWindowInjectFullscreen();
        }

        /// <summary>
        /// 전체화면 전환, 해상도 동적변경되게할거면 BetterMaximal로 하고 
        /// 해상도 비율 유지되게할거면 BetterMinimal으로 빌드할것 (BetterMinimal도 전체화면은 됨. 현재는 최소화시 문제가 있을뿐)
        /// </summary>
        /// <returns></returns>
        public static IEnumerator IE_MakeFullscreen()
        {
            yield return null;
            if (!IsInit)
                Init();

            yield return null;
            ExitFullscreen();
            yield return new WaitForSeconds(1);
            MakeFullscreen();
        }

        public static string GetCanvasName()
        {
            return WebGLWindowPlugin.WebGLWindowGetCanvasName();
        }

        public static void MakeFullscreen(string fullscreenElementName = null)
        {
            WebGLWindowPlugin.MakeFullscreen(fullscreenElementName ?? GetCanvasName());
        }


        public static void ExitFullscreen()
        {
            WebGLWindowPlugin.ExitFullscreen();
        }

        public static void GoUrl(string url, bool isSaveHistory = false, bool isOpenPopup = false)
        {
            if (isOpenPopup)
            {
                WebGLWindowPlugin.OpenUrlWithNewPopup(url);
                return;
            }

            if (isSaveHistory)
                WebGLWindowPlugin.GoUrlWithHistory(url);
            else
                WebGLWindowPlugin.GoUrlWithoutHistory(url);
        }

        public static bool IsFullscreen()
        {
            return WebGLWindowPlugin.IsFullscreen();
        }
        public static void SwitchFullscreen()
        {
            if (IsFullscreen())
            {
                ExitFullscreen();
            }
            else
            {
                MakeFullscreen();
            }
        }
    }
}
