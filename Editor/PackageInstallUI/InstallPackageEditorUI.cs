#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using System.Diagnostics;
using System;

namespace CWJ.YU.Editor
{
    public static class InstallPackageEditorUI
    {
        private const string This_Installer_Name = "com.cwj.yu.mobility";
        private const string GitRepoUrl = "https://github.com/DevCWJ/yu.mobility.git";
        private const string DemoDriveUrl = "https://drive.google.com/drive/folders/1m_9dOMbPEMA4S9hDls5PpNCcNXUULWh1?usp=sharing";
        private const string descStr = "[RIS 영남대 스마트 모빌리티 패키지]";

        [InitializeOnLoadMethod]
        static void Init()
        {
            PackageManagerExtensions.RegisterExtension(new UpmExtension());
        }

        class UpmExtension : IPackageManagerExtension
        {
            public VisualElement CreateExtensionUI()
            {
                VisualElement ExtentionRoot = new VisualElement();

                VisualElement labelLine = new VisualElement();
                ExtentionRoot.Add(labelLine);
                descLbl = new Label { text = " " };
                labelLine.Add(descLbl);

                VisualElement buttonsLine1 = new VisualElement();
                ExtentionRoot.Add(buttonsLine1);

                buttonsLine1.style.flexDirection = FlexDirection.Row;
                buttonsLine1.style.flexWrap = Wrap.Wrap;

                const int width = 160;

                changeApiCompatibilityBtn = new Button();
                changeApiCompatibilityBtn.text = "Change API Compatibility";
                changeApiCompatibilityBtn.clicked += ChangeApiCompatibility;
                changeApiCompatibilityBtn.style.width = width;
                buttonsLine1.Add(changeApiCompatibilityBtn);

                downloadAssetsBtn = new Button();
                downloadAssetsBtn.text = "Download Assets";
                downloadAssetsBtn.clicked += OnDownloadAssetsBtnClicked;
                downloadAssetsBtn.style.width = width;
                downloadAssetsBtn.visible = false;
                buttonsLine1.Add(downloadAssetsBtn);

                VisualElement buttonsLine2 = new VisualElement();
                ExtentionRoot.Add(buttonsLine2);
                buttonsLine2.style.flexDirection = FlexDirection.Row;
                buttonsLine2.style.flexWrap = Wrap.Wrap;

                openDemoUrlBtn = new Button();
                openDemoUrlBtn.text = "PC_데모.exe Drive URL 열기";
                openDemoUrlBtn.clicked += OnOpenDemoUrlBtnClicked;
                openDemoUrlBtn.style.width = width;
                buttonsLine2.Add(openDemoUrlBtn);
                //

                VisualElement lastLine = new VisualElement();
                ExtentionRoot.Add(lastLine);
                lastLine.style.flexDirection = FlexDirection.Row;
                lastLine.style.flexWrap = Wrap.Wrap;

                sampleDescLbl = new Label { text = "아래 Samples에서 데모씬을 import 받을수있습니다.\n(import이후 데모씬 위치 : Assets/Samples/CWJ.YU.Mobility/1.1.2/데모용 Resources, Scene/)" };
                lastLine.Add(sampleDescLbl);
                return ExtentionRoot;
            }

            private Button downloadAssetsBtn, openDemoUrlBtn, changeApiCompatibilityBtn;
            private Label descLbl, sampleDescLbl;
            private PackageInfo current = null;

            public void OnPackageSelectionChange(PackageInfo packageInfo)
            {
                current = packageInfo;
                bool isTargetPackage = current != null && current.name == This_Installer_Name;

                descLbl.visible = isTargetPackage;
                downloadAssetsBtn.visible = false; //다운받을필요없어짐
                openDemoUrlBtn.visible = isTargetPackage;
                changeApiCompatibilityBtn.visible = isTargetPackage;
                sampleDescLbl.visible = isTargetPackage;

                if (!isTargetPackage)
                {
                    return;
                }

                bool needChangeApi = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup) !=
                                     ApiCompatibilityLevel.NET_Unity_4_8;

                changeApiCompatibilityBtn.SetEnabled(needChangeApi);
                downloadAssetsBtn.SetEnabled(!needChangeApi);
                openDemoUrlBtn.SetEnabled(!needChangeApi);

                if (needChangeApi)
                {
                    descLbl.text = $"[{changeApiCompatibilityBtn.text}] 버튼을 눌러주세요.";
                }
                else
                {
                    CheckForUpdates();
                }
            }

