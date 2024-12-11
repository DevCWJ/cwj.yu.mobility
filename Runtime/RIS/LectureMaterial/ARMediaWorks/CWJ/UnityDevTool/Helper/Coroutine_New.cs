using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
using CWJ.Unity.EditorCor.Editor;
#endif

namespace CWJ
{
    /// <summary>
    /// 중복실행 되는걸 원하면 그냥 StartCoroutine쓰기
    /// <para/>실행중인지 인스펙터를 통해 알수있음, 중복실행 안됨
    /// <br/>(중복실행막거나 실행중인거 중지후 실행되도록 설정가능)
    /// <br/>(Coroutine != null 은 StartCoroutine이 yield return 에서 넘겨주기전까지 null이기때문에 만든 coroutine helper클래스)
    /// <para/>[Runtime중인지 아닌지 자동체크후 알아서 Editor에서도 실행됨]
    /// <br/>아래는 Editor 실행시 주의사항
    /// <br/>Editor <see cref="IEnumerator"/> 에서 실행할땐
    /// <br/>yeild 시간대기는 <see cref="WaitForSecondsRealtime"/> 만 사용할것. 
    /// <br/>최소대기(프레임단위) 중에 시간체크가 필요하면  <see cref="Time.realtimeSinceStartup"/>를 이용.
    /// <br/><see langword="yield return"/> WaitForSeconds 은 null처럼 프레임지날때마다 넘어감
    /// </summary>
    [System.Serializable]
    public class Coroutine_New : IDisposable
    {
        [SerializeField] MonoBehaviour behaviour;
        [Readonly, SerializeField] private string name;

        [Readonly] private bool isNotStartWhenAlreadyRunning;

        [Readonly] private bool _isReadyToRun_safetyTrigger;
        [Readonly] private bool _isRunning_insideCor;
        public bool isRunning => _isReadyToRun_safetyTrigger || _isRunning_insideCor;

        public bool IsValid() => behaviour != null;


        public UnityEvent startEvent = null;
        public UnityEvent endEvent = null;

        private Coroutine wrapperCoroutine;
        private Coroutine doCoroutine;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
        private Coroutine_Editor editor_wrapperCoroutine;
        private Coroutine_Editor editor_doCoroutine;
#endif


        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour">필수</param>
        /// <param name="isNotStartWhenAlreadyRunning"> <see langword="true"/>: 중복실행막음/ <see langword="false"/>: 실행중이던거 중지후 실행</param>
        /// <param name="nameForClassify"></param>
        public Coroutine_New(MonoBehaviour behaviour, bool isNotStartWhenAlreadyRunning = true, string nameForClassify = null)
        {
            _AllStopCorImmediately();
            Debug.Assert(behaviour);
            this.behaviour = behaviour;
            if (nameForClassify != null)
                name = nameForClassify;
            else
                name = $"{behaviour.gameObject.name}.{behaviour.GetType().Name}";
            this.isNotStartWhenAlreadyRunning = isNotStartWhenAlreadyRunning;
        }

        public void ModifySettings(UnityAction startAction = null, UnityAction endAction = null, bool forcedStop = false)
        {
            if (startAction != null)
            {
                if (startEvent == null) startEvent = new UnityEvent();
                startEvent.AddListener_New(startAction);
            }
            if (endAction != null)
            {
                if (endEvent == null) endEvent = new UnityEvent();
                endEvent.AddListener_New(endAction);
            }

            if (forcedStop)
                _AllStopCorImmediately();
        }

