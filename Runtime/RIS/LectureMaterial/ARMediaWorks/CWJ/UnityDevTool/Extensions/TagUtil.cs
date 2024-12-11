using System;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

using UnityEditorInternal;

#endif

using CWJ.AccessibleEditor;

namespace CWJ
{
    public static class TagUtil
    {
        #region 편한 배열 태그체크

        public static Transform CompareTag(this List<Transform> transformList, string tag)
        {
            return transformList.Find(trf => trf.CompareTag(tag));
        }

        public static Transform[] CompareTags(this List<Transform> transformList, string tag)
        {
            return transformList.FindAll(trf => trf.CompareTag(tag)).ToArray();
        }

        public static Transform CompareTag(this Transform[] transformArray, string tag)
        {
            return transformArray.Find(trf => trf.CompareTag(tag));
        }

        public static Transform[] CompareTags(this Transform[] transformArray, string tag)
        {
            return transformArray.FindAll(trf => trf.CompareTag(tag));
        }

        #endregion 편한 배열 태그체크

        public static void SetTag(this GameObject obj, string tagName, bool isRecursively = false, bool isTagNeedExistsCheck = true)
        {
            if (isTagNeedExistsCheck && !TagEditorUtil.IsExists(tagName, false))
            {
                TagEditorUtil.AddNewTag(tagName);
            }

            if (!isRecursively)
            {
                obj.tag = tagName;
            }
            else
            {
                SetTagRecursively(obj.transform, tagName);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }

        private static void SetTagRecursively(Transform rootTrf, string tagName)
        {
            rootTrf.gameObject.tag = tagName;
            foreach (Transform child in rootTrf)
            {
                SetTagRecursively(child, tagName);
            }
        }
    }

    public static class TagEditorUtil
    {
        #region <Tag> ContainsTagList, AddTagList

        public static bool IsExists(string tagName, bool printLog = true)
        {
#if UNITY_EDITOR
            for (int i = 0; i < InternalEditorUtility.tags.Length; i++)
            {
                if (InternalEditorUtility.tags[i].Equals(tagName))
                {
                    if (printLog)
                    {
                        typeof(TagEditorUtil).PrintLogWithClassName("태그리스트에 '" + tagName + "' 가 존재합니다");
                    }

                    return true;
                }
            }
            if (printLog)
            {
                typeof(TagEditorUtil).PrintLogWithClassName("태그리스트에 '" + tagName + "' 가 존재하지 않습니다");
            }
            return false;
#else
            return true;
#endif
        }

        private const int maxTagLength = 10000;

        /// <summary>
        /// ExecuteInEditMode이여야지 태그가 추가된것이 저장이됨(Play 중일때 추가되어도 저장이 안되므로 주의)
        /// Reset에서 해주면 딱좋음
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static bool AddNewTag(string tagName, bool isPrintLog = true)
        {
#if UNITY_EDITOR
            if (IsExists(tagName, false))
            {
                if (isPrintLog) typeof(LayerMask).PrintLogWithClassName("Tag list에 '" + tagName + "' 가 이미 존재해서 추가하지 않았습니다.", LogType.Log);

                return true;
            }

            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty tagsProp = manager.FindProperty("tags");
            if (tagsProp.arraySize >= maxTagLength)
            {
                if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Tag list가 가득차서 '" + tagName + "' 를 추가하지 못했습니다.", isError: true);
                return false;
            }

            int index = tagsProp.arraySize;

            tagsProp.InsertArrayElementAtIndex(index);
            SerializedProperty sp = tagsProp.GetArrayElementAtIndex(index);

            sp.stringValue = tagName;
            if (Application.isPlaying) //
            {
                if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Tag list에 '" + tagName + "' 를 일시적으로 추가했습니다.\n(저장되지는 않았습니다 실행중이 아닌 에디터모드에서만 저장됩니다)", isError: true);
            }
            else
            {
                if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Tag list에 '" + tagName + "' 추가를 성공했습니다.");
            }

            manager.ApplyModifiedProperties();
            return true;
#else
            return false;
#endif
        }

        #endregion <Tag> ContainsTagList, AddTagList
    }
}