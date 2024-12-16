using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.UI
{
    [DisallowMultipleComponent]
    public class DoubleClickEventSystem : MonoBehaviour, IPointerClickHandler
    {
        public void Constructor(UnityEngine.UI.Selectable selectable, UnityEngine.Events.UnityAction doubleClickAction, float availableTime)
        {
            this.selectable = selectable;
            doubleClickEvent.AddListener_New(doubleClickAction);
            if (availableTime > 0) this.availableTime = availableTime;
        }

        public UnityEngine.UI.Selectable selectable;

        public float availableTime = 0.3f;                                                                                                                                                                                                    

        public UnityEvent doubleClickEvent = new UnityEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!selectable.interactable) return;

            if (CO_CheckDoubleClick == null)
                CO_CheckDoubleClick = StartCoroutine(DO_CheckDoubleClick());
            else
            {
                StopCoroutine(CO_CheckDoubleClick);
                CO_CheckDoubleClick = null;
                doubleClickEvent?.Invoke();
            }
        }

        private Coroutine CO_CheckDoubleClick = null;

        private IEnumerator DO_CheckDoubleClick()
        {
            yield return null;
            yield return new WaitForSeconds(availableTime);

            CO_CheckDoubleClick = null;
            yield break;
        }
    }

    public static class DoubleClickEventSystemHelper
    {
        /// <summary>
        /// availableTime : doubleClickEvent 유효 시간
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void AddDoubleClickEvent(this Button button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0)
        {
            var doubleClickEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<DoubleClickEventSystem>();
            doubleClickEventSystem.Constructor(button, unityAction, availableTime);
        }

        /// <summary>
        /// availableTime : doubleClickEvent 유효 시간
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void RemoveDoubleClickEvent(this Button button, UnityEngine.Events.UnityAction unityAction)
        {
            var doubleClickEventSystem = button.targetGraphic.GetComponent<DoubleClickEventSystem>();
            if (doubleClickEventSystem == null)
            { 
                return;
            }
            doubleClickEventSystem.doubleClickEvent.RemoveListener_New(unityAction);
        }
    }
}