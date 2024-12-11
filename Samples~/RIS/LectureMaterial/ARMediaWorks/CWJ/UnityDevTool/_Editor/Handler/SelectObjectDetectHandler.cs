//namespace CWJ.EditorOnly
//{
    //public class ObjectHierarchyDetectBot
    //{
    //// [InitializeOnLoadMethod]
    //    public static void InitializeOnLoad()
    //    {
    //        Selection.selectionChanged += OnSelectionChanged;
    //        EditorEventSystem.EditorOneFrameEvent += OnSelectionChanged;
    //        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
    //        EditorEventSystem.MonoBehaviourAwakeEvent += OnSelectionChanged;
    //        EditorEventSystem.MonoBehaviourDestroyEvent += OnMonoBehaviourDestroy;
    //    }

    //    enum DragObjType
    //    {
    //        None = 0, //Scene, or others 등등
    //        GameObject,
    //        Component
    //    }

    //    private static void HierarchyWindowItemCallback(int instanceID, Rect selectionRect)
    //    {
    //        EventType eventType = Event.current.type;

    //        if (!isDragStart)
    //        {
    //            if (eventType == EventType.DragUpdated)
    //            {
    //                DragStart();
    //                isDragStart = true;
    //            }
    //        }
    //        else
    //        {
    //            if (eventType == EventType.DragPerform)
    //            {
    //                DragEnd();
    //                DragInit();
    //            }
    //            else if (eventType == EventType.DragExited)
    //            {
    //                DragInit();
    //            }
    //        }
    //    }

    //    static bool isDragStart = false;
    //    static DragObjType dragObjType = DragObjType.None;
    //    static Transform[] dragTrfs = null;

    //    static void DragStart()
    //    {
    //        UnityObject[] dragObjs = DragAndDrop.objectReferences;

    //        if (dragObjs.Length > 0)
    //        {
    //            if (dragObjs[0].GetType() == typeof(GameObject))
    //            {
    //                dragObjType = DragObjType.GameObject;
    //            }
    //            else if (dragObjs[0] as Component || dragObjs[0] as MonoScript)
    //            {
    //                dragObjType = DragObjType.Component;
    //            }
    //            else
    //            {
    //                dragObjs = null;
    //            }
    //        }
    //        else
    //        {
    //            dragObjs = null;
    //        }

    //        if (dragObjs == null)
    //        {
    //            dragObjType = DragObjType.None;
    //        }
    //        else
    //        {
    //            dragTrfs = Array.ConvertAll(dragObjs, (o) => (o as GameObject).transform);
    //        }

    //    }

    //    static void DragEnd()
    //    {
    //        if (dragObjType == DragObjType.GameObject)
    //        {
    //            Transform[] prevParents = Array.ConvertAll(dragTrfs, (t) => t.parent);
    //            Transform[] backupTrfs = dragTrfs;

    //            EditorCallback.AddWaitForFrameCallback(() =>
    //            {
    //                Transform[] curParents = Array.ConvertAll(backupTrfs, (t) => t.parent);
    //                int len = curParents.Length;

    //                for (int i = 0; i < len; ++i)
    //                {
    //                    if (curParents[i] != prevParents[i])
    //                    {
    //                        EditorEventSystem.TransformHierarchyChangedEvent?.Invoke();
    //                        break;
    //                    }
    //                }
    //                backupTrfs = null;
    //                curParents = null;
    //                prevParents = null;
    //            });
    //        }
    //        else if (dragObjType == DragObjType.Component)
    //        {
    //            EditorCallback.AddWaitForFrameCallback(() =>
    //            {
    //                ComponentCacheSystem.UpdateComponentCache(FindUtil.FindObjectsOfType_New<MonoBehaviour>(includeInactive: true, includeDontDestroyOnLoadObjs: false).ConvertCompIDs());
    //            });
    //        }
    //    }

    //    static void DragInit()
    //    {
    //        isDragStart = false;
    //        dragTrfs = null;
    //        dragObjType = DragObjType.None;
    //    }

    //    static bool isClicked;

    //    static int backupChildCount = 0;
    //    static Transform[] clickTrfs = null;
    //    static MonoBehaviour[] clickComps = null;
    //    static int[] clickCompIDs = null;

    //    private static void OnSelectionChanged()
    //    {
    //        if (Selection.activeTransform)
    //        {
    //            InitCompCache(Selection.transforms);
    //            isClicked = true;
    //            isDestroyed = false;
    //        }
    //        else
    //        {
    //            if (!isClicked)
    //            {
    //                return;
    //            }

    //            if (clickTrfs != null && clickTrfs[0] == null)
    //            {
    //                OnMonoBehaviourDestroy();
    //            }

    //            clickTrfs = null;
    //            clickComps = null;
    //            clickCompIDs = null;
    //            isClicked = false;
    //            isDestroyed = false;
    //        }
    //    }

    //    private static void InitCompCache(Transform[] trfs)
    //    {
    //        if (clickTrfs != null && clickTrfs[0] != null)
    //        {
    //            int curChildCount = clickTrfs[0]?.GetComponentsInChildren_New<Transform>(isWithoutMe: true, includeInactive: true).Length ?? 0;
    //            if(curChildCount!= backupChildCount)
    //            {
    //                EditorEventSystem.TransformHierarchyChangedEvent?.Invoke();
    //            }
    //        }
    //        backupChildCount = trfs[0]?.GetComponentsInChildren_New<Transform>(isWithoutMe: true, includeInactive: true).Length ?? 0;

    //        clickTrfs = trfs;
    //        int length = clickTrfs?.Length ?? 0;
    //        List<MonoBehaviour> comps = new List<MonoBehaviour>();
    //        for (int i = 0; i < length; ++i)
    //        {
    //            comps.AddRange(clickTrfs[i].GetComponentsInChildren<MonoBehaviour>(true));
    //        }

    //        clickComps = comps.ToArray();
    //        clickCompIDs = clickComps.ConvertCompIDs();
    //        ComponentCacheSystem.UpdateComponentCache(clickCompIDs);
    //    }

    //    private static bool isDestroyed;

    //    private static void OnMonoBehaviourDestroy()
    //    {
    //        if (isDestroyed)
    //        {
    //            return;
    //        }
    //        isDestroyed = true;

    //        EditorEventSystem.TransformHierarchyChangedEvent?.Invoke();

    //        int length = clickComps?.Length ?? 0;
    //        List<int> removeIDs = new List<int>();
    //        for (int i = 0; i < length; ++i)
    //        {
    //            if (clickComps[i] == null && clickCompIDs[i] != 0)
    //            {
    //                removeIDs.Add(clickCompIDs[i]);
    //                clickCompIDs[i] = 0;
    //            }
    //        }
    //        ComponentCacheSystem.RemoveComponentCache(removeIDs.ToArray());
    //    }

    //}
//}