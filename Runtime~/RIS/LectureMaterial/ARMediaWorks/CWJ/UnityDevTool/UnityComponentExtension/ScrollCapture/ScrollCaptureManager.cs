using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using CWJ.Singleton;
using CWJ.AccessibleEditor;

namespace CWJ.UI
{
    /// 해상도에 상관없이 기능하는 스크롤 전체화면 저장 매니저
    /// <summary>
    /// ScrollView Capture &amp; Save to PNG
    /// <para><see langword="&lt;ScrollRect의 RectTransform 설정&gt;"/></para>
    /// <para>[Anchor] vertical:stretch, horizontal:stretch (Min:0,0/ Max:1,1/ Pivot:0.5,0.5)</para>
    /// <para>[Position] Top,PosZ,Bottom은 모두 0이여야함 (Left,Right는 상관없음)</para>
    /// <para>ScrollRect의 크기는 스크린의 위아래로 가득채워야함</para>
    /// <para><see langword="&lt;Content의 RectTransfrom 설정&gt;"/></para>
    /// <para>[Anchor] vertical:top, horizontal:stretch (Min:0,1/ Max:1,1/ Pivot:0.5,1.0)</para>
    /// <para>[Position] Height외에는 모두 0이여야함</para>
    /// <para><see langword="&lt;TIP&gt;"/></para>
    /// <para>스크롤바는 Capture 직전에 자동으로 숨겨줄거니까 신경쓰지말것</para>
    /// <para>content의 모든 부모오브젝트를 자동으로 활성화시켜서 보이도록 함</para>
    /// <para>Canvas의 reference해상도와 Display의 해상도에 상관없이 캡쳐해줌</para>
    /// </summary>
    public class ScrollCaptureManager : SingletonBehaviour<ScrollCaptureManager>
    {
#if UNITY_EDITOR
        public ScrollRect editor_ScrollRect = null;
        [ResizableTextArea] public string editor_SavePath = "";
        [SerializeField] private string editor_LastSavePath = "";
#endif

        public bool IsCaptureInProgress() => CO_ScrollCapture != null;

        /// <summary>
        /// (companyName/productName 폴더까지는 자동생성) /
        /// 테스트용 혹은 날짜만 나오는용으로는 fileName에 공백 넣기 /
        /// 매개변수 명명 필수
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="callback"></param>
        /// <param name="isPrintToast"></param>
        public void Capture(ScrollRect scrollRect, string folderName = null, string fileName = null, Action callback = null, bool isPrintToast = true)
        {
            SaveCaptureToPath(scrollRect, GetPath(folderName: folderName, fileName: fileName), callback, isPrintToast);
        }

