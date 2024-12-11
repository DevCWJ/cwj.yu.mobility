using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    public enum EKeyState
    {
        /// <summary>
        /// KeyDown
        /// </summary>
        Began = 0,

        /// <summary>
        /// Hold중 + 커서움직임
        /// </summary>
        Move = 1,

        /// <summary>
        /// Hold중 + 커서안움직임
        /// </summary>
        Stationary = 2,

        /// <summary>
        /// KeyUp
        /// </summary>
        Ended = 3,

        None = 4
    }

    public class KeyEventManager_PC : _KeyEventManager
    {
        public static KeyListener GetKeyListener(KeyCode keyCode, bool isAlwaysEnabled)
        {
            KeyListener alwaysEnabled;

            var keyEventManager = KeyEventManager_PC.Instance;
            if (!keyEventManager.keyListenerDic.TryGetValue(keyCode, out var listenerList))
                alwaysEnabled = CreateKeyListener(keyEventManager.transform, "Always");
            else
                alwaysEnabled = listenerList[0];

            KeyListener targetListener;
            if (isAlwaysEnabled)
            {
                targetListener = alwaysEnabled;
                if (!targetListener.enabled)
                    targetListener.enabled = true;
            }
            else
            {
                targetListener = CreateKeyListener(alwaysEnabled.transform, $"CanDisable[{(listenerList == null ? 0 : (listenerList.Count - 1))}]");
            }

            KeyListener CreateKeyListener(Transform parent, string tag)
            {
                var newListener = new GameObject($"{keyCode.ToString_Fast()}_{tag}").AddComponent<KeyListener>();
                newListener.detectTargetKey = keyCode;
                newListener.transform.SetParent(parent);
                keyEventManager.AddKeyListener(newListener);
                return newListener;
            }

            return targetListener;
        }

#if UNITY_EDITOR
        [SerializeField,Readonly]
#endif
        protected Dictionary<KeyCode, UnityEvent[]> eventListByKeycode = new Dictionary<KeyCode, UnityEvent[]>();

        public override sealed bool AddKeyListener(KeyListener listener)
        {
            if (!base.AddKeyListener(listener))
            {
                return false;
            }

            var targetKeyCode = listener.detectTargetKey;

            if (!eventListByKeycode.TryGetValue(targetKeyCode, out var stateEvents))
            {
                stateEvents = new UnityEvent[5];
                for (int i = 0; i < 5; i++) //5 = EnumUtil.GetLength<TouchPhase>()
                {
                    stateEvents[i] = new UnityEvent();
                    stateEvents[i].AddListener_New(listener.touchEvents[i].Invoke, false);
                }

                eventListByKeycode.Add(targetKeyCode, stateEvents);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    stateEvents[i].AddListener_New(listener.touchEvents[i].Invoke, false);
                }
            }

            onUpdateEnded.AddListener_New(listener.onUpdateEnded.Invoke, false);
            return true;
        }

        public override sealed bool RemoveKeyListener(KeyListener listener)
        {
            if (base.RemoveKeyListener(listener))
            {
                var targetKeyCode = listener.detectTargetKey;

                if (eventListByKeycode.TryGetValue(targetKeyCode, out var listenerEvts))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        listenerEvts[i].RemoveListener_New(listener.touchEvents[i].Invoke);
                    }
                    onUpdateEnded.RemoveListener_New(listener.onUpdateEnded.Invoke);
                }
                return true;
            }
            return false;
        }

        public override sealed bool OnDestroyKeyListener(KeyListener listener)
        {
            this.RemoveKeyListener(listener);
            return base.OnDestroyKeyListener(listener);
        }

        public override sealed void UpdateInputSystem()
        {
            EKeyState keyState;

            bool isAnyKeyClicked = false;

            bool? _cursorMoving = null;

            foreach (var kv in eventListByKeycode)
            {
                KeyCode keyCode = kv.Key;
                keyState = EKeyState.None;

                if (Input.GetKeyDown(keyCode))
                {
                    isAnyKeyClicked = true;
                    keyState = EKeyState.Began;
                }
                else if (Input.GetKey(keyCode))
                {
                    isAnyKeyClicked = true;
                    if (_cursorMoving == null)
                        _cursorMoving = DetectIsCursorMoving();
                    bool _moving = _cursorMoving.Value;
                    keyState = _moving ? EKeyState.Move : EKeyState.Stationary;
                }
                else if (Input.GetKeyUp(keyCode))
                {
                    keyState = EKeyState.Ended;
                }

                if (keyState != EKeyState.None)
                {
                    kv.Value[keyState.ToInt()]?.Invoke();
                }
            }

            isHoldDown = isAnyKeyClicked;
            isCursorMoving = _cursorMoving == null ? false : _cursorMoving.Value;

            if (!isAnyKeyClicked)
            {
                if (CO_StationaryCheck != null)
                {
                    StopCoroutine(CO_StationaryCheck);
                    CO_StationaryCheck = null;
                }
            }
            
            onUpdateEnded?.Invoke();
        }

        private Vector3 prevMousePos;

        //(Input.GetAxisRaw("Mouse X") == .0f && Input.GetAxisRaw("Mouse Y") == .0f); touch되는 windows에서 touch 커서드래그를 인식못함
        private bool GetMouseStationary()
        {
            Vector3 prevPos = prevMousePos;
            prevMousePos = Input.mousePosition;
            return prevPos == Input.mousePosition;
        }

        //GetAxisRaw 가 버그가있어서 코루틴필요
        private bool DetectIsCursorMoving()
        {
            if (CO_StationaryCheck == null)
            {
                if (GetMouseStationary())
                {
                    return false;
                }
                else
                {
                    CO_StationaryCheck = StartCoroutine(DO_StationaryCheck());
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private static Coroutine CO_StationaryCheck = null;

        private IEnumerator DO_StationaryCheck()
        {
            yield return null;

            int staionaryCnt = 0;

            while (staionaryCnt < 3)
            {
                yield return null;
                if (GetMouseStationary())
                {
                    ++staionaryCnt;
                }
                else
                {
                    staionaryCnt = 0;
                }
            }
            CO_StationaryCheck = null;
        }
    }
}