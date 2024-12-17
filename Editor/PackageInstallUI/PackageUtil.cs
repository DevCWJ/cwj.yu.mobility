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
        [SerializeField]
        private static string _PackageRelativePath;

        /// <summary>
        /// Returns the fully qualified path of the package.
        /// </summary>
        public static string PackageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_PackageFullPath_))
                    _PackageFullPath_ = GetPackageFullPath();

                return _PackageFullPath_;
            }
        }

        /// <summary>
        /// Returns the relative path of the package.
        /// </summary>
        public static string PackageRelativePath
        {
            get
            {
                if (string.IsNullOrEmpty(_PackageRelativePath))
                    _PackageRelativePath = GetPackageRelativePath();

                return _PackageRelativePath;
            }
        }

        [SerializeField]
        private static string _PackageFullPath_;

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
            string packagePath = Path.GetFullPath($"Packages/{MyPackageName}");
            if (Directory.Exists(packagePath))
            {
                return $"Packages/{MyPackageName}";
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


        public static void ImportSamplesToPath(PackageInfo curPackageInfo, string downloadFolderPath)
        {
            string packagePath = curPackageInfo.GetPackagePath();
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

        public static void ExportAndImportPackageAnywhere(string srcFolderRootInPackagePath = RuntimeIgnoreFolderName)
        {
            if (srcFolderRootInPackagePath == null) srcFolderRootInPackagePath = RuntimeIgnoreFolderName;

            string downloadSrcFolderRoot = Path.Combine(PackageRelativePath, srcFolderRootInPackagePath);
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

            if (!UnityEditor.EditorUtility.DisplayDialog(MyPackageInAssetName, "다소 시간이 소모됩니다.\n unitypackage를 다운로드 받으시겠습니까?\n기존 파일들은 지우고 다시 Download하는걸 추천합니다.", "Ok", "Cancel"))
            {
                return;
            }

            string targetFolder = null;
            string unitypackageFilePath = null;
            string folderName = null;

            try
            {
                AssetDatabase.StartAssetEditing();
                EditorApplication.LockReloadAssemblies();
                string sourceFolder = subDirectories[0]; // 복사 대상 폴더 (첫 번째 폴더)
                folderName = Path.GetFileName(sourceFolder); // 폴더 이름 추출
                string randomFolderName = $"temp_{UnityEngine.Random.Range(1000, 9999)}";
                targetFolder = Path.Combine(RuntimeDownloadAssetsPath, randomFolderName, folderName);

                if (!Directory.Exists(RuntimeDownloadAssetsPath))
                    Directory.CreateDirectory(RuntimeDownloadAssetsPath);

                // 폴더 복사
                CopyDirectory(sourceFolder, targetFolder);
                AssetDatabase.Refresh();

                // .unitypackage 파일 경로 설정
                unitypackageFilePath = Path.Combine(RuntimeDownloadAssetsPath, $"{MyPackageInAssetName}.{folderName}.unitypackage");

                // 패키지 내보내기
                AssetDatabase.ExportPackage(targetFolder, unitypackageFilePath, ExportPackageOptions.Recurse);
                Debug.Log($"Exported folder to UnityPackage: {unitypackageFilePath}");


            }
            catch (Exception ex)
            {
                Debug.LogError($"Error exporting/importing folder: {ex}");
            }
            finally
            {
                bool hasTargetFolder = targetFolder != null && Directory.Exists(targetFolder);
                // 임시 폴더 정리
                if (hasTargetFolder)
                {
                    Directory.Delete(targetFolder, true);
                    string _metaFile = targetFolder + ".meta";
                    if (File.Exists(_metaFile))
                        File.Delete(_metaFile);
                }

                AssetDatabase.StopAssetEditing();

                // 패키지 가져오기 (Import)
                if (hasTargetFolder && unitypackageFilePath != null && File.Exists(unitypackageFilePath))
                {
                    ImportUnityPackage(unitypackageFilePath, $"Assets/{folderName}");

                    if (File.Exists(unitypackageFilePath) &&
                        UnityEditor.EditorUtility.DisplayDialog(MyPackageInAssetName, "다운로드 받은 unitypackage는 삭제하시겠습니까?", "Ok", "Cancel"))
                    {
                        File.Delete(unitypackageFilePath);
                        string _metaFile = unitypackageFilePath + ".meta";
                        if (File.Exists(_metaFile))
                            File.Delete(_metaFile);
                    }
                }

                EditorApplication.UnlockReloadAssemblies();
                AssetDatabase.Refresh();
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            // 하위 파일 복사
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            // 하위 폴더 복사 (재귀)
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(targetDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir);
            }
        }

        public static void ImportUnityPackage(string unitypackageFilePath, string importTargetPath)
        {
            if (!File.Exists(unitypackageFilePath))
            {
                Debug.LogError($"Unitypackage file not found: {unitypackageFilePath}");
                return;
            }

            EnsureEditorCacheFolderExists(importTargetPath);
            CodeStage.PackageToFolder.Partial.Package2Folder_Partial.ImportPackageToFolder(unitypackageFilePath, importTargetPath, true);
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
