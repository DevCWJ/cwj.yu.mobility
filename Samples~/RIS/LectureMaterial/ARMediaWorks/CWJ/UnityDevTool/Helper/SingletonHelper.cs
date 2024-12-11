using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using System.Linq;
using CWJ.Singleton;

#if UNITY_EDITOR
using UnityEditor;

#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
#endif
namespace CWJ
{
    public class SingletonHelper : SingletonBehaviourDontDestroy<SingletonHelper>, IDontPrecreatedInScene
    {
        private static List<MonoBehaviourCWJ_AwakableInInactive> awakableInInactives = new List<MonoBehaviourCWJ_AwakableInInactive>();
        private static SingletonHelper instanceForNonMono = null;
        public static void AddAwakableInInactive(MonoBehaviourCWJ_AwakableInInactive awakableInInactive)
        {
            awakableInInactives.Add(awakableInInactive);

            if (instanceForNonMono != null && CO_CallAwakeForInactiveObj == null)
            {
                CO_CallAwakeForInactiveObj = instanceForNonMono.StartCoroutine(DO_CallAwakeForInactiveObj());
            }
        }
        public static void RemoveAwakableInInactive(MonoBehaviourCWJ_AwakableInInactive awakableInInactive)
        {
            awakableInInactives.Remove(awakableInInactive);
        }



        private static List<Component> _AllSingletonComps = new List<Component>();

        [VisualizeField, Readonly] private List<Component> _allSingletonComps = new List<Component>();
        public Component[] allSingletonComps
        {
            get => _allSingletonComps.ToArray();
        }

        public static void AddSingletonAllElem(Component singletonComp)
        {
            if (HasInstance) Instance._allSingletonComps.Add(singletonComp);
            else _AllSingletonComps.Add(singletonComp);
        }
        public static void RemoveSingletonAllElem(Component singletonComp)
        {
            if (HasInstance) Instance._allSingletonComps.Remove(singletonComp);
            else _AllSingletonComps.Remove(singletonComp);
        }

        private static List<Component> _SingletonInstanceComps { get; set; } = new List<Component>();

        [VisualizeProperty, Readonly] private List<Component> _singletonInstanceComps { get; set; } = new List<Component>();
        public Component[] GetSingletonInstanceComps()
        {
            return _singletonInstanceComps.ToArray();
        }

        public static void AddSingletonInstanceElem(Component singletonComp)
        {
            if (HasInstance) Instance._singletonInstanceComps.Add(singletonComp);
            else _SingletonInstanceComps.Add(singletonComp);
        }
        public static void RemoveSingletonInstanceElem(Component singletonComp)
        {
            if (HasInstance) Instance._singletonInstanceComps.Remove(singletonComp);
            else _SingletonInstanceComps.Remove(singletonComp);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            UpdateInstance(false);
        }

        protected override void _Awake()
        {
            HideGameObject();
            transform.SetAsFirstSibling();
        }

        static Coroutine CO_CallAwakeForInactiveObj = null;
        static IEnumerator DO_CallAwakeForInactiveObj()
        {
            yield return null;
            awakableInInactives = awakableInInactives.Where(x => x != null).ToList();
            if (awakableInInactives.Count > 0)
            {
                for (int i = awakableInInactives.Count - 1; i > 0; --i)
                {
                    var comp = awakableInInactives[i];
                    awakableInInactives.RemoveAt(i);

                    if (comp == null || comp.gameObject == null || !comp.gameObject.scene.IsValid()) 
                        continue;
#if UNITY_EDITOR
                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    bool isPrefabObj = prefabStage != null ? (PrefabUtility.GetOutermostPrefabInstanceRoot(comp.gameObject).Equals(prefabStage.prefabContentsRoot)) : PrefabUtility.IsPartOfAnyPrefab(comp.gameObject);

                    if (isPrefabObj && comp.gameObject.scene.IsNull())
                    {
                        continue;
                    }
#endif

                    comp.AwakeInInactive();
                }
            }

            yield return null;


            if (awakableInInactives.Count == 0)
            {
                CO_CallAwakeForInactiveObj = null;
            }
            else
            {
                CO_CallAwakeForInactiveObj = (instanceForNonMono != null) ? instanceForNonMono.StartCoroutine(DO_CallAwakeForInactiveObj()) : null;
            }
        }

        protected override void OnAfterInstanceAssigned()
        {
            instanceForNonMono = _Instance;

            if (CO_CallAwakeForInactiveObj != null) StopCoroutine(CO_CallAwakeForInactiveObj);
            CO_CallAwakeForInactiveObj = StartCoroutine(DO_CallAwakeForInactiveObj());

            _singletonInstanceComps.AddRange(_SingletonInstanceComps.ToArray());
            _SingletonInstanceComps.Clear();
            _allSingletonComps.AddRange(_AllSingletonComps.ToArray());
            _AllSingletonComps.Clear();
        }
    }
}