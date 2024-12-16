#if UNITY_EDITOR
using System;
using System.IO;

namespace CWJ.AccessibleEditor
{
    [Serializable]
    public sealed class FolderInfoCache_ScriptableObject : Initializable_ScriptableObject
    {
        public FolderData[] rootFolderDatas;
        public FolderData[] cwjFolderDatas;
    }

    [Serializable]
    public struct FolderData
    {
        [UnityEngine.HideInInspector] public string name;
        public string creationTime;

        public FolderData(DirectoryInfo directoryInfo)
        {
            this.name = directoryInfo.Name;
            this.creationTime = directoryInfo.CreationTime.ToString("yy.MM.dd HH:mm:ss");
        }
    }
} 
#endif