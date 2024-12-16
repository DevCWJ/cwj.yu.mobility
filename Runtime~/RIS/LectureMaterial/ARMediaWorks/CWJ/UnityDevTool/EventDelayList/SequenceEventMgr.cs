using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CWJ
{
    public class SequenceEventMgr : MonoBehaviour
    {
        public bool autoStart;
        private bool started = false;

        public bool onlyOnceExcute;

        public List<SequenceEvent> eventList = new List<SequenceEvent>();

        private void Start()
        {
            if (autoStart)
            {
                ExcuteSequence();
            }
        }

        public void ExcuteSequence()
        {
            if (!onlyOnceExcute || (onlyOnceExcute && !started))
            {
                started = true;
                CO_ProcessSequence = StartCoroutine(DO_ProcessSequence());
            }
        }

        private Coroutine CO_ProcessSequence;

        private IEnumerator DO_ProcessSequence()
        {
            int count = eventList.Count;

            for (int i = 0; i < count; i++)
            {
                if (eventList[i].delay > 0)
                {
                    yield return new WaitForSeconds(eventList[i].delay);
                }
                eventList[i].unityEvent.Invoke();
            }
        }
    }
}