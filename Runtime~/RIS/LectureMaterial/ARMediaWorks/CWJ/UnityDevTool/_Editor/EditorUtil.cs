#if UNITY_EDITOR
using System;

using Microsoft.CSharp;
using System.CodeDom;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CWJ.AccessibleEditor;
using Debug = UnityEngine.Debug;

namespace CWJ.EditorOnly
{
    public static class EditorUtil
    {
        [MenuItem("Assets/CWJ_Export Package This", false, 1994)]
        private static void ExportPackageHere()
        {
            // 패키지 저장 경로 입력
            var packagePath = EditorUtility.SaveFilePanel("Export package path...", "", "ExportedPackage.unitypackage", "unitypackage");
            if (string.IsNullOrEmpty(packagePath)) return;

            // 선택된 폴더 경로 가져오기
            var selectedFolderPath = GetSelectedFolderPath();
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                Debug.LogError("No folder selected for export.");
                return;
            }

            // 제외 조건 설정 (_EditorCache 폴더 제외)
            List<string> includePaths = new List<string>();
            try
            {
                EditorApplication.LockReloadAssemblies();
                string ignoreKeyword = "UnityDevTool/" + ScriptableObjectStore.EditorCacheFolderName;
                includePaths = CollectAssetPaths(selectedFolderPath, ignoreKeyword, true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during asset path collection: {e}");
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();

                // 내보낼 경로가 없으면 중단
                if (includePaths.Count == 0)
                {
                    Debug.LogError("No assets to export after applying ignore conditions.");
                }
                else
                {
                    // 패키지 내보내기
                    AssetDatabase.ExportPackage(includePaths.ToArray(), packagePath, ExportPackageOptions.Recurse);
                    EditorApplication.delayCall += () =>
                    {
                        Debug.Log($"Package exported to: {packagePath}");
                        System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(packagePath));
                    };
                }
            }
        }

        private static string GetSelectedFolderPath()
        {
            if (Selection.assetGUIDs == null || Selection.assetGUIDs.Length == 0)
                return null;

            var assetGuid = Selection.assetGUIDs[0];
            var path = AssetDatabase.GUIDToAssetPath(assetGuid);
            return !Directory.Exists(path) ? null : path;
        }

        // 에셋 경로 수집 함수
        public static List<string> CollectAssetPaths(string rootPath, string ignoreKeyword, bool useEndsWithCheck = true, bool ignoreKeywordMetaFile = true)
        {
            var assetPaths = new List<string>();
            string normalizedIgnoreKeyword = ignoreKeyword.Replace("\\", "/").TrimEnd("/");
            string ignoreKeywordMetaFilename = normalizedIgnoreKeyword + ".meta";

            void Collect(string currentPath)
            {
                foreach (var directory in Directory.GetDirectories(currentPath))
                {
                    string normalizedPath = directory.Replace("\\", "/");

                    // 제외 조건 확인
                    if (useEndsWithCheck ? normalizedPath.EndsWith(normalizedIgnoreKeyword, StringComparison.OrdinalIgnoreCase)
                            : normalizedPath.Contains(normalizedIgnoreKeyword, StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.LogWarning($"Ignoring folder: {normalizedPath}");
                        continue;
                    }

                    // 재귀적으로 하위 폴더 처리
                    Collect(normalizedPath );
                }

                foreach (var file in Directory.GetFiles(currentPath))
                {
                    string assetPath = file.Replace("\\", "/");
                    if (!ignoreKeywordMetaFile || !assetPath.EndsWith(ignoreKeywordMetaFilename, StringComparison.OrdinalIgnoreCase))
                    {
                        // 파일 경로를 Unity 프로젝트 상대 경로로 변환
                        assetPaths.Add(assetPath);
                    }
                }
            }

            Collect(rootPath.Replace("\\", "/"));
            return assetPaths;
        }




        public const bool isCWJDebuggingMode =
#if CWJ_EDITOR_DEBUG_ENABLED
                true;
#else
            false;
#endif

        public static void OpenScriptFromStackTrace(string stackTrace)
        {
            var regex = Regex.Match(stackTrace, @"\(at .*\.cs:[0-9]+\)$", RegexOptions.Multiline);
            if (regex.Success)
            {
                string line = stackTrace.Substring(regex.Index + 4, regex.Length - 5);
                int lineSeparator = line.IndexOf(':');
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(line.Substring(0, lineSeparator));
                if (script != null)
                    AssetDatabase.OpenAsset(script, int.Parse(line.Substring(lineSeparator + 1)));
            }
        }

        public static string[] ToCsFriendlyTypeNames(this Type[] systemTypes)
        {
            string[] typeNames = new string[systemTypes.Length];

            //Cannot use C# Dotnet 2.0 without Editor Folder
            using (var provider = new CSharpCodeProvider())
            {
                for (int i = 0; i < systemTypes.Length; i++)
                {
                    if (string.Equals(systemTypes[i].Namespace, ReflectionUtil.SystemNameSpace))
                    {
                        string csFriendlyName = provider.GetTypeOutput(new CodeTypeReference(systemTypes[i]));
                        if (csFriendlyName.IndexOf(ReflectionUtil.Dot) == -1)
                        {
                            typeNames[i] = csFriendlyName;
                            continue;
                        }
                    }

                    typeNames[i] = systemTypes[i].Name;
                }
            }

            return typeNames;
        }

    }
}
#endif
