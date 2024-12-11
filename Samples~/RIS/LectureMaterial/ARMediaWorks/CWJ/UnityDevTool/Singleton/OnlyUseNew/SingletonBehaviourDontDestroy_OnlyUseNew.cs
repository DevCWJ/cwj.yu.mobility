
using UnityEngine;
using CWJ.Singleton.Core;

namespace CWJ.Singleton.OnlyUseNew
{
    /// <summary>
    /// ?
    /// <para/><see cref="SingletonBehaviour_OnlyUseNew{T}"/>와 <see cref="SingletonBehaviourDontDestroy{T}"/>의 특징이 공존하는 특수 Singleton
    /// <para/>: 생성된 씬에서는 새로 생성되는 오브젝트가 instance로 바뀌어 <see cref="SingletonBehaviour_OnlyUseNew{T}"/>와 같지만, 
    /// <br/>씬이 전환되고 난 후 부터는 기존의 <see cref="SingletonBehaviourDontDestroy{T}"/>처럼 새로생성되는것들이 삭제됨
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CWJInfoBox]
    public class SingletonBehaviourDontDestroy_OnlyUseNew<T> : SingletonCoreAbstract<T> where T : MonoBehaviour
    {
        #region Use these unity's magic-methods instead of original method
        protected override void _Reset() { }
        protected override void _OnValidate() { }
        protected override void _Awake() { }
        protected override void _OnEnable() { }
        protected override void _OnDisable() { }
        protected override void _Start() { }
        protected override void OnDispose() { }

        protected override void _OnDestroy() { }
        protected override void _OnApplicationQuit() { }
        #endregion Use these unity's magic-methods instead of original method

        public override sealed bool isDontDestroyOnLoad => true;
        public override sealed bool isOnlyUseNew => true;

        /// <summary>
        /// 씬이 전환되어 현재 Instance가 확정되었음을 식별하게 해주는 변수
        /// </summary>
        [Readonly] public bool isConfirmedInstance = false;

        /// <summary>
        /// Instance가 Awake에서 할당됨 (SingletonBehaviour는 Instance 호출 시 에 할당됨)
        /// </summary>
        protected override sealed void Awake()
        {
            base.Awake();

            if (!HasInstance)//_instance가 없다는말은 최초 생성된 SingletonBehaviourDontDestroy_OnlyUseNew 클래스임
            {
                UpdateInstance();

                UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene> sceneUnloadAction = null;
                sceneUnloadAction = (_) =>
                {
                    (_Instance as SingletonBehaviourDontDestroy_OnlyUseNew<T>).isConfirmedInstance = true;
                    UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= sceneUnloadAction;
                }; //씬전환되면 isConfirmedInstance 를 true시켜줄 일회용 이벤트등록
                UnityEngine.SceneManagement.SceneManager.sceneUnloaded += sceneUnloadAction;
            }
            else
            {
                if (!(_Instance as SingletonBehaviourDontDestroy_OnlyUseNew<T>).isConfirmedInstance)
                {
                    UpdateInstanceForcibly(); //_instance가 있는데 isConfirmedInstance가 false라는 말은 아직 생성된씬에서 벗어나지않은거니까 새로생성된것을 instance로
                }
                else
                {
                    DestroySingletonObj(gameObject);
                    return;
                }
            }

            _Awake();
        }
    }
}