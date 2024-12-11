using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    using static MonoBehaviourEventHelper;

    [UnityEngine.DefaultExecutionOrder(32000)]
    public class LastCloseObject : MonoBehaviour
    {
        public UnityEvent quitEvent = new UnityEvent();
        public UnityEvent lastQuitEvent = new UnityEvent();
        public List<Coroutine_New> coroutineTrackeds;
        private void OnApplicationQuit()
        {
            quitEvent?.Invoke();

            if (IS_EDITOR)
            {
                lastQuitEvent?.Invoke();
                lastQuitEvent = null;
            }
        }

        //OnApplicationQuit보다 OnDestroy가 더 늦게 실행됨 즉, DontDestroyOnLoad 싱글톤의 OnDestroy가 프로그램종료 시 가장 늦게 실행될거임
        //에디터에서는 실행종료된다해도 실행되지않음.
        private void OnDestroy()
        {
            if (!IS_EDITOR)
            {
                lastQuitEvent?.Invoke();
                lastQuitEvent = null;
            }
        }
    }
}
