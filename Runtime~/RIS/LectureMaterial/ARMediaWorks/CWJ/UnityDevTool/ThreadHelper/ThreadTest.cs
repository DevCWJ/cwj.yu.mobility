using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace CWJ
{
    public class ThreadTest : MonoBehaviour
    {
        private SortedSet<MultiThreadHelper.DelayedQueueItem> actionSortedSet = new SortedSet<MultiThreadHelper.DelayedQueueItem>();
        Queue<MultiThreadHelper.DelayedQueueItem> actionsCache;
        [SerializeField] SortedQueue<MultiThreadHelper.DelayedQueueItem> actionQueue = null;
        [InvokeButton]
        void Enqueue(Action action, float delay = 0)
        {
            if (actionQueue == null)
                actionQueue = new SortedQueue<MultiThreadHelper.DelayedQueueItem>(new MultiThreadHelper.DelayedQueueItem[5]);
            void actionTest()
            {

            }
            if (action == null)
            {
                action = actionTest;
                //������ return;
            }
            lock (actionQueue)
            {
                actionQueue.Enqueue(new MultiThreadHelper.DelayedQueueItem(action, delay));
                Debug.LogError("[ADD] " + delay);
            }
        }

        [InvokeButton]
        void CheckList()
        {
            Debug.LogError("-----------\n" + (actionQueue.TryDequeue(out var a) ? (a.time + " " + a.hasValue) : ""));

            foreach (var item in actionQueue)
            {
                Debug.LogError(item.time + " " + item.hasValue);
            }
        }
        [InvokeButton]
        void New()
        {
            actionQueue = new SortedQueue<MultiThreadHelper.DelayedQueueItem>(new MultiThreadHelper.DelayedQueueItem[4]);
        }
        [InvokeButton]
        void Clear()
        {
            actionQueue.Clear();
        }
        [InvokeButton]
        void FindAndClassify()
        {
            Debug.LogError("-------------------------" + actionSortedSet.Count);
            if (actionSortedSet.Count == 0)
            {
                return;
            }

            bool isTimeToAct(float t)
            {
                return t == 0 || t <= Time.time;
            }
            try
            {
                int cnt = actionSortedSet.Count;
                lock (actionSortedSet)
                {
                    actionsCache = new Queue<MultiThreadHelper.DelayedQueueItem>(cnt);
                    foreach (var item in actionSortedSet)
                        actionsCache.Enqueue(item);
                    actionSortedSet.Clear();
                }
                for (int i = 0; i < cnt; i++)
                {

                }
                actionSortedSet = new SortedSet<MultiThreadHelper.DelayedQueueItem>(actionsCache);
                for (int i = 0; i < cnt; i++)
                {
                }
                MultiThreadHelper.DelayedQueueItem min = actionSortedSet.Min;
                while (min.hasValue && isTimeToAct(min.time))
                {
                    actionSortedSet.Remove(min);
                    min = actionSortedSet.Min;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
