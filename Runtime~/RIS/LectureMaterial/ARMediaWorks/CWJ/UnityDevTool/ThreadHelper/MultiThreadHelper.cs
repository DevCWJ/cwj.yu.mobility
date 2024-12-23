#if UNITY_WITHOUT_MULTITHREADING
#define USE_THREAD_SAFETY
#else
#undef USE_THREAD_SAFETY
#endif

using ActionQueue
#if USE_THREAD_SAFETY
    = System.Collections.Concurrent.ConcurrentQueue<System.Action>; //서버나 하드웨어 통신작업때문에 찐 multi thread 환경이 필요하다면 Thread-Safe한 ConcurrentQueue쓰기
#else
    = System.Collections.Generic.Queue<System.Action>;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// Thread처리할때 만듬.
    /// </summary>
    public class MultiThreadHelper : CWJ.Singleton.SingletonBehaviour<MultiThreadHelper>
    {
        private static readonly ActionQueue _ActionQueue = new();
        private const int MAX_ACTIONS_PER_FRAME = 30;

        private static readonly ActionQueue _UiActionsQueue = new();
        private const int MAX_UI_ACTIONS_PER_FRAME = 50;

        private static readonly ActionQueue _LateUpdateActionsQueue = new();
        private const int MAX_LATEUPDATE_ACTIONS_PER_FRAME = 30;

        private static readonly SortedSet<DelayedQueueItem> _DelayedActionsSet = new();
        private static readonly object _DelayedActionsLock = new object();
        private const int MAX_DELAYED_ACTIONS_PER_FRAME = 20;
        private const float DELAYED_QUEUE_INTERVAL = 0.05f; // 50ms 간격
        private static float _FrameTimeThreshold = -1f; // 초기화 상태

        public static float GetUpdateTime() => Time.deltaTime;
        public static float GetLateUpdateDelayTime() => Time.deltaTime * 1.1f;

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize_AfterSceneLoad()
        {
            if (Application.isPlaying)
            {
                _FrameTimeThreshold = 1f / Application.targetFrameRate;
                var insObj = Instance.gameObject;
                if (!insObj.activeSelf || !insObj.activeInHierarchy)
                {
                    insObj.transform.SetParent(null);
                    insObj.SetActive(true);
                }

                MonoBehaviourEventHelper.QuitEvent += ClearAllQueue;
            }
        }

        private static float CacheFrameTimeThreshold()
        {
            if (_FrameTimeThreshold < 0)
                _FrameTimeThreshold = 1f / Application.targetFrameRate;
            return _FrameTimeThreshold;
        }

        private static void ClearAllQueue()
        {
            ClearDelayQueue();
            ClearQueue();
            ClearUIQueue();
            ClearLateUpdate();
        }

#if !CWJ_DEVELOPMENT_BUILD
        protected override void _Awake()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
#endif


        private void Update()
        {
            ProcessQueue(_ActionQueue, MAX_ACTIONS_PER_FRAME
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                       , nameof(_ActionQueue)
#endif
            );

            ProcessQueue(_UiActionsQueue, MAX_UI_ACTIONS_PER_FRAME
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                       , nameof(_UiActionsQueue)
#endif
            );

            ProcessDelayedQueue();
        }

        private void LateUpdate()
        {
            ProcessQueue(_LateUpdateActionsQueue, MAX_LATEUPDATE_ACTIONS_PER_FRAME
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                       , nameof(_LateUpdateActionsQueue)
#endif
            );
        }

        public static void Enqueue(Action action)
        {
            if (action == null)
            {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                Debug.LogError("Enqueue action is null");
#endif
                return;
            }

            _ActionQueue.Enqueue(action);
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
            int cnt = _ActionQueue.Count;
            if (MAX_ACTIONS_PER_FRAME < cnt)
            {
                Debug.LogWarning($"_ActionQueue가 최대허가수({MAX_ACTIONS_PER_FRAME}) 보다 많은 작업량({cnt}개)을 갖고있습니다.");
            }
#endif
        }

        public static void Enqueue<T>(Action<T> action, T data)
        {
            Enqueue(action == null ? null : () => action.Invoke(data));
        }

        public static void UIEnqueue(Action action)
        {
            if (action == null)
            {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                Debug.LogError("UIEnqueue action is null");
#endif
                return;
            }

            _UiActionsQueue.Enqueue(action);
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
            int cnt = _UiActionsQueue.Count;
            if (MAX_UI_ACTIONS_PER_FRAME < cnt)
            {
                Debug.LogWarning($"_UiActionsQueue가 최대허가수({MAX_UI_ACTIONS_PER_FRAME}) 보다 많은 작업량({cnt}개)을 갖고있습니다.");
            }
#endif
        }

        public static void UIEnqueue<T>(Action<T> action, T data)
        {
            UIEnqueue(action == null ? null : () => action.Invoke(data));
        }

        public static void LateUpdateQueue(Action action)
        {
            if (action == null)
            {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                Debug.LogError("LateUpdateQueue action is null");
#endif
                return;
            }

            _LateUpdateActionsQueue.Enqueue(action);
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
            int cnt = _LateUpdateActionsQueue.Count;
            if (MAX_LATEUPDATE_ACTIONS_PER_FRAME < cnt)
            {
                Debug.LogWarning($"_LateUpdateActionsQueue가 최대허가수({MAX_LATEUPDATE_ACTIONS_PER_FRAME}) 보다 많은 작업량({cnt}개)을 갖고있습니다.");
            }
#endif
        }

        public static void LateUpdateQueue<T>(Action<T> action, T data)
        {
            LateUpdateQueue(action == null ? null : () => action.Invoke(data));
        }


        /// <summary>
        /// TODO : 테스트 필요
        /// </summary>
        /// <param name="action">딜레이 콜백</param>
        /// <param name="delay">딜레이 시간</param>
        public static void EnqueueDelayed(Action action, float delay)
        {
            if (action == null)
            {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                Debug.LogError("EnqueueDelayed action is null");
#endif
                return;
            }

            if (delay <= CacheFrameTimeThreshold())
            {
                // delay가 프레임 시간보다 작으면 바로 Enqueue
                Enqueue(action);
                return;
            }

            float executeTime = Time.time + delay;

            lock (_DelayedActionsLock)
            {
                _DelayedActionsSet.Add(new DelayedQueueItem(action, executeTime));
            }
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
            int cnt = _DelayedActionsSet.Count;
            if (MAX_DELAYED_ACTIONS_PER_FRAME < cnt)
            {
                Debug.LogWarning($"_DelayedActionsSet가 최대허가수({MAX_DELAYED_ACTIONS_PER_FRAME}) 보다 많은 작업량({cnt}개)을 갖고있습니다.");
            }
#endif
        }

        public static void EnqueueDelayed<T>(Action<T> action, T data, float delay)
        {
            EnqueueDelayed(action == null ? null : () => action.Invoke(data), delay);
        }

        public static void ClearQueue()
        {
            _ActionQueue.Clear();
        }

        public static void ClearDelayQueue()
        {
            _DelayedActionsSet.Clear();
        }


        public static void ClearUIQueue()
        {
            _UiActionsQueue.Clear();
        }

        public static void ClearLateUpdate()
        {
            _LateUpdateActionsQueue.Clear();
        }

        private void ProcessQueue(ActionQueue queue, int maxActionCnt
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                                , string queueName
#endif
        )
        {
            int processedCount = 0;
            while (processedCount < maxActionCnt && queue.TryDequeue(out var action))
            {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                try
                {
#endif
                    action.Invoke();
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception in {queueName} action: {e}");
                }
#endif
                ++processedCount;
            }
        }
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
        [Serializable]
#endif
        public struct DelayedQueueItem : IComparable<DelayedQueueItem>, IEquatable<DelayedQueueItem>
        {
            public bool   hasValue;
            public float  time;
            public Action action;

            public DelayedQueueItem(Action action, float executeTime)
            {
                this.time = executeTime;
                this.action = action;
                hasValue = true;
            }

            public int CompareTo(DelayedQueueItem other)
            {
                int timeComparison = time.CompareTo(other.time);
                if (timeComparison != 0)
                    return timeComparison;

                return GetHashCode().CompareTo(other.GetHashCode());
            }

            public bool Equals(DelayedQueueItem other)
            {
                return hasValue == other.hasValue && Mathf.Approximately(time, other.time) && action == other.action;
            }

            public override int GetHashCode()
            {
                return HashCodeHelper.GetHashCode(hasValue, time, action);
            }
        }

        // private static bool IsApproximatelyEqual(float a, float b, float epsilon = 0.01f)
        // {
        //     return Mathf.Abs(a - b) <= epsilon;
        // }

        private float delayedQueueTimer = 0f;

        private void ProcessDelayedQueue()
        {
            delayedQueueTimer += Time.deltaTime;

            if (delayedQueueTimer < DELAYED_QUEUE_INTERVAL)
                return;

            delayedQueueTimer = 0f;

            int processedCount = 0;
            float currentTime = Time.time;

            lock (_DelayedActionsLock)
            {
                while (processedCount < MAX_DELAYED_ACTIONS_PER_FRAME && _DelayedActionsSet.Count > 0)
                {
                    var delayedItem = _DelayedActionsSet.Min;
                    if (!delayedItem.hasValue || delayedItem.action == null)
                    {
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                        Debug.LogError("_DelayedActionsSet default값 또는 Action 삭제 된 문제 발견 (이게 가능한가?)");
#endif
                        _DelayedActionsSet.Remove(delayedItem);
                        continue;
                    }

                    if (currentTime < delayedItem.time)
                    {
                        break;
                    }

                    _DelayedActionsSet.Remove(delayedItem);
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                    try
                    {
#endif
                        delayedItem.action.Invoke();
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Exception in delayed action: {e}");
                    }
#endif
                    processedCount++;
                }
            }
        }
    }
}
