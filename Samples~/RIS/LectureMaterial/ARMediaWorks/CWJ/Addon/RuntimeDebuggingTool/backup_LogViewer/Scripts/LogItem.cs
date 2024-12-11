using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR

using UnityEditor;

using System.Text.RegularExpressions;

#endif

namespace CWJ.RuntimeDebugging
{
    public class LogItem : MonoBehaviour, IPointerClickHandler,IPointerDownHandler
    {
        [SerializeField]
        private RectTransform transformComponent = null;

        public RectTransform Transform { get { return transformComponent; } }

        [SerializeField]
        private Image imageComponent = null;

        public Image Image { get { return imageComponent; } }

        [SerializeField]
        private Text logText = null;

        [SerializeField]
        private Image logTypeImage = null;

        [SerializeField]
        private GameObject logCountParent = null;

        [SerializeField]
        private Text logCountText = null;

        private DebugLogEntry logEntry;

        private int entryIndex;

        public int Index { get { return entryIndex; } }

        private DebugLogRecycledListView manager;

        public void Initialize(DebugLogRecycledListView manager)
        {
            this.manager = manager;
        }

        public void SetContent(DebugLogEntry logEntry, int entryIndex, bool isExpanded)
        {
            this.logEntry = logEntry;
            this.entryIndex = entryIndex;

            Vector2 size = transformComponent.sizeDelta;
            if (isExpanded)
            {
                logText.horizontalOverflow = HorizontalWrapMode.Wrap;
                size.y = manager.SelectedItemHeight;
            }
            else
            {
                logText.horizontalOverflow = HorizontalWrapMode.Overflow;
                size.y = manager.ItemHeight;
            }
            transformComponent.sizeDelta = size;

            logText.text = isExpanded ? logEntry.ToString() : logEntry.logString;
            logTypeImage.sprite = logEntry.logTypeSpriteRepresentation;
        }

        public void ShowCount()
        {
            logCountText.text = logEntry.count.ToString();
            logCountParent.SetActive(true);
        }

        public void HideCount()
        {
            logCountParent.SetActive(false);
        }

        float lastClickTime;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                lastClickTime = (manager.indexOfSelectedLogEntry == Index) ? Time.time : 0;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (manager.indexOfSelectedLogEntry == Index)
                {
                    if (Time.time - lastClickTime >= 1f)
                    {
                        CopyToClipboard();
                    }
                }
                lastClickTime = 0;
                manager.OnLogItemClicked(this);
            }
            else
            {
#if UNITY_EDITOR
                Match regex = Regex.Match(logEntry.stackTrace, @"\(at .*\.cs:[0-9]+\)$", RegexOptions.Multiline);
                if (regex.Success)
                {
                    string line = logEntry.stackTrace.Substring(regex.Index + 4, regex.Length - 5);
                    int lineSeparator = line.IndexOf(':');
                    MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(line.Substring(0, lineSeparator));
                    if (script != null)
                        AssetDatabase.OpenAsset(script, int.Parse(line.Substring(lineSeparator + 1)));
                }
#else
                if (manager.indexOfSelectedLogEntry == Index)
                    CopyToClipboard();
#endif
            }
        }

        private void CopyToClipboard()
        {
            //GUIUtility.systemCopyBuffer = logEntry.ToString();
            var textEditor = new TextEditor();
            textEditor.text = logEntry.ToString();
            textEditor.SelectAll();
            textEditor.Copy();
            Debug.Log($"Copied! (log : \"{logEntry.logString}\")");
        }

        public float CalculateExpandedHeight(string content)
        {
            string text = logText.text;
            HorizontalWrapMode wrapMode = logText.horizontalOverflow;

            logText.text = content;
            logText.horizontalOverflow = HorizontalWrapMode.Wrap;

            float result = logText.preferredHeight;

            logText.text = text;
            logText.horizontalOverflow = wrapMode;

            return Mathf.Max(manager.ItemHeight, result);
        }

        public override string ToString()
        {
            return logEntry.ToString();
        }


    }
}