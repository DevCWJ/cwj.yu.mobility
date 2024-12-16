using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

namespace CWJ
{
    [System.Serializable] public class UnityEvent_Select : UnityEngine.Events.UnityEvent<string> { }

    [DisallowMultipleComponent, SelectionBase]
    public class EzExplorer : MonoBehaviour, CWJ.AccessibleEditor.InspectorHandler.ISelectHandler
    {
        [Header("File Prefab")]
        [SerializeField] EzExplorerItem fileItem;
        [SerializeField] GameObject emptyFolderTextObj;
        [SerializeField] Image fileIconImg;
        Sprite fileIcon;
        [SerializeField] Image folderIconImg;
        Sprite folderIcon;

        [Header("UI")]
        [SerializeField] Transform contentTrf;

        [SerializeField] InputField filePathIpf;

        [SerializeField] Button selectButton;

        [SerializeField] Button backButton;
        [SerializeField] Button forwardButton;
        [SerializeField] Button upButton;
        [SerializeField] Text folderPathText;
        [SerializeField] Button closeButton;

        public UnityEvent_Select onSelectClick = new UnityEvent_Select();

        [Header("Variable")]
        [SerializeField] bool isDoubleClickSelect = true;
        [SerializeField] bool isAutoSyncCompanyName = false;
        // Save할 때 overwrite 여부
        //public bool forceWrite = false;

        [DrawHeaderAndLine("Readonly")]
        [SerializeField, Readonly] string companyName = null;
        [Readonly] public string curDirPath;
        [Readonly] public string curSelectFilePath;
        [SerializeField, Readonly] List<string> backBuffer; // 뒤로가기
        [SerializeField, Readonly] List<string> forwardBuffer; // 앞으로 가기..? 뒤로가기 취소.

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (isAutoSyncCompanyName && !Application.isPlaying)
            {
                string settingsName = UnityEditor.PlayerSettings.companyName.Trim();

                if (companyName != settingsName)
                {
                    companyName = settingsName;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }

        public void CWJEditor_OnSelect(MonoBehaviour target)
        {
            if (!Application.isPlaying && string.IsNullOrEmpty(companyName.Trim()))
            {
                companyName = UnityEditor.PlayerSettings.companyName.Trim();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif

        private void Start()
        {
            fileIcon = fileIconImg.sprite;
            fileIconImg.gameObject.SetActive(false);
            folderIcon = folderIconImg.sprite;
            folderIconImg.gameObject.SetActive(false);
            backBuffer = new List<string>();
            forwardBuffer = new List<string>();

            if (!companyName.Equals(Application.companyName.Trim()))
            {
                Debug.LogError("CompanyName is difference! " + Application.companyName + "<->" + companyName);
            }

            // 폴더 생성 및 folderpath 초기화
            CreateDir("Resources");

            Initialize();

            onSelectClick.AddListener((s) => Debug.Log(s + " clicked."));
            // Path를 고치면 overwrite을 재확인해야함.
            //filePathIpf.onValueChanged.AddListener((str) => { forceWrite = false; });
        }

        private void Initialize()
        {
            ShowAllFiles(curDirPath);

            backButton.onClick.AddListener(() =>
            {
                // backBuffer에 아무것도 없으면 리턴.
                if (backBuffer.Count == 0) return;
                int cnt = backBuffer.Count;

                forwardBuffer.Add(curDirPath);

                curDirPath = backBuffer[cnt - 1];
                backBuffer.RemoveAt(cnt - 1);

                ShowAllFiles(curDirPath);
            });
            forwardButton.onClick.AddListener(() =>
            {
                if (forwardBuffer.Count == 0) return;
                int cnt = forwardBuffer.Count;

                backBuffer.Add(curDirPath);

                curDirPath = forwardBuffer[cnt - 1];
                forwardBuffer.RemoveAt(cnt - 1);

                ShowAllFiles(curDirPath);
            });
            upButton.onClick.AddListener(() =>
            {
                string[] folders = curDirPath.Split(Path.DirectorySeparatorChar);

                int len = folders.Length;

                if (len == 2) // 항상 끝이 Empty
                {
                    curDirPath = folders[0] + Path.DirectorySeparatorChar;
                    return;
                }

                forwardBuffer.Clear();

                backBuffer.Add(curDirPath);

                curDirPath = folders[0] + Path.DirectorySeparatorChar;

                for (int i = 1; i < len - 2; i++)
                {
                    curDirPath += folders[i] + Path.DirectorySeparatorChar;
                }

                ShowAllFiles(curDirPath);
            });

            selectButton.onClick.AddListener(() => onSelectClick.Invoke(curSelectFilePath));

            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void ShowAllFiles(string folderpath)
        {
            backButton.interactable = backBuffer.Count != 0;
            forwardButton.interactable = forwardBuffer.Count != 0;

            folderPathText.text = string.Join(" > ", curDirPath.Split(Path.DirectorySeparatorChar));

            //forceWrite = false;

            int cnt = contentTrf.childCount;

            for (int i = 0; i < cnt; ++i)
            {
                Destroy(contentTrf.GetChild(i).gameObject);
            }

            string[] fileEntries = null;
            string[] dirEntries = null;

            try
            {
                fileEntries = Directory.GetFiles(folderpath);
                dirEntries = Directory.GetDirectories(folderpath);
                foreach (string dirPath in dirEntries)
                {
                    var item = Instantiate(fileItem, contentTrf, false);

                    item.Initialized(Path.GetFileName(dirPath), folderIcon,
                    clickAction: () =>
                    {
                        backBuffer.Add(this.curDirPath);
                        forwardBuffer.Clear();
                        this.curDirPath = dirPath + Path.DirectorySeparatorChar;
                        ShowAllFiles(this.curDirPath);
                    });
                }

                foreach (string filePath in fileEntries)
                {
                    var item = Instantiate(fileItem, contentTrf, false);
                    UnityEngine.Events.UnityAction doubleClickAction = null;
                    if (isDoubleClickSelect)
                        doubleClickAction = () => onSelectClick.Invoke(curSelectFilePath);
                    string fileName = Path.GetFileName(filePath);
                    
                    item.Initialized(fileName, fileIcon,
                    clickAction: () =>
                    {
                        curSelectFilePath = filePath;
                        filePathIpf.text = fileName;
                    },
                    doubleClickAction: doubleClickAction);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                //AndroidPlugin.Instance.Toast("Unauthorized Access Detected.");
                backButton.onClick.Invoke();
            }

            // Folder is Empty
            if ((fileEntries == null || fileEntries.Length == 0) && (dirEntries == null || dirEntries.Length == 0))
            {
                Instantiate(emptyFolderTextObj, contentTrf, false);
            }
        }

        private void CreateDir(string dirName)
        {
            curDirPath = (Application.platform == RuntimePlatform.Android) ? ($"/storage/emulated/0/{companyName}/" + dirName) :
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("Program Files", companyName + Path.DirectorySeparatorChar + dirName);

            DirectoryInfo di = new DirectoryInfo(curDirPath);
            if (!di.Exists)
            {
                di.Create();
            }
        }
    }
}