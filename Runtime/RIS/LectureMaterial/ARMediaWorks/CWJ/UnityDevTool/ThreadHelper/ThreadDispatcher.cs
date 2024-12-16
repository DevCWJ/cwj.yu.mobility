using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace CWJ
{

    public class ThreadDispatcher : CWJ.Singleton.SingletonBehaviour<ThreadDispatcher>
    {
        static readonly object _ActQueueLock = new();
        private static readonly Queue<Action> _ActionQueue = new();

        static readonly object _UiActQueueLock = new();
        private static readonly Queue<Action> _UiActionsQueue = new();

        static readonly object _LateUpdateActQueueLock = new();
        private static readonly Queue<Action> _LateUpdateActionsQueue = new();

        static readonly object _AfterUIUpdateQueueLock = new();
        private static readonly Queue<Action> _AfterUIUpdateQueue = new();

        protected override void _Awake()
        {
#if !CWJ_DEVELOPMENT_BUILD
            gameObject.hideFlags = HideFlags.HideInInspector;
#endif
        }

        [System.Serializable]
        public struct DelayedQueueItem : IComparable<DelayedQueueItem>, IEquatable<DelayedQueueItem>
        {
            public bool hasValue;
            public float time;
            public Action action;

            public DelayedQueueItem(Action action, float delay)
            {
                this.time = delay > 0.0f ? delay /*+ Time.time*/ : 0;
                this.action = action;
                hasValue = true;
            }

            public int CompareTo(DelayedQueueItem other)
            {
                return Math.Truncate(time).CompareTo(Math.Truncate(other.time));
            }

            public bool Equals(DelayedQueueItem other)
            {
                return other.hasValue == hasValue && other.time == time && other.action == action;
            }
            public override int GetHashCode()
            {
                return HashCodeHelper.GetHashCode(hasValue, time, action);
            }
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize_AfterSceneLoad()
        {
            if (Application.isPlaying)
            {
                if (!HasInstance)
                    UpdateInstance();
                ThreadDispatcher.Instance.transform.SetParent(null);
                ThreadDispatcher.Instance.gameObject.SetActive(true);
            }
        }

        private void Update()
        {

            if (_ActionQueue.Count > 0)
            {
                Action[] tmpActs = null;
                lock (_ActQueueLock)
                {
                    tmpActs = _ActionQueue.ToArray();
                    _ActionQueue.Clear();
                }
                foreach (Action act in tmpActs)
                {
                    act.Invoke();
                }
                tmpActs = null;
            }

            if (_UiActionsQueue.Count > 0)
            {
                Action[] tmpActs = null;
                lock (_UiActQueueLock)
                {
                    tmpActs = _UiActionsQueue.ToArray();
                    _UiActionsQueue.Clear();
                }
                foreach (Action uiAct in tmpActs)
                {
                    uiAct.Invoke();
                }
                tmpActs = null;
            }
        }

        static int t = 0;
        private void LateUpdate()
        {
            if (_LateUpdateActionsQueue.Count > 0)
            {
                Action[] tmpActs = null;
                lock (_LateUpdateActQueueLock)
                {
                    tmpActs = _LateUpdateActionsQueue.ToArray();
                    _LateUpdateActionsQueue.Clear();
                }
                foreach (Action uiAct in tmpActs)
                {
                    uiAct.Invoke();
                }
                tmpActs = null;
            }
            if (t < 2)
            {
                t++;
            }
            else
            {
                t = 0;
                if (_AfterUIUpdateQueue.Count > 0)
                {
                    Action[] tmpActs = null;
                    lock (_AfterUIUpdateQueueLock)
                    {
                        tmpActs = _AfterUIUpdateQueue.ToArray();
                        _AfterUIUpdateQueue.Clear();
                    }
                    foreach (Action uiAct in tmpActs)
                    {
                        uiAct.Invoke();
                    }
                    tmpActs = null;
                }
            }

        }

        protected override void _OnApplicationQuit()
        {
            Clear();
            UIClear();
            LateUpdateClear();
        }
        public static void AfterUIUpdateQueue(Action action)
        {
            if (action == null)
            {
                Debug.LogError("LateUpdateQueue action is null");
                return;
            }
            lock (_AfterUIUpdateQueueLock)
            {
                _AfterUIUpdateQueue.Enqueue(action);
            }
        }
        public static void AfterUIUpdateQueue<T>(System.Action<T> action, T data)
        {
            if (action != null)
            {
                AfterUIUpdateQueue(() => action.Invoke(data));
            }
        }

        public static void LateUpdateQueue(Action action)
        {
            if (action == null)
            {
                Debug.LogError("LateUpdateQueue action is null");
                return;
            }
            lock (_LateUpdateActQueueLock)
            {
                _LateUpdateActionsQueue.Enqueue(action);
            }
        }
        public static void LateUpdateQueue<T>(System.Action<T> action, T data)
        {
            if (action != null)
            {
                LateUpdateQueue(() => action.Invoke(data));
            }
        }

        public static void UIEnqueue(Action action)
        {
            if (action == null)
            {
                Debug.LogError("UIEnqueue action is null");
                return;
            }
            lock (_UiActQueueLock)
            {
                _UiActionsQueue.Enqueue(action);
            }
        }
        public static void UIEnqueue<T>(System.Action<T> action, T data)
        {
            if (action != null)
            {
                UIEnqueue(() => action.Invoke(data));
            }
        }

        public static void Enqueue(System.Action action)
        {
            if (action == null)
            {
                Debug.LogError("Enqueue action is null");
                return;
            }
            lock (_ActQueueLock)
            {
                _ActionQueue.Enqueue(action);
            }
        }

        public static void Enqueue<T>(System.Action<T> action, T data)
        {
            if (action != null)
            {
                Enqueue(() => action.Invoke(data));
            }
        }

        public static void Clear()
        {
            lock (_ActQueueLock)
            {
                _ActionQueue.Clear();
            }
        }

        public static void UIClear()
        {
            lock (_UiActQueueLock)
            {
                _UiActionsQueue.Clear();
            }
        }

        public static void LateUpdateClear()
        {
            lock (_LateUpdateActQueueLock)
            {
                _LateUpdateActionsQueue.Clear();
            }
        }

        public static void AfterUIUpdateClear()
        {
            lock (_AfterUIUpdateQueueLock)
            {
                _AfterUIUpdateQueue.Clear();
            }
        }
    }

}