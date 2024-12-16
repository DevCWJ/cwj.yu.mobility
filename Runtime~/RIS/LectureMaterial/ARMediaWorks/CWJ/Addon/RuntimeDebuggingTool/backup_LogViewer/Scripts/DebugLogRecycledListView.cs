using System.Collections.Generic;

using UnityEngine;

namespace CWJ.RuntimeDebugging
{
    public class DebugLogRecycledListView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform transformComponent = null;

        [SerializeField]
        private RectTransform viewportTransform = null;

        [SerializeField]
        private LogViewerManager debugManager = null;

        [SerializeField]
        private Color logItemNormalColor1 = Color.black;

        [SerializeField]
        private Color logItemNormalColor2 = Color.black;

        [SerializeField]
        private Color logItemSelectedColor = Color.blue;

        private LogViewerManager manager = null;

        private float logItemHeight, _1OverLogItemHeight;
        private float viewportHeight;

        private List<DebugLogEntry> collapsedLogEntries = null;

        private DebugLogIndexList indicesOfEntriesToShow = null;

        public int indexOfSelectedLogEntry { get; private set; } = int.MaxValue;
        private float positionOfSelectedLogEntry = float.MaxValue;
        private float heightOfSelectedLogEntry;
        private float deltaHeightOfSelectedLogEntry;

        private Dictionary<int, LogItem> logItemsAtIndices = new Dictionary<int, LogItem>();

        private bool isCollapseOn = false;

        private int currentTopIndex = -1, currentBottomIndex = -1;

        public float ItemHeight { get { return logItemHeight; } }
        public float SelectedItemHeight { get { return heightOfSelectedLogEntry; } }

        private void Awake()
        {
            viewportHeight = viewportTransform.rect.height;
        }

        public void Initialize(LogViewerManager manager, List<DebugLogEntry> collapsedLogEntries,
            DebugLogIndexList indicesOfEntriesToShow, float logItemHeight)
        {
            this.manager = manager;
            this.collapsedLogEntries = collapsedLogEntries;
            this.indicesOfEntriesToShow = indicesOfEntriesToShow;
            this.logItemHeight = logItemHeight;
            _1OverLogItemHeight = 1f / logItemHeight;
        }

        public void SetCollapseMode(bool collapse)
        {
            isCollapseOn = collapse;
        }

        public void OnLogItemClicked(LogItem item)
        {
            if (indexOfSelectedLogEntry != item.Index)
            {
                DeselectSelectedLogItem();

                indexOfSelectedLogEntry = item.Index;
                positionOfSelectedLogEntry = item.Index * logItemHeight;
                heightOfSelectedLogEntry = item.CalculateExpandedHeight(item.ToString());
                deltaHeightOfSelectedLogEntry = heightOfSelectedLogEntry - logItemHeight;

                manager.SetSnapToBottom(false);
            }
            else
                DeselectSelectedLogItem();

            if (indexOfSelectedLogEntry >= currentTopIndex && indexOfSelectedLogEntry <= currentBottomIndex)
                ColorLogItem(logItemsAtIndices[indexOfSelectedLogEntry], indexOfSelectedLogEntry);

            CalculateContentHeight();

            HardResetItems();
            UpdateItemsInTheList(true);

            manager.ValidateScrollPosition();
        }

        public void DeselectSelectedLogItem()
        {
            int indexOfPreviouslySelectedLogEntry = indexOfSelectedLogEntry;
            indexOfSelectedLogEntry = int.MaxValue;

            positionOfSelectedLogEntry = float.MaxValue;
            heightOfSelectedLogEntry = deltaHeightOfSelectedLogEntry = 0f;

            if (indexOfPreviouslySelectedLogEntry >= currentTopIndex && indexOfPreviouslySelectedLogEntry <= currentBottomIndex)
                ColorLogItem(logItemsAtIndices[indexOfPreviouslySelectedLogEntry], indexOfPreviouslySelectedLogEntry);
        }

        public void OnLogEntriesUpdated(bool updateAllVisibleItemContents)
        {
            CalculateContentHeight();
            viewportHeight = viewportTransform.rect.height;

            if (updateAllVisibleItemContents)
                HardResetItems();

            UpdateItemsInTheList(updateAllVisibleItemContents);
        }

        public void OnCollapsedLogEntryAtIndexUpdated(int index)
        {
            if (logItemsAtIndices.TryGetValue(index, out LogItem logItem))
                logItem.ShowCount();
        }

        public void OnViewportDimensionsChanged()
        {
            viewportHeight = viewportTransform.rect.height;
            UpdateItemsInTheList(false);
        }

        private void HardResetItems()
        {
            if (currentTopIndex != -1)
            {
                DestroyLogItemsBetweenIndices(currentTopIndex, currentBottomIndex);
                currentTopIndex = -1;
            }
        }

        private void CalculateContentHeight()
        {
            float newHeight = Mathf.Max(1f, indicesOfEntriesToShow.Count * logItemHeight + deltaHeightOfSelectedLogEntry);
            transformComponent.sizeDelta = new Vector2(0f, newHeight);
        }

