#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace CWJ.Editor
{
    using static PackageDefine;

    public static class PackageUtil
    {

        /// <summary>
        /// Returns the fully qualified path of the package.
        /// </summary>
        public static string PackageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_PackageFullPath))
                    _PackageFullPath = GetPackageFullPath();

                return _PackageFullPath;
            }
        }

        [SerializeField]
        private static string _PackageFullPath;

        private static string GetPackageFullPath()
        {
            //UPM 패키지 확인
            string packagePath = Path.GetFullPath($"Packages/{MyPackageName}");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                // 패키지의 기본 위치 검색
                if (Directory.Exists(packagePath + $"/Assets/{MyPackageInAssetName}/{UnityPackageFolderName}"))
                {
                    return packagePath + $"/Assets/{MyPackageInAssetName}";
                }

                // 사용자 프로젝트에서 대체 위치 검색
                string[] matchingPaths = Directory.GetDirectories(packagePath, MyPackageInAssetName, SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return packagePath + path;
            }

            return null;
        }

        private static string GetPackageRelativePath()
        {
            //UPM 패키지 확인
            string packagePath = Path.GetFullPath($"Packages/{MyPackageName}"); //D:\CWJ\!Rise\TEST\TEST2\Library\PackageCache\com.cwj.yu.mobility@4c6e8e607f
            if (Directory.Exists(packagePath))
            {
                return $"Packages/{MyPackageName}"; //Library\PackageCache\com.cwj.yu.mobility@4c6e8e607f
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                // 패키지의 기본 위치 검색
                if (Directory.Exists(packagePath + $"/Assets/{MyPackageInAssetName}/{UnityPackageFolderName}"))
                {
                    return $"Assets/{MyPackageInAssetName}";
                }

                //사용자 프로젝트에서 대체 위치 검색
                string[] matchingPaths = Directory.GetDirectories(packagePath, MyPackageInAssetName, SearchOption.AllDirectories);
                packagePath = ValidateLocation(matchingPaths, packagePath);
                if (packagePath != null) return packagePath;
            }

            return null;
        }



        /// <summary>
        /// Method to validate the location of the asset folder by making sure the GUISkins folder exists.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private static string ValidateLocation(string[] paths, string projectPath)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                // Check if any of the matching directories contain a GUISkins directory.
                if (Directory.Exists(paths[i] + $"/{UnityPackageFolderName}"))
                {
                    string folderPath = paths[i].Replace(projectPath, "");
                    folderPath = folderPath.TrimStart('\\', '/');
                    return folderPath;
                }
            }

            return null;
        }


        // public static void ImportSamplesToPath(PackageInfo curPackageInfo, string downloadFolderPath)
        // {
        //     string packagePath = curPackageInfo.GetPackagePath();
        //     if (string.IsNullOrEmpty(packagePath))
        //     {
        //         Debug.LogError("패키지 경로를 찾을 수 없습니다.");
        //         return;
        //     }
        //
        //     string samplesPath = Path.Combine(packagePath, "Samples~");
        //     if (!Directory.Exists(samplesPath))
        //     {
        //         Debug.LogError("Samples~ 폴더를 찾을 수 없습니다.");
        //         return;
        //     }
        //
        //     if (!Directory.Exists(downloadFolderPath))
        //     {
        //         Directory.CreateDirectory(downloadFolderPath);
        //     }
        //
        //     foreach (string dirPath in Directory.GetDirectories(samplesPath, "*", SearchOption.AllDirectories))
        //     {
        //         Directory.CreateDirectory(dirPath.Replace(samplesPath, downloadFolderPath));
        //     }
        //
        //     foreach (string newPath in Directory.GetFiles(samplesPath, "*.*", SearchOption.AllDirectories))
        //     {
        //         File.Copy(newPath, newPath.Replace(samplesPath, downloadFolderPath), true);
        //     }
        //
        //     AssetDatabase.Refresh();
        //     Debug.Log("Samples~ 폴더의 샘플이 프로젝트로 복사되었습니다: " + downloadFolderPath);
        // }

        public static string GetPackagePath(this PackageInfo packageInfo)
        {
            var request = UnityEditor.PackageManager.Client.List(true, false);
            while (!request.IsCompleted) { }


            packageInfo ??= request.Result.FirstOrDefault(p => p.name == MyPackageName);
            return packageInfo?.resolvedPath;
        }

        public static bool TrySelectDownloadPath(out string folderPath, string lastFilePath = null)
        {
            string defaultPath = string.IsNullOrEmpty(lastFilePath) ? Directory.GetParent(Application.dataPath)?.FullName : null;

            folderPath = EditorUtility.OpenFolderPanel("unitypackage 다운로드 받을 폴더 선택", defaultPath, "");

            return !string.IsNullOrEmpty(folderPath);
        }

        public static bool CheckNeedUpdateByLastUpdate(this PackageInfo packageInfo, out string latestVersion)
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
                   .Replace("\n", string.Empty) // 줄바꿈 제거
                   .Replace("\r", string.Empty) // 캐리지 리턴 제거
                   .Replace("\t", string.Empty); // 탭 제거
        }

        private static string GetLatestGitTag()
        {
            string latestTag = null;
            try
            {
                System.Diagnostics.ProcessStartInfo startInfo = new()
                                                                {
                                                                    FileName = "git",
                                                                    Arguments = $"ls-remote --tags {MyGitRepoUrl}",
                                                                    RedirectStandardOutput = true,
                                                                    UseShellExecute = false,
                                                                    CreateNoWindow = true
                                                                }; // Git 명령 실행

                using var process = System.Diagnostics.Process.Start(startInfo);
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Git 태그 목록에서 최신 태그 추출
                    string[] lines = output.Split('\n');
                    latestTag = lines
                                .Where(line => line.Contains("refs/tags/"))
                                .Select(line => line.Split('/').Last())
                                .OrderByDescending(tag => tag)
                                .FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Git 태그를 가져오는 중 오류 발생: " + ex.Message);
                latestTag = null;
            }

            return latestTag;
        }

        private static string ToUnityPath(string path) => path.Replace('\\', '/');
        private static string ToSystemIoPath(string path) => path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        public static void DownloadOrImportRuntimePackage()
        {
            _PackageFullPath = null;
            string packageFullPath = PackageFullPath;

            string releaseUnitypackagePath = ToSystemIoPath($"{packageFullPath}/{UnityPackageFolderName}/{MyPackageInAssetName}.unitypackage");

            if (File.Exists(releaseUnitypackagePath))
            {
                ImportUnityPackage(releaseUnitypackagePath, "Assets", false);
                return;
            }

            string downloadSrcFolderRoot = ToSystemIoPath($"{packageFullPath}/{RuntimeIgnoreFolderName}".Replace("\\", "/"));
            if (!Directory.Exists(downloadSrcFolderRoot))
            {
                Debug.LogError($"Source folder not found: {downloadSrcFolderRoot}");
                return;
            }

            string[] subDirectories = Directory.GetDirectories(downloadSrcFolderRoot);
            if (subDirectories.Length == 0)
            {
                Debug.LogError("No subdirectories found in the Runtime~ folder.");
                return;
            }

            if (!UnityEditor.EditorUtility.DisplayDialog(MyPackageInAssetName,
                                                         "다소 시간이 소모됩니다.\n unitypackage를 다운로드 받으시겠습니까?\n기존 파일들은 지우고 다시 Download하는걸 추천합니다.", "Ok", "Cancel"))
            {
                return;
            }

            string assetsFullPath = Application.dataPath;

            string sourceFolder = subDirectories[0];
            string folderName = Path.GetFileName(sourceFolder); //RIS

            string downloadDestPath = $"{UnityPacakgesFolderName}/{folderName}";

            string downloadRelativePath = $"Assets/{downloadDestPath}";
            if (AssetDatabase.IsValidFolder(downloadRelativePath))
                AssetDatabase.DeleteAsset(downloadRelativePath);
            CreateFoldersUsingAssetDatabase(downloadRelativePath);


            string unitypackageFileFullPath = $"{assetsFullPath}/{UnityPacakgesFolderName}/{MyPackageInAssetName}.unitypackage";
            unitypackageFileFullPath = ToSystemIoPath(unitypackageFileFullPath);

            bool alreadyDownload = File.Exists(unitypackageFileFullPath);
            try
            {
                if (!alreadyDownload)
                {
                    EditorApplication.LockReloadAssemblies();

                    // AssetDatabase.Refresh();

                    string downloadFolderFullPath = ToSystemIoPath($"{assetsFullPath}/{downloadDestPath}");

                    CopyDirectory(sourceFolder, downloadFolderFullPath);

                    // AssetDatabase.Refresh();
                    // string downloadFolderRelativePath = $"Assets/{downloadDestPath}";
                    // AssetDatabase.ExportPackage(downloadFolderRelativePath, unitypackageFileFullPath, ExportPackageOptions.Recurse); //너무느려서 Export는 하지않기로
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error exporting/importing folder: {ex}");
            }
            finally
            {
                if (!alreadyDownload)
                {
                    // if (AssetDatabase.IsValidFolder(downloadRelativePath))
                    //     AssetDatabase.DeleteAsset(downloadRelativePath); //너무느려 Export는 하지않기로해서 리소스폴더 살림
                    EditorApplication.UnlockReloadAssemblies();
                    AssetDatabase.Refresh();
                }

                if (!string.IsNullOrEmpty(unitypackageFileFullPath) && File.Exists(unitypackageFileFullPath))
                {
                    EditorApplication.delayCall += () =>
                    {
                        ImportUnityPackage(unitypackageFileFullPath, "Assets", false);

                        // EditorApplication.delayCall += () =>
                        // {
                        //     if (UnityEditor.EditorUtility.DisplayDialog(MyPackageInAssetName, "다운로드 받은 unitypackage는 삭제하시겠습니까?", "Ok", "Cancel"))
                        //     {
                        //         File.Delete(unitypackageFileFullPath);
                        //         string metaFile = unitypackageFileFullPath + ".meta";
                        //         if (File.Exists(metaFile)) File.Delete(metaFile);
                        //     }
                        // };
                    };
                }

            }
        }

        private static void CreateFoldersUsingAssetDatabase(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath) || relativePath == "Assets") return;
            relativePath = ToUnityPath(relativePath);
            // 경로를 '/' 기준으로 나누기
            string[] folders = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // 첫 번째 폴더는 항상 "Assets"로 시작해야 함
            if (folders.Length == 0 || folders[0] != "Assets")
            {
                Debug.LogError("경로는 'Assets'로 시작해야 합니다.");
                return;
            }

            string currentPath = folders[0]; // "Assets"

            for (int i = 1; i < folders.Length; i++)
            {
                string newFolderName = folders[i];
                string nextPath = $"{currentPath}/{newFolderName}";

                // 폴더가 없으면 생성
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, newFolderName);
                }

                // 경로를 다음 폴더로 업데이트
                currentPath = nextPath;
            }

            Debug.Log($"폴더 생성 완료: {relativePath}");
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            // 경로 통일 및 끝에 슬래시 제거
            sourceDir = ToSystemIoPath(sourceDir.TrimEnd(Path.DirectorySeparatorChar, '/'));
            targetDir = ToSystemIoPath(targetDir.TrimEnd(Path.DirectorySeparatorChar, '/'));

            // 대상 폴더가 없으면 먼저 생성
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            try
            {
                // 하위 폴더 먼저 복사 (재귀)
                foreach (string subDir in Directory.GetDirectories(sourceDir))
                {
                    string destSubDir = Path.Combine(targetDir, Path.GetFileName(subDir));
                    CopyDirectory(subDir, destSubDir);
                }

                // 하위 파일 복사
                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error copying directory: {ex.Message}");
            }
        }

        // private static void CheckFolderMeta(string path)
        // {
        //     // 상위 폴더 검사
        //     DirectoryInfo currentDir = new DirectoryInfo(ToSystemIoPath(path));
        //     string assetsPath = ToSystemIoPath(Application.dataPath).TrimEnd(Path.DirectorySeparatorChar);
        //
        //     while (currentDir != null) // Assets 폴더까지 검사
        //     {
        //         if (currentDir.FullName.TrimEnd(Path.DirectorySeparatorChar)
        //                       .Equals(assetsPath, System.StringComparison.OrdinalIgnoreCase))
        //         {
        //             break;
        //         }
        //
        //         string folderMetaPath = currentDir.FullName + ".meta";
        //
        //         CreateMetaFile(folderMetaPath);
        //
        //         // 상위 폴더로 이동
        //         currentDir = currentDir.Parent;
        //     }
        // }
        //
        // private static void CreateMetaFile(string assetPath)
        // {
        //     string metaPath = assetPath + ".meta";
        //
        //     if (!File.Exists(metaPath))
        //     {
        //         string metaContent = "fileFormatVersion: 2\n" +
        //                              "guid: " + System.Guid.NewGuid().ToString("N") + "\n" +
        //                              "timeCreated: " + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "\n" +
        //                              "licenseType: Pro\n";
        //         File.WriteAllText(metaPath, metaContent);
        //     }
        // }

        private static void DeleteDirectoryAndMeta(string folderPath)
        {
            if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                string metaFile = folderPath + ".meta";
                if (File.Exists(metaFile)) File.Delete(metaFile);
            }
        }

        private static void DeleteFileAndMeta(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                string metaFile = filePath + ".meta";
                if (File.Exists(metaFile)) File.Delete(metaFile);
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="unitypackageFilePath"></param>
        /// <param name="importTargetPath">Assets넣으면 Assets root에 import함</param>
        /// <param name="needCheckExists"></param>
        public static void ImportUnityPackage(string unitypackageFilePath, string importTargetPath, bool needCheckExists = true)
        {
            if (!File.Exists(unitypackageFilePath))
            {
                Debug.LogError($"Unitypackage file not found: {unitypackageFilePath}");
                return;
            }

            if (needCheckExists && importTargetPath != "Assets")
                EnsureEditorCacheFolderExists(importTargetPath);
            EditorApplication.delayCall += () =>
                CodeStage.PackageToFolder.Partial.Package2Folder_Partial.ImportPackageToPath(unitypackageFilePath, importTargetPath, true);
        }

        private static void EnsureEditorCacheFolderExists(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            string[] subFolders = relativePath.Replace('\\', '/').Split('/'); // "Assets", "CWJ", "UnityDevTool", ...

            string currentPath = subFolders[0];
            if (!currentPath.StartsWith("Assets"))
            {
                Debug.LogError("EnsureEditorCacheFolderExists: Path must start with 'Assets/'");
                return;
            }

            for (int i = 1; i < subFolders.Length; i++)
            {
                string folderName = subFolders[i];
                currentPath = currentPath + "/" + folderName;

                if (!AssetDatabase.IsValidFolder(currentPath))
                {
                    string parentFolder = System.IO.Path.GetDirectoryName(currentPath)?.Replace('\\', '/');
                    if (!string.IsNullOrEmpty(parentFolder))
                        AssetDatabase.CreateFolder(parentFolder, folderName);
                }
            }
        }

        // private static void ExportSamplesPath(PackageInfo packageInfo, string exportTargetPath, string moveToPath)
        // {
        //     string packagePath = GetPackagePath(packageInfo);
        //     if (string.IsNullOrEmpty(packagePath))
        //     {
        //         Debug.LogError("패키지 경로를 찾을 수 없습니다.");
        //         return;
        //     }
        //
        //     string samplesPath = Path.Combine(packagePath, "Samples~");
        //     if (!Directory.Exists(samplesPath))
        //     {
        //         Debug.LogError("Samples~ 폴더를 찾을 수 없습니다.");
        //         return;
        //     }
        //
        //     if (!Directory.Exists(exportTargetPath))
        //     {
        //         return;
        //     }
        //     //
        //
        //     string[] samples = Directory.GetDirectories(exportTargetPath);
        //     foreach (string samplePath in samples)
        //     {
        //         string sampleName = Path.GetFileName(samplePath);
        //         string packagePath = Path.Combine(moveToPath, $"{sampleName}.unitypackage");
        //
        //         Debug.Log($"Exporting {sampleName} to {packagePath}");
        //
        //         AssetDatabase.ExportPackage(samplePath, packagePath, ExportPackageOptions.Default);
        //     }
        // }

    }
}

#endif
