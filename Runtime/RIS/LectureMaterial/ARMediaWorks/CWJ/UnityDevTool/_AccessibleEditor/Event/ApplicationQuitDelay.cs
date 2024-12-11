using System;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CWJ.AccessibleEditor
{
    /// <summary>
    /// 아직 에디터에서만 실행가능
    /// </summary>
    public static class ApplicationQuitDelay
    {
#if UNITY_EDITOR

        private static event Action WaitCallback = null;

        private static event Action WaitEndCallback = null;

#endif

        /// <summary>
        /// action도 static으로 선언할것
        /// <br/>선언되어있는 static Action에도 메소드를 그대로 넣지말고 람다식으로 넣기 Static_WaitCallback = ()=> { Instance.StopModeWhenAppQuit(); }; 이런식
        /// </summary>
        /// <param name="waitCallback"></param>
        /// <param name="waitEndCallback"></param>
        public static void AddWaitCallback(ref Action waitCallback, ref Action waitEndCallback)
        {
            //콜백 등록
#if UNITY_EDITOR
            WaitCallback += waitCallback;
            WaitEndCallback += waitEndCallback;
#endif
            waitEndCallback += () =>
            {
#if UNITY_EDITOR
                WaitCallback = null;
                QuitTryCount = 0;
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
            };

            //시스템 이벤트 등록
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += ModeChanged;
            //#else
            //            ApplicationQuitEvent.Instance.AddCallback(() => {
            //                if (WaitCallback != null)
            //                {
            //                    Application.CancelQuit();
            //                    WaitCallback.Invoke();
            //                }
            //            }, false);
#endif
        }

#if UNITY_EDITOR
        private static int QuitTryCount = 0;

        private static void ModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                if (QuitTryCount >= 1)
                {
                    typeof(ApplicationQuitDelay).PrintLogWithClassName("Forced To Quit!");
                    WaitEndCallback?.Invoke();
                    return;
                }

                if (WaitCallback != null)
                {
                    QuitTryCount += 1;
                    EditorApplication.isPlaying = true;
                    WaitCallback.Invoke();
                }
            }
        }

#endif
    }
}