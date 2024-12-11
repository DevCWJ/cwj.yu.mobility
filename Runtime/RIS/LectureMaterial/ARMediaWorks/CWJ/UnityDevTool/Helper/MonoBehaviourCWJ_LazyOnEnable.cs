using UnityEngine;

namespace CWJ
{
    public abstract class MonoBehaviourCWJ_LazyOnEnable : DisposableMonoBehaviour
    {
        private bool isCallOnEnable { get; set; } = false;

        /// <summary>
        /// 상속받는곳에선 OnEnable대신 _OnEnable써주세요 제발!!
        /// <br/>Plz Use '_OnEnable()' instead of 'OnEnable()' in override class
        /// </summary>
        protected void OnEnable()
        {
            if (isCallOnEnable)
            {
                _OnEnable();
            }
        }

        /// <summary>
        /// Use '_OnEnable' instead of 'OnEnable' for safety. (but lazy)
        /// (타 오브젝트의 Awake()가 실행되기전에 현재 오브젝트의 OnEnable()이 실행되는것을 방지)
        /// <para><see href="https://forum.unity.com/threads/onenable-before-awake.361429/">referenced to here</see></para>
        /// <para>CWJ.InitializableBehaviour</para>
        /// </summary>
        protected abstract void _OnEnable();

        /// <summary>
        /// 상속받는곳에선 Start대신 _Start써주세요 제발!!
        /// <br/>Plz Use '_Start()' instead of 'Start()' in override class
        /// </summary>
        protected void Start()
        {
            if (!isCallOnEnable)
            {
                isCallOnEnable = true;
                _OnEnable();
            }

            _Start();
        }

        /// <summary>
        /// Use instead of Start 
        /// <para>CWJ.InitializableBehaviour</para>
        /// </summary>
        protected abstract void _Start();
    }
}