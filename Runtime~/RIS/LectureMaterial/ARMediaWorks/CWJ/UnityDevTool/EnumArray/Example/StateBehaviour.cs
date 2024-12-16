using System;

using UnityEngine;

namespace CWJ.State
{
    public enum State
    {
        Idle = 0,
        Walk,
        Attack
    }

    [Serializable]
    public class StateArray : EnumArray<State, StateBehaviour>
    {
        public StateArray(int length) : base(length)
        {
        }

        public override void InitValue(StateBehaviour value)
        {
            value.enabled = false;
        }
    }

    public interface StateInterface
    {
        string GetStateName();

        void OnEnable();

        void OnDisable();

        void Update();
    }

    public class StateBehaviour : MonoBehaviour, StateInterface
    {
        public string GetStateName()
        {
            return GetType().FullName;
        }

        public virtual void Awake()
        {
            this.enabled = false;
        }

        public void OnEnable()
        {
            Debug.LogError(string.Format("{0} 켜짐", GetStateName()));
        }

        public void OnDisable()
        {
            Debug.LogError(string.Format("{0} 꺼짐", GetStateName()));
        }

        public virtual void Update()
        {
            Debug.LogWarning(GetStateName());
        }
    }
}