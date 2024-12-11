
using UnityEngine;
using CWJ.Singleton.Core;

namespace CWJ.Singleton.OnlyUseNew
{
    /// <summary>
    /// 새로 생성되는것을 Instance로 지정함.
    /// <br/>기존의 <see cref="SingletonBehaviour{T}"/>와는 정반대
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CWJInfoBox]
    public class SingletonBehaviour_OnlyUseNew<T> : SingletonCoreAbstract<T> where T : MonoBehaviour
    {
        #region Use these methods instead of original unity's magic-methods
        protected override void _Reset() { }
        protected override void _OnValidate() { }
        protected override void _Awake() { }
        protected override void _OnEnable() { }
        protected override void _OnDisable() { }
        protected override void _Start() { }
        protected override void OnDispose() { }

        protected override void _OnDestroy() { }
        protected override void _OnApplicationQuit() { }
        #endregion Use these methods instead of original unity's magic-methods

        public override sealed bool isDontDestroyOnLoad => false;
        public override sealed bool isOnlyUseNew => true;

        protected override sealed void Awake()
        {
            base.Awake();

            UpdateInstanceForcibly();

            _Awake();
        }
    }
}