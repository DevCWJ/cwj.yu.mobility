using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.UI
{
    //InputField를 비활성화 하고싶을 시 이 스크립트를 enable false 하면됨
    /// <summary>
    /// Obsolete. Use <seealso cref="FixedInputFieldBug"/> instead of this.
    /// 
    /// </summary>  
    [RequireComponent(typeof(InputField))]
    [System.Obsolete("Use " + nameof(FixedInputFieldBug) + " instead of this", true)]
    public class InputFieldBugHelper : MonoBehaviour,
                                        IPointerClickHandler,
                                        ISelectHandler,
                                        ISubmitHandler,
                                        IDeselectHandler,
                                        IUpdateSelectedHandler
    {

        private static InputFieldBugHelper CurFocusedTarget;
        [GetComponent] public InputField inputField;
        [Range(0, 1)] public float delayTime = 0;

        private void Awake()
        {
            if (inputField == null) inputField = GetComponent<InputField>();
            //waitForEndOfFrame = new WaitForEndOfFrame();
            waitForSeconds = new WaitForSeconds(delayTime);
            DeactivateFocus();
        }

        private void OnDisable()
        {
            DeactivateFocus();
        }

        //InputField가 비활성화 되어있을 때 ActivateFocus해주는 용도
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (!inputField.enabled)
            {
                ActivateFocus();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            ActivateFocus();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (inputField.isFocused) return;

            ActivateFocus();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            DeactivateFocus();
        }

        private Event processingEvent = new Event();

        private bool IsEditFinished(Event e)
        {
            return (e.rawType == EventType.KeyDown) &&
                (e.keyCode == KeyCode.Escape || (inputField.lineType != InputField.LineType.MultiLineNewline && e.keyCode == KeyCode.KeypadEnter));
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (!inputField.isFocused) return;

            while (Event.PopEvent(processingEvent))
            {
                if (IsEditFinished(processingEvent))
                {
                    DeactivateFocus();
                    break;
                }
            }
        }

        private void ActivateFocus()
        {
            if (!inputField.interactable) return;

            CurFocusedTarget?.DeactivateFocus(this);
            CurFocusedTarget = this;

            if (CO_SetFocusInputField != null)
            {
                StopCoroutine(CO_SetFocusInputField);
            }
            CO_SetFocusInputField = StartCoroutine(DO_SetFocusInputField());
        }

        protected void DeactivateFocus(InputFieldBugHelper newFocusedScript = null)
        {
            if (!inputField.interactable) return;

            if (CurFocusedTarget == this)
            {
                CurFocusedTarget = null;
            }

            if (CO_SetFocusInputField != null)
            {
                StopCoroutine(CO_SetFocusInputField);
                CO_SetFocusInputField = null;
            }

            if (newFocusedScript != this) //새로 포커싱 될 inputField가 자기자신이라면 enabled를 굳이 false 안해줌
            {
                inputField.enabled = false;
            }
        }

        //private WaitForEndOfFrame waitForEndOfFrame = null;
        private WaitForSeconds waitForSeconds = null;

        private Coroutine CO_SetFocusInputField = null;

        private IEnumerator DO_SetFocusInputField()
        {
            if (delayTime > 0)
            {
                yield return waitForSeconds;
            }
            else
            {
                yield return null;
            }

            inputField.enabled = true;
            inputField.Select();
            inputField.ActivateInputField();
            yield return null; // or WaitForEndOfFrame;
            inputField.MoveTextEnd(false);

            CO_SetFocusInputField = null;
        }
    }
}