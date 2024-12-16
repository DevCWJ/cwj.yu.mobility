#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

using CWJ.Serializable;
using CWJ.AccessibleEditor;
using UnityEditor.Experimental.SceneManagement;


namespace CWJ.EditorOnly.Hierarchy.Comment
{
    using static HierarchyCommentExtension;

    public static class HierarchyCommentExtension
    {
        public static HierarchyCommentCache GetPrefabInsCommentCache(GameObject go)
        {
            if (go == null) return null;
            return go.transform.GetComponentsInChildren_New<HierarchyCommentCache>(predicate: x => x.isInPrefabObj).FirstOrDefault();
        }

        public static HierarchyCommentCache GetCommentCache(GameObject go, PrefabStage prefabStage = null)
        {
            if (go == null || go.Equals(null)) return null;
            return GetCommentCache(go.scene, prefabStage);
        }

        public static HierarchyCommentCache GetCommentCache(Scene scene, PrefabStage prefabStage = null, bool isAutoCreate = true)
        {
            if (prefabStage == null && !scene.isLoaded)
            {
                return null;
            }
            HierarchyCommentCache cache;
            if (prefabStage != null)
            {
                cache = prefabStage.prefabContentsRoot.transform.GetComponentsInChildren_New<HierarchyCommentCache>(predicate: x => x.isInPrefabObj).FirstOrDefault();
            }
            else
            {
                cache = CWJ.FindUtil.FindObjectOfType_New<HierarchyCommentCache>
                        (false, false, predicate: (x) => x.gameObject != null && x.gameObject.scene.Equals(scene) && !x.isInPrefabObj);
            }

            if (isAutoCreate && cache == null)
            {
                if (prefabStage != null)
                {
                    cache = new GameObject(nameof(HierarchyCommentCache), typeof(HierarchyCommentCache)).GetComponent<HierarchyCommentCache>();
                    cache.isInPrefabObj = true;
                    cache.transform.SetParent(prefabStage.prefabContentsRoot.transform);
                    //string prefabPath = prefabStage.prefabAssetPath;
                    //var prefabRootObj = PrefabUtility.LoadPrefabContents(prefabPath);
                    //try
                    //{
                    //    cache = new GameObject(nameof(HierarchyCommentCache), typeof(HierarchyCommentCache)).GetComponent<HierarchyCommentCache>();
                    //    cache.transform.SetParent(prefabRootObj.transform);
                    //    //PrefabUtility.ApplyAddedGameObject(prefabRootObj, prefabPath, InteractionMode.AutomatedAction);
                    //    PrefabUtility.SaveAsPrefabAsset(prefabRootObj, prefabPath);
                    //}
                    //finally
                    //{
                    //    PrefabUtility.UnloadPrefabContents(prefabRootObj);
                    //}
                }
                else
                {
                    cache = new GameObject(nameof(HierarchyCommentCache), typeof(HierarchyCommentCache)).GetComponent<HierarchyCommentCache>();
                    cache.isInPrefabObj = false;
                    SceneManager.MoveGameObjectToScene(cache.gameObject, scene);
                }
            }

            return cache;
        }

        private const string TimeTag_Start = "\n（Date : ";
        private const string TimeTag_End = "）";


        public static string RemoveTimeTag(this string comment)
        {
            if (comment.Contains(TimeTag_Start))
            {
                comment = comment.Split(new string[] { TimeTag_Start }, StringSplitOptions.None)[0];
            }
            return comment;
        }

        public static string AddTimeTag(this string comment)
        {
            return comment + $"{TimeTag_Start}{DateTime.Now}{TimeTag_End}";
        }
    }

    [DisallowMultipleComponent]
    public class HierarchyCommentCache : MonoBehaviour
    {        
        public static readonly HideFlags GetCacheHideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInBuild;

        //[InitializeOnLoadMethod] //disabled 211026
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += SceneCacheRemoveNullKey;
            PrefabStage.prefabStageOpened += PrefabCacheRemoveNullKey;
            CWJ.AccessibleEditor.CWJ_EditorEventHelper.EditorSaveModifiedEvent += OnEditorSaveModifiedEvent;
        }

