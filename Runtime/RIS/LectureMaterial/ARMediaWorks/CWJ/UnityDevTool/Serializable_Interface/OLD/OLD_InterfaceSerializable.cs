//using System;
//using UnityEngine;
////인터페이스 직렬화
//namespace CWJ.Serializable
//{
//    /// <summary>
//    /// 인터페이스 직렬화.
//    /// 명명규칙은 SI_이름 으로 한다 (SI:SerializableInterface/ ex:SI_SceneEvent[] si_sceneEvents;)
//    ///
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    [Serializable]
//    [Obsolete("OLD_InterfaceSerializable is obsolete. Please use InterfaceSerializable instead")]
//    public class OLD_InterfaceSerializable<T> where T : class //SerializableInterface가 맞는말이지만 인텔리센스를 사용한다면 형태 이름(Interface)이 먼저나오는게 더 편함
//    {
//        public Component component;
//        public T @interface;

//        public OLD_InterfaceSerializable(Component target)
//        {
//            this.component = target;

//            this.@interface = target as T;
//        }
//    }
//}