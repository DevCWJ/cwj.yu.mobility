using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace CWJ
{
    public partial class RuntimeDebuggingTool
    {

        public event Func<bool> allVisibleMultipleEvent;

        public event Func<bool> fpsVisibleSingleEvent;

        public event Func<bool> fpsResetSingleEvent;

        public event Func<bool> timePauseSingleEvent;

        public void AddSavingLog(string context, string title = null)
        {
            if (savingLog == null)
            {
                savingLog = new System.Text.StringBuilder();
            }

            savingLog.AppendLine($"[{title}]");
            savingLog.AppendLine($" {context}");
        }
        GameObject curEventSystemObj;
        public void SetVisible(bool enabled)
        {
            if (enabled == isVisible) return;
            isVisible = enabled;

            if (curEventSystemObj != null && FindObjectsOfType<EventSystem>().Length > 1)
            {
                curEventSystemObj.SetActive(false);
            }

            if (enabled)
            {
                if (EventSystem.current == null)
                {
                    if (curEventSystemObj == null)
                    {
                        curEventSystemObj = new GameObject("[Dangerous] EventSystem is null !!!", typeof(EventSystem), typeof(StandaloneInputModule));
                        curEventSystemObj.transform.SetParent(transform);
                        curEventSystemObj.transform.SetParent(null);
                        Debug.LogError("[Error] EventSystem is null. 기본적으로 EventSystem이 없길래 생성해줌");
#if UNITY_EDITOR
                        UnityEditor.EditorGUIUtility.PingObject(curEventSystemObj.gameObject);
#endif
                    }
                    else
                        curEventSystemObj.SetActive(true);
                }


                logViewerMgr.Show();
                fpsDisplayMgr.Show();
                if (!isEnabledAtLeastOnce)
                    isEnabledAtLeastOnce = true;
            }
            else
            {  
                logViewerMgr.Hide();
                fpsDisplayMgr.Hide();
            }
        }

        [SerializeField, HideInInspector] bool enabledBackup;
#if CWJ_RUNTIMEDEBUGGING_DISABLED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>isChanged</returns>
        public bool SetDebuggingEnabled(bool enabled)
        {
            if (enabled == isDebuggingEnabled) return false;
            if (isDebuggingEnabled && !enabledBackup)
            {
                enabledBackup = true;
            }
            isDebuggingEnabled = enabled;
            return true;
        }
#else
        public bool RollbackDebuggingEnabled()
        {
            if (enabledBackup)
            {
                isDebuggingEnabled = true;
                enabledBackup = false;
                return true;
            }
            return false;
        }
#endif

        public string gameVersion => _gameVersion;

        private static readonly object _lock = new object();
        private static RuntimeDebuggingTool instance;

        public static RuntimeDebuggingTool Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        RuntimeDebuggingTool[] ins = FindObjectsOfType<RuntimeDebuggingTool>();
                        if (ins.Length > 0)
                        {
                            for (int i = 0; i < ins.Length; i++)
                            {
                                if (instance == null && ins[i].enabled && ins[i].isDebuggingEnabled)
                                {
                                    instance = ins[i];
                                }
                                else
                                {
                                    if (Application.isPlaying)
                                    {
                                        Destroy(ins[i].gameObject);
                                    }
                                    else
                                    {
                                        DestroyImmediate(ins[i].gameObject);
                                    }
                                }
                            }
                        }

                        if (instance != null)
                        {
                            var o = instance.gameObject;
                            var root = o.transform.root.gameObject;
                            o = root != o ? root : o;
                            if (Application.isPlaying)
                            {
#if UNITY_EDITOR
                                bool isHidden = UnityEditor.SceneVisibilityManager.instance.IsHidden(o, true);
                                if (isHidden)
                                    UnityEditor.SceneVisibilityManager.instance.Show(o, true);
#endif
                                DontDestroyOnLoad(o);
#if UNITY_EDITOR
                                if (isHidden)
                                    UnityEditor.SceneVisibilityManager.instance.Hide(o, false);
#endif
                            }
                            instance.isInit = false;
                        }
                        else
                        {
                            Debug.LogWarning($"[ERROR] {nameof(RuntimeDebuggingTool)} is not exist");
                        }
                    }
                    return instance;
                }
            }
        }

    }
}