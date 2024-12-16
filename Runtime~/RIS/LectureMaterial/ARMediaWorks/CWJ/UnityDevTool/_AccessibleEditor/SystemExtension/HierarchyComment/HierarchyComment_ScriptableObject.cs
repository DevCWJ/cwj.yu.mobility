#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using CWJ.Serializable;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace CWJ.EditorOnly.Hierarchy.Comment
{
    using static HierarchyCommentExtension;

    public sealed class HierarchyComment_ScriptableObject : Initializable_ScriptableObject
    {
        public override bool IsAutoReset => true;
        public GameObject targetObj { get; set; }
        public bool hasComment { get; private set; }
        public PrefabStage prefabStage { get; private set; }
        public string newCommment { get; set; }
        public string lastComment { get; private set; }
        public bool isInit { get; private set; }

        public bool OnSetCommentBtn()
        {
            if (targetObj == null) return false;

            CWJ.AccessibleEditor.EditorGUI_CWJ.RemoveFocusFromText();

            if (newCommment.RemoveAllSpaces().Length == 0) return false;
            
            var cache = GetCommentCache(targetObj, PrefabStageUtility.GetCurrentPrefabStage());
            if (cache == null) return false;

            cache.AddComment(targetObj, newCommment.AddTimeTag(), hasComment);
            lastComment = newCommment;
            hasComment = true;

            return true;
        }

        public bool OnRemoveCommentBtn()
        {
            if (targetObj == null) return false;

            CWJ.AccessibleEditor.EditorGUI_CWJ.RemoveFocusFromText();

            if (!hasComment) return false;

            var cache = GetCommentCache(targetObj, PrefabStageUtility.GetCurrentPrefabStage());
            if (cache == null) return false;

            cache.RemoveComment(targetObj);
            lastComment = newCommment = string.Empty;
            hasComment = false;

            return true;
        }

        public void ChangeCacheData(GameObject targetObj)
        {
            string comment;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            bool hasComment = GetCommentCache(targetObj, prefabStage).TryGetComment(targetObj, out comment);
            ChangeCacheData(targetObj, hasComment, comment, prefabStage);
        }

        public void ChangeCacheData(GameObject targetObj, bool hasComment, string comment, PrefabStage prefabStage)
        {
            this.targetObj = targetObj;
            this.hasComment = hasComment;
            this.prefabStage = prefabStage;
            this.newCommment = this.lastComment = comment.RemoveTimeTag();
            this.isInit = true;
            SaveScriptableObj();
        }

        public void ClearCache()
        {
            isInit = false;
            targetObj = null;
            newCommment = null;
            lastComment = null;
            hasComment = false;
            prefabStage = null;
            SaveScriptableObj();
        }
    }
} 
#endif