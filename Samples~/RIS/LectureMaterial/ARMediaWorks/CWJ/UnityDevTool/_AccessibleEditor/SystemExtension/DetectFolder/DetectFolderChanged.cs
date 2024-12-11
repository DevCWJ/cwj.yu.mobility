#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    public static class DetectFolderChanged
    {
        private static FolderInfoCache_ScriptableObject _ScriptableObj = null;

        public static FolderInfoCache_ScriptableObject ScriptableObj
        {
            get
            {
                if (_ScriptableObj == null)
                {
                    _ScriptableObj = ScriptableObjectStore.Instanced.GetScriptableObj<FolderInfoCache_ScriptableObject>();
                }
                return _ScriptableObj;
            }
        }


        public static void InitFolderCache()
        {
            if (ScriptableObj == null) return;

            try
            {
                ScriptableObj.rootFolderDatas = new DirectoryInfo(Application.dataPath).GetDirectories()?.ToFolderDatas();
                ScriptableObj.cwjFolderDatas = new DirectoryInfo(PathUtil.MyAbsolutePath_CWJ)?.GetDirectories()?.ToFolderDatas();
                SaveFolderCache();
            }
            catch { }
        }

        private static void SaveFolderCache()
        {
            ScriptableObj.isInitialized = true;
            EditorUtility.SetDirty(ScriptableObj);
        }

        public static void CheckFolderModifiy()
        {
            if (!CWJ_EditorEventHelper.IsProjectOpened || ScriptableObj == null)
            {
                return;
            }

            if (CWJ_EditorEventHelper.IsImportLoading || !ScriptableObj.isInitialized)
            {
                InitFolderCache();
                return;
            }

            try
            {
                ScriptableObj.cwjFolderDatas = new DirectoryInfo(PathUtil.MyAbsolutePath_CWJ)?.GetDirectories()?.ToFolderDatas();

                FolderData[] lastFolderDatas = ScriptableObj.rootFolderDatas;
                FolderData[] curFolderDatas = new DirectoryInfo(Application.dataPath).GetDirectories().ToFolderDatas();

                List<UnityObject> newFolderObjs = new List<UnityObject>();

                for (int i = 0; i < curFolderDatas.Length; ++i)
                {
                    if (!lastFolderDatas.ContainsData(curFolderDatas[i]))
                    {
                        UnityObject asset = AssetDatabase.LoadAssetAtPath<UnityObject>("Assets/" + curFolderDatas[i].name);
                        if (asset != null)
                        {
                            newFolderObjs.Add(asset);
                        }
                    }
                }

                if (newFolderObjs.Count > 0 || lastFolderDatas.Length != curFolderDatas.Length)
                {
                    ScriptableObj.rootFolderDatas = curFolderDatas;
                }

                if (newFolderObjs.Count > 0)
                {
                    typeof(UnityEditor.Editor).PrintLogWithClassName("New Root Folder Name : '" + string.Join("', '", newFolderObjs.ConvertAll((o) => o.name)) + "'.", LogType.Log, newFolderObjs[0], isPreventStackTrace: true);

                    EditorCallback.AddWaitForFrameCallback(() =>
                    {
                        Selection.objects = newFolderObjs.ToArray();
                        EditorGUIUtility.PingObject(newFolderObjs[0]);
                    });
                }

                SaveFolderCache();
            }
            catch { }

        }

        public static string[] ToFolderNames(this FolderData[] folderDatas)
        {
            return folderDatas.ConvertAll((f) => f.name);
        }

        private static FolderData[] ToFolderDatas(this DirectoryInfo[] directoryInfos)
        {
            return directoryInfos.ConvertAll((d) => new FolderData(d));
        }

        private static bool ContainsData(this FolderData[] folderDatas, FolderData folderData)
        {
            return (Array.FindIndex(folderDatas, (f) => f.name.Equals(folderData.name) && f.creationTime.Equals(folderData.creationTime)) >= 0);
        }
    }
}
#endif