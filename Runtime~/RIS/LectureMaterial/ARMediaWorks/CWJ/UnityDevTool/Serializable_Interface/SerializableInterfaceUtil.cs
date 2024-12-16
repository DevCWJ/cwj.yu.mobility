using System;

using CWJ.Serializable;

//using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

namespace CWJ
{
    public static class SerializableInterfaceUtil
    {
        public static void ThrowIfNotInterfaceException(Type type)
        {
            if(!type.IsInterface) throw new SystemException("Specified type is not an interface!");
        }

        /// <summary>
        /// 인터페이스(TI)를 InterfaceSerializable(TSI)로 변형
        /// </summary>
        /// <typeparam name="TI">Interface</typeparam>
        /// <typeparam name="TSI">SI</typeparam>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        public static TSI[] ToSerializableInterfaces<TI, TSI>(this TI[] interfaces) where TI : class where TSI : InterfaceSerializable<TI>, new()//TI : 인터페이스
        {
            TSI[] siArray = new TSI[interfaces.Length];

            for (int i = 0; i < interfaces.Length; i++)
            {
                siArray[i] = interfaces[i].ToSerializableInterface<TI, TSI>();
            }

            return siArray;
        }

        /// <summary>
        /// 인터페이스(TI)를 InterfaceSerializable(TSI)로 변형
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TSI"></typeparam>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static TSI ToSerializableInterface<TI, TSI>(this TI @interface) where TI : class where TSI : InterfaceSerializable<TI>, new()//TI : 인터페이스
        {
            ThrowIfNotInterfaceException(typeof(TI));

            TSI si = new TSI();
            si.Interface = @interface;

            return si;
        }

        /// <summary>
        /// FindObjectOfTypes 을 SerializableInterface버전으로 만든것
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TSI"></typeparam>
        /// <param name="includeInactive"></param>
        /// <param name="includeDontDestroyOnLoadObjs"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TSI[] FindSerializableInterfaces<TI, TSI>(bool includeInactive = false, bool includeDontDestroyOnLoadObjs = true, System.Predicate<TI> predicate = null) where TI : class where TSI : InterfaceSerializable<TI>, new()//TI : 인터페이스
        {
            ThrowIfNotInterfaceException(typeof(TI));

            TI[] interfaces = FindUtil.FindInterfaces<TI>(includeInactive: includeInactive, includeDontDestroyOnLoadObjs: includeDontDestroyOnLoadObjs, predicate: predicate);

            return interfaces.ToSerializableInterfaces<TI, TSI>();
        }

        /// <summary>
        /// FindObjectOfType 을 SerializableInterface버전으로 만든것
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TSI"></typeparam>
        /// <param name="includeInactive"></param>
        /// <param name="includeDontDestroyOnLoadObjs"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TSI FindSerializableInterface<TI, TSI>(bool includeInactive = false, bool includeDontDestroyOnLoadObjs = true, System.Predicate<TI> predicate = null) where TI : class where TSI : InterfaceSerializable<TI>, new()//TI : 인터페이스
        {
            ThrowIfNotInterfaceException(typeof(TI));

            TI Interface = FindUtil.FindInterface<TI>(includeInactive: includeInactive, includeDontDestroyOnLoadObjs: includeDontDestroyOnLoadObjs, predicate: predicate);

            return Interface.ToSerializableInterface<TI, TSI>();
        }
    }
}
