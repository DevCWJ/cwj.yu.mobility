using System;

using CWJ.Singleton;

using UnityEngine;

namespace CWJ.State
{
    /// <summary>
    /// 현재 StateBehaviour이랑 한 세트임
    /// <para>[19.06.18]</para>
    /// </summary>
    public class StateManager : SingletonBehaviour<StateManager>
    {
        [GetComponent] public new Transform transform;

        [Readonly] public new GameObject gameObject;
        [Readonly] [SerializeField] private State _curState;

        public State curState
        {
            get
            {
                return _curState;
            }
            set
            {
                _curState = value;
                stateList.AllInit(stateList[value]);
                stateList[value].enabled = true;
            }
        }

        [Readonly] [SerializeField] private int stateLength;
        [Readonly] public StateArray stateList;

        [InvokeButton]
        public void Context_SetStateScript()
        {
            if (transform.childCount > 0)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; ++i)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }

            gameObject = transform.gameObject;
            stateLength = System.Enum.GetNames(typeof(State)).Length;
            stateList = new StateArray(stateLength);
            for (int i = 0; i < stateLength; i++)
            {
                createStateObj((State)i);
            }
        }

        private void createStateObj(State state)
        {
            string stateStr = state.ToString();
            GameObject obj = new GameObject(stateStr);
            obj.transform.SetParent(transform, false);
            Type stateScriptType = Type.GetType(string.Format("{0}{1}", state.GetType().FullName, stateStr), true, false);
            StateBehaviour stateBehaviour = obj.AddComponent(stateScriptType) as StateBehaviour;
            stateList[state.ToInt()] = stateBehaviour;
            Debug.LogError(string.Format("{0}.{1} complete", state.ToInt(), stateStr));
        }

        protected override void _Start()
        {
            Debug.LogError(string.Format("test print : {0}", stateList[State.Idle].GetStateName()));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                curState = State.Idle;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                curState = State.Walk;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                curState = State.Attack;
            }
        }
    }
}