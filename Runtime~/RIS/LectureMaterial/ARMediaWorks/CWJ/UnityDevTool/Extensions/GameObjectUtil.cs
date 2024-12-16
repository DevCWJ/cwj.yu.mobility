using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CWJ
{
    public static class GameObjectUtil
    {
        public static bool IsNull(this Scene scene)
        {
            return !scene.IsValid() || scene.handle == 0;
        }

        public static void SetParentNull(bool worldpositionStays, bool isDontDestroyOnLoad, params Transform[] transforms)
        {
            Transform tmpParent = null;
            if (isDontDestroyOnLoad)
            {
                tmpParent = SingletonHelper.Instance.transform;
            }
            else
            {
                var rootObj = FindUtil.GetRootGameObjects_New(false)?.FirstOrDefault(g => !g.GetComponent<Canvas>());
                if (!rootObj)
                    rootObj = new GameObject();
                tmpParent = rootObj.transform;
            }

            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].SetParent(tmpParent, true);
                transforms[i].SetParent(null, worldpositionStays);
            }
        }

        public static void SetParentNull(this Transform transform, bool worldpositionStays, bool isDontDestroyOnLoad)
        {
            SetParentNull(worldpositionStays, isDontDestroyOnLoad, transform);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetObj"></param>
        /// <returns>SetDontDestroySafety가 실제로 적용된 root object</returns>
        public static GameObject SetDontDestroySafety(GameObject targetObj)
        {
            if (!targetObj || !MonoBehaviourEventHelper.GetIsPlayingBeforeQuit())
            {
                return null;
            }

            var root = targetObj.transform.root;
            if (!root)
                root = targetObj.transform;
            var rootObj = root.gameObject;
#if UNITY_EDITOR
            bool isHidden = UnityEditor.SceneVisibilityManager.instance.IsHidden(rootObj);
            if (isHidden)
                UnityEditor.SceneVisibilityManager.instance.Show(rootObj, false);
#endif
            UnityEngine.Object.DontDestroyOnLoad(rootObj);
#if UNITY_EDITOR
            if (isHidden)
                UnityEditor.SceneVisibilityManager.instance.Hide(rootObj, false);
#endif
            return rootObj;
        }

        public static bool IsDontDestroyOnLoad(this GameObject gameObject)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return SingletonHelper.Instance.gameObject.scene.handle == gameObject.scene.handle;
            else
                return false;
#else
             return gameObject.scene.buildIndex == -1;
#endif
        }

        public static bool IsGameObjectSource(object obj)
        {
            return (obj is GameObject || obj is Component);
        }

        public static IEnumerable<Transform> IterateAllChildren(this Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                yield return trans.GetChild(i);
            }

            for (int i = 0; i < trans.childCount; i++)
            {
                foreach (var c in IterateAllChildren(trans.GetChild(i)))
                    yield return c;
            }
        }

        public static List<Transform> GetAllChildren(this Transform rootTrf)
        {
            List<Transform> children = new List<Transform>();
            if (rootTrf.transform.childCount > 0)
            {
                foreach (Transform child in rootTrf.transform)
                {
                    AddChildren(children, child);
                }
            }
            return children;
        }

        private static void AddChildren(List<Transform> list, Transform rootTrf)
        {
            list.Add(rootTrf);
            if (rootTrf.transform.childCount > 0)
            {
                foreach (Transform child in rootTrf.transform)
                {
                    AddChildren(list, child);
                }
            }
        }

        public static bool IsParentOf(this GameObject parent, GameObject possibleChild)
        {
            if (!parent || !possibleChild) return false;
            return possibleChild.transform.IsChildOf(parent.transform);
        }

        public static bool IsParentOf(this Transform parent, GameObject possibleChild)
        {
            if (!parent || !possibleChild) return false;
            return possibleChild.transform.IsChildOf(parent);
        }

        public static bool IsParentOf(this GameObject parent, Transform possibleChild)
        {
            if (!parent || !possibleChild) return false;
            return possibleChild.IsChildOf(parent.transform);
        }

        public static bool IsParentOf(this Transform parent, Transform possibleChild)
        {
            if (!parent || !possibleChild) return false;
            /*
             * Since implementation of this, Unity has since added 'IsChildOf' that is far superior in efficiency
             *
            while (possibleChild != null)
            {
                if (parent == possibleChild.parent) return true;
                possibleChild = possibleChild.parent;
            }
            return false;
            */

            return possibleChild.IsChildOf(parent);
        }

        /// <summary>
        /// find directly parents
        /// </summary>
        /// <param name="childTrf"></param>
        /// <returns></returns>
        public static Transform[] GetAllParents(this Transform childTrf)
        {
            Transform curParent = childTrf.parent;
            List<Transform> parents = new List<Transform>();
            while (curParent)
            {
                parents.Add(curParent);
                curParent = curParent.parent;
            }
            return parents.ToArray();
        }

        ///// <summary>
        ///// Set the parent of some GameObject to this GameObject.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="child"></param>
        ///// <param name="suppressChangeHierarchyMessage"></param>
        //public static void AddChild(this GameObject obj, GameObject child)
        //{
        //    var p = (obj != null) ? obj.transform : null;
        //    var t = (child != null) ? child.transform : null;
        //    AddChild(p, t);
        //}

        ///// <summary>
        ///// Set the parent of some GameObject to this GameObject.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="child"></param>
        //public static void AddChild(this GameObject obj, Transform child)
        //{
        //    var p = (obj != null) ? obj.transform : null;
        //    AddChild(p, child);
        //}

        ///// <summary>
        ///// Set the parent of some GameObject to this GameObject.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="child"></param>
        //public static void AddChild(this Transform obj, GameObject child)
        //{
        //    var t = (child != null) ? child.transform : null;
        //    AddChild(obj, t);
        //}

        ///// <summary>
        ///// Set the parent of some GameObject to this GameObject.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="child"></param>
        //public static void AddChild(this Transform obj, Transform child)
        //{
        //    if (!child) throw new System.ArgumentNullException(nameof(child));

        //    if (child.parent == obj) return;
        //    child.parent = obj;
        //}

        ///// <summary>
        ///// Sets the parent property of this GameObject to null.
        ///// </summary>
        ///// <param name="obj"></param>
        //public static void RemoveFromParent(this GameObject obj)
        //{
        //    if (!obj) throw new System.ArgumentNullException(nameof(obj));

        //    var t = obj.transform;
        //    if (!t.parent) return;
        //    t.parent = null;
        //}

        ///// <summary>
        ///// Sets the parent property of this GameObject to null.
        ///// </summary>
        ///// <param name="obj"></param>
        //public static void RemoveFromParent(this Transform obj)
        //{
        //    if (!obj) throw new System.ArgumentNullException(nameof(obj));

        //    if (!obj.parent) return;
        //    obj.parent = null;
        //}
    }
}
