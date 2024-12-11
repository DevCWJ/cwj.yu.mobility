using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
namespace CWJ
{
    [DefaultExecutionOrder(ThreadHelper.ExecutionOrder)]
    public class ThreadHelper : MonoBehaviour
	{
		const int ExecutionOrder = -31000;

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
        public static void InitializeOnLoad()
		{
            CWJ.AccessibleEditor.CWJ_EditorEventHelper.ProjectOpenEvent += EditorEventSystem_ProjectOpenEvent;
        }

        private static void EditorEventSystem_ProjectOpenEvent()
        {
            try
            {
                CWJ.AccessibleEditor.ScriptExecutionOrder.SetMonoBehaviourExecutionOrder<ThreadHelper>(ThreadHelper.ExecutionOrder);
            }
            finally
            {
            }
        }
#endif


		private static int curProcessorCnt = 0;
		public static int MaxThreadCnt
		{
			get
			{
				if (curProcessorCnt == 0) 
					curProcessorCnt = SystemInfo.processorCount;
				return curProcessorCnt;
			}
		}
		public static int CurRunThreadCnt = 0;

		private static ThreadHelper _current;
		public static ThreadHelper Instance
		{
			get
			{
				Initialize();
				return _current;
			}
		}

		void Awake()
		{
			_current = this;
			initialized = true;
		}

		static bool initialized = false;

		static void Initialize()
		{
			if (!Application.isPlaying || initialized)
				return;
			initialized = true;
			var g = new GameObject(nameof(ThreadHelper));
			g.SetActive(false);
			DontDestroyOnLoad(g);
			_current = g.AddComponent<ThreadHelper>();
			g.SetActive(true);
		}

		private List<Action> _actions = new List<Action>();
		public struct DelayedQueueItem
		{
			public float time;
			public Action action;
		}
		private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

		List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

		public static void QueueOnMainThread(Action action, float time = 0.0f)
		{
			if (action == null)
			{
				return;
			}
			if (time > 0.0f)
			{
				lock (Instance._delayed)
				{
					Instance._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
				}
			}
			else
			{
				lock (Instance._actions)
				{
					Instance._actions.Add(action);
				}
			}
		}

		public Thread RunAsync(Action a)
		{
			while (CurRunThreadCnt >= MaxThreadCnt)
			{
				Thread.Sleep(1);
			}
			Interlocked.Increment(ref CurRunThreadCnt);
			ThreadPool.QueueUserWorkItem(RunAction, a);
			return null;
		}

		private static void RunAction(object objAct)
		{
			try
			{
				var action = objAct as Action;
				if (action != null)
					action();
			}
#if UNITY_EDITOR
			catch (Exception e)
			{
				Debug.LogError("loom exception: " + "(num)" + CurRunThreadCnt + " :" + e);
			}
#endif
			finally
			{
				Interlocked.Decrement(ref CurRunThreadCnt);
                //Debug.Log("loom decream: " + "(num)" + CurRunThreadCnt);
            }

        }


		void OnDisable()
		{
			if (_current == this)
			{
				_current = null;
			}
		}


		List<Action> _currentActions = new List<Action>();

		void Update()
		{
			lock (_actions)
			{
				_currentActions.Clear();
				_currentActions.AddRange(_actions);
				_actions.Clear();
			}
			foreach (var act in _currentActions)
			{
				act?.Invoke();
			}
			lock (_delayed)
			{
				_currentDelayed.Clear();
				_currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
				foreach (var item in _currentDelayed)
					_delayed.Remove(item);
			}
			foreach (var delayed in _currentDelayed)
			{
				delayed.action();
			}

		}
	} 
}