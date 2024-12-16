using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CWJ.UI
{
    [DisallowMultipleComponent]
    public class LongPressEventSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// loop
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="longPressLoopAction"></param>
        /// <param name="availableTime"></param>
        /// <param name="loopInterval"></param>
        public void ConstructorLoop(UnityEngine.UI.Selectable selectable, UnityEngine.Events.UnityAction longPressLoopAction, float availableTime, float loopInterval)
        {
            this.selectable = selectable;
            longPressLoopEvent.AddListener_New(longPressLoopAction);
            this.availableTime = availableTime;
            this.loopInterval = loopInterval;
        }

        /// <summary>
        /// up or down
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="isLongPressedAfter"></param>
        /// <param name="pressUpAction"></param>
        /// <param name="availableTime"></param>
        public void ConstructorUp(UnityEngine.UI.Selectable selectable, UnityEngine.Events.UnityAction pressUpAction, float availableTime, bool isLongPressedAfter)
        {
            this.selectable = selectable;
            if (isLongPressedAfter) longPressedUpEvent.AddListener_New(pressUpAction);
            else shortPressedUpEvent.AddListener_New(pressUpAction);
            this.availableTime = availableTime;
        }

        public void ConstructorDown(UnityEngine.UI.Selectable selectable, UnityEngine.Events.UnityAction longPressStartAction, float availableTime)
        {
            this.selectable = selectable;
            longPressStartEvent.AddListener_New(longPressStartAction);
            this.availableTime = availableTime;
        }

        public void RemoveAllListener()
        {
            longPressStartEvent.RemoveAllListeners_New();
            longPressLoopEvent.RemoveAllListeners_New();
            shortPressedUpEvent.RemoveAllListeners_New();
            longPressedUpEvent.RemoveAllListeners_New();
        }

        public UnityEngine.UI.Selectable selectable;

        public bool Interactable => selectable.interactable;

        public float availableTime = .0f;
        public UnityEvent longPressStartEvent = new UnityEvent();

        public float loopInterval = .0f;
        public UnityEvent longPressLoopEvent = new UnityEvent();

        public bool isLongPressing { get; private set; }
        public UnityEvent longPressedUpEvent = new UnityEvent();

        public UnityEvent shortPressedUpEvent = new UnityEvent();
        public void OnPointerDown(PointerEventData eventData)
        {
            StartCheckLongPress();
        }

        private void StartCheckLongPress()
        {
            if (CO_CheckLongPress != null)
            {
                StopCoroutine(CO_CheckLongPress);
                CO_CheckLongPress = null;
            }

            if (Interactable)
                CO_CheckLongPress = StartCoroutine(DO_CheckLongPress());
        }

        public void StopCheckLongPress()
        {
            bool isPressed = isLongPressing;
            isLongPressing = false;

            if (CO_CheckLongPress != null)
            {
                StopCoroutine(CO_CheckLongPress);
                CO_CheckLongPress = null;
            }
            if (isPressed)
                longPressedUpEvent?.Invoke();
            else
                shortPressedUpEvent?.Invoke();
        }

        private Coroutine CO_CheckLongPress = null;

        private IEnumerator DO_CheckLongPress()
        {
            yield return null;

            if (availableTime > 0)
                yield return new WaitForSeconds(availableTime);

            isLongPressing = true;

            if (!Interactable || !isLongPressing)
            {
                StopCheckLongPress();
                yield break;
            }

            longPressStartEvent?.Invoke();

            if (loopInterval == 0)
            {
                do
                {
                    yield return null;

                    longPressLoopEvent?.Invoke();

                } while (isLongPressing && Interactable);
            }
            else
            {
                WaitForSeconds waitForInterval = new WaitForSeconds(loopInterval);

                do
                {
                    yield return waitForInterval;

                    longPressLoopEvent?.Invoke();

                } while (isLongPressing && Interactable);
            }

            StopCheckLongPress();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopCheckLongPress();
        }
    }
}