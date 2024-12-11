using System;
using System.Collections;
using System.IO;

using UnityEngine;
using CWJ.RuntimeDebugging;
using System.Linq;
using UnityEngine.EventSystems;

namespace CWJ
{
    /// <summary>
    /// [18.09.02]
    /// </summary>
    [DefaultExecutionOrder(-32000)]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public partial class RuntimeDebuggingTool : MonoBehaviour
    {
        #region Settings Field

        [Header("<Settings>")]
        public bool isDebuggingEnabled;

        public bool isVisible = true;

        public bool isVisibleOnStart = false;
        [Range(0, 10), SerializeField] int startVisibleDelay = 2;

        [SerializeField] bool isWriteDataWhenEnabledAtLeastOnce = false;
        bool isEnabledAtLeastOnce = false;

        [Tooltip("버전 확인용(read only)")]
        [SerializeField]
        private string _gameVersion;

        #endregion Settings Field

        #region FrameRate Setting

        [Header("<Framerate Setting>")]
        [Tooltip("시작할때 마지막으로 저장되었던값으로 불러올것인지")]
        [SerializeField] private bool loadFPSDataOnStart = false;

        [Tooltip("Framerate Fixed 출력 빈도")]
        [Range(5, 30)]
        [SerializeField] private int frameUpdateFrequency = 7;

        [SerializeField] private Color frameLabelColor;

        [SerializeField] private Color frameLabelOutlineColor;

        [SerializeField] private Color frameValueColor;

        [SerializeField] private Color frameValueOutlineColor;

        [Header("<Framerate Fixed Setting> (stop function for a moment)")]
        [Tooltip("프레임고정 on off 여부")]
        [SerializeField] private bool isFrameRateFixed = false;

        private void OnValidate()
        {
            OnFrameRateSetting();
        }

        private void OnFrameRateSetting()
        {
            if (Application.isPlaying || !isDebuggingEnabled)
            {
                return;
            }
            //if (isFrameRateFixed)
            //{
            //    lowFrameWarningValue = frameRateFixValue;
            //    Application.targetFrameRate = frameRateFixValue;
            //    QualitySettings.vSyncCount = 0;
            //}
            //else
            //{
            //    Application.targetFrameRate = 1;
            //    QualitySettings.vSyncCount = 1;
            //}
        }

        [Tooltip("Framerate Fixed Setting")]
        [Range(1, 150)]
        [SerializeField] private int frameRateFixValue = 0;

        [Header("<Framerate warning setting>")]
        [Tooltip("fps모니터에 설정될 최저 fps수치\nfps가 이 수치 아래로 떨어지면 경고표시")]
        [Range(1, 150)]
        [SerializeField] private int lowFrameWarningValue = 60;

        [SerializeField] private Color lowFrameWarningColor;

        private DateTime startDate;

        private void WriteDebuggingData()
        {
            if (isWriteDataWhenEnabledAtLeastOnce && !isEnabledAtLeastOnce && savingLog == null)
            {
                return;
            }

            DateTime endDate = DateTime.Now;
            TimeSpan playingTime = endDate - startDate;
            if (savingLog == null && playingTime.Seconds < (startVisibleDelay * 1.25f))
            {
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (!dirInfo.Exists)
            {
                return;
            }
            FileInfo fileInfo = new FileInfo(Directory.GetCurrentDirectory() + "\\CWJ_ProfilingLog.txt");
            string fullPath = fileInfo.FullName;
            if (!fileInfo.Exists) //없으면 만들어주기
            {
                try
                {
                    StreamWriter setFile = File.CreateText(fullPath);
                    setFile.Close();
                    PlayerPrefs.DeleteKey("CWJ_ProfilingLogCnt");
                    Debug.LogWarning("[SYSTEM] Create CWJ_ProfilingLog.txt");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[Save_Exception]" + e.ToString());
                    return;
                }
            }
            int fpsSaveCount = PlayerPrefs.GetInt("CWJ_ProfilingLogCnt", 0);
            PlayerPrefs.SetInt("CWJ_ProfilingLogCnt", ++fpsSaveCount);
            bool isNeedDeviceInfo = fpsSaveCount == 1;
            var strBuilder = new System.Text.StringBuilder((isNeedDeviceInfo ? 596 : 292) + (savingLog == null ? 0 : (savingLog.Capacity + 18)));

            //string body_settingStr = "       Setting : isFrameRateFixed = " + isFrameRateFixed + (isFrameRateFixed ? ";    frameRateFixValue = " + frameRateFixValue + ";" : ";");

            if (isNeedDeviceInfo)
            {
                strBuilder.AppendLine(" Device : \r\n {");
                for (int i = 7; i < fpsDisplayMgr.labelTextList.Length; i++)
                {
                    strBuilder.AppendLine("\t" + fpsDisplayMgr.labelTextList[i].text + " " + fpsDisplayMgr.valueTextList[i].text);
                }
                strBuilder.AppendLine(" }");
            }

            strBuilder.AppendLine("--------------------------------------------------------------------\r\n[" + fpsSaveCount + "]");
            strBuilder.AppendLine("{");
            strBuilder.AppendLine($" Period : ({startDate.ToString("yyMMdd")}){startDate.ToString("HH:mm:ss")} ~ ({endDate.ToString("yyMMdd")}){endDate.ToString("HH:mm:ss")}");
            strBuilder.AppendLine($" Timer : {playingTime.Hours}Hour {playingTime.Minutes}Min {playingTime.Seconds}Sec");
            strBuilder.AppendLine(" Ram : \r\n {");
            for (int i = 4; i <= 6; i++)
            {
                strBuilder.AppendLine("\t" + fpsDisplayMgr.labelTextList[i].text + " " + fpsDisplayMgr.valueTextList[i].text);
            }
            strBuilder.AppendLine(" }");
            strBuilder.AppendLine($" FPS : fpsAvg = {fpsDisplayMgr.avg};    fpsMin = {fpsDisplayMgr.min};    fpsMax = {fpsDisplayMgr.max};");
            if (savingLog != null)
            {
                strBuilder.AppendLine(" Note : \r\n {");
                strBuilder.AppendLine("\t" + savingLog.ToString().Replace("\n", "\n\t"));
                strBuilder.AppendLine(" }");
            }
            strBuilder.AppendLine("}");

            using (StreamWriter writeFile = new StreamWriter(fullPath, true))
            {
                writeFile.WriteLine(strBuilder.ToString());
                strBuilder.Clear();
            }
            Debug.LogWarning("[CWJ] Save ProfilingLog.txt\n" + fullPath);
        }

        #endregion FrameRate Setting

        #region GUI Setting

        [Header("<GUI Font Setting>")]
        [Tooltip("스크립트이름 폰트 설정")]
        [SerializeField] private GUIStyle guiHeaderFontStyle;

        [Tooltip("본문 폰트 설정")]
        [SerializeField] private GUIStyle guiBodyFontStyle;

        [Tooltip("경고 폰트 설정")]
        [SerializeField] private GUIStyle guiWarningFontStyle;

        [Tooltip("주석 폰트 설정")]
        [SerializeField] private GUIStyle guiComentFontStyle;

        [Tooltip("아웃라인 색상설정")]
        [SerializeField] private Color guiOutlineColor;

        private float spaceSize = 5;

        private float xPos, yPos, yStart, yLast;

        private const string Blank_GUI = "      ";
        private const string Coment_GUI = "//";
        private const string Header_GUI = "public class " + nameof(RuntimeDebuggingTool);
        private const string Opener_GUI = "{";

        private const string DebuggingEnabled_GUI = Blank_GUI + nameof(isDebuggingEnabled) + " = ";

        private const string VisibleOnStart_GUI = Blank_GUI + nameof(isVisibleOnStart) + " = ";

        private string getGuiText_isFrameFixed
        {
            get
            {
                return Blank_GUI + "isFrameRateFixed = " + isFrameRateFixed + ";";
            }
        }

        private string _guiLine4_frameFixValue_isComent = "";

        private string getGuiText_frameFixValue
        {
            get
            {
                return Blank_GUI + _guiLine4_frameFixValue_isComent + "frameRateFixValue = " + frameRateFixValue + ";";
            }
        }

        private const string guiCloser = "}";
        private const string guiMade = "_(Help : Contact CWJ)";
        private const string guiGameVersion = "product_version : ";

#if UNITY_EDITOR
        [NonSerialized] private string _MyVersion = null;
        public string MyVersion
        {
            get
            {
                if (_MyVersion == null)
                {
                    _MyVersion = GetMyVer();
                }
                return _MyVersion;
            }
        }

        private void OnGUI()
        {
            if (Application.isPlaying) return;

            xPos = Display.displays[0].systemWidth * 0.015f;
            yStart = Display.displays[0].systemHeight * 0.01f;
            yLast = Display.displays[0].systemHeight * 0.95f;
            if (isFrameRateFixed)
            {
                _guiLine4_frameFixValue_isComent = "";
            }
            else
            {
                _guiLine4_frameFixValue_isComent = Coment_GUI;
            }

            yPos = yStart;
            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yStart, 0, 0), Header_GUI, guiHeaderFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//header
            yPos += guiHeaderFontStyle.fontSize + spaceSize;

            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), Opener_GUI, guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//{
            yPos += guiBodyFontStyle.fontSize + spaceSize;

            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), DebuggingEnabled_GUI, guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//1 isDebuggingEnabled
            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos + (DebuggingEnabled_GUI.Length * guiBodyFontStyle.fontSize * 0.5f), yPos, 0, 0), isDebuggingEnabled + ";", isDebuggingEnabled ? guiWarningFontStyle : guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));
            yPos += guiBodyFontStyle.fontSize + spaceSize;

            if (isDebuggingEnabled)
            {
                GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), VisibleOnStart_GUI, guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//1 isVisibleOnStart
                GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos + (VisibleOnStart_GUI.Length * guiBodyFontStyle.fontSize * 0.5f), yPos, 0, 0), isVisibleOnStart + ";", isVisibleOnStart ? guiWarningFontStyle : guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));
                yPos += guiBodyFontStyle.fontSize + spaceSize;
            }

            //GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), getGuiText_isFrameFixed, guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//2 isFrameFixed
            //yPos += guiBodyFontStyle.fontSize + spaceSize;

            //GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), getGuiText_frameFixValue, getGuiText_frameFixValue.Contains(guiComent) ? guiComentFontStyle : guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//3 frameRateFixValue 주석가능
            //yPos += guiBodyFontStyle.fontSize + spaceSize;
            //yPos = yLast; //여기서 예전엔 yLast를 y값으로 줌
            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), guiCloser, guiBodyFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//}
            //GUIoutlineAndShadow.LabelAndOutline(new Rect(xPos + guiBodyFontStyle.fontSize, yLast, 0, 0), guiComent + "VER_" + thisVersion + guiMade, guiComentFontStyle, guiOutlineColor, 2);//coment
            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos + (guiCloser.Length * guiBodyFontStyle.fontSize * 0.5f), yPos, 0, 0), " " + Coment_GUI + "VER_" + MyVersion + guiMade, guiComentFontStyle, guiOutlineColor, new Vector2(2, 2));
            yPos += guiBodyFontStyle.fontSize + spaceSize;
            _gameVersion = Application.version;
            GUIoutlineAndShadow.LabelAndShadow(new Rect(xPos, yPos, 0, 0), Coment_GUI + guiGameVersion + _gameVersion, guiComentFontStyle, guiOutlineColor, new Vector2(1.7f, 1.7f));//}
        }

        private static string GetMyVer()
        {
            string changelogPath = Application.dataPath + "/CWJ/RuntimeDebuggingTool/" + "Changelog_RuntimeDebuggingTool.md";
            if (!new FileInfo(changelogPath).Exists)
            {
                return null;
            }

            string[] txtLines = null;
            StreamReader sr = null;
            try
            {
                using (sr = new StreamReader(changelogPath, System.Text.Encoding.UTF8))
                {
                    txtLines = sr.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    sr.Close();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                sr?.Close();
                sr = null;
            }

            int index = Array.FindIndex(txtLines, (s) => s.StartsWith("##") && s.EndsWith("Version"));

            return index >= 0 ? txtLines[index + 1].Replace("#", "").TrimStart() : "";
        }

#endif

        #endregion GUI Setting

        private InfoGatherer fpsDisplayMgr;
        private LogViewerManager logViewerMgr;

        private bool isInit = false;
        System.Text.StringBuilder savingLog = null;

        private void Init()
        {
            if (Instance == null) { Destroy(gameObject); return; }

            isInit = false;

            startDate = DateTime.Now;

            transform.SetAsFirstSibling();
            logViewerMgr = GetComponentInChildren<LogViewerManager>(true);
            logViewerMgr?.Show(); //로그는 바로켜고

            fpsDisplayMgr = GetComponentInChildren<InfoGatherer>(true);

            if (fpsDisplayMgr == null) return;

            fpsDisplayMgr.gameObject.SetActive(true);
            fpsDisplayMgr.Hide(); //fps카운터는 끄고

            fpsDisplayMgr.showOnStart = false;
            fpsDisplayMgr.SetFPSWarning(lowFrameWarningValue, lowFrameWarningColor);
            fpsDisplayMgr.loadMinMaxAvgData = loadFPSDataOnStart;
            fpsDisplayMgr.updateFrequency = frameUpdateFrequency;
            fpsDisplayMgr.init = false;

            if (isFrameRateFixed)
            {
                Debug.LogWarning("isFrameRateFixed = true;\nframeRateFixValue = " + lowFrameWarningValue + ";");
            }
            for (int i = 0; i < fpsDisplayMgr.valueTextList.Length; i++)
            {
                fpsDisplayMgr.labelTextList[i].color = frameLabelColor;
                fpsDisplayMgr.labelTextList[i].GetComponent<UnityEngine.UI.Outline>().effectColor = frameLabelOutlineColor;

                fpsDisplayMgr.valueTextList[i].color = frameValueColor;
                fpsDisplayMgr.valueTextList[i].GetComponent<UnityEngine.UI.Outline>().effectColor = frameValueOutlineColor;
            }
            StartCoroutine(IE_Init());
        }

        private IEnumerator IE_Init()
        {
            yield return null;
            if (isVisibleOnStart && startVisibleDelay > 0)
            {
                yield return new WaitForSeconds(startVisibleDelay);
            }
            isInit = true;
            SetVisible(isVisibleOnStart);
        }

        private void Awake()
        {
#if CWJ_RUNTIMEDEBUGGING_DISABLED
            if (!Application.isEditor)
            {
                this.enabled = isDebuggingEnabled = false;
                Destroy(gameObject);
                return;
            }
#endif

            if (!Application.isPlaying) return;

            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Init();
        }

        private void Update()
        {
            if (!Application.isPlaying || !isInit) return;

            if (allVisibleMultipleEvent != null)
            {
                foreach (var d in allVisibleMultipleEvent.GetInvocationList())
                {
                    if (d != null && (bool)d.DynamicInvoke())
                    {
                        SetVisible(!isVisible);
                        break;
                    }
                }
            }

            if (fpsVisibleSingleEvent?.Invoke() ?? false)
            {
                fpsDisplayMgr.enabled = !fpsDisplayMgr.enabled;
            }

            if (fpsResetSingleEvent?.Invoke() ?? false)
            {
                fpsDisplayMgr.ResetFPSMin();
                fpsDisplayMgr.ResetFPSMax();
                fpsDisplayMgr.ResetFPSAvg();
            }

            if (timePauseSingleEvent?.Invoke() ?? false)
            {
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying) return;

            if (isInit && isDebuggingEnabled)
            {
                WriteDebuggingData();
            }
        }
    }
}