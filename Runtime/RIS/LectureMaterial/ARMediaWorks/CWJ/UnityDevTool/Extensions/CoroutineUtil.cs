using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
using CWJ.Unity.EditorCor.Editor;
#endif

namespace CWJ
{
    public static class CoroutineUtil
    {
        public static System.Collections.IEnumerator GetCoroutine_RepeatUntilPredicate(System.Action action, System.Func<bool> predicate, float startDelay = 0, float repeatTime = 0, System.Action afterAction = null)
        {
            yield return null;

            if (startDelay > 0)
                yield return new WaitForSeconds(startDelay);

            if (repeatTime <= 0f)
            {
                do
                {
                    yield return null;
                    action();
                }
                while (!predicate());
            }
            else
            {
                var waitSec = new WaitForSeconds(repeatTime);
                do
                {
                    yield return waitSec;
                    action();
                }
                while (!predicate());
            }

            afterAction?.Invoke();
        }

        public static System.Collections.IEnumerator GetCoroutine_WaitUntilWithTimeout(float timeout, Func<bool> predicate, Func<bool> forceStop = null, Action successAction = null, Action timeoutAction = null, float startDelay = 0, float repeatTime = 0)
        {
            yield return null;

            if (startDelay > 0)
                yield return new WaitForSeconds(startDelay);

            if (predicate.Invoke())
            {
                successAction?.Invoke();
                yield break;
            }

            if (forceStop == null)
            {
                forceStop = () => MonoBehaviourEventHelper.IS_QUIT;
            }

            if (forceStop.Invoke())
            {
                yield break;
            }

            bool isForceStop = false;
            bool isSuccess = false;
            float t = 0;

            if (repeatTime > 0f)
            {
                var waitSec = new WaitForSeconds(repeatTime);
                do
                {
                    if (forceStop.Invoke())
                    {
                        isForceStop = true;
                        break;
                    }
                    yield return waitSec;
                    t += repeatTime;
                    isSuccess = predicate.Invoke();
                }
                while (t < timeout && !isSuccess);
            }
            else
            {
                do
                {
                    if (forceStop.Invoke())
                    {
                        isForceStop = true;
                        break;
                    }
                    yield return null;
                    t += Time.deltaTime;
                    isSuccess = predicate.Invoke();
                }
                while (t < timeout && !isSuccess);
            }

            if (isForceStop)
            {
                yield break;
            }
            else
            {
                if (isSuccess)
                    successAction?.Invoke();
                else if (t >= timeout)
                    timeoutAction?.Invoke();
            }
        }

        public static System.Collections.IEnumerator GetCoroutine_RepeatUntilTimer(System.Action action, float timer, float startDelay = 0, float repeatTime = 0, System.Action afterAction = null)
        {
            yield return null;

            if (startDelay > 0)
                yield return new WaitForSeconds(startDelay);

            float t = 0;
            if (repeatTime <= 0f)
            {
                do
                {
                    yield return null;
                    t += Time.deltaTime;
                    action();
                }
                while (t < timer);
            }
            else
            {
                var waitSec = new WaitForSeconds(repeatTime);
                do
                {
                    yield return waitSec;
                    t += repeatTime;
                    action();
                }
                while (t < timer);
            }

            afterAction?.Invoke();
        }

        public static System.Collections.IEnumerator Wait(object instruction, System.Action<object> callback)
        {
            if (callback == null) throw new System.ArgumentNullException(nameof(callback));
            yield return instruction;
            callback(instruction);
        }

        public static Coroutine_New StaticStartCoroutine(System.Collections.IEnumerator enumerator, UnityAction startAct, UnityAction endAct)
        {
            return MonoBehaviourEventHelper.StartNewCoroutine_Static(enumerator, true, startAct, endAct);
        }

#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
        public static Coroutine_Editor Editor_StartCoroutine(this MonoBehaviour behaviour, System.Collections.IEnumerator enumerator)
        {
            return EditorCoroutineUtil.StartCoroutine(enumerator, behaviour);
        }
#endif


        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Collections.IEnumerable enumerable)
        {
            if (behaviour == null) throw new System.ArgumentNullException(nameof(behaviour));
            return behaviour.StartCoroutine(enumerable.GetEnumerator());
        }

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Func<System.Collections.IEnumerator> method)
        {
            if (behaviour == null) throw new System.ArgumentNullException(nameof(behaviour));
            if (method == null) throw new System.ArgumentNullException(nameof(method));

            return behaviour.StartCoroutine(method());
        }

        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Delegate method, params object[] args)
        {
            if (behaviour == null) throw new System.ArgumentNullException(nameof(behaviour));
            if (method == null) throw new System.ArgumentNullException(nameof(method));

            System.Collections.IEnumerator e;
            if (TypeUtil.IsType(method.Method.ReturnType, typeof(System.Collections.IEnumerable)))
            {
                e = (method.DynamicInvoke(args) as System.Collections.IEnumerable).GetEnumerator();
            }
            else if (TypeUtil.IsType(method.Method.ReturnType, typeof(System.Collections.IEnumerator)))
            {
                e = (method.DynamicInvoke(args) as System.Collections.IEnumerator);
            }
            else
            {
                throw new System.ArgumentException("Delegate must have a return type of IEnumerable or IEnumerator.", nameof(method));
            }

            return behaviour.StartCoroutine(e);
        }





        public static Coroutine InvokeLegacy(this MonoBehaviour behaviour, System.Action method, float delay)
        {
            if (behaviour == null) throw new System.ArgumentNullException(nameof(behaviour));
            if (method == null) throw new System.ArgumentNullException(nameof(method));

            return behaviour.StartCoroutine(InvokeRedirect(method, delay));
        }

        private static System.Collections.IEnumerator InvokeRedirect(System.Action action, float delay = 0, float repeatRate = -1f, System.Action afterAction = null)
        {
            yield return null;

            if (delay > 0)
                yield return new WaitForSeconds(delay);

            if (repeatRate < 0f)
            {
                action();
            }
            else if (repeatRate == 0f)
            {
                while (true)
                {
                    action();
                    yield return null;
                }
            }
            else
            {
                var r = new WaitForSeconds(repeatRate);
                while (true)
                {
                    action();
                    yield return r;
                }
            }

            afterAction?.Invoke();
        }

        internal static System.Collections.IEnumerator InvokeAfterYieldRedirect(System.Action method, object yieldInstruction)
        {
            yield return null;
            yield return yieldInstruction;
            method();
        }
    }
}
