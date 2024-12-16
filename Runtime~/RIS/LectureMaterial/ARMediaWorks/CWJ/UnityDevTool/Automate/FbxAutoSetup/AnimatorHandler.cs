
using System;
using UnityEngine;
using UnityEngine.Events;
using CWJ.Serializable;
using System.Collections.Generic;
using System.Collections;

namespace CWJ
{
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class AnimatorHandler : MonoBehaviour
    {
        [SerializeField] private Animator myAnimator;
        public bool isReserveSetTriggerContinuously = false;
        public string reserveSetTriggerName = null;
        public bool isSetIdleOnDisable;

        [SerializeField, Readonly] private string[] triggerNamesCache;
        [SerializeField, Readonly] private string[] animClipNamesCache;

        public string[] GetTriggerNames() => triggerNamesCache;
        public string[] GetAnimClipNames() => animClipNamesCache;


        public void InitWhenAutoSetup(Animator animator, string[] triggerNameList, string[] animClipNames)
        {
            myAnimator = animator;
            this.triggerNamesCache = triggerNameList;
            this.animClipNamesCache = animClipNames;
        }

        public UnityEvent<string, bool> onAnimEventWithTrigger = new();

        public DictionaryVisualized<string, UnityEvent<bool>> eventByTriggerName = new();
        public void AddAnimEvent_TriggerName(string triggerName, UnityAction<bool> callback)
        { _AddAnimEvent(eventByTriggerName, triggerName, callback); }
        public void RemoveAnimEvent_TriggerName(string triggerName, UnityAction<bool> callback)
        { _RemoveAnimEvent(eventByTriggerName, triggerName, callback); }

        public DictionaryVisualized<string, UnityEvent<bool>> eventByAnimStateName = new();
        public void AddAnimEvent_StateName(string animStateName, UnityAction<bool> callback)
        { _AddAnimEvent(eventByAnimStateName, animStateName, callback); }
        public void RemoveAnimEvent_StateName(string animStateName, UnityAction<bool> callback)
        { _RemoveAnimEvent(eventByAnimStateName, animStateName, callback); }

        private void Awake()
        {
            myAnimator.keepAnimatorStateOnDisable = true;
        }

        private void OnEnable()
        {
            CheckReserveTrigger();
        }

        private void OnDisable()
        {
            if (isSetIdleOnDisable)
                SetOff();
        }

        string lastSetTriggerName = null;
        bool isStartEventInvoked = false;
        bool isEndEventInvoked = false;

        void InitEventMark()
        {
            lastSetTriggerName = null;
            isStartEventInvoked = true;
            isEndEventInvoked = true;
        }
        [InvokeButton]
        public void SetTrigger(string triggerName)
        {
            if (!myAnimator || string.IsNullOrEmpty(triggerName)) return;
            if (!triggerNamesCache.IsExists(triggerName))
            {
                Debug.LogError("존재하지 않는 Trigger 이름");
                return;
            }
            ResetAllTrigger();
            InitEventMark();
            ThreadDispatcher.Enqueue(() =>
            {
                lastSetTriggerName = triggerName;
                isStartEventInvoked = false;
                isEndEventInvoked = false;
                myAnimator.SetTrigger(triggerName);
            });
        }

        [InvokeButton]
        public void SetOff()
        {
            if (!myAnimator) return;
            // Debug.LogError("SetOff - " + gameObject.name, gameObject);
            ResetAllTrigger();
            InitEventMark();
            myAnimator.SetTrigger(FbxAutoSetupHelper.TurnOffTrigger);
        }

        void ResetAllTrigger()
        {
            if (!myAnimator) return;
            myAnimator.ResetTrigger(FbxAutoSetupHelper.TurnOffTrigger);
            for (int i = 0; i < triggerNamesCache.Length; i++)
            {
                myAnimator.ResetTrigger(triggerNamesCache[i]);
            }
        }

        bool IsMatchedLastTriggeredName(string animStateName, out string triggerName)
        {
            triggerName = null;
            if (string.IsNullOrEmpty(animStateName) || string.IsNullOrEmpty(lastSetTriggerName))
                return false;
            triggerName = lastSetTriggerName;
            return animStateName.Equals(lastSetTriggerName) || animStateName.StartsWith(lastSetTriggerName + "_");
        }

        public void Evt_OnAnimationStart(string animClipName)
        {
//#if UNITY_EDITOR
//            Debug.Log("Start : " + animClipName);
//#endif
            if (this.enabled && !isStartEventInvoked)
                _OnAnimStartOrEnd(animClipName, true);
        }

        public void Evt_OnAnimationEnd(string animClipName)
        {
//#if UNITY_EDITOR
//            Debug.Log("End : " + animClipName);
//#endif
            if (this.enabled && !isEndEventInvoked)
                _OnAnimStartOrEnd(animClipName, false);
        }

        void _OnAnimStartOrEnd(string animClipName, bool isStart)
        {
            if (!IsMatchedLastTriggeredName(animClipName, out string triggerName))
            {
                return;
            }
            if (isStart)
                isStartEventInvoked = true;
            else
                isEndEventInvoked = true;
            if (eventByAnimStateName.TryGetValue(animClipName, out var animStateEvt) && animStateEvt != null)
                animStateEvt.Invoke(isStart);

            if (eventByTriggerName.TryGetValue(triggerName, out var triggerEvt) && triggerEvt != null)
                triggerEvt.Invoke(isStart);

            onAnimEventWithTrigger?.Invoke(triggerName, isStart);
#if UNITY_EDITOR
            Debug.Log("AnimEvent Invoke > " + $"{animClipName} : {(isStart ? "Start" : "End")}".SetColor(isStart?new Color().GetLightGreen(): new Color().GetLightRed()));
#endif
        }

        static void _AddAnimEvent(DictionaryVisualized<string, UnityEvent<bool>> dict, string key, UnityAction<bool> callback)
        {
            if (!dict.TryGetValue(key, out var uevent))
            {
                uevent = new UnityEvent<bool>();
                dict.Add(key, uevent);
            }
            uevent.AddListener(callback);
        }

        static void _RemoveAnimEvent(DictionaryVisualized<string, UnityEvent<bool>> dict, string key, UnityAction<bool> callback)
        {
            if (!dict.TryGetValue(key, out var uevent) || uevent == null)
            {
                return;
            }
            uevent.RemoveListener(callback);
        }

        void CheckReserveTrigger()
        {
            if (!string.IsNullOrEmpty(reserveSetTriggerName))
            {
                ThreadDispatcher.Enqueue(SetTrigger, reserveSetTriggerName);
                if (!isReserveSetTriggerContinuously)
                    reserveSetTriggerName = null;
            }
        }

        // public void SetActiveObject_AnimSafely(GameObject rootObj, bool isActive)
        // {
        //     if (!rootObj)
        //     {
        //         return;
        //     }
        //     if (!myAnimator)
        //     {
        //         if (rootObj.activeSelf != isActive)
        //             rootObj.SetActive(isActive);
        //         return;
        //     }
        //
        //     if (rootObj.activeSelf != isActive)
        //         rootObj.SetActive(isActive);
        //     return;
        //
        //     // myAnimator.keepAnimatorStateOnDisable = true; << 해주면 오브젝트 꺼지기전에 애니메이션 초기화 (Idle 변경 + 대기 등등) 해줄 필요없음.
        //     // 오브젝트 꺼지기전에 애니메이션 초기화 (Idle 변경 + 대기 등등) 해주던 예전 작업들...1
        //     // if (!_hasReservedSetActive)
        //     // {
        //     //     _hasReservedSetActive = true;
        //     //     _reservedSetActive = isActive;
        //     //     if (rootObj.activeSelf)
        //     //         SetOff();
        //     //     ThreadDispatcher.LateUpdateQueue(() =>
        //     //     {
        //     //         if (!_hasReservedSetActive)
        //     //         {
        //     //             return;
        //     //         }
        //     //         _hasReservedSetActive = false;
        //     //
        //     //         bool active = _reservedSetActive;
        //     //         if (active != rootObj.activeSelf)
        //     //             rootObj.SetActive(active);
        //     //         //Debug.LogError("실제실행: " + rootObj.name + " > " + active, rootObj);
        //     //
        //     //         if (active && rootObj.activeInHierarchy)
        //     //         {
        //     //             CheckReserveTrigger();
        //     //         }
        //     //     });
        //     // }
        //     // else
        //     // {
        //     //     _reservedSetActive = isActive;
        //     // }
        //
        // }

        // 오브젝트 꺼지기전에 애니메이션 초기화 (Idle 변경 + 대기 등등) 해주던 예전 작업들...2
        // private bool _hasReservedSetActive;
        // private bool _reservedSetActive;
        //
        // private bool _IsActiveReserved()
        // {
        //     return _hasReservedSetActive && _reservedSetActive;
        // }

        // Coroutine CO_SetActivateCoroutine = null;

        // private IEnumerator SetActivateCoroutine(GameObject targetObj)
        // {
        //     yield return null;
        //     SetOff();
        //
        //     float t = 0;
        //     // Animator가 "OffState"로 전환될 때까지 대기
        //     bool isOffState = false;
        //     while (!isOffState && t <= 1 && gameObject.activeSelf)
        //     {
        //         AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
        //         if (stateInfo.IsName(FbxAutoSetupHelper.AnimEmptyState))
        //         {
        //             isOffState = true;
        //         }
        //         yield return null;
        //         t += Time.deltaTime;
        //     }
        //
        //     ResetAllTrigger();
        //
        //     yield return null;
        //
        //     CO_SetActivateCoroutine = null;
        //
        //     // 트랜지션이 완료된 후 오브젝트 비활성화
        //     if (targetObj)
        //     {
        //         bool isActive = _IsActiveReserved();
        //         if (isActive != targetObj.activeSelf)
        //             targetObj.SetActive(isActive);
        //         if (isActive && targetObj.activeInHierarchy)
        //             CheckReserveTrigger();
        //     }
        //
        //
        // }
    }
}
