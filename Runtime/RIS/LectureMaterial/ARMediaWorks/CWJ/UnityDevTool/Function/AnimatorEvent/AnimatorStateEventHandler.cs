using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if CWJ_UNITYDEVTOOL && CWJ_EXISTS_UNIRX
using UniRx;
using UniRx.Triggers;
#endif

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    /// <summary>
    /// <para>다른 레이어에 같은 이름의 state가 있다면 _1_ 이런식으로 underscore 사이에 layer 인덱스 적어놓아야 구분됨</para>
    /// <para>animState이름이 많아지면 조금 처리가 곤란해질듯</para>
    /// animState의 공백 제거는 안되어있음
    /// </summary>
    public enum AnimStateEnum //예시
    {
        _0_BaseAnimation,
        BaseAnimation2,
        _1_BaseAnimation,
    }

    [System.Serializable] public class UnityEvent_AnimState : UnityEvent<AnimStateEnum> { }

    /// <summary>
    /// Assetstore에서 UniRx 다운받기
    /// </summary>
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class AnimatorStateEventHandler : MonoBehaviour
    {
#if CWJ_UNITYDEVTOOL && CWJ_EXISTS_UNIRX
#if CWJ_UNITYDEVTOOL
        [GetComponent, Readonly]
#endif
        public Animator animator;
        private ObservableStateMachineTrigger stateMachineObservable;
        private Dictionary<int, AnimStateEnum> hashToAnimStateDic = new Dictionary<int, AnimStateEnum>();

        public UnityEvent_AnimState onStateEnterEvent = new UnityEvent_AnimState();
        public UnityEvent_AnimState onStateUpdateEvent = new UnityEvent_AnimState();
        public UnityEvent_AnimState onStateExitEvent = new UnityEvent_AnimState();
        public UnityEvent_AnimState onStateMachineEnterEvent = new UnityEvent_AnimState();
        public UnityEvent_AnimState onStateMachineExitEvent = new UnityEvent_AnimState();

        [SerializeField, HideInInspector] private string[] animStateNames = null;

        [Tooltip("수정금지. Reset(), OnValidate()에서 자동으로 입력됨.")]
#if CWJ_UNITYDEVTOOL
        [Readonly]
#endif
        public string[] animStateNameWithLayerName = null;

        #region #if UNITY_EDITOR AnimStateEnum 이름의 유효성 검사 & animStateNameWithLayerName에 fullPath 입력
#if UNITY_EDITOR
        private void Reset()
        {
            animStateNames = null;
            CheckValidOfAnimStateEnum();
        }

        private void OnValidate()
        {
            CheckValidOfAnimStateEnum();
        }

        /// <summary>
        /// <see cref="AnimStateEnum"/> 의 animState이름들을 자동으로 <see cref="animStateNameWithLayerName"/> 에 넣어줌
        /// </summary>
        private void CheckValidOfAnimStateEnum()
        {
            animator = GetComponent<Animator>();
            if (animator == null) throw new NullReferenceException();

            var animatorCtrlr = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

            if (animatorCtrlr == null)
            {
                Debug.LogError($"{nameof(animator)}에 Controller를 넣어주세요", animator);
                return;
            }

            string[] curAnimStateNames;
            if(!IsAnimStateEnumModified(out curAnimStateNames))
            {
                return;
            }

            int layerLength = animatorCtrlr.layers.Length;

            string[] allAnimLayerNames = new string[layerLength];

            Dictionary<string, int> stateFullNameAndLayerIndex = new Dictionary<string, int>();

            animStateNameWithLayerName = new string[curAnimStateNames.Length];

            Func<UnityEditor.Animations.AnimatorControllerLayer, string, bool> findLayerByStateName = (layer, stateName) =>
            {
                foreach (var animState in layer.stateMachine.states)
                {
                    if (animState.state.name.Equals(stateName)) return true;
                    //^ stateName을 공백과 상관없이 체크하고싶으면 위 라인에 animState.state.name.Replace(" ", "").Equals(stateName) 로 바꿔넣기
                }
                return false;
            };

            for (int i = 0; i < curAnimStateNames.Length; i++)
            {
                string layerName = null;
                string stateName = null;

                if (curAnimStateNames[i].StartsWith("_"))
                {
                    int endIndex = curAnimStateNames[i].IndexOf('_', 1);
                    string layerIndexStr = curAnimStateNames[i].Substring(1, endIndex - 1);
                    int layerIndex = -1;
                    if (int.TryParse(layerIndexStr, out layerIndex) && 0 <= layerIndex && layerIndex < animatorCtrlr.layers.Length)
                    {
                        stateName = curAnimStateNames[i].Substring(endIndex + 1, curAnimStateNames[i].Length - (endIndex + 1));
                        if (findLayerByStateName(animatorCtrlr.layers[layerIndex], stateName))
                        {
                            layerName = animatorCtrlr.layers[layerIndex].name;
                        }
                        else
                        {
                            stateName = null;
                        }
                    }
                }

                if (stateName == null)
                {
                    stateName = curAnimStateNames[i];
                }

                if (layerName == null)
                {
                    int layerIndex = Array.FindIndex(animatorCtrlr.layers, layer =>
                   {
                       foreach (var animState in layer.stateMachine.states)
                       {
                           if (animState.state.name.Equals(stateName))
                           {
                               return true;
                           }
                       }
                       return false;
                   });

                    if (layerIndex < 0)
                    {
                        Debug.LogError(curAnimStateNames[i].StartsWith("_") ? $"{nameof(AnimStateEnum)}의 '{curAnimStateNames[i]}' 이름에서 첫'_'과 두번째 '_'사이엔 AnimatorController의 레이어 index가 들어가야합니다" : $"{nameof(AnimStateEnum)}의 {i}번째 '{curAnimStateNames[i]}' 는 animator에는 존재하지않는 state이름입니다");
                        continue;
                    }

                    layerName = animatorCtrlr.layers[layerIndex].name;
                }

                if (!string.IsNullOrEmpty(layerName) && !string.IsNullOrEmpty(stateName))
                {
                    animStateNameWithLayerName[i] = layerName + "." + stateName;
                }
                else
                {
                    animStateNameWithLayerName[i] = null;
                }
            }

            if (!Array.Exists(animStateNameWithLayerName,(x => x == null)))
            {
                animStateNames = curAnimStateNames;
            }
        }
#endif
        #endregion #if UNITY_EDITOR AnimStateEnum 이름의 유효성 검사 & animStateNameWithLayerName에 fullPath 입력

        private bool IsAnimStateEnumModified(out string[] curAnimStateNames)
        {
            curAnimStateNames = Enum.GetNames(typeof(AnimStateEnum));
            if (animStateNames.SequenceEqual(curAnimStateNames)) //현재 AnimStateEnum와 캐싱된 상태가 같음.
            {
                return false;
            }
            return true;
        }

        private void Awake()
        {
            if (animator == null) throw new NullReferenceException();

            string[] animStateNames;
            if(IsAnimStateEnumModified(out animStateNames))
            {
                Debug.LogError($"{nameof(AnimStateEnum)}이 수정되어서 문제가 생겼습니다.\n실행종료후 이 씬({gameObject.scene.name})을 다시 실행해주시면 자동으로 update됩니다", gameObject);
                return;
            }

            for (int i = 0; i < animStateNameWithLayerName.Length; i++)
            {
                hashToAnimStateDic.Add(Animator.StringToHash(animStateNameWithLayerName[i]), (AnimStateEnum)i);
            }

            foreach (var behaviour in animator.GetBehaviours<ObservableStateMachineTrigger>())
            {
                behaviour.OnStateEnterAsObservable().Subscribe(OnStateEnter);
                behaviour.OnStateExitAsObservable().Subscribe(OnStateExit);
                //behaviour.OnStateUpdateAsObservable().Subscribe(OnStateUpdate);

                behaviour.OnStateMachineEnterAsObservable().Subscribe(OnStateMachineEnter);
                behaviour.OnStateMachineExitAsObservable().Subscribe(OnStateMachineExit);
            }
        }

        private void OnStateMachineEnter(ObservableStateMachineTrigger.OnStateMachineInfo obj)
        {
            var animState = hashToAnimStateDic[obj.StateMachinePathHash];
            Debug.LogError(animState + " : OnStateMachineEnter");
            onStateMachineEnterEvent?.Invoke(animState);
        }

        private void OnStateUpdate(ObservableStateMachineTrigger.OnStateInfo obj)
        {
            var animState = hashToAnimStateDic[obj.StateInfo.fullPathHash];
            Debug.LogError(animState + " : OnStateUpdate");
            onStateUpdateEvent?.Invoke(animState);
        }

        private void OnStateMachineExit(ObservableStateMachineTrigger.OnStateMachineInfo obj)
        {
            var animState = hashToAnimStateDic[obj.StateMachinePathHash];
            Debug.LogError(animState + " : OnStateMachineExit");
            onStateMachineExitEvent?.Invoke(animState);
        }

        void OnStateEnter(ObservableStateMachineTrigger.OnStateInfo obj)
        {
            var animState = hashToAnimStateDic[obj.StateInfo.fullPathHash];
            Debug.LogError(animState + " : OnStateEnter");
            onStateEnterEvent?.Invoke(animState);
        }

        void OnStateExit(ObservableStateMachineTrigger.OnStateInfo obj)
        {
            var animState = hashToAnimStateDic[obj.StateInfo.fullPathHash];
            Debug.LogError(animState + " : OnStateExit");
            onStateExitEvent?.Invoke(animState);
        }
#endif
    }
}