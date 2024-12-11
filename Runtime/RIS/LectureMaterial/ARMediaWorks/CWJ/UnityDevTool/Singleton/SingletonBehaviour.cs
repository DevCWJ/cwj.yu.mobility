
using UnityEngine;
using CWJ.Singleton.Core;

namespace CWJ.Singleton
{
    /// <summary>
    /// 싱글톤 스크립트 간편 적용을 위한 <see langword="abstract"/> Generic 클래스
    /// <para/>사용하면 안되는 기존 유니티 메소드가 있기때문에 꼭 summary들을 읽어볼것 Use these methods instead of original unity's magic-methods
    /// <br/>DontDestroyOnLoad 싱글톤을 원한다면 <see cref="SingletonBehaviour{T}"/>가 아닌 <see cref="SingletonBehaviourDontDestroy{T}"/>를 상속받을것
    /// <para/>[18.11.28]
    /// </summary>
    [CWJInfoBox]
    public class SingletonBehaviour<T> : SingletonCoreAbstract<T> where T : MonoBehaviour
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
        public override sealed bool isOnlyUseNew => false;

        /// <summary>
        /// 상속받는곳에선 Awake대신 _Awake써주세요 제발!!
        /// <br/>Plz Use '_Awake()' instead of 'Awake()' in override class
        /// </summary>
        protected override sealed void Awake()
        {
            base.Awake();

            if (IsPreventNewInstance())
            {
                return;
            }

            _Awake();
        }
    }
}