using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ
{
    public static class FindUtil
    {
        //static GameObject[] objectPooler;


        //public static GameObject[] GetRootGameObjects_New(Predicate<GameObject> predicate = null, bool includeDontDestroyObjs = true)
        //{
        //    if (predicate == null)
        //    {
        //        return _GetRootGameObjects_New(includeDontDestroyObjs);
        //    }

        //    _GetRootGameObjects_New(includeDontDestroyObjs).
        //}


        /// <summary>
        /// DontDestroyOnLoad <see cref="GameObject"/>까지 포함해서 반환 (비활성화된건 기본적으로 포함되어있음)
        /// </summary>
        /// <returns></returns>
        public static GameObject[] GetRootGameObjects_New(bool includeDontDestroyObjs)
        {
            int sceneLength = UnityEngine.SceneManagement.SceneManager.sceneCount;
            List<GameObject> rootObjs = new List<GameObject>();
            for (int i = 0; i < sceneLength; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
#if UNITY_EDITOR
                if (!Application.isPlaying && !scene.isLoaded) //!Application.isPlaying가 없으면 싱글톤 관련된것들 때문에 만들고도 못찾아서 무한루프.. StackOverflow됨
                {
                    continue;
                }//좀더 테스트 필요
#endif
                rootObjs.AddRange(scene.GetRootGameObjsOnlyValidScene());
            }

            if (includeDontDestroyObjs)
            {
                rootObjs.AddRange(GetRootObjsOfDontDestroyOnLoad());
            }
#if UNITY_EDITOR
            else
            {
                if(SingletonHelper.HasInstance && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    var dontDestroyOnLoadScene = SingletonHelper.Instance.gameObject.scene;
                    rootObjs = rootObjs.FindAll((o) => o.scene != dontDestroyOnLoadScene);
                }
            }
#endif

            return rootObjs.ToArray();
        }

        public static GameObject[] GetRootObjsOfDontDestroyOnLoad()
        {
            return MonoBehaviourEventHelper.GetIsPlayingBeforeQuit() ? SingletonHelper.Instance.gameObject.scene.GetRootGameObjsOnlyValidScene() : new GameObject[0];
        }

        public static GameObject[] GetRootGameObjsOnlyValidScene(this UnityEngine.SceneManagement.Scene scene)
        {
            return scene.IsValid() ? scene.GetRootGameObjects() : new GameObject[0];
        }

        #region 비활성화되어있는 컴포넌트까지 찾는 FindObjectOfType 상위호환

        public static void CheckIsValidFindDontDestroyObject(Type findType, ref bool includeDontDestroyObj)
        {
            if (!includeDontDestroyObj) return;

            includeDontDestroyObj = MonoBehaviourEventHelper.GetIsPlayingBeforeQuit() && !findType.Equals(typeof(SingletonHelper));
        }

        public static T FindObjectFirst_ForSingleton<T>(Predicate<T> predicate = null) where T : MonoBehaviour
        {
            try
            {
                int sceneLength = UnityEngine.SceneManagement.SceneManager.sceneCount;
            }
            catch (UnityEngine.UnityException)
            {
                Debug.LogError($"{typeof(T).Name} class 의 Awake() 에서 미리 {nameof(Singleton.Core.SingletonCoreAbstract<T>.UpdateInstance)}(); 코드 작성하여 Instance를 생성해주세요.\n 또는 MainThread 콜백으로 처리하는 방법도 있습니다.");
            }
            return FindObjectOfType_New<T>(true, true, predicate);
        }

        public static T[] FindObjects_ForSingleton<T>(Predicate<T> predicate = null) where T : MonoBehaviour
        {
            try
            {
                int sceneLength = UnityEngine.SceneManagement.SceneManager.sceneCount;
            }
            catch (UnityEngine.UnityException)
            {
                Debug.LogError($"{typeof(T).Name} class 의 Awake() 에서 미리 {nameof(Singleton.Core.SingletonCoreAbstract<T>.UpdateInstance)}(); 코드 작성하여 Instance를 생성해주세요.\n 또는 MainThread 콜백으로 처리하는 방법도 있습니다.");
            }
            return FindObjectsOfType_New<T>(true, true, predicate);
        }

        /// <summary>
        /// 비활성화 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>와 DontDestroyOnLoad 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>까지도 검색가능
        /// </summary>
        public static T[] FindObjectsOfType_New<T>(bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<T> predicate = null) where T : UnityObject
        {
            if(typeof(T).Equals(typeof(GameObject)))
            {
                return (FindObjectsOfType_New<Transform>(includeInactive, includeDontDestroyOnLoadObjs).ConvertAll(t => t.gameObject)
                                                                                                       .FindAll(predicate as Predicate<GameObject>) as T[]);
            }
            CheckIsValidFindDontDestroyObject(typeof(T), ref includeDontDestroyOnLoadObjs);

            List<T> findList = new List<T>();

            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive)
            {
                findList.AddRange(UnityObject.FindObjectsOfType<T>());
            }
            else
            {
                foreach (var rootGo in GetRootGameObjects_New(includeDontDestroyOnLoadObjs))
                {
                    if (!includeInactive && !rootGo.activeInHierarchy) continue;
                    var comps = rootGo.GetComponentsInChildren<T>(includeInactive);
                    if (comps != null && comps.Length > 0)
                    {
                        findList.AddRange(comps);
                    }
                }
            }

            T[] finds = findList.ToArray();

            return (predicate == null) ? finds : finds.FindAll(predicate);
        }

        /// <summary>
        /// 비활성화 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>와 DontDestroyOnLoad 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>까지도 검색가능
        /// </summary>
        public static T FindObjectOfType_New<T>(bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<T> predicate = null) where T : UnityObject
        {
            if (typeof(T).Equals(typeof(GameObject)))
            {
                return (FindObjectsOfType_New<Transform>(includeInactive, includeDontDestroyOnLoadObjs).ConvertAll(t => t.gameObject).Find(predicate as Predicate<GameObject>) as T);
            }
            CheckIsValidFindDontDestroyObject(typeof(T), ref includeDontDestroyOnLoadObjs);

            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive && predicate == null)
            {
                return UnityObject.FindObjectOfType<T>();
            }
            else
            {
                foreach (var rootGo in GetRootGameObjects_New(includeDontDestroyOnLoadObjs))
                {
                    if (!includeInactive && !rootGo.activeInHierarchy) continue;

                    T comp = null;

                    if (predicate != null)
                    {
                        T[] comps = rootGo.GetComponentsInChildren<T>(includeInactive);
                        if (comps.Length > 0)
                        {
                            comp = comps.Find(predicate);
                        }
                    }
                    else
                    {
                        comp = rootGo.GetComponentInChildren<T>(includeInactive);
                    }

                    if (comp != null) return comp;
                }
            }

            return null;
        }

        /// <summary>
        /// 비활성화 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>와 DontDestroyOnLoad 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>까지도 검색가능
        /// </summary>
        public static UnityObject[] FindObjectsOfType_NonGeneric(Type findType, bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<UnityObject> predicate = null)
        {
            if (findType.Equals(typeof(GameObject)))
            {
                return FindObjectsOfType_New<Transform>(includeInactive, includeDontDestroyOnLoadObjs).ConvertAll(t=>t.gameObject).FindAll(predicate as Predicate<GameObject>);
            }
            CheckIsValidFindDontDestroyObject(findType, ref includeDontDestroyOnLoadObjs);

            List<UnityObject> findList = new List<UnityObject>();

            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive)
            {
                findList.AddRange(UnityObject.FindObjectsOfType(findType));
            }
            else
            {
                foreach (var rootGo in GetRootGameObjects_New(includeDontDestroyOnLoadObjs))
                {
                    if (!includeInactive && !rootGo.activeInHierarchy) continue;
                    UnityObject[] objects = rootGo.GetComponentsInChildren(findType, includeInactive);
                    if (objects != null && objects.Length > 0)
                    {
                        findList.AddRange(objects);
                    }
                }
            }

            UnityObject[] finds = findList.ToArray();

            return predicate == null ? finds : finds.FindAll(predicate);
        }

        /// <summary>
        /// 비활성화 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>와 DontDestroyOnLoad 되어있는 <see cref="GameObject"/>의 <see cref="Component"/>까지도 검색가능
        /// </summary>
        public static UnityObject FindObjectOfType_NonGeneric(Type findType, bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<UnityObject> predicate = null)
        {
            if (findType.Equals(typeof(GameObject)))
            {
                return FindObjectsOfType_New<Transform>(includeInactive, includeDontDestroyOnLoadObjs).ConvertAll(t => t.gameObject).Find(predicate as Predicate<GameObject>);
            }
            CheckIsValidFindDontDestroyObject(findType, ref includeDontDestroyOnLoadObjs);

            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive)
            {
                return predicate == null ? UnityObject.FindObjectOfType(findType) : UnityObject.FindObjectsOfType(findType).Find(predicate);
            }
            else
            {
                if (predicate == null)
                {
                    foreach (var rootGo in GetRootGameObjects_New(includeDontDestroyOnLoadObjs))
                    {
                        if (!includeInactive && !rootGo.activeInHierarchy) continue;
                        Component component = rootGo.GetComponentInChildren(findType, includeInactive);
                        if (component != null)
                            return component;
                    }
                }
                else
                {
                    return FindObjectsOfType_NonGeneric(findType, includeInactive, includeDontDestroyOnLoadObjs).Find(predicate);
                }
            }

            return null;
        }

        #endregion 비활성화되어있는 컴포넌트까지 찾는 FindObjectOfType 상위호환

        #region GetComponent 확장메소드
        //이렇게하면 매개변수에 던지는순간에 애초에 b가 실행된후 들어온다는점을 잊지말자
        //public static T ReturnAOrB<T>(this T a, T b) where T : Component
        //{
        //    if (a != null) return a; else return b;
        //}

        /// <summary>
        /// GetComponent() ?? AddComponent(); 는 유니티에디터에서 오류가 나는 문제때문에 만들게되었음
        /// <para>빌드후에는 상관없는데 에디터에서는 GetComponent를 했을때 NULL일 경우 <see langword="null"/>이 아닌 문자형 "null"이 반환되기때문</para>
        /// https://gamedev.stackexchange.com/questions/155384/why-does-component-getcomponentrigidbody-return-null-when-a-rigid-body-is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T comp = gameObject.GetComponent<T>();
            if (comp)
                return comp;

            return gameObject.AddComponent<T>();
        }

        public static T AddComponentInChild<T>(this GameObject gameObject, bool isCreateNewChild = true) where T : Component
        {
            if (isCreateNewChild || gameObject.transform.childCount == 0)
            {
                var newObj = new GameObject(typeof(T).Name);
                newObj.transform.SetParentAndReset(gameObject.transform);
                newObj.transform.SetAsFirstSibling();
                return newObj.AddComponent<T>();
            }

            return gameObject.transform.GetChild(0).gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponentInChild<T>(this GameObject gameObject, bool isCreateNewChild = true) where T : Component
        {
            T comp = gameObject.GetComponentInChildren<T>(true);
            if (comp)
                return comp;
            return gameObject.AddComponentInChild<T>(isCreateNewChild);
        }

        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component comp = gameObject.GetComponent(type);
            if (comp)
                return comp;
            return gameObject.AddComponent(type);
        }

        public static T GetOrAddComponent_New<T>(this Transform transform) where T : Component => GetOrAddComponent<T>(transform.gameObject);

        public static void GetAndRemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (typeof(T).Equals(typeof(Transform))) return;
            T comp = gameObject.GetComponent<T>();
            if (comp) Component.Destroy(comp);
        }
        public static void GetAndRemoveComponent<T>(this Transform transform) where T : Component => GetAndRemoveComponent<T>(transform.gameObject);

        public static bool TryGetComponentInParent_New<T>(this Component comp, out T result, out GameObject resultTargetObj)
        {
            return TryGetComponentInParent_New<T>(comp.gameObject, out result, out resultTargetObj);
        }

        public static bool TryGetComponentInParent_New<T>(this GameObject gameObject, out T result, out GameObject resultTargetObj)
        {
            Transform transform = gameObject.transform;
            do
            {
                if (transform.TryGetComponent(out result))
                {
                    resultTargetObj = transform.gameObject;
                    return true;
                }

                transform = transform.parent;
            }
            while (transform);

            result = default(T);
            resultTargetObj = null;
            return false;
        }

        public static T[] GetComponentsInParent_New<T>(this Transform transform, bool includeInactive = false, bool isWithoutMe = false, Predicate<T> predicate = null) where T : class
        {
            Type t = typeof(T);
            if (t == null || (!t.IsInterface && !typeof(Component).IsAssignableFrom(t)))
            {
                return new T[0];
            }

            if (isWithoutMe)
            {
                if (transform.root == transform)
                {
                    return new T[] { };
                }
                else
                    transform = transform.parent;
            }

            T[] interfaces = transform.GetComponentsInParent<T>(includeInactive);
            if (predicate != null)
            {
                interfaces = interfaces.FindAll(predicate);
            }

            return interfaces;
        }

        public static T[] GetComponentsInParent_New<T>(this Transform[] transforms, bool includeInactive = false, bool isWithoutMe = false, Predicate<T> predicate = null) where T : class
        {
            Type t = typeof(T);
            if (t == null || (!t.IsInterface && !typeof(Component).IsAssignableFrom(t)))
            {
                return new T[0];
            }
            List<T> components = new List<T>();

            for (int i = 0; i < transforms.Length; i++)
            {
                components.AddRange(transforms[i].GetComponentsInParent_New<T>(includeInactive: includeInactive, isWithoutMe: isWithoutMe, predicate: predicate));
            }

            return components.ToArray();
        }

        public static T[] GetComponentsInChildren_New<T>(this Transform transform, bool includeInactive = false, bool isWithoutMe = false, Predicate<T> predicate = null) where T : class
        {
            Type t = typeof(T);
            if (t == null || (!t.IsInterface && !typeof(Component).IsAssignableFrom(t)))
            {
                return new T[0];
            }

            var childComponents = transform.GetComponentsInChildren<T>(includeInactive);

            if (predicate != null)
            {
                childComponents = childComponents.FindAll(predicate);
            }

            if (isWithoutMe && childComponents.Length > 0 && typeof(Component).IsAssignableFrom(t) && (childComponents[0] as Component).transform == transform)
            {
                var compList = childComponents.ToList();
                compList.RemoveAt(0);
                childComponents = compList.ToArray();
            }

            return childComponents;
        }

        public static T[] GetComponentsInChildren_New<T>(this Transform[] transforms, bool includeInactive = false, bool isWithoutMe = false, Predicate<T> predicate = null) where T : class
        {
            Type t = typeof(T);
            if (t == null || (!t.IsInterface && !typeof(Component).IsAssignableFrom(t)))
            {
                return new T[0];
            }
            List<T> components = new List<T>();

            for (int i = 0; i < transforms.Length; i++)
            {
                components.AddRange(transforms[i].GetComponentsInChildren_New(includeInactive: includeInactive, isWithoutMe: isWithoutMe, predicate: predicate));
            }

            return components.ToArray();
        }

        #endregion GetComponent 확장메소드

        #region FindObjectOfType_New GameObject 검색 버전

        /// <summary>
        /// 비활성화 혹은 DontDestroyOnLoad 되어있는 <see cref="GameObject"/> 배열 검색가능
        /// </summary>
        public static GameObject[] FindGameObjects(bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<GameObject> predicate = null)
        {
            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive)
            {
                GameObject[] objs = UnityObject.FindObjectsOfType(typeof(GameObject)).ConvertObjects<GameObject>();
                return predicate == null ? objs : objs.FindAll(predicate);
            }
            else
            {
                GameObject[] rootObjs = GetRootGameObjects_New(includeDontDestroyOnLoadObjs);

                List<UnityObject> findList = new List<UnityObject>();

                for (int i = 0; i < rootObjs.Length; i++)
                {
                    if (!includeInactive && !rootObjs[i].activeInHierarchy) continue;
                    UnityObject[] objs = rootObjs[i].GetComponentsInChildren(typeof(Transform), includeInactive);
                    if (objs != null && objs.Length > 0)
                    {
                        findList.AddRange(objs);
                    }
                }

                GameObject[] finds = findList.ToArray().ConvertObjects<Transform>().ConvertAll((t) => t.gameObject);

                return predicate == null ? finds : finds.FindAll(predicate);
            }
        }

        /// <summary>
        /// 비활성화 혹은 DontDestroyOnLoad 되어있는 <see cref="GameObject"/> 검색가능
        /// </summary>
        public static GameObject FindGameObject(bool includeInactive, bool includeDontDestroyOnLoadObjs, Predicate<GameObject> predicate = null)
        {
            if (!Application.isEditor && !includeDontDestroyOnLoadObjs && !includeInactive && predicate == null)
            {
                return UnityObject.FindObjectOfType(typeof(GameObject)) as GameObject;
            }
            else
            {
                List<GameObject> findList = new List<GameObject>();

                GameObject[] rootObjs = GetRootGameObjects_New(includeDontDestroyOnLoadObjs);

                for (int i = 0; i < rootObjs.Length; i++)
                {
                    if (!includeInactive && !rootObjs[i].activeInHierarchy) continue;
                    Transform trf = rootObjs[i].GetComponentInChildren(typeof(Transform), includeInactive) as Transform;
                    if (trf != null)
                    {
                        if (predicate == null || predicate(trf.gameObject))
                        {
                            return trf.gameObject;
                        }
                    }
                }
                return null;
            }
        }

