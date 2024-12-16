//namespace CWJ.EditorOnly
//{
    //public static class ComponentCacheSystem
    //{
    //     [InitializeOnLoadMethod]
    //    public static void InitializeOnLoad()
    //    {
    //        EditorEventSystem.EditorSceneOpenedEvent += (s) => InitAllComponentCaches();
    //    }

    //    private static string CompIDCachesTag { get { return ".CWJ.CompIDCaches/" + AccessibleEditorUtil.GetBuildPackageName(); } }

    //    public static void InitAllComponentCaches()
    //    {
    //        VolatileEditorPrefsUtil.InitPrefsCache();

    //        UpdateComponentCache(true, FindUtil.FindObjectsOfType_New<MonoBehaviour>(includeInactive: true, includeDontDestroyOnLoadObjs: false).ConvertCompIDs());
    //    }

    //    public static bool UpdateComponentCache(params int[] compIDs)
    //    {
    //        return UpdateComponentCache(false, compIDs);
    //    }

    //    private static bool UpdateComponentCache(bool isInit, params int[] compIDs)
    //    {
    //        if (compIDs == null || compIDs.Length == 0) return false;

    //        VolatileEditorPrefsUtil.AddVolatileKey(CompIDCachesTag);

    //        var addedIDs = new System.Collections.Generic.List<int>();

    //        int compLength = compIDs.Length;
    //        for (int i = 0; i < compLength; ++i)
    //        {
    //            if (compIDs[i] == 0) continue;
    //            bool isNewComp = VolatileEditorPrefsUtil.AddKeyStack(CompIDCachesTag, compIDs[i].ToString());
    //            if (isNewComp)
    //            {
    //                if (!isInit)
    //                {
    //                    EditorEventSystem.AddComponentWithIDEvent?.Invoke(compIDs[i]);
    //                    addedIDs.Add(compIDs[i]);
    //                }
    //            }
    //        }
    //        int addedLength = addedIDs.Count;
    //        if (addedLength > 0)
    //        {
    //            EditorEventSystem.AddComponentEvent?.Invoke();

    //            if (Selection.activeTransform == null)
    //            {
    //                InstanceIDUtil.SetSelectionObject(addedIDs.ToArray());
    //            }
    //            for (int i = 0; i < addedLength; ++i)
    //            {
    //                Object obj = EditorUtility.InstanceIDToObject(addedIDs[i]);

    //                //ReflectionUtil.InvokeMethodForcibly(obj, false, false, obj?.GetType().FullName, "Reset");
    //            }

    //        }
    //        return addedLength > 0;
    //    }

    //    public static void RemoveComponentCache(params int[] compIDs)
    //    {
    //        if (compIDs == null || compIDs.Length == 0) return;
    //        int length = compIDs.Length;
    //        for (int i = 0; i < length; ++i)
    //        {
    //            if (compIDs[i] == 0) continue;

    //            //bool isCompsAllRemoved = RemoveKeyStack(CompIDCachesTag, compIDs[i].ToString());

    //            //if (isCompsAllRemoved)
    //            //{
    //            //    RemoveVolatileKey(CompIDCachesTag);
    //            //}
    //            //주석처리 이유: Undo 할 수도 있기때문

    //            EditorEventSystem.RemoveComponentWithIDEvent?.Invoke(compIDs[i]);
    //        }
    //        EditorEventSystem.RemoveComponentEvent?.Invoke();
    //    }

    //}

    //public static class InstanceIDUtil
    //{
    //    private static int ToInstanceID(Object o)
    //    {
    //        MonoBehaviour m = (o as MonoBehaviour);
    //        return m?.GetType() != null ? m.GetInstanceID() : 0;
    //    }
    //    public static int[] ConvertObjToCompIDs(this Object[] targets)
    //    {
    //        return System.Array.ConvertAll(targets, new System.Converter<Object, int>(ToInstanceID));
    //    }

    //    private static int ToInstanceID(MonoBehaviour m)
    //    {
    //        return m?.GetType() != null ? m.GetInstanceID() : 0;
    //    }
    //    public static int[] ConvertCompIDs(this MonoBehaviour[] comps)
    //    {
    //        return System.Array.ConvertAll(comps, new System.Converter<MonoBehaviour, int>(ToInstanceID));
    //    }

    //    public static void SetSelectionObject(params int[] instanceID)
    //    {
    //        Selection.objects = System.Array.ConvertAll(instanceID, (i) => (EditorUtility.InstanceIDToObject(i) as MonoBehaviour)?.gameObject);
    //    }
    //}
//}