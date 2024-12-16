using System.Collections.Generic;
using CWJ.Singleton;
using UnityEngine;
using System;
using System.Linq;
using CWJ.Singleton.Core;

namespace CWJ
{
    [DefaultExecutionOrder(-31999)]
    public class SingletonHelper : SingletonBehaviourDontDestroy<SingletonHelper>, IDontPrecreatedInScene
    {
        public static readonly Type[] BackendSingletonTypes =
        {
            typeof(SingletonHelper),
            typeof(MonoBehaviourEventHelper),
            typeof(CWJ.AccessibleEditor.DebugSetting.UnityDevConsoleVisible)
        };


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            UpdateInstance();
        }
#if UNITY_EDITOR
        [VisualizeField, Readonly] private static List<MonoBehaviour> _AllSingletonObjs = new List<MonoBehaviour>();
#endif
        private static Dictionary<int, List<MonoBehaviour>>
            _AllSingletonObjDict = new Dictionary<int, List<MonoBehaviour>>();

        public static void AddSingletonCache<T>(int typeHeshCode, T singletonClassObj) where T : MonoBehaviour
        {
            if (!_AllSingletonObjDict.TryGetValue(typeHeshCode, out List<MonoBehaviour> objList))
                _AllSingletonObjDict.Add(typeHeshCode, objList = new List<MonoBehaviour>());

            objList.Add(singletonClassObj);

#if UNITY_EDITOR
            _AllSingletonObjs.Add(singletonClassObj);
#endif
        }

        public static void RemoveSingletonCache<T>(int typeHeshCode, T singletonClassObj) where T : MonoBehaviour
        {
            if (!_AllSingletonObjDict.TryGetValue(typeHeshCode, out List<MonoBehaviour> objList))
            {
                return;
            }

            bool isRemoved = objList.Remove(singletonClassObj);
#if UNITY_EDITOR
            if (isRemoved)
                _AllSingletonObjs.Remove(singletonClassObj);
#endif
        }

        public static bool IsFoundAllSingleton = false;

        public static bool TryGetSingletonCache<T>(int typeHeshCode, out T[] cacheList) where T : MonoBehaviour
        {
            if (_AllSingletonObjDict.TryGetValue(typeHeshCode, out var cacheMonoList))
            {
                cacheList = cacheMonoList.GetItemsOfType_UnityObj<MonoBehaviour, T>();
            }
            else
            {
                cacheList = Array.Empty<T>();
            }

            return false;
        }


        public static void AddRangeSingletonCache<T>(int typeHeshCode, T[] cacheList) where T : MonoBehaviour
        {
            if (!_AllSingletonObjDict.TryGetValue(typeHeshCode, out var existsCacheList))
                _AllSingletonObjDict.Add(typeHeshCode, existsCacheList = new List<MonoBehaviour>(cacheList.Length));

            existsCacheList.AddRange(cacheList);
        }


        [VisualizeField, Readonly] private static List<MonoBehaviour> _InstancedSingletonObjs = new List<MonoBehaviour>();

        public static MonoBehaviour[] GetSingletonInstanceComps()
        {
            return _InstancedSingletonObjs.ToArray();
        }


        public static void AddSingletonInstanceElem<T>(T singletonObj) where T : MonoBehaviour
        {
            _InstancedSingletonObjs.Add(singletonObj);
        }

        public static void RemoveSingletonInstanceElem<T>(T singletonObj) where T : MonoBehaviour
        {
            _InstancedSingletonObjs.Remove(singletonObj);
        }

    }
}
