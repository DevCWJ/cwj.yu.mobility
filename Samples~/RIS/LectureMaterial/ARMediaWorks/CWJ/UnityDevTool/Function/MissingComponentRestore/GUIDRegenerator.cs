
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ
{
    public class GUIDRegenerator_Menu
    {

        [MenuItem("Assets/Regenerate GUID/Files Only", true)]
        public static bool RegenerateGUID_Validation()
        {
            return IsValidate();
        }

        [MenuItem("Assets/Regenerate GUID/Files and Folders", true)]
        public static bool RegenerateGUIDWithFolders_Validation()
        {
            return IsValidate();
        }

        private static bool IsValidate()
        {
            var isSelectedValid = true;

            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                isSelectedValid = !string.IsNullOrEmpty(guid) && guid != "0";
            }

            return isSelectedValid;
        }

        [MenuItem("Assets/Regenerate GUID/Files Only")]
        public static void RegenerateGUID_Implementation()
        {
            DoImplementation(false);
        }

        [MenuItem("Assets/Regenerate GUID/Files and Folders")]
        public static void RegenerateGUIDWithFolders_Implementation()
        {
            DoImplementation(true);
        }

        const string FilterOfAllObject = "t:Object";

        private static void DoImplementation(bool includeFolders)
        {
            List<string> selectedGUIDs = new List<string>();
            foreach (var guid in Selection.assetGUIDs)
            {
                var itemPath = AssetDatabase.GUIDToAssetPath(guid);
                if (PathUtil.IsDirectory(itemPath))
                {
                    if (includeFolders) selectedGUIDs.Add(guid);

                    selectedGUIDs.AddRange(AssetDatabase.FindAssets(FilterOfAllObject, new string[] { itemPath }));
                }
                else
                {
                    //if (File.Exists(assetPath)) //아래에서 throw처리함
                    selectedGUIDs.Add(guid);
                }
            }

            int selectedCnt = selectedGUIDs.Count();

            if (selectedCnt == 0) return;

            if (typeof(GUIDRegenerator_Menu).DisplayDialog($"Regenerate GUID for {selectedCnt} assets\n" +
                "DISCLAIMER: Intentionally modifying asset GUID is not recommended unless certain issues are encountered.\n" +
                "\nMake sure you have a backup or is using a version control system. \n\nThis operation can take a long time on larger projects. Do you want to proceed?",
                 ok: "Yes, please", cancel: "Nope"))
            {
                AssetDatabase.StartAssetEditing();
                GUIDRegenerator.RegenerateGUIDs(selectedGUIDs);
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }

    internal class GUIDRegenerator
    {
        public static void RegenerateGUIDs(IEnumerable<string> selectedGUIDs)
        {
            int cnt = selectedGUIDs.Count();
            var regeneratedAssets = new List<(string assetName, List<string> updatedPaths)>(cnt);
            var skippedAssets = new List<(string assetName, string skipReason)>(cnt);

            foreach (var selectedGUID in selectedGUIDs)
            {
                if (ReplaceGUID(selectedGUID, GUID.Generate().ToString(), ref regeneratedAssets, ref skippedAssets, false))
                {
                    //success
                }
            }

            PrintLog(regeneratedAssets, skippedAssets);
        }

        static readonly string[] SearchDirectories = { "Assets" };
        const string FilterOfAllScene = "t:Scene";
        const string FilterOfAllPrefab = "t:Prefab";


        public static bool ReplaceGUID(string oldGUID, string newGUID,
            ref List<(string, List<string>)> regeneratedAssets,
            ref List<(string, string)> skippedAssets,
            bool isForMissingComp)
        {
            bool isSuccess;
            string targetAssetPath = AssetDatabase.GUIDToAssetPath(oldGUID);
            List<string> updatedPaths = null;

            try
            {
                if (!isForMissingComp)
                {
                    var metaPath = AssetDatabase.GetTextMetaFilePathFromAssetPath(targetAssetPath);
                    if (!File.Exists(metaPath))
                    {
                        throw new FileNotFoundException($"The meta file of selected asset cannot be found. \npath: {targetAssetPath}");
                    }

                    var metaContents = File.ReadAllText(metaPath);

                    if (!metaContents.Contains(oldGUID))
                    {
                        throw new ArgumentException($"The GUID of [{targetAssetPath}] does not match the GUID in its meta file.");
                    }

                    if (targetAssetPath.EndsWith(".unity"))
                    {
                        throw new FormatException($"The GUID of [{targetAssetPath}] is Scene.");
                    }

                    var metaAttributes = File.GetAttributes(metaPath);
                    var isMetaFileHidden = false;

                    if (metaAttributes.HasFlag(FileAttributes.Hidden))
                    {
                        isMetaFileHidden = true;
                        PathUtil.SetFileHidden(metaPath, metaAttributes, false);
                    }

                    metaContents = metaContents.Replace(oldGUID, newGUID);
                    File.WriteAllText(metaPath, metaContents);

                    if (isMetaFileHidden)
                        PathUtil.SetFileHidden(metaPath, metaAttributes, true);
                }

                if (isForMissingComp || !PathUtil.IsDirectory(targetAssetPath))
                {
                    List<string> refAssetGUIDs = new List<string>();
                    refAssetGUIDs.AddRange(AssetDatabase.FindAssets(isForMissingComp ? FilterOfAllPrefab : $"ref:{targetAssetPath}", SearchDirectories));
                    refAssetGUIDs.AddRange(AssetDatabase.FindAssets(FilterOfAllScene, SearchDirectories));

                    int cntProgress = 0;
                    int progressCnt = refAssetGUIDs.Count;
                    updatedPaths = new List<string>(progressCnt);

                    string progressBarTitle = $"Regenerating GUID: {targetAssetPath}";
                    if (isForMissingComp) progressBarTitle += " (Missing Component)";
                    EditorUtility.DisplayProgressBar(progressBarTitle, $"count : {progressCnt}", 0);

                    foreach (var guid in refAssetGUIDs)
                    {
                        cntProgress++;
                        string path = AssetDatabase.GUIDToAssetPath(guid);

                        EditorUtility.DisplayProgressBar(progressBarTitle, path, (float)cntProgress / progressCnt);

                        if (PathUtil.IsDirectory(path) || !File.Exists(path)) continue;

                        string contents = File.ReadAllText(path);

                        if (!contents.Contains(oldGUID)) continue;

                        contents = contents.Replace(oldGUID, newGUID);
                        File.WriteAllText(path, contents);
                        updatedPaths.Add(path);
                    }
                }

                isSuccess = true;
            }
            catch (Exception e)
            {
                if (skippedAssets == null) skippedAssets = new List<(string, string)>();
                skippedAssets.Add((targetAssetPath, e.Message));
                Debug.LogError(e);
                isSuccess = false;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            if (isSuccess)
            {
                if (regeneratedAssets == null) regeneratedAssets = new List<(string, List<string>)>();
                regeneratedAssets.Add((targetAssetPath, updatedPaths));
            }

            return isSuccess;
        }

        public static void PrintLog(List<(string assetName, List<string> updatedPaths)> updatedAssets,
            List<(string assetName, string skipReason)> skippedAssets)
        {
            string message = ("GUID Regenerator").SetBold() + "\n";

            int updatedCnt = updatedAssets.Count;
            if (updatedCnt > 0)
            {
                message += ($"{updatedCnt} Updated Assets").SetStyle(new Color().GetCommentsColor(), isBold: true) + "\n";
                message = updatedAssets.Aggregate(message, (prev, cur) => prev + $"▶{(cur.updatedPaths.Count + " references by " + cur.assetName).SetBold()}◀\n\t\t {string.Join("\n\t\t ", cur.updatedPaths)}\n");
            }
            int skipCnt = skippedAssets.Count;
            if (skipCnt > 0)
            {
                message += ($"\n{skipCnt} Skipped Assets\n").SetStyle(new Color().GetDarkRed(), isBold: true);
                message = skippedAssets.Aggregate(message, (prev, cur) => prev + $"{cur.assetName} : {cur.skipReason}\n");
            }

            string title = $"Regenerated GUID for {updatedCnt} assets";
            typeof(GUIDRegenerator_Menu).PrintLogWithClassName($"{title} \n\n{message}", LogType.Log, isComment: false, isBigFont: false);
            typeof(GUIDRegenerator_Menu).DisplayDialog($"{title}. \n\nPlease wait for AssetDatabase Refresh after click 'Done' button. See unity Console for detailed report.", "Done", isPrintLog: false);
        }
    } 
}
#endif