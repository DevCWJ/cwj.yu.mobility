
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using CWJ.Singleton;

namespace CWJ
{
    /// <summary> 안드로이드 토스트 메시지 표시 싱글톤. 메인씬에 미리 두고 Start이후에 부를것.</summary>
    public class AndroidToast : SingletonBehaviourDontDestroy<AndroidToast>, CWJ.Singleton.IDontAutoCreatedWhenNull
    {
#if UNITY_ANDROID
        public static AndroidJavaClass UnityPlayer;
        public static AndroidJavaObject UnityActivity;
        public static AndroidJavaClass ToastClass;

        protected override void _Awake()
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                return;
            }
#endif

            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            UnityActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            ToastClass = new AndroidJavaClass("android.widget.Toast");
        }

        protected override void _OnDestroy()
        {
            UnityActivity?.Dispose();
            UnityActivity = null;
            UnityPlayer?.Dispose();
            UnityPlayer = null;
            ToastClass?.Dispose();
            ToastClass = null;
        }
#endif

        public enum ToastTime
        {
            /// <summary> 약 2초 </summary>
            Short = 0,
            /// <summary> 약 4초 </summary>
            Long = 1
        }

        /// <summary> 안드로이드 토스트 메시지 표시하기 </summary>
        [System.Diagnostics.Conditional("UNITY_ANDROID")]
        public void ShowToastMessage(string message, ToastTime length = ToastTime.Short)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }
#if UNITY_EDITOR
                if (Application.isEditor)
                {
                    Debug.Log(message);
                    editorGuiTime = (length.ToInt() + 1 * 2);
                    editorGuiMessage = message;
                    return;
                }
#endif
#if UNITY_ANDROID
                if (UnityActivity != null && ToastClass != null)
                {
                    UnityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaObject toastObject = ToastClass.CallStatic<AndroidJavaObject>("makeText", UnityActivity, message, (int)length);
                        toastObject.Call("show");
                    }));
                }
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }

        }


#if UNITY_EDITOR
        private float editorGuiTime = 0f;
        private string editorGuiMessage;
        private GUIStyle toastStyle;

        private void OnGUI()
        {
            if (editorGuiTime <= 0f) return;

            float width = Screen.width * 0.7f;
            float height = Screen.height * 0.08f;
            Rect rect = new Rect((Screen.width - width) * 0.5f, Screen.height * 0.8f, width, height);

            if (toastStyle == null)
            {
                toastStyle = new GUIStyle(GUI.skin.box);
                toastStyle.fontSize = 12;
                toastStyle.fontStyle = FontStyle.Bold;
                toastStyle.alignment = TextAnchor.MiddleCenter;
                toastStyle.normal.textColor = Color.white;
            }

            GUI.Box(rect, editorGuiMessage, toastStyle);
        }
        private void Update()
        {
            if (editorGuiTime > 0f)
                editorGuiTime -= Time.unscaledDeltaTime;
        }
#endif
    }
}