        public bool StartCoroutine(IEnumerator enumerator, UnityAction startAction = null, UnityAction endAction = null)
        {
            Debug.Assert(behaviour);
            void _StartCor()
            {
                if (startAction != null || endAction != null)
                    ModifySettings(startAction, endAction);
                _isReadyToRun_safetyTrigger = true;
                _isRunning_insideCor = false;

                if (startEvent != null)
                {
                    startEvent.Invoke();
                    startEvent.RemoveAllListeners_New();
                }

                try
                {
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
                    if (!UnityEditor.EditorApplication.isPlaying)
                        editor_wrapperCoroutine = EditorCoroutineUtil.StartCoroutine(WrapperRoutine(enumerator), behaviour);
                    else
#endif
                        wrapperCoroutine = behaviour.StartCoroutine(WrapperRoutine(enumerator));
                }
                catch
                {
                    StopCoroutineSafely();
                    return;
                }
            }

            if (isRunning)
            {
                if (isNotStartWhenAlreadyRunning)
                {
                    //Debug.LogException(new System.InvalidProgramException("I Can't run when it's already running\n" + nameof(IsCanNotStartWhenAlreadyRunning) + " : " + IsCanNotStartWhenAlreadyRunning), behaviour);
                    return false;
                }
                else
                {
                    ModifySettings(null, _StartCor);
                    StopCoroutineSafely();
                }
            }
            else
            {
                _StartCor();
            }



            return true;
        }


        IEnumerator WrapperRoutine(IEnumerator enumerator)
        {
            _isReadyToRun_safetyTrigger = true;
            _isRunning_insideCor = false;
            yield return null;
            _isRunning_insideCor = true;
            _isReadyToRun_safetyTrigger = false;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
            if (!UnityEditor.EditorApplication.isPlaying)
                yield return (editor_doCoroutine = EditorCoroutineUtil.StartCoroutine(enumerator, this.behaviour));
            else
#endif
                yield return (doCoroutine = this.behaviour.StartCoroutine(enumerator));

            _isRunning_insideCor = false;
            doCoroutine = null;
            wrapperCoroutine = null;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
            editor_wrapperCoroutine = null;
            editor_doCoroutine = null;
#endif
        }

        bool CanStop()
        {
            return !_isReadyToRun_safetyTrigger && _isRunning_insideCor;
        }

        public IEnumerator StopUntilWaitForDoRoutineExists()
        {
            float timeout = 3f;

#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                float interval = 0.03f;
                var waitInEditor = new WaitForSecondsRealtime(interval);
                do
                {
                    yield return waitInEditor;
                    timeout -= interval;
                }
                while (timeout > 0 && !CanStop());
            }
            else
#endif
                yield return new WaitUntilWithTimeout(CanStop, timeout);

            _AllStopCorImmediately();
            yield break;
        }

        bool isStopping = false;
        public void StopCoroutineSafely()
        {
            if (!isRunning || CanStop())
            {
                _AllStopCorImmediately();
                return;
            }

            if (!isStopping)
            {
                isStopping = true;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
                if (!UnityEditor.EditorApplication.isPlaying)
                    EditorCoroutineUtil.StartCoroutine(StopUntilWaitForDoRoutineExists(), behaviour);
                else
#endif
                    behaviour.StartCoroutine(StopUntilWaitForDoRoutineExists());
            }
        }

        public void _AllStopCorImmediately()
        {
            if (doCoroutine != null) behaviour.StopCoroutine(doCoroutine);
            doCoroutine = null;
            if (wrapperCoroutine != null) behaviour.StopCoroutine(wrapperCoroutine);
            wrapperCoroutine = null;
#if UNITY_EDITOR && CWJ_EXISTS_EDITORCOROUTINE
            if (editor_doCoroutine != null) EditorCoroutineUtil.StopCoroutine(editor_doCoroutine);
            editor_doCoroutine = null;
            if (editor_wrapperCoroutine != null) EditorCoroutineUtil.StopCoroutine(editor_wrapperCoroutine);
            editor_wrapperCoroutine = null;
#endif
            isStopping = false;

            _isReadyToRun_safetyTrigger = _isRunning_insideCor = false;
            if (endEvent != null)
            {
                endEvent.Invoke();
                endEvent.RemoveAllListeners_New();
            }
        }

        public void Dispose()
        {
            _AllStopCorImmediately();
            behaviour = null;
            startEvent = null;
            endEvent = null;
        }
    }
}