        /// <summary>
        /// 파일경로를 직접적어야함.
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="_fileName"></param>
        /// <param name="callback"></param>
        public void SaveCaptureToPath(ScrollRect scrollRect, string savePath, Action callback = null, bool isPrintToast = true)
        {
            if (CO_ScrollCapture != null)
            {
                typeof(ScrollCaptureManager).PrintLogWithClassName($"Wait! Capture is in progress...", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                return;
            }

            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);
            if (CheckIsGetScreenSizeError(screenSize))
            {
                typeof(ScrollCaptureManager).PrintLogWithClassName("[Capture detection through Inspector function such as 'InvokeButton' or 'ContextMenu' Attribute] Screen.width, height is replaced by MainCamera's pixelSize to avoid screen size errors." +
                               "\nThis error is only relevant in editor mode. For more accurately test, run in a way other than editor events (e.g. UI event or code)."
                               , logType: LogType.Warning, isComment: false, isBigFont: false, obj: gameObject);
                Camera camera = Camera.main;
                if (camera == null) camera = FindObjectOfType<Camera>();
                screenSize = new Vector2Int(camera.pixelWidth, camera.pixelHeight); //오류가 있을경우 카메라의 pixel사이즈로 대치
            }

#if UNITY_EDITOR
            if (scrollRect == null) scrollRect = this.editor_ScrollRect;

            if (string.IsNullOrEmpty(savePath))
            {
                savePath = this.editor_SavePath;
                if (string.IsNullOrEmpty(savePath))
                {
                    if (!string.IsNullOrEmpty(editor_LastSavePath))
                    {
                        savePath = editor_LastSavePath;
                    }
                    else
                    {
                        if (CWJ.AccessibleEditor.AccessibleEditorUtil.TryGetScriptPath(nameof(ScrollCaptureManager), out string path))
                        {
                            savePath = GetPath(System.IO.Path.GetDirectoryName(path), null);
                        }
                    }
                }
            }
#endif

            if (scrollRect == null || string.IsNullOrEmpty(savePath))
            {
                typeof(ScrollCaptureManager).PrintLogWithClassName($"null {nameof(scrollRect)} or {nameof(savePath)}", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                return;
            }

            CO_ScrollCapture = StartCoroutine(Do_ScrollCapture(scrollRect, savePath, screenSize, callback, isPrintToast));
        }

        #region 스위칭1: 캔버스 래이케스터 켜기 / 끄기 스위칭

        private GraphicRaycaster graphicRaycaster = null;

        public void SetDisableCanvasRaycaster(Canvas rootCanvas, bool isDoOrUndo)
        {
            if (isDoOrUndo)
            {
                graphicRaycaster = rootCanvas.GetComponent<GraphicRaycaster>();
                if (!graphicRaycaster.enabled) graphicRaycaster = null;
            }

            if (graphicRaycaster == null) return;

            graphicRaycaster.enabled = !isDoOrUndo;

            if (!isDoOrUndo) graphicRaycaster = null;
        }

        #endregion 스위칭1: 캔버스 래이케스터 켜기 / 끄기 스위칭

        #region 스위칭2: 스크롤바 켜져있는것들 모두 숨김 / 되돌림 + w같은 Viewport Mask 활성화/비활성화

        public struct BackupScrollbarSetting
        {
            public bool isVertical;

            public UnityEngine.UI.ScrollRect.ScrollbarVisibility visibilityType;
            public bool enabled;

            public BackupScrollbarSetting(bool isVertical, ScrollRect scrollRect)
            {
                if (this.isVertical = isVertical)
                {
                    this.visibilityType = scrollRect.verticalScrollbarVisibility;
                    this.enabled = scrollRect.vertical;
                }
                else
                {
                    this.visibilityType = scrollRect.horizontalScrollbarVisibility;
                    this.enabled = scrollRect.horizontal;
                }
            }

            public void SetInvisibleSetting(ScrollRect scrollRect, bool isDoOrUndo)
            {
                var setVisibility = isDoOrUndo ? ScrollRect.ScrollbarVisibility.Permanent : visibilityType;
                var setEnabled = isDoOrUndo ? false : enabled;

                if (isVertical)
                {
                    scrollRect.verticalScrollbarVisibility = setVisibility;
                    scrollRect.vertical = setEnabled;
                }
                else
                {
                    scrollRect.horizontalScrollbarVisibility = setVisibility;
                    scrollRect.horizontal = setEnabled;
                }
            }
        }

        private Mask backupViewportMask = null;
        private BackupScrollbarSetting verticalBarSetting;
        private BackupScrollbarSetting horizontalBarSetting;

        private void SetInvisibleScrollbar(ScrollRect scrollRect, bool isDoOrUndo)
        {
            bool hasVerticalScrollbar = scrollRect.verticalScrollbar != null;
            bool hasHorizontalScrollbar = scrollRect.horizontalScrollbar != null;

            if (isDoOrUndo)
            {
                backupViewportMask = scrollRect.viewport?.GetComponent<Mask>();
                if (hasVerticalScrollbar) verticalBarSetting = new BackupScrollbarSetting(true, scrollRect);
                if (hasHorizontalScrollbar) horizontalBarSetting = new BackupScrollbarSetting(false, scrollRect);
            }

            if (hasVerticalScrollbar) verticalBarSetting.SetInvisibleSetting(scrollRect, isDoOrUndo);
            if (hasHorizontalScrollbar) horizontalBarSetting.SetInvisibleSetting(scrollRect, isDoOrUndo);
            if (backupViewportMask != null)
            {
                backupViewportMask.enabled = !isDoOrUndo;
                if (isDoOrUndo)
                {
                    backupViewportMask.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                else
                {
                    backupViewportMask = null;
                }
            }
        }

        #endregion 스위칭2: 스크롤바 켜져있는것들 모두 숨김 / 되돌림

        #region 스위칭3: 모든 부모오브젝트 활성화 / 활성화시킨거만 비활성화

        private Transform[] inactiveParents = null;

        private void SetActivateAllParents(Transform contentTrf, bool isDoOrUndo)
        {
            if (isDoOrUndo)
            {
                if (contentTrf.gameObject.activeInHierarchy)
                {
                    inactiveParents = null;
                }
                else
                {
                    inactiveParents = contentTrf.GetComponentsInParent_New<Transform>(includeInactive: true, isWithoutMe: false, predicate: (t) => !t.gameObject.activeSelf);
                }
            }

            if (inactiveParents == null) return;

            foreach (var trf in inactiveParents)
            {
                trf.gameObject.SetActive(isDoOrUndo);
            }

            if (!isDoOrUndo) inactiveParents = null;
        }

        #endregion 스위칭3: 모든 부모오브젝트 활성화 / 활성화시킨거만 비활성화

        #region (Obsolete) 스위칭4: 화면크기가 캔버스보다 큰 경우 캔버스 해상도 적용 / 되돌리기 

        //private Vector2? backupCanvasResolution = null;

        //private void SetScreenResolutionInCanvas(CanvasScaler canvasScaler, Vector2 screenResol, bool isDoOrUndo)
        //{
        //    if (isDoOrUndo)
        //    {
        //        if (screenResol.x > canvasScaler.referenceResolution.x) //캔버스 레퍼런스 해상도보다 화면크기가 큰경우 캔버스 레퍼런스 해상도를 화면해상도에 맞춤
        //        {
        //            backupCanvasResolution = canvasScaler.referenceResolution;
        //        }
        //        else
        //        {
        //            backupCanvasResolution = null;
        //        }
        //    }

        //    if (backupCanvasResolution == null) return;

        //    canvasScaler.referenceResolution = isDoOrUndo ? screenResol : backupCanvasResolution.Value;

        //    if (!isDoOrUndo) backupCanvasResolution = null;
        //}

        #endregion 스위칭4: 화면크기가 캔버스보다 큰 경우 캔버스 해상도 적용 / 되돌리기

        #region Get ScreenSize & Check ScreenSize Error (ContextMenu, InvokeButton 등 Editor-Inspector Event 를 사용해서 실행한경우 Screen.width가 Inspector사이즈를 반환하는 문제)
        private bool CheckIsGetScreenSizeError(Vector2Int screenSize)
        {
#if UNITY_EDITOR
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor");
            if (inspectorType != null)
            {
                int screenWidth = screenSize.x;
                int screenHeight = screenSize.y;

                foreach (Rect inspectorRect in Array.ConvertAll(Resources.FindObjectsOfTypeAll(inspectorType).FindAll(o => o != null), (o => (o as EditorWindow).position)))
                {
                    int inspectorWidth = (int)inspectorRect.width;
                    int inspectorHeight = (int)inspectorRect.height;
                    if ((inspectorWidth <= screenWidth || inspectorWidth >= screenWidth - 2)
                        && (inspectorHeight <= screenHeight || inspectorHeight <= screenHeight - 21))
                    {
                        return true; //Is invoked via InspectorEvent
                    }
                }
            }
#endif
            return false;
        }
        #endregion

        private Coroutine CO_ScrollCapture = null;

        private IEnumerator Do_ScrollCapture(ScrollRect scrollRect, string savePath, Vector2Int screenResolution, Action callback, bool isPrintToast)
        {
            Canvas canvas = scrollRect.GetComponentInParent<Canvas>().rootCanvas;
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();

            Vector2 canvasResolution = canvasScaler.referenceResolution;
            RectTransform contentRtf = scrollRect.content;
            RectTransform scrollRtf = scrollRect.GetComponent<RectTransform>();
            Vector2 backupContentPos = contentRtf.anchoredPosition;
            bool backupScrollRectEnabled = scrollRect.enabled;

            Canvas.ForceUpdateCanvases();
            SetDisableCanvasRaycaster(canvas, true); // 캔버스 레이캐스터 비활성화
            scrollRect.enabled = true;
            SetInvisibleScrollbar(scrollRect, true); // 스크롤바 UI 숨김
            SetActivateAllParents(contentRtf.transform, true); // 모든 부모오브젝트 활성화
            //SetScreenResolutionInCanvas(canvasScaler, screenResolution, true); // (Obsolete) // 캔버스해상도 조정  

            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            yield return waitForEndOfFrame;

            Vector2Int contentSizeInOnceCapture = new Vector2Int((int)contentRtf.rect.width, (int)scrollRtf.rect.height); //한 화면에 보이는 컨텐츠의 사이즈 == content폭, scroll높이
            scrollRect.enabled = false;
            Vector2Int onceCaptureSize;

            float captureOffset_x = 0; //ScrollRect가 Canvas 정중앙에 있을때 기준 Screen좌측에서 부터 ScrollRect까지의 거리 차

            // (Obsolete)
            //bool isScreenSmaller;
            //if (isScreenSmaller = (screenResolution.x <= contentSizeInOnceCapture.x)) //스크롤뷰크기(Content크기)보다 스크린크기가 작음
            //{
            //    onceCaptureSize = new Vector2Int((int)((screenResolution.x * contentSizeInOnceCapture.x) / canvasResolution.x), screenResolution.y);
            //}
            //else
            //{
            //    //onceCaptureSize = new Vector2Int(contentSizeInOnceCapture.x, screenResolution.y); //한번에 캡쳐되는 width를 scroll사이즈로 줄임
            //}
            onceCaptureSize = new Vector2Int((int)((screenResolution.x * contentSizeInOnceCapture.x) / canvasResolution.x), screenResolution.y);

            captureOffset_x = (screenResolution.x - onceCaptureSize.x) * 0.5f;
            captureOffset_x -= (scrollRtf.rect.width - contentRtf.rect.width) * 0.5f;

            float wholeCaptureHeight = (contentRtf.rect.height * onceCaptureSize.y) / contentSizeInOnceCapture.y; //해상도에 맞게 캡쳐될 전체 height사이즈
            int intWholeCaptureHeight = (int)System.Math.Ceiling(wholeCaptureHeight) /*+ 1*/;
            int captureCount = intWholeCaptureHeight / onceCaptureSize.y;

            Vector2 contentPosition = backupContentPos;
            contentPosition.y = 0;

            Texture2D texture = new Texture2D(onceCaptureSize.x, intWholeCaptureHeight, TextureFormat.RGB24, false);
            float curReadDestY = 0;

            bool hasError = false;
            //float contentMoveDist = (isScreenSmaller ? contentSizeInOnceCapture.y : screenResolution.y); // (Obsolete)
            float contentMoveDist = contentSizeInOnceCapture.y;
            for (int i = 0; i < captureCount; i++)
            {
                contentRtf.anchoredPosition = contentPosition;

                yield return waitForEndOfFrame;

                curReadDestY = wholeCaptureHeight - ((i + 1) * onceCaptureSize.y);

                try
                {
                    texture.ReadPixels(new Rect(captureOffset_x, 0, onceCaptureSize.x, onceCaptureSize.y), 0, (int)curReadDestY);
                }
                catch (System.Exception e)
                {
                    typeof(ScrollCaptureManager).PrintLogWithClassName($"ReadPixels(index: {i}) error of captured texture\nOS:{Application.platform.ToString()}\n{e.ToString()}", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                    hasError = true;
                }

                contentPosition.y += contentMoveDist;
            }

            if (!hasError && curReadDestY > 0) //캡쳐할것이 남음
            {
                contentRtf.anchoredPosition = contentPosition;
                yield return waitForEndOfFrame;

                try
                {
                    texture.ReadPixels(new Rect(captureOffset_x, onceCaptureSize.y - curReadDestY + 0.1f, onceCaptureSize.x, curReadDestY), 0, 0);
                }
                catch (System.Exception e)
                {
                    typeof(ScrollCaptureManager).PrintLogWithClassName($"ReadPixels(index: last) error of captured texture\nOS:{Application.platform.ToString()}\n{e.ToString()}", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                    hasError = true;
                }
            }

            if (!hasError)
            {
                try
                {
                    texture.Apply();

                    byte[] bytes = texture.EncodeToPNG();

                    System.IO.File.WriteAllBytes(savePath, bytes);
                }
                catch (System.Exception e)
                {
                    typeof(ScrollCaptureManager).PrintLogWithClassName($"WriteAllBytes error of captured PNG file\nOS:{Application.platform.ToString()}\n{e.ToString()}", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                    hasError = true;
                }

                if (!hasError)
                {
                    typeof(ScrollCaptureManager).PrintLogWithClassName("Success of save capture image.\n" + savePath, logType: LogType.Log, isComment: false, isBigFont: false, obj: gameObject);
#if UNITY_EDITOR
                    editor_LastSavePath = savePath;
#endif
                    if (isPrintToast)
                    {
                        //AndroidPlugin.Instance?.Toast(_filePath + LocalizationService.Instance.GetLocalizeText(" 에 저장되었습니다."));
                    }

                    if (!Application.isMobilePlatform)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(savePath);
                        }
                        catch (System.Exception e)
                        {
                            typeof(ScrollCaptureManager).PrintLogWithClassName($"Open error of captured image\nOS:{Application.platform.ToString()}\n{e.ToString()}", logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                        }
                    }
                    else if (Application.platform == RuntimePlatform.Android)
                    {
                        //ViewFile
                    }
                }
            }

            if (!Application.isPlaying)
            {
                DestroyImmediate(texture);
            }
            else
            {
                Destroy(texture);
            }
            texture = null;

            Canvas.ForceUpdateCanvases();
            scrollRect.enabled = true;
            SetInvisibleScrollbar(scrollRect, false); // 스크롤바 UI 숨김상태 원상복귀
            //SetScreenResolutionInCanvas(canvasScaler, screenResolution, false); // (Obsolete) // 캔버스해상도 원상복귀 
            SetActivateAllParents(contentRtf.transform, false); // 모든 부모오브젝트 활성화상태 원상복귀
            SetDisableCanvasRaycaster(canvas, false); // 캔버스 레이캐스터 활성화상태 원상복귀

            yield return waitForEndOfFrame;

            scrollRect.enabled = backupScrollRectEnabled;
            contentRtf.anchoredPosition = backupContentPos;

            yield return null;
            CO_ScrollCapture = null;

            callback?.Invoke();

            yield break;
        }

        /// <summary>
        /// return 'C:/companyName/productName/<see cref="folderName"/>/<see cref="fileName"/>.png'
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetPath(string folderName, string fileName)
        {
            string path = string.Empty;
            var curPlatform = Application.platform;
            if (curPlatform == RuntimePlatform.WindowsPlayer || curPlatform == RuntimePlatform.WindowsEditor) //윈도우or윈도우에디터or안드로이드
            {
                string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                char separator = System.IO.Path.DirectorySeparatorChar;
                path = programFilesPath.Substring(0, programFilesPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar))
                     + separator + Application.companyName
                    + separator + Application.productName;
            }
            else if (curPlatform == RuntimePlatform.Android)
            {
                path = $"/storage/emulated/0/{Application.companyName}/{Application.productName}";
            }
            else
            {
                typeof(ScrollCaptureManager).PrintLogWithClassName("Excepted platform : " + curPlatform.ToString(), logType: LogType.Error, isComment: false, isBigFont: true, obj: gameObject);
                return null;
            }

            if (!string.IsNullOrEmpty(folderName))
            {
                path = System.IO.Path.Combine(path, folderName);
            }

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = (DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") + "_Capture");
            }

            path = System.IO.Path.Combine(path, (fileName + ".png"));

            return path;
        }
    }
}