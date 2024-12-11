using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CWJ
{
    public static class GameObjectUtil
    {
        public static bool IsNull(this Scene scene)
        {
            return scene == null || scene.name == null;
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
                var rootObj = FindUtil.GetRootGameObjects_New(false)?.Find(g => g.GetComponent<Canvas>() == null);
                if(rootObj == null)
                {
                    rootObj = new GameObject();
                }
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

        public static bool IsDontDestroyOnLoad(this GameObject gameObject)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return SingletonHelper.Instance.gameObject.scene == gameObject.scene;
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
            if (parent == null || possibleChild == null) return false;
            return possibleChild.transform.IsChildOf(parent.transform);
        }

        public static bool IsParentOf(this Transform parent, GameObject possibleChild)
        {
            if (parent == null || possibleChild == null) return false;
            return possibleChild.transform.IsChildOf(parent);
        }

        public static bool IsParentOf(this GameObject parent, Transform possibleChild)
        {
            if (parent == null || possibleChild == null) return false;
            return possibleChild.IsChildOf(parent.transform);
        }

        public static bool IsParentOf(this Transform parent, Transform possibleChild)
        {
            if (parent == null || possibleChild == null) return false;
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
            while (curParent != null)
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
        //    if (child == null) throw new System.ArgumentNullException(nameof(child));

        //    if (child.parent == obj) return;
        //    child.parent = obj;
        //}

        ///// <summary>
        ///// Sets the parent property of this GameObject to null.
        ///// </summary>
        ///// <param name="obj"></param>
        //public static void RemoveFromParent(this GameObject obj)
        //{
        //    if (obj == null) throw new System.ArgumentNullException(nameof(obj));

        //    var t = obj.transform;
        //    if (t.parent == null) return;
        //    t.parent = null;
        //}

        ///// <summary>
        ///// Sets the parent property of this GameObject to null.
        ///// </summary>
        ///// <param name="obj"></param>
        //public static void RemoveFromParent(this Transform obj)
        //{
        //    if (obj == null) throw new System.ArgumentNullException(nameof(obj));

        //    if (obj.parent == null) return;
        //    obj.parent = null;
        //}
    }
}