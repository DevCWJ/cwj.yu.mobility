using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.RuntimeDebugging
{
    public class DebugLogPopup : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform popupTransform = null;

        private Vector2 halfSize = Vector2.zero;

        [SerializeField]
        private Image backgroundImage = null;

        [SerializeField]
        private CanvasGroup canvasGroup = null;

        [SerializeField]
        private LogViewerManager debugManager = null;

        [SerializeField]
        private Text newInfoCountText = null;

        [SerializeField]
        private Text newWarningCountText = null;

        [SerializeField]
        private Text newErrorCountText = null;

        private int newInfoCount = 0, newWarningCount = 0, newErrorCount = 0;

        [SerializeField]
        private Color normalColor = Color.grey;

        [SerializeField]
        private Color alertColorInfo = Color.blue;

        [SerializeField]
        private Color alertColorWarning = Color.yellow;

        [SerializeField]
        private Color alertColorError = Color.red;

        private bool isPopupBeingDragged = false;

        private IEnumerator moveToPosCoroutine = null;

        private void Start()
        {
            halfSize = popupTransform.sizeDelta * 0.5f * popupTransform.root.localScale.x;
        }

        public void OnViewportDimensionsChanged()
        {
            halfSize = popupTransform.sizeDelta * 0.5f * popupTransform.root.localScale.x;
            OnEndDrag(null);
        }

        public void NewInfoLogArrived()
        {
            newInfoCount++;
            newInfoCountText.text = newInfoCount.ToString();

            if (newWarningCount == 0 && newErrorCount == 0)
                backgroundImage.color = alertColorInfo;
        }

        public void NewWarningLogArrived()
        {
            newWarningCount++;
            newWarningCountText.text = newWarningCount.ToString();

            if (newErrorCount == 0)
                backgroundImage.color = alertColorWarning;
        }

        public void NewErrorLogArrived()
        {
            newErrorCount++;
            newErrorCountText.text = newErrorCount.ToString();

            backgroundImage.color = alertColorError;
        }

        private void Reset()
        {
            newInfoCount = 0;
            newWarningCount = 0;
            newErrorCount = 0;

            newInfoCountText.text = "0";
            newWarningCountText.text = "0";
            newErrorCountText.text = "0";

            backgroundImage.color = normalColor;
        }

        private IEnumerator MoveToPosAnimation(Vector3 targetPos)
        {
            float modifier = 0f;
            Vector3 initialPos = popupTransform.position;
            while (modifier < 1f)
            {
                modifier += 4f * Time.unscaledDeltaTime;
                popupTransform.position = Vector3.Lerp(initialPos, targetPos, modifier);

                yield return null;
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (!isPopupBeingDragged)
            {
                debugManager.logWindowShow();
                Hide();
            }
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;

            Reset();

            OnViewportDimensionsChanged();
        }

        public void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }

        public void OnBeginDrag(PointerEventData data)
        {
            isPopupBeingDragged = true;

            if (moveToPosCoroutine != null)
            {
                StopCoroutine(moveToPosCoroutine);
                moveToPosCoroutine = null;
            }
        }

        public void OnDrag(PointerEventData data)
        {
            popupTransform.position = data.position;
        }

        public void OnEndDrag(PointerEventData data)
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            Vector3 pos = popupTransform.position;

            float distToLeft = pos.x;
            float distToRight = Mathf.Abs(pos.x - screenWidth);

            float distToBottom = Mathf.Abs(pos.y);
            float distToTop = Mathf.Abs(pos.y - screenHeight);

            float horDistance = Mathf.Min(distToLeft, distToRight);
            float vertDistance = Mathf.Min(distToBottom, distToTop);

            if (horDistance < vertDistance)
            {
                if (distToLeft < distToRight)
                    pos = new Vector3(halfSize.x, pos.y, 0f);
                else
                    pos = new Vector3(screenWidth - halfSize.x, pos.y, 0f);

                pos.y = Mathf.Clamp(pos.y, halfSize.y, screenHeight - halfSize.y);
            }
            else
            {
                if (distToBottom < distToTop)
                    pos = new Vector3(pos.x, halfSize.y, 0f);
                else
                    pos = new Vector3(pos.x, screenHeight - halfSize.y, 0f);

                pos.x = Mathf.Clamp(pos.x, halfSize.x, screenWidth - halfSize.x);
            }

            if (moveToPosCoroutine != null)
                StopCoroutine(moveToPosCoroutine);

            moveToPosCoroutine = MoveToPosAnimation(pos);
            if (debugManager.rootCanvas.enabled)
            {
                StartCoroutine(moveToPosCoroutine);
            }
            else
            {
                popupTransform.rect.Set(pos.x, pos.y, 72, 72);
                //Debug.LogWarning(popupTransform.position + " " + pos);
                //Debug.LogWarning(popupTransform.localPosition);
            }

            isPopupBeingDragged = false;
        }
    }
}