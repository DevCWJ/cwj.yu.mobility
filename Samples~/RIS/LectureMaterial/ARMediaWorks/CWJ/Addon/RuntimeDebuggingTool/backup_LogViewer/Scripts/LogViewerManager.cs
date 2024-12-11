using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CWJ.RuntimeDebugging
{
    public enum DebugLogFilter
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        All = 7
    }

    public class LogViewerManager : MonoBehaviour
    {
        private static LogViewerManager instance = null;

        [Header("Properties")]
        public Canvas rootCanvas;

        [SerializeField]
        private bool singleton = true;

        [SerializeField]
        private float minimumHeight = 200f;

        [SerializeField]
        private bool startInPopupMode = true;

        [SerializeField]
        private bool clearCommandAfterExecution = true;

        [SerializeField]
        private bool receiveLogcatLogsInAndroid = false;

        [SerializeField]
        private string logcatArguments = "";

        [Header("Visuals")]
        [SerializeField]
        private LogItem logItemPrefab = null;

        [SerializeField]
        private Sprite infoLog = null;

        [SerializeField]
        private Sprite warningLog = null;

        [SerializeField]
        private Sprite errorLog = null;

        private Dictionary<LogType, Sprite> logSpriteRepresentations;

        [SerializeField]
        private Color collapseButtonNormalColor = Color.black;

        [SerializeField]
        private Color collapseButtonSelectedColor = Color.black;

        [SerializeField]
        private Color filterButtonsNormalColor = Color.black;

        [SerializeField]
        private Color filterButtonsSelectedColor = Color.black;

        [Header("Internal References")]
        [SerializeField]
        private RectTransform logWindowTR = null;

        [SerializeField]
        private RectTransform canvasTR = null;

        [SerializeField]
        private RectTransform logItemsContainer = null;

        [SerializeField]
        private InputField commandInputField = null;

        [SerializeField]
        private Image collapseButton = null;

        [SerializeField]
        private Image filterInfoButton = null;

        [SerializeField]
        private Image filterWarningButton = null;

        [SerializeField]
        private Image filterErrorButton = null;

        [SerializeField]
        private Text infoEntryCountText = null;

        [SerializeField]
        private Text warningEntryCountText = null;

        [SerializeField]
        private Text errorEntryCountText = null;

        [SerializeField]
        private GameObject snapToBottomButton = null;

        private int infoEntryCount = 0, warningEntryCount = 0, errorEntryCount = 0;

        [SerializeField]
        private CanvasGroup logWindowCanvasGroup = null;

        private bool isLogWindowVisible = true;
        private bool screenDimensionsChanged = false;

        [SerializeField]
        private DebugLogPopup popupManager = null;

        [SerializeField]
        private ScrollRect logItemsScrollRect = null;

        [SerializeField]
        private DebugLogRecycledListView recycledListView = null;

        private bool isCollapseOn = false;

        private DebugLogFilter logFilter = DebugLogFilter.All;

        private bool snapToBottom = true;

        private List<DebugLogEntry> collapsedLogEntries;

        private Dictionary<DebugLogEntry, int> collapsedLogEntriesMap;

        private DebugLogIndexList uncollapsedLogEntriesIndices;

        private DebugLogIndexList indicesOfListEntriesToShow;

        private List<LogItem> pooledLogItems;

        private PointerEventData nullPointerEventData;

#if !UNITY_EDITOR && UNITY_ANDROID
		private DebugLogLogcatListener logcatListener;
#endif
        static Thread mainThread = null;
        private void Awake()
        {
            mainThread = System.Threading.Thread.CurrentThread;
        }
        static bool IsMainThread()
        {
            return mainThread != null && mainThread.Equals(System.Threading.Thread.CurrentThread);
        }

        static bool isRebuildingGraphics = false;
        void StartRebuild()
        {
            isRebuildingGraphics = true;
        }

        private void OnEnable()
        {
#if CWJ_DEVELOPMENT_BUILD
            Canvas.preWillRenderCanvases += StartRebuild;
#endif
            if (instance == null)
            {
                instance = this;
                pooledLogItems = new List<LogItem>();

                canvasTR = (RectTransform)transform;

                logSpriteRepresentations = new Dictionary<LogType, Sprite>
                {
                    { LogType.Log, infoLog },
                    { LogType.Warning, warningLog },
                    { LogType.Error, errorLog },
                    { LogType.Exception, errorLog },
                    { LogType.Assert, errorLog }
                };

                filterInfoButton.color = filterButtonsSelectedColor;
                filterWarningButton.color = filterButtonsSelectedColor;
                filterErrorButton.color = filterButtonsSelectedColor;

                collapsedLogEntries = new List<DebugLogEntry>(128);
                collapsedLogEntriesMap = new Dictionary<DebugLogEntry, int>(128);
                uncollapsedLogEntriesIndices = new DebugLogIndexList();
                indicesOfListEntriesToShow = new DebugLogIndexList();

                recycledListView.Initialize(this, collapsedLogEntries, indicesOfListEntriesToShow, logItemPrefab.Transform.sizeDelta.y);
                recycledListView.UpdateItemsInTheList(true);

                nullPointerEventData = new PointerEventData(null);

       
                if (singleton && transform.root == gameObject)
                    DontDestroyOnLoad(gameObject);
            }
            else if (this != instance)
            {
                Destroy(gameObject);
                return;
            }

            Application.logMessageReceived -= ReceivedLog;
            Application.logMessageReceived += ReceivedLog;

            if (receiveLogcatLogsInAndroid)
            {
                Debug.Log(logcatArguments);
#if !UNITY_EDITOR && UNITY_ANDROID
				if( logcatListener == null )
					logcatListener = new DebugLogLogcatListener();

				logcatListener.Start( logcatArguments );
#endif
            }

            commandInputField.onValidateInput -= OnValidateCommand;
            commandInputField.onValidateInput += OnValidateCommand;

            if (minimumHeight < 200f)
                minimumHeight = 200f;
        }

        private void OnDisable()
        {
#if CWJ_DEVELOPMENT_BUILD
            Canvas.preWillRenderCanvases -= StartRebuild;
#endif

            Application.logMessageReceived -= ReceivedLog;

#if !UNITY_EDITOR && UNITY_ANDROID
			if( logcatListener != null )
				logcatListener.Stop();
#endif

   
            commandInputField.onValidateInput -= OnValidateCommand;
        }

        private void Start()
        {
            if (startInPopupMode)
                HideButtonPressed();
            else
                popupManager.OnPointerClick(null);
        }

        private void OnRectTransformDimensionsChange()
        {
            screenDimensionsChanged = true;
        }

        private void Update()
        {
            if (isRebuildingGraphics)
            {
                isRebuildingGraphics = false;
            }
        }

        private void LateUpdate()
        {
            if (_WaitingLogData.Count > 0)
            {
                while (_WaitingLogData.TryDequeue(out var logData))
                {
                    ReceivedLog(logData.logString, logData.stackTrace, logData.logType);
                }
            }

            if (screenDimensionsChanged)
            {
                if (isLogWindowVisible)
                    recycledListView.OnViewportDimensionsChanged();
                else
                    popupManager.OnViewportDimensionsChanged();

                screenDimensionsChanged = false;
            }

            if (snapToBottom)
            {
                logItemsScrollRect.verticalNormalizedPosition = 0f;

                if (snapToBottomButton.activeSelf)
                    snapToBottomButton.SetActive(false);
            }
            else
            {
                float scrollPos = logItemsScrollRect.verticalNormalizedPosition;
                if (snapToBottomButton.activeSelf != (scrollPos > 1E-6f && scrollPos < 0.9999f))
                    snapToBottomButton.SetActive(!snapToBottomButton.activeSelf);
            }

#if !UNITY_EDITOR && UNITY_ANDROID
			if( logcatListener != null )
			{
				string log;
				while( ( log = logcatListener.GetLog() ) != null )
					ReceivedLog( "LOGCAT: " + log, string.Empty, LogType.Log );
			}
#endif
        }

        public char OnValidateCommand(string text, int charIndex, char addedChar)
        {
            if (addedChar == '\n')
            {
                if (clearCommandAfterExecution)
                    commandInputField.text = "";

                if (text.Length > 0)
                {
                    LogViewerHelper.ExecuteCommand(text);

                    SetSnapToBottom(true);
                }

                return '\0';
            }

            return addedChar;
        }
        private static Queue<(string logString, string stackTrace, LogType logType)> _WaitingLogData = new Queue<(string logString, string stackTrace, LogType logType)>();

        private void ReceivedLog(string logString, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception)
            {
                logType = LogType.Error;
            }

            if (!IsMainThread() || isRebuildingGraphics)
            {
                _WaitingLogData.Enqueue((logString, stackTrace, logType));
                return;
            }

            DebugLogEntry logEntry = new DebugLogEntry(logString, stackTrace, null);

            bool isEntryInCollapsedEntryList = collapsedLogEntriesMap.TryGetValue(logEntry, out int logEntryIndex);
            if (!isEntryInCollapsedEntryList)
            {
                logEntry.logTypeSpriteRepresentation = logSpriteRepresentations[logType];

                logEntryIndex = collapsedLogEntries.Count;
                collapsedLogEntries.Add(logEntry);
                collapsedLogEntriesMap[logEntry] = logEntryIndex;
            }
            else
            {
                logEntry = collapsedLogEntries[logEntryIndex];
                logEntry.count++;
            }

            uncollapsedLogEntriesIndices.Add(logEntryIndex);

            Sprite logTypeSpriteRepresentation = logEntry.logTypeSpriteRepresentation;
            if (isCollapseOn && isEntryInCollapsedEntryList)
            {
                if (isLogWindowVisible)
                    recycledListView.OnCollapsedLogEntryAtIndexUpdated(logEntryIndex);
            }
            else if (logFilter == DebugLogFilter.All ||
               (logTypeSpriteRepresentation == infoLog && ((logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)) ||
               (logTypeSpriteRepresentation == warningLog && ((logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)) ||
               (logTypeSpriteRepresentation == errorLog && ((logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)))
            {
                indicesOfListEntriesToShow.Add(logEntryIndex);

                if (isLogWindowVisible)
                    recycledListView.OnLogEntriesUpdated(false);
            }

            if (logType == LogType.Log)
            {
                infoEntryCount++;
                infoEntryCountText.text = infoEntryCount.ToString();

                if (!isLogWindowVisible)
                    popupManager.NewInfoLogArrived();
            }
            else if (logType == LogType.Warning)
            {
                warningEntryCount++;
                warningEntryCountText.text = warningEntryCount.ToString();

                if (!isLogWindowVisible)
                    popupManager.NewWarningLogArrived();
            }
            else
            {
                errorEntryCount++;
                errorEntryCountText.text = errorEntryCount.ToString();

                if (!isLogWindowVisible)
                    popupManager.NewErrorLogArrived();
            }
        }

        public void SetSnapToBottom(bool snapToBottom)
        {
            this.snapToBottom = snapToBottom;
        }

        public void ValidateScrollPosition()
        {
            logItemsScrollRect.OnScroll(nullPointerEventData);
        }

        public void logWindowShow()
        {
            recycledListView.OnLogEntriesUpdated(true);

            logWindowCanvasGroup.interactable = true;
            logWindowCanvasGroup.blocksRaycasts = true;
            logWindowCanvasGroup.alpha = 1f;

            isLogWindowVisible = true;
        }

        public void logWindowHide()
        {
            logWindowCanvasGroup.interactable = false;
            logWindowCanvasGroup.blocksRaycasts = false;
            logWindowCanvasGroup.alpha = 0f;

            isLogWindowVisible = false;
        }

        public void Hide()
        {
            rootCanvas.enabled = false;
        }

        public void Show()
        {
            rootCanvas.enabled = true;
        }

        public void HideButtonPressed()
        {
            logWindowHide();
            popupManager.Show();
        }

        public void ClearButtonPressed()
        {
            snapToBottom = true;

            infoEntryCount = 0;
            warningEntryCount = 0;
            errorEntryCount = 0;

            infoEntryCountText.text = "0";
            warningEntryCountText.text = "0";
            errorEntryCountText.text = "0";

            collapsedLogEntries.Clear();
            collapsedLogEntriesMap.Clear();
            uncollapsedLogEntriesIndices.Clear();
            indicesOfListEntriesToShow.Clear();

            recycledListView.DeselectSelectedLogItem();
            recycledListView.OnLogEntriesUpdated(true);
        }

        public void CollapseButtonPressed()
        {
            isCollapseOn = !isCollapseOn;

            snapToBottom = true;
            collapseButton.color = isCollapseOn ? collapseButtonSelectedColor : collapseButtonNormalColor;
            recycledListView.SetCollapseMode(isCollapseOn);

            FilterLogs();
        }

        public void FilterLogButtonPressed()
        {
            logFilter = logFilter ^ DebugLogFilter.Info;

            if ((logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)
                filterInfoButton.color = filterButtonsSelectedColor;
            else
                filterInfoButton.color = filterButtonsNormalColor;

            FilterLogs();
        }

        public void FilterWarningButtonPressed()
        {
            logFilter = logFilter ^ DebugLogFilter.Warning;

            if ((logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)
                filterWarningButton.color = filterButtonsSelectedColor;
            else
                filterWarningButton.color = filterButtonsNormalColor;

            FilterLogs();
        }

        public void FilterErrorButtonPressed()
        {
            logFilter = logFilter ^ DebugLogFilter.Error;

            if ((logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)
                filterErrorButton.color = filterButtonsSelectedColor;
            else
                filterErrorButton.color = filterButtonsNormalColor;

            FilterLogs();
        }

        public void Resize(BaseEventData dat)
        {
            PointerEventData eventData = (PointerEventData)dat;

            float newHeight = (eventData.position.y - logWindowTR.position.y) / -canvasTR.localScale.y + 36f;
            if (newHeight < minimumHeight)
                newHeight = minimumHeight;

            Vector2 anchorMin = logWindowTR.anchorMin;
            anchorMin.y = Mathf.Max(0f, 1f - newHeight / canvasTR.sizeDelta.y);
            logWindowTR.anchorMin = anchorMin;

            recycledListView.OnViewportDimensionsChanged();
        }

        private void FilterLogs()
        {
            if (logFilter == DebugLogFilter.None)
            {
                indicesOfListEntriesToShow.Clear();
            }
            else if (logFilter == DebugLogFilter.All)
            {
                if (isCollapseOn)
                {
                    indicesOfListEntriesToShow.Clear();
                    for (int i = 0; i < collapsedLogEntries.Count; i++)
                        indicesOfListEntriesToShow.Add(i);
                }
                else
                {
                    indicesOfListEntriesToShow.Clear();
                    for (int i = 0; i < uncollapsedLogEntriesIndices.Count; i++)
                        indicesOfListEntriesToShow.Add(uncollapsedLogEntriesIndices[i]);
                }
            }
            else
            {
                bool isInfoEnabled = (logFilter & DebugLogFilter.Info) == DebugLogFilter.Info;
                bool isWarningEnabled = (logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning;
                bool isErrorEnabled = (logFilter & DebugLogFilter.Error) == DebugLogFilter.Error;

                if (isCollapseOn)
                {
                    indicesOfListEntriesToShow.Clear();
                    for (int i = 0; i < collapsedLogEntries.Count; i++)
                    {
                        DebugLogEntry logEntry = collapsedLogEntries[i];
                        if (logEntry.logTypeSpriteRepresentation == infoLog && isInfoEnabled)
                            indicesOfListEntriesToShow.Add(i);
                        else if (logEntry.logTypeSpriteRepresentation == warningLog && isWarningEnabled)
                            indicesOfListEntriesToShow.Add(i);
                        else if (logEntry.logTypeSpriteRepresentation == errorLog && isErrorEnabled)
                            indicesOfListEntriesToShow.Add(i);
                    }
                }
                else
                {
                    indicesOfListEntriesToShow.Clear();
                    for (int i = 0; i < uncollapsedLogEntriesIndices.Count; i++)
                    {
                        DebugLogEntry logEntry = collapsedLogEntries[uncollapsedLogEntriesIndices[i]];
                        if (logEntry.logTypeSpriteRepresentation == infoLog && isInfoEnabled)
                            indicesOfListEntriesToShow.Add(uncollapsedLogEntriesIndices[i]);
                        else if (logEntry.logTypeSpriteRepresentation == warningLog && isWarningEnabled)
                            indicesOfListEntriesToShow.Add(uncollapsedLogEntriesIndices[i]);
                        else if (logEntry.logTypeSpriteRepresentation == errorLog && isErrorEnabled)
                            indicesOfListEntriesToShow.Add(uncollapsedLogEntriesIndices[i]);
                    }
                }
            }

            recycledListView.DeselectSelectedLogItem();
            recycledListView.OnLogEntriesUpdated(true);

            ValidateScrollPosition();
        }

        public void PoolLogItem(LogItem logItem)
        {
            logItem.gameObject.SetActive(false);
            pooledLogItems.Add(logItem);
        }

        public LogItem PopLogItem()
        {
            LogItem newLogItem;

            if (pooledLogItems.Count > 0)
            {
                newLogItem = pooledLogItems[pooledLogItems.Count - 1];
                pooledLogItems.RemoveAt(pooledLogItems.Count - 1);
                newLogItem.gameObject.SetActive(true);
            }
            else
            {
                newLogItem = Instantiate<LogItem>(logItemPrefab, logItemsContainer, false);
                newLogItem.Initialize(recycledListView);
            }

            return newLogItem;
        }
    }
}