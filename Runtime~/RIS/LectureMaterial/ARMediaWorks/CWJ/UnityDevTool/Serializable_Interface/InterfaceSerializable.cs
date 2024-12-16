using System;

using UnityEngine;

namespace CWJ.Serializable
{
    /// <summary>
    /// 인터페이스 직렬화.
    /// <para>naming convention은 SI_이름 으로 한다 (SI:SerializableInterface/ ex:SI_SceneEvent[] si_sceneEvents;)</para>
    /// </summary>
    /// <typeparam name="TI"></typeparam>
    [Serializable]
    public class InterfaceSerializable<TI> where TI : class //SerializableInterface가 맞는말이지만 인텔리센스를 사용한다면 형태 이름(Interface)이 먼저나오는게 더 편함
    {
        private TI _interface = null;
        public TI Interface
        {
            get
            {
                if (lastValidComp != _component)
                {
                    Interface = _component != null ? _component.GetComponent<TI>() : null;
                }
                else
                {
                    if (_interface == null && lastValidComp != null)
                    {
                        Interface = lastValidComp as TI;
                    }
                }

                return _interface;
            }

            set
            {
                if (value == null)
                {
                    _interface = null;
                    _component = null;
                    lastValidComp = null;
                }
                else
                {
                    _interface = value;
                    lastValidComp = _component = (value as Component);
                }
            }
        }

        [SerializeField, HideInInspector] Component lastValidComp;
        [SerializeField] private Component _component;

        public InterfaceSerializable()
        {
        }
    } //[20.01.09]
}