#endregion FindObjectOfType_New GameObject 검색 버전

        #region FindObjectOfType_New Interface 검색 버전

        public static TI[] FindInterfaces<TI>(bool includeInactive = false, bool includeDontDestroyOnLoadObjs = true, Predicate<TI> predicate = null) where TI : class
        {
            SerializableInterfaceUtil.ThrowIfNotInterfaceException(typeof(TI));

            List<TI> findList = new List<TI>();

            GameObject[] rootObjs = GetRootGameObjects_New(includeDontDestroyOnLoadObjs);

            Action<TI[]> AddInterfaces = null;
            if (predicate == null)
            {
                AddInterfaces = findList.AddRange;
            }
            else
            {
                AddInterfaces = (addList) =>
                {
                    for (int i = 0; i < addList.Length; i++)
                    {
                        if (predicate(addList[i]))
                        {
                            findList.Add(addList[i]);
                        }
                    }
                };
            }
            for (int i = 0; i < rootObjs.Length; i++)
            {
                if (!includeInactive && !rootObjs[i].activeSelf) continue;

                AddInterfaces(rootObjs[i].GetComponentsInChildren<TI>(includeInactive));
            }

            return findList.ToArray();
        }

        public static TI FindInterface<TI>(bool includeInactive = false, bool includeDontDestroyOnLoadObjs = true, Predicate<TI> predicate = null) where TI : class
        {
            return FindInterfaces<TI>(includeInactive: includeInactive, includeDontDestroyOnLoadObjs: includeDontDestroyOnLoadObjs, predicate: predicate).FirstOrDefault();
        }

        #endregion FindObjectOfType_New Interface 검색 버전
    }
}
