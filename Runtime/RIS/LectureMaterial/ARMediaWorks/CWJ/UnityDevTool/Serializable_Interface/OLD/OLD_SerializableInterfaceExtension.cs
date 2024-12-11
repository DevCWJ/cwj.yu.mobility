namespace CWJ.Serializable
{
    //public static class OLD_SerializableInterfaceExtension
    //{
    //    #region SerializeInterface find 용도
    //    public static T GetSerializeInterfaceByComponent<TI, T>(this Component _component, Func<Component, T> creator) where TI : class where T : class //TI : 인터페이스
    //    {
    //        return _component.GetType().GetInterfaces().Any(i => i == typeof(TI)) ? creator(_component) : null;
    //    }

    //    public static T[] GetSerializeInterfaces<TI, T>(this Transform _transform, Func<Component, T> creator) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponents<Component>();
    //        int compLength = comps.Length;

    //        List<T> serializeInterfaces = new List<T>();

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                serializeInterfaces.Add(si);
    //            }
    //        }

    //        return serializeInterfaces.ToArray();
    //    }
    //    [Obsolete("GetSerializeInterface is obsolete. Please use GetComponent<interface>().ConvertInterfaceSerializable<interface,si>() instead")]
    //    public static T GetSerializeInterface<TI, T>(this Transform _transform, Func<Component, T> creator) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponents<Component>();
    //        int compLength = comps.Length;

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                return si;
    //            }
    //        }

    //        return null;
    //    }
    //    [Obsolete("GetSerializeInterfacesInChild is obsolete. Please use GetComponentsInChildren<interface>().ConvertInterfacesSerializable<interface,si>() instead")]
    //    public static T[] GetSerializeInterfacesInChild<TI, T>(this Transform _transform, Func<Component, T> creator, bool isWithoutMe = false, bool includeInactive = false) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponentsInChildren_New<Component>(isWithoutMe: isWithoutMe, includeInactive: includeInactive, (c) => c != null);
    //        int compLength = comps.Length;

    //        List<T> serializeInterfaces = new List<T>();

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                serializeInterfaces.Add(si);
    //            }
    //        }
    //        //TI[] interfaces = transform.GetComponentsInChildren_New<TI>(isWithoutMe: isWithoutMe, includeInactive: includeInactive, (c) => c != null);

    //        //int compLength = interfaces.Length;

    //        //List<T> serializeInterfaces = new List<T>();

    //        //for (int i = 0; i < compLength; i++)
    //        //{
    //        //    T si = creator(interfaces[i] as Component); //200109
    //        //    if (si != null)
    //        //    {
    //        //        serializeInterfaces.Add(si);
    //        //    }
    //        //}
    //        return serializeInterfaces.ToArray();
    //    }
    //    [Obsolete("GetSerializeInterfaceInChild is obsolete. Please use GetComponentInChildren<interface>().ConvertInterfaceSerializable<interface,si>() instead")]
    //    public static T GetSerializeInterfaceInChild<TI, T>(this Transform _transform, Func<Component, T> creator, bool isWithoutMe = false, bool includeInactive = false) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponentsInChildren<Component>(true);
    //        int compLength = comps.Length;

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                return si;
    //            }
    //        }

    //        return null;
    //    }
    //    [Obsolete("GetSerializeInterfacesInParent is obsolete. Please use GetComponentsInParent<interface>().ConvertInterfacesSerializable<interface,si>() instead")]
    //    public static T[] GetSerializeInterfacesInParent<TI, T>(this Transform _transform, Func<Component, T> creator, bool isWithoutMe = false, bool includeInactive = false) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponentsInParent_New<Component>(isWithoutMe: isWithoutMe, includeInactive: includeInactive, (c) => c != null);
    //        int compLength = comps.Length;

    //        List<T> serializeInterfaces = new List<T>();

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                serializeInterfaces.Add(si);
    //            }
    //        }

    //        return serializeInterfaces.ToArray();
    //    }
    //    [Obsolete("GetSerializeInterfaceInParent is obsolete. Please use GetComponentInParent<interface>().ConvertInterfaceSerializable<interface,si>() instead")]
    //    public static T GetSerializeInterfaceInParent<TI, T>(this Transform _transform, Func<Component, T> creator, bool isWithoutMe = false, bool includeInactive = false) where TI : class where T : class //TI : 인터페이스
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        Component[] comps = _transform.GetComponentsInParent<Component>(true);
    //        int compLength = comps.Length;

    //        for (int i = 0; i < compLength; i++)
    //        {
    //            T si = comps[i].GetSerializeInterfaceByComponent<TI, T>(creator);
    //            if (si != null)
    //            {
    //                return si;
    //            }
    //        }

    //        return null;
    //    }
    //    #endregion SerializeInterface find 용도

    //    /// <summary>
    //    /// 씬의 SerializableInterface를 검색
    //    /// </summary>
    //    /// <typeparam name="TI"></typeparam>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="creator"></param>
    //    /// <param name="includeDontDestroyOnLoadObjs"></param>
    //    /// <returns></returns>
    //    [Obsolete("GetCurSceneEventInterfaces is obsolete. Please use GetComponent<interface>().ConvertInterfaceSerializable<interface,si>() instead")]
    //    public static T[] GetCurSceneEventInterfaces<TI, T>(Func<Component, T> creator, bool includeInactive = false, bool includeDontDestroyOnLoadObjs = true) where TI : class where T : class
    //    {
    //        if (!typeof(TI).IsInterface) throw new SystemException("its not an interface!");

    //        GameObject[] rootObjs = FindUtil.GetRootGameObjects(includeDontDestroyOnLoadObjs);
    //        int rootObjsLength = rootObjs.Length;

    //        List<T> _curSceneEventInterfaces = new List<T>();

    //        for (int i = 0; i < rootObjsLength; i++)
    //        {
    //            T[] _eventInterfaces = rootObjs[i].transform.GetSerializeInterfacesInChild<TI, T>(creator, includeInactive: includeInactive);
    //            if (_eventInterfaces != null && _eventInterfaces.Length > 0)
    //            {
    //                _curSceneEventInterfaces.AddRange(_eventInterfaces);
    //            }
    //        }

    //        return _curSceneEventInterfaces.ToArray();
    //    }

    //}
}