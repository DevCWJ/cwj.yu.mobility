#if UNITY_EDITOR

using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
    public static class EditorCallback
    {
        private static List<EditorCallbackStruct> CallbackStructList = new List<EditorCallbackStruct>();

        public static EditorCallbackStruct AddWaitForFrameCallback(Action callback, byte frameDelay = 1)
        {
            EditorCallbackStruct callbackStruct = null;
            callbackStruct = new EditorCallbackStruct(callback, frameDelay);
            CallbackStructList.Add(callbackStruct);
            return callbackStruct;
        }

        public static EditorCallbackStruct AddLoopCallback(Action callback, int maxLifeTime)
        {
            EditorCallbackStruct callbackStruct = null;
            callbackStruct = new EditorCallbackStruct(callback, maxLifeTime);
            CallbackStructList.Add(callbackStruct);
            return callbackStruct;
        }

        public static EditorCallbackStruct AddWaitForSecondsCallback(Action callback, float delayTime)
        {
            EditorCallbackStruct callbackStruct = null;
            callbackStruct = new EditorCallbackStruct(callback, delayTime: delayTime);
            CallbackStructList.Add(callbackStruct);
            return callbackStruct;
        }

        public static EditorCallbackStruct AddWaitForPredicateCallback(Action callback, Func<bool> predicate, int maxLifeTime)
        {
            EditorCallbackStruct callbackStruct = null;
            callbackStruct = new EditorCallbackStruct(callback, predicate, maxLifeTime);
            CallbackStructList.Add(callbackStruct);
            return callbackStruct;
        }

        public static EditorCallbackStruct AddWaitForPredicateNotSafeCallback(Action callback, Func<bool> predicate, int maxLifeTime, string notSafeMsg = "")
        {
            EditorCallbackStruct callbackStruct = null;
            callbackStruct = new EditorCallbackStruct(callback, predicate, maxLifeTime, notSafeMsg);
            CallbackStructList.Add(callbackStruct);
            return callbackStruct;
        }

        public static void RemoveEditorCallback(EditorCallbackStruct callbackStruct)
        {
            if (CallbackStructList.Contains(callbackStruct))
            {
                callbackStruct.Dispose();
            }
            callbackStruct = null;
        }

        public class EditorCallbackStruct : IDisposable
        {
            private EditorApplication.CallbackFunction editorUpdateLoop;
            private Action callback;
            private byte curFrameCnt = 0;
            private float countLifeTime = 0;
            private float lastRealTime = 0;
            private float delayTime = 0;
            private int maxLifeTime = 0;

            public void Dispose()
            {
                if (EditorCallback.CallbackStructList.Contains(this))
                {
                    EditorCallback.CallbackStructList.Remove(this);
                }

                EditorApplication.update -= editorUpdateLoop;
                callback = null;
                editorUpdateLoop = null;
            }

            /// <summary>
            /// WaitForEndOfFrame
            /// 한프레임 뒤 콜백
            /// (InitializeOnLoad와 함께쓰면 에디터 로딩후 실행되도록 구현가능)
            /// </summary>
            /// <param name="callback"></param>
            /// <param name="_maxLifeTime"></param>
            public EditorCallbackStruct(Action callback, byte frameDelay)
            {
                curFrameCnt = 0;
                editorUpdateLoop = () =>
                {
                    curFrameCnt++;
                    if (curFrameCnt == frameDelay)
                    {
                        callback.Invoke();
                        Dispose();
                    }
                    //frameCnt++;
                };
                EditorApplication.update += editorUpdateLoop;
            }

            /// <summary>
            /// While(Second) loop
            /// 정해진 시간동안 콜백 loop
            /// </summary>
            /// <param name="callback"></param>
            /// <param name="maxLifeTime"></param>
            public EditorCallbackStruct(Action callback, int maxLifeTime)
            {
                this.callback = callback;
                this.maxLifeTime = maxLifeTime;
                lastRealTime = Time.realtimeSinceStartup;

                editorUpdateLoop = () =>
                {
                    this.callback?.Invoke();

                    countLifeTime = Time.realtimeSinceStartup - lastRealTime;
                    if (countLifeTime >= this.maxLifeTime)
                    {
                        Dispose();
                        return;
                    }
                };
                EditorApplication.update += editorUpdateLoop;
            }

            /// <summary>
            /// WaitForSecond
            /// 시간초 Delay 후 콜백
            /// </summary>
            /// <param name="callback"></param>
            /// <param name="delayTime"></param>
            public EditorCallbackStruct(Action callback, float delayTime)
            {
                this.callback = callback;
                this.delayTime = delayTime < 0 ? 0 : delayTime;
                lastRealTime = Time.realtimeSinceStartup;

                editorUpdateLoop = () =>
                {
                    countLifeTime = Time.realtimeSinceStartup - lastRealTime;
                    if (countLifeTime >= this.delayTime)
                    {
                        this.callback?.Invoke();
                        Dispose();
                        return;
                    }
                };
                EditorApplication.update += editorUpdateLoop;
            }

            /// <summary>
            /// WaitUntil
            /// 특정 조건까지 Wait 후 콜백
            /// </summary>
            /// <param name="callback"></param>
            /// <param name="predicate"></param>
            /// <param name="maxLifeTime"></param>
            public EditorCallbackStruct(Action callback, Func<bool> predicate, int maxLifeTime)
            {
                this.callback = callback;
                this.maxLifeTime = maxLifeTime;

                lastRealTime = Time.realtimeSinceStartup;

                editorUpdateLoop = () =>
                {
                    countLifeTime = Time.realtimeSinceStartup - lastRealTime;
                    if (countLifeTime >= this.maxLifeTime)
                    {
                        Dispose();
                        return;
                    }
                    if (predicate())
                    {
                        this.callback?.Invoke();
                        Dispose();
                        return;
                    }
                };
                EditorApplication.update += editorUpdateLoop;
            }

            public EditorCallbackStruct(Action callback, Func<bool> predicate, int maxLifeTime, string notSafeMsg)
            {
                this.callback = callback;
                this.maxLifeTime = maxLifeTime;

                lastRealTime = Time.realtimeSinceStartup;

                editorUpdateLoop = () =>
                {
                    countLifeTime = Time.realtimeSinceStartup - lastRealTime;
                    if (countLifeTime >= this.maxLifeTime)
                    {
                        typeof(EditorCallback).PrintLogWithClassName(notSafeMsg);
                        this.callback?.Invoke();
                        Dispose();
                        return;
                    }
                    if (predicate())
                    {
                        this.callback?.Invoke();
                        Dispose();
                        return;
                    }
                };
                EditorApplication.update += editorUpdateLoop;
            }

            ~EditorCallbackStruct()
            {
                Dispose();
            }
        }
    }
}

#endif