            private void OnDownloadAssetsBtnClicked()
            {
                if (UnityEditor.EditorUtility.DisplayDialog("CWJ.YU.Mobility Info",
                                                            "다운로드 받으시겠습니까?", "Download", "Cancel"))
                {
                    if (TrySelectDownloadPath(out string downloadFolderPath))
                    {
                        ImportSamples(downloadFolderPath);
                        UnityEditor.EditorUtility.DisplayDialog("CWJ.YU.Mobility Info", "Download 완료", "OK");
                    }
                }
            }

            private void OnOpenDemoUrlBtnClicked()
            {
                Application.OpenURL(DemoDriveUrl);
            }



            private void CheckForUpdates()
            {
                bool needUpdate = current.CheckNeedUpdateByLastUpdateDate(out string latestVersion);
                downloadAssetsBtn.SetEnabled(string.IsNullOrEmpty(latestVersion) || !needUpdate);
                descLbl.text = $"{descStr}\n" + (needUpdate ? "Update가 필요합니다." : "현재 최신 버전입니다.");
            }

            public void OnPackageAddedOrUpdated(PackageInfo packageInfo) { }

            public void OnPackageRemoved(PackageInfo packageInfo) { }
        }

        private static void ImportSamples(string downloadFolderPath)
        {
            string packagePath = GetPackagePath();
            if (string.IsNullOrEmpty(packagePath))
            {
                Debug.LogError("패키지 경로를 찾을 수 없습니다.");
                return;
            }

            string samplesPath = Path.Combine(packagePath, "Samples~");
            if (!Directory.Exists(samplesPath))
            {
                Debug.LogError("Samples~ 폴더를 찾을 수 없습니다.");
                return;
            }

            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }

            foreach (string dirPath in Directory.GetDirectories(samplesPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(samplesPath, downloadFolderPath));
            }

            foreach (string newPath in Directory.GetFiles(samplesPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(samplesPath, downloadFolderPath), true);
            }

            AssetDatabase.Refresh();
            Debug.Log("Samples~ 폴더의 샘플이 프로젝트로 복사되었습니다: " + downloadFolderPath);
        }

        private static string GetPackagePath()
        {
            var request = UnityEditor.PackageManager.Client.List(true, false);
            while (!request.IsCompleted) { }
            var packageInfo = request.Result.FirstOrDefault(p => p.name == This_Installer_Name);
            return packageInfo?.resolvedPath;
        }

        private static bool TrySelectDownloadPath(out string folderPath, string lastFilePath = null)
        {
            string defaultPath = string.IsNullOrEmpty(lastFilePath) ? Directory.GetParent(Application.dataPath)?.FullName : null;

            folderPath = EditorUtility.OpenFolderPanel("unitypackage 다운로드 받을 폴더 선택", defaultPath, "");

            return !string.IsNullOrEmpty(folderPath);
        }

        private static bool CheckNeedUpdateByLastUpdateDate(this PackageInfo packageInfo, out string latestVersion)
        {
            latestVersion = GetLatestGitTag();

            if (string.IsNullOrEmpty(latestVersion))
            {
                return true;
            }

            latestVersion = latestVersion.VersionNormalized()[1..];
            string currentVersion = packageInfo.version.VersionNormalized();
            return !string.IsNullOrEmpty(latestVersion) && currentVersion != latestVersion;
        }

        private static string VersionNormalized(this string input)
        {
            return input
                   .Trim()
                   .Replace("\n", "") // 줄바꿈 제거
                   .Replace("\r", "") // 캐리지 리턴 제거
                   .Replace("\t", ""); // 탭 제거
        }
        private static string GetLatestGitTag()
        {
            try
            {
                // Git 명령 실행
                ProcessStartInfo startInfo = new ProcessStartInfo
                                             {
                                                 FileName = "git",
                                                 Arguments = $"ls-remote --tags {GitRepoUrl}",
                                                 RedirectStandardOutput = true,
                                                 UseShellExecute = false,
                                                 CreateNoWindow = true
                                             };

                using Process process = Process.Start(startInfo);
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Git 태그 목록에서 최신 태그를 추출
                string[] lines = output.Split('\n');
                string latestTag = lines
                                   .Where(line => line.Contains("refs/tags/"))
                                   .Select(line => line.Split('/').Last())
                                   .OrderByDescending(tag => tag)
                                   .FirstOrDefault();

                return latestTag;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Git 태그를 가져오는 중 오류 발생: " + ex.Message);
                return null;
            }
        }

        private static void ChangeApiCompatibility()
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            PlayerSettings.SetApiCompatibilityLevel(targetGroup, ApiCompatibilityLevel.NET_Unity_4_8);

            Debug.Log($"API Compatibility Level changed to: {ApiCompatibilityLevel.NET_Unity_4_8.ToString()} (.NET Framework)");
        }
    }
}
#endif