        public void UpdateItemsInTheList(bool updateAllVisibleItemContents)
        {
            if (indicesOfEntriesToShow.Count > 0)
            {
                float contentPosTop = transformComponent.anchoredPosition.y - 1f;
                float contentPosBottom = contentPosTop + viewportHeight + 2f;

                if (positionOfSelectedLogEntry <= contentPosBottom)
                {
                    if (positionOfSelectedLogEntry <= contentPosTop)
                    {
                        contentPosTop -= deltaHeightOfSelectedLogEntry;
                        contentPosBottom -= deltaHeightOfSelectedLogEntry;

                        if (contentPosTop < positionOfSelectedLogEntry - 1f)
                            contentPosTop = positionOfSelectedLogEntry - 1f;

                        if (contentPosBottom < contentPosTop + 2f)
                            contentPosBottom = contentPosTop + 2f;
                    }
                    else
                    {
                        contentPosBottom -= deltaHeightOfSelectedLogEntry;
                        if (contentPosBottom < positionOfSelectedLogEntry + 1f)
                            contentPosBottom = positionOfSelectedLogEntry + 1f;
                    }
                }

                int newTopIndex = (int)(contentPosTop * _1OverLogItemHeight);
                int newBottomIndex = (int)(contentPosBottom * _1OverLogItemHeight);

                if (newTopIndex < 0)
                    newTopIndex = 0;

                if (newBottomIndex > indicesOfEntriesToShow.Count - 1)
                    newBottomIndex = indicesOfEntriesToShow.Count - 1;

                if (currentTopIndex == -1)
                {
                    updateAllVisibleItemContents = true;

                    currentTopIndex = newTopIndex;
                    currentBottomIndex = newBottomIndex;

                    CreateLogItemsBetweenIndices(newTopIndex, newBottomIndex);
                }
                else
                {

                    if (newBottomIndex < currentTopIndex || newTopIndex > currentBottomIndex)
                    {
                        updateAllVisibleItemContents = true;

                        DestroyLogItemsBetweenIndices(currentTopIndex, currentBottomIndex);
                        CreateLogItemsBetweenIndices(newTopIndex, newBottomIndex);
                    }
                    else
                    {
                        if (newTopIndex > currentTopIndex)
                            DestroyLogItemsBetweenIndices(currentTopIndex, newTopIndex - 1);

                        if (newBottomIndex < currentBottomIndex)
                            DestroyLogItemsBetweenIndices(newBottomIndex + 1, currentBottomIndex);

                        if (newTopIndex < currentTopIndex)
                        {
                            CreateLogItemsBetweenIndices(newTopIndex, currentTopIndex - 1);

                            if (!updateAllVisibleItemContents)
                                UpdateLogItemContentsBetweenIndices(newTopIndex, currentTopIndex - 1);
                        }

                        if (newBottomIndex > currentBottomIndex)
                        {
                            CreateLogItemsBetweenIndices(currentBottomIndex + 1, newBottomIndex);

                            if (!updateAllVisibleItemContents)
                                UpdateLogItemContentsBetweenIndices(currentBottomIndex + 1, newBottomIndex);
                        }
                    }

                    currentTopIndex = newTopIndex;
                    currentBottomIndex = newBottomIndex;
                }

                if (updateAllVisibleItemContents)
                {
                    UpdateLogItemContentsBetweenIndices(currentTopIndex, currentBottomIndex);
                }
            }
            else
                HardResetItems();
        }

        private void CreateLogItemsBetweenIndices(int topIndex, int bottomIndex)
        {
            for (int i = topIndex; i <= bottomIndex; i++)
                CreateLogItemAtIndex(i);
        }

        private void CreateLogItemAtIndex(int index)
        {
            LogItem logItem = debugManager.PopLogItem();

            Vector2 anchoredPosition = new Vector2(1f, -index * logItemHeight);
            if (index > indexOfSelectedLogEntry)
                anchoredPosition.y -= deltaHeightOfSelectedLogEntry;

            logItem.Transform.anchoredPosition = anchoredPosition;

            ColorLogItem(logItem, index);

            logItemsAtIndices[index] = logItem;
        }

        private void DestroyLogItemsBetweenIndices(int topIndex, int bottomIndex)
        {
            for (int i = topIndex; i <= bottomIndex; i++)
                debugManager.PoolLogItem(logItemsAtIndices[i]);
        }

        private void UpdateLogItemContentsBetweenIndices(int topIndex, int bottomIndex)
        {
            LogItem logItem;
            for (int i = topIndex; i <= bottomIndex; i++)
            {
                logItem = logItemsAtIndices[i];
                logItem.SetContent(collapsedLogEntries[indicesOfEntriesToShow[i]], i, i == indexOfSelectedLogEntry);

                if (isCollapseOn)
                    logItem.ShowCount();
                else
                    logItem.HideCount();
            }
        }

        private void ColorLogItem(LogItem logItem, int index)
        {
            if (index == indexOfSelectedLogEntry)
                logItem.Image.color = logItemSelectedColor;
            else if (index % 2 == 0)
                logItem.Image.color = logItemNormalColor1;
            else
                logItem.Image.color = logItemNormalColor2;
        }
    }
}