        private static void PrefabCacheRemoveNullKey(PrefabStage prefabStage)
        {
            if (prefabStage == null) return;
            var cache = GetCommentCache(prefabStage.prefabContentsRoot, prefabStage);
            if (cache != null)
            {
                cache.RemoveNullKey();
            }
        }

        private static void SceneCacheRemoveNullKey(Scene scene)
        {
            if (scene == null) return;

            var cache = GetCommentCache(scene, isAutoCreate: false);
            if (cache != null)
            {
                cache.RemoveNullKey();
            }
        }

        private static void OnEditorSaveModifiedEvent(CWJ_EditorEventHelper.SaveTarget saveTarget, string[] modifiedObjsPath)
        {
            if (saveTarget == CWJ_EditorEventHelper.SaveTarget.Scene)
            {
                foreach (var scene in modifiedObjsPath.ConvertAll(p => EditorSceneManager.GetSceneByPath(p)))
                {
                    if (scene != null)
                    {
                        SceneCacheRemoveNullKey(scene);
                    }
                }
            }
            else
            {
                PrefabCacheRemoveNullKey(PrefabStageUtility.GetCurrentPrefabStage());
            }
        }

        //InstanceID는 씬전환시 최신화되기때문에 어쩔수없이 GameObject 사용
        //[SerializeField] private DictionaryStoreGameObjectString commentCacheStore = DictionaryStoreGameObjectString.New<DictionaryStoreGameObjectString>();
        public Dictionary<GameObject, string> cacheDic = new Dictionary<GameObject, string>();
        //{
        //    get => commentCacheStore.dictionary;
        //    set => commentCacheStore.dictionary = value;
        //}
        // TODO Dictionary_Serializable

        [SerializeField, Readonly] bool isInit = false;
        [Readonly] public bool isInPrefabObj;

        private void Reset()
        {
            Initialized();
        }

        private void OnValidate()
        {
            SetHideFlags();
            if (!isInit)
            {
                Initialized();
            }
        }

        private void SetHideFlags()
        {
            if (gameObject.hideFlags == HideFlags.None)
                gameObject.hideFlags = GetCacheHideFlags;
        }

        private void Initialized()
        {
            gameObject.SetActive(true); //for easy to find
            SetHideFlags();
            isInit = true;
            SetDirty();
        }

        public bool TryGetComment(GameObject go, out string comment)
        {
            bool hasComment = cacheDic.ContainsKey(go);
            comment = hasComment ? cacheDic[go] : string.Empty;

            return hasComment;
        }

        /// <summary>
        /// Should only be used when after use TryGetComment()
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="hasComment"></param>
        public void AddComment(GameObject go, string comment, bool hasComment)
        {
            if (!hasComment)
            {
                cacheDic.Add(go, comment);
            }
            else
            {
                cacheDic[go] = comment;
            }

            SetDirty();
            PintObj(go);
        }

        /// <summary>
        /// Should only be used when after use TryGetComment()
        /// </summary>
        /// <param name="instanceID"></param>
        public void RemoveComment(GameObject go)
        {
            cacheDic.Remove(go);
            SetDirty();
            PintObj(go);
        }

        private void SetDirty()
        {
            //EditorSceneManager.SaveScene(gameObject.scene);
            EditorUtility.SetDirty(this);
        }

        private void PintObj(GameObject go)
        {
            EditorGUIUtility.PingObject(go);
        }

        public void RemoveNullKey()
        {
            if (cacheDic == null || cacheDic.Count == 0)
            {
                return;
            }
            
            int cnt = cacheDic.Count;

            cacheDic = cacheDic.Where(f => f.Key != null && !f.Key.Equals(null)).ToDictionary(x => x.Key, x => x.Value);

            if (cnt != cacheDic.Count) SetDirty();
        }
    }
}

#endif