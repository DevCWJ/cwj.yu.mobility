#if UNITY_EDITOR

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CWJ.AccessibleEditor
{
    /// <summary>
    /// 말그대로 휘발성 EditorPrefs
    /// <br/>프로젝트 끌때 삭제시킴
    /// <para>이름에 Comma(,)랑 Dot(.) 사용하지말것</para>
    /// </summary>
    public static class VolatileEditorPrefs
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.ProjectOpenEvent += InitPrefsCache;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += RemoveAllSceneVolatileKey;
            CWJ_EditorEventHelper.EditorQuitEvent += RemoveAllVolatileKey;
        }

        public static string GetVolatilePrefsKey_Root(string usageName) => "..CWJ<" + AccessibleEditorUtil.GetProjectUniqueIdentifier() + ">" + usageName + "/";

        public static string GetVolatilePrefsKey_Child(string usageName, string description) => "..CWJ<" + AccessibleEditorUtil.GetProjectUniqueIdentifier() + ">" + usageName + "." + description + "/";


        private static string ProjectVolatilePrefsKey_Root => $" .CWJ<{AccessibleEditorUtil.GetProjectUniqueIdentifier()}>ProjectVolatilePrefsKeys/";
        private static string SceneVolatilePrefsKey_Root => $" .CWJ<{AccessibleEditorUtil.GetProjectUniqueIdentifier()}>SceneVolatilePrefsKeys/";

        public enum EKeyType
        {
            FoldoutCache,
        }

        private const string Comma = ",";

        public static bool AddStack(string stackKeyName, string value)
        {
            var stackValues = GetStackArray(stackKeyName);

            if (stackValues.IsExists(value))
            {
                return false;
            }
            else
            {
                if (stackValues.Length > 0)
                {
                    EditorPrefs.SetString(stackKeyName, (EditorPrefs.GetString(stackKeyName) + Comma + value));
                }
                else
                {
                    EditorPrefs.SetString(stackKeyName, value);
                }
                return true;
            }
        }

        public static string[] GetStackArray(string stackKeyName)
        {
            string stackValues = EditorPrefs.GetString(stackKeyName, string.Empty);
            return stackValues.Equals(string.Empty) ? new string[0] : stackValues.Split(new string[] { Comma }, StringSplitOptions.None);
        }

        public static string ConvertToLine(string[] stackArray)
        {
            return string.Join(Comma, stackArray);
        }

        public static bool RemoveStack(string stackKeyName, string value)
        {
            var stackValueList = GetStackArray(stackKeyName).ToList();

            if (!stackValueList.IsExists(value)) return false;

            stackValueList.Remove(value);

            if (stackValueList.Count == 0)
            {
                EditorPrefs.DeleteKey(stackKeyName);
                return true;
            }
            else
            {
                EditorPrefs.SetString(stackKeyName, ConvertToLine(stackValueList.ToArray()));
                return false;
            }
        }

        //주의사항. '/'는 ChildKey의 마침표로 사용됨. 각 ChildKey들의 중복여부는 Contains로 확인하기때문에 앞이나 뒤에 키워드가 추가되는 경우엔 중복이라고 판단될수도있기에 '/' 는 함부로 사용하지말것

        public static bool AddProjectVolatileKey(string name)
        {
            return AddStack(ProjectVolatilePrefsKey_Root, name);
        }

        public static bool ExistsProjectVolatileKey(string name)
        {
            return GetStackArray(ProjectVolatilePrefsKey_Root).IsExists(name);
        }

        public static void RemoveProjectVolatileKey(string name)
        {
            RemoveStack(ProjectVolatilePrefsKey_Root, name);
        }

        public static bool AddSceneVolatileKey(string name)
        {
            return AddStack(SceneVolatilePrefsKey_Root, name);
        }

        public static void RemoveAllSceneVolatileKey(Scene scene)
        {
            foreach (var keyName in GetStackArray(SceneVolatilePrefsKey_Root))
            {
                if (string.IsNullOrEmpty(keyName)) continue;
                EditorPrefs.DeleteKey(keyName);
            }
        }

        public static void RemoveSceneVolatileKey(string name)
        {
            RemoveStack(SceneVolatilePrefsKey_Root, name);
        }

        public static bool AddStackValue(string key, string value, bool isRemoveWhenSceneChanged = true)
        {
            if (isRemoveWhenSceneChanged)
            {
                AddSceneVolatileKey(key);
            }
            AddProjectVolatileKey(key);
            return AddStack(key, value);
        }

        public static bool ExistsStackValue(string key, string value, bool defaultBool = false)
        {
            if (!ExistsProjectVolatileKey(key))
            {
                return defaultBool;
            }
            else
            {
                if (GetStackArray(key).IsExists(value))
                {
                    return true;
                }
                else
                {
                    return defaultBool;
                }
            }
        }

        public static bool RemoveStackValue(string key, string value, bool isRemoveWhenSceneChanged = true)
        {
            if (isRemoveWhenSceneChanged)
            {

            }
            if (RemoveStack(key, value))
            {
                RemoveProjectVolatileKey(key);
                return true;
            }
            return false;
        }

        public static bool GetBool(string name, bool defaultBool)
        {
            return EditorPrefs.GetBool(name, defaultBool);
        }

        public static void SetBool(string name, bool value)
        {
            AddProjectVolatileKey(name);
            EditorPrefs.SetBool(name, value);
        }

        public static void RemoveBool(string name)
        {
            EditorPrefs.DeleteKey(name);
            RemoveProjectVolatileKey(name);
        }

        public static void SetString(string name, string value)
        {
            AddProjectVolatileKey(name);
            EditorPrefs.SetString(name, value);
        }

        public static void InitPrefsCache()
        {
            RemoveAllVolatileKey(false);
        }

        private static void RemoveAllVolatileKey()
        {
            RemoveAllVolatileKey(true);
        }

        private static void RemoveAllVolatileKey(bool isRemoveProjectInitKey)
        {
            if (isRemoveProjectInitKey)
            {
                EditorPrefs.DeleteKey(CWJ_EditorEventHelper.GetEditorPrefs_ProjectInitKey());
            }

            foreach (var keyName in GetStackArray(ProjectVolatilePrefsKey_Root))
            {
                if (string.IsNullOrEmpty(keyName)) continue;
                EditorPrefs.DeleteKey(keyName);
            }

            EditorPrefs.DeleteKey(ProjectVolatilePrefsKey_Root);
            EditorPrefs.DeleteKey(SceneVolatilePrefsKey_Root);
        }
    }
}
#endif