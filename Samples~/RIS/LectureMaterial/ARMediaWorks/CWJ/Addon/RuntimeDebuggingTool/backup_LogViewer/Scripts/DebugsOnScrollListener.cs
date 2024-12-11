using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.RuntimeDebugging
{
    public class DebugsOnScrollListener : MonoBehaviour, IScrollHandler, IBeginDragHandler, IEndDragHandler
    {
        public ScrollRect debugsScrollRect;
        public LogViewerManager debugLogManager;

        public void OnScroll(PointerEventData data)
        {
            if (IsScrollbarAtBottom())
                debugLogManager.SetSnapToBottom(true);
            else
                debugLogManager.SetSnapToBottom(false);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            debugLogManager.SetSnapToBottom(false);
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (IsScrollbarAtBottom())
                debugLogManager.SetSnapToBottom(true);
            else
                debugLogManager.SetSnapToBottom(false);
        }

        public void OnScrollbarDragStart(BaseEventData data)
        {
            debugLogManager.SetSnapToBottom(false);
        }

        public void OnScrollbarDragEnd(BaseEventData data)
        {
            if (IsScrollbarAtBottom())
                debugLogManager.SetSnapToBottom(true);
            else
                debugLogManager.SetSnapToBottom(false);
        }

        private bool IsScrollbarAtBottom()
        {
            float scrollbarYPos = debugsScrollRect.verticalNormalizedPosition;
            if (scrollbarYPos <= 1E-6f)
                return true;

            return false;
        }
    }
}