using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections;
using System.Reflection;

#if UNITY_EDITOR

using UnityEditor.Events;

#endif

namespace CWJ
{
    //Flags일땐 비교함수 Equals 쓰지말것.
    [System.Flags]
    public enum EPlayMode
    {
        Off = 0,
        /// <summary>
        /// 실행중이지 않을때
        /// </summary>
        NotPlayMode = 1 << 0,
        /// <summary>
        /// 실행중일때
        /// </summary>
        PlayMode = 1 << 1,
        Always = ~0
    }
    [System.Serializable]
    public class UnityEventCWJ<T>
    {
        [SerializeField] UnityEvent<T> _unityEvent = null;
        [SerializeField, Readonly] bool isInit = false;

        [SerializeField, Readonly] int _listenerLength = 0;
        public int listenerLength
        {
            get
            {
                if (!isInit)
                    return 0;
                return _listenerLength;
            }
        }

        public bool HasListener()
        {
            if (!isInit)
                return false;
            return _listenerLength > 0;
        }

        public UnityEventCWJ()
        {
            isInit = false;
            _listenerLength = 0;
            this._unityEvent = new UnityEvent<T>();
        }

        void Initialize()
        {
            isInit = true;
            if (_unityEvent == null)
            {
                _unityEvent = new UnityEvent<T>();
                _listenerLength = 0;
            }
            else
            {
                _listenerLength = _unityEvent.GetRuntimeInvokableCallList().Count;
            }
        }

        public void Invoke(T param)
        {
            if (!isInit)
            {
                Initialize();
                return;
            }
            if (_listenerLength > 0 && _unityEvent != null)
                _unityEvent.Invoke(param);
        }

        public void Clear()
        {
            if (!isInit)
            {
                Initialize();
                return;
            }
            if (_unityEvent != null)
                _unityEvent.RemoveAllListeners_New();
            else
                _unityEvent = new UnityEvent<T>();
            _listenerLength = 0;
        }

        public event UnityAction<T> listener
        {
            add
            {
                if (value != null)
                {
                    if (!isInit)
                        Initialize();
                    _unityEvent.AddListener_New(value);
                    ++_listenerLength;
                }
            }
            remove
            {
                if (value != null)
                {
                    if (_unityEvent != null)
                        _unityEvent.RemoveListener_New(value);
                    Initialize();
                }
            }
        }
    }
    public static class UnityEventUtil
    {
        public static void InvokeWhenDiff<T>(this UnityEvent<T> unityEvent, ref T prevValue, T curValue, bool isPreventDuplicate)
        {
            if (!isPreventDuplicate || !prevValue.Equals(curValue))
            {
                unityEvent?.Invoke(curValue);
            }
            prevValue = curValue;
        }

        static readonly Type T_UnityEventBase = typeof(UnityEventBase);
        public static readonly Type T_InvokableCallList = Type.GetType("UnityEngine.Events.InvokableCallList, UnityEngine");

        static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        public static readonly FieldInfo F_Calls = T_UnityEventBase.GetField("m_Calls", bindingFlags);
        //static FieldInfo F_NeedsUpdate = null;
        static MethodInfo _M_PrepareInvoke = null;
        static MethodInfo M_PrepareInvoke
        {
            get
            {
                if (_M_PrepareInvoke == null)
                    _M_PrepareInvoke = T_UnityEventBase.GetMethod("PrepareInvoke", BindingFlags.Instance | BindingFlags.NonPublic);
                return _M_PrepareInvoke;
            }
        }

        //static FieldInfo F_CallsDirty = null;

        /// <summary>
        /// 현재는 Runtime에 실행 될(UnityEventCallState 기준) UnityEvent 만 가져와짐
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityEvent"></param>
        /// <returns></returns>
        public static IList GetRuntimeInvokableCallList<T>(this T unityEvent) where T : UnityEventBase
        {
            if(unityEvent == null) return null;

            //if(F_NeedsUpdate == null)
            //    F_NeedsUpdate = T_InvokableCallList.GetField("m_NeedsUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
            //F_NeedsUpdate.SetValue(F_Calls.GetValue(unityEvent), true);
            //if (F_CallsDirty == null)
            //    F_CallsDirty = T_UnityEventBase.GetField("m_CallsDirty", BindingFlags.Instance | BindingFlags.NonPublic);
            //F_CallsDirty.SetValue(unityEvent, true);

            var list = M_PrepareInvoke.Invoke(unityEvent, new object[0]);
            return (IList)list;
        }

        static MethodInfo _M_AddCall = null;
        public static MethodInfo M_AddCall
        {
            get
            {
                if (_M_AddCall == null)
                    _M_AddCall = T_UnityEventBase.GetMethod("AddCall", BindingFlags.Instance | BindingFlags.NonPublic);
                return _M_AddCall;
            }
        }


        public static void SetInvokableCallList<T>(this T unityEvent, in IList list) where T : UnityEventBase
        {
            if (unityEvent == null || list == null) return;
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != null)
                    M_AddCall.Invoke(unityEvent, new object[1] { list[i] });
            }
        }

        #region void

        /// <summary>
        /// Inspector에서 UnityEvent에 UnityAction이 추가된것을 확인가능하게끔 AddListener를 해주는 함수. <br/>Editor던 Runtime에서든 문제없이 사용가능
        /// <para/>Inspector에서 persistent call의 Method Name에 missing 이 표기되어 있을경우 빌드했을때 실행 안될 수도 있습니다.
        /// <para/>(조건1:missing이 표기되어있을때, 조건2:빌드후에 해당 UnityEvent를 가진 GameObject를 동적으로 복제 할 경우)
        /// <para/>익명메소드를 AddListener_New하는 경우는 지양하고 함수를 만들어 AddListener_New를 추천.
        /// <para/>AddListener로 등록한 action 보다 먼저 실행됨을 주의
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <param name="call"></param>
        public static void AddListener_New<T>(this T unityEvent, UnityAction call, bool isPrintWarningLog = true) where T : UnityEvent
        {
            if (unityEvent == null) Debug.LogError(unityEvent + " UnityEvent is Null");
#if UNITY_EDITOR
            try
            {
                UnityEventTools.AddPersistentListener(unityEvent, call);
            }
            catch/* (Exception e)*/
            {
                unityEvent.AddListener(call);
                if (isPrintWarningLog)
                {
                    Debug.LogWarning("경고, 테스트 필수".SetColor(new Color().GetLightRed()) + "(무명메소드 혹은 Invoke()를 담을 경우 AddPersistentListener_Editor가 적용되지 않거나 Remove가 작동하지 않을 수 있습니다)/ 이벤트 기능은 작동할거임 다만, 인스펙터에 표시 안될수도있음\n" /*+ e.ToString()*/);
                }
            }
#else
            unityEvent.AddListener(call);
#endif
        }

        public static void RemoveListener_New<T>(this T unityEvent, UnityAction call) where T : UnityEvent
        {
            if (unityEvent == null) return;
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#endif
            unityEvent.RemoveListener(call); //무명메소드가 RemovePersistentListener로는 지워지지않는경우 대비(인스펙터에는 빈거로 남아있음)
        }

        public static void RemoveAllListeners_New<T>(this T unityEvent, in bool isRemoveRuntimeCalls = true) where T: UnityEventBase
        {
            if (unityEvent == null) return;

#if UNITY_EDITOR
            int listenerCount = unityEvent.GetPersistentEventCount();
            for (int i = 0; i < listenerCount; i++)
            {
                UnityEventTools.RemovePersistentListener(unityEvent, 0);
            }
#endif
            if (isRemoveRuntimeCalls)
                unityEvent.RemoveAllListeners();
        }

#endregion void

#region Generic (T1)

        /// <summary>
        /// Inspector에서 UnityEvent에 UnityAction이 추가된것을 확인가능하게끔 AddListener를 해주는 함수. <br/>Editor던 Runtime에서든 문제없이 사용가능
        /// <para/>Inspector에서 persistent call의 Method Name에 missing 이 표기되어 있을경우 빌드했을때 실행 안될 수도 있습니다.
        /// <para/>(조건1:missing이 표기되어있을때, 조건2:빌드후에 해당 UnityEvent를 가진 GameObject를 동적으로 복제 할 경우)
        /// <para/>익명메소드를 AddListener_New하는 경우는 지양하고 함수를 만들어 AddListener_New를 추천.
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <param name="call"></param>
        public static void AddListener_New<T>(this UnityEvent<T> unityEvent, UnityAction<T> call, bool isPrintWarningLog = true)
        {
            if (unityEvent == null) Debug.LogError(unityEvent + " UnityEvent is Null");
#if UNITY_EDITOR
            try
            {
                UnityEventTools.AddPersistentListener(unityEvent, call);
            }
            catch (Exception e)
            {
                unityEvent.AddListener(call);
                if (isPrintWarningLog)
                {
                    Debug.LogWarning("경고, 테스트 필수".SetColor(new Color().GetLightRed()) + "(무명메소드 혹은 Invoke()를 담을 경우 AddPersistentListener_Editor가 적용되지 않거나 Remove가 작동하지 않을 수 있습니다)/ 이벤트 기능은 작동할거임 다만, 인스펙터에 표시 안될수도있음\n" + e.ToString());
                }
            }
            return;
#else
            unityEvent.AddListener(call);
#endif
        }

        public static void RegistListener_New<T>(this UnityEvent<T> unityEvent, bool isAdd, UnityAction<T> call)
        {
            if (unityEvent == null) return;
            if (isAdd) AddListener_New(unityEvent, call);
            else RemoveListener_New(unityEvent, call);
        }

        /// <summary>
        /// 이벤트에 제네릭함수를 등록할때 사용할것
        /// <para>Inspector에서 persistent call의 Method Name에 missing 이 표기되어 있을경우 빌드했을때 실행 안될 수도 있습니다.</para>
        /// <para>(조건1:missing이 표기되어있을때, 조건2:빌드후에 해당 UnityEvent를 가진 GameObject를 동적으로 복제 할 경우)</para>
        /// <para>익명메소드를 AddListener_New하는 경우는 지양하고 함수를 만들어 AddListener_New를 추천.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericEvent"></param>
        /// <param name="callAction"></param>
        /// <param name="isPrintWarningLog"></param>
        public static void AddListener_NewGeneric<T>(this UnityEvent<T> unityEvent, System.Action<T> systemAction, bool isPrintWarningLog = false)
        {
            if (unityEvent == null) return;
            unityEvent.AddListener_New<T>(new UnityAction<T>(systemAction), isPrintWarningLog);
        }

        public static void RemoveListener_New<T>(this UnityEvent<T> unityEvent, UnityAction<T> call)
        {
            if (unityEvent == null) return;
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#endif
            unityEvent.RemoveListener(call); //무명메소드가 RemovePersistentListener로는 지워지지않는경우 대비(인스펙터에는 빈거로 남아있음)
        }

        /// <summary>
        /// 이벤트에 제네릭함수를 제거할때 사용할것
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityEvent"></param>
        /// <param name="systemAction"></param>
        /// <param name="isPrintWarningLog"></param>
        public static void RemoveListener_NewGeneric<T>(this UnityEvent<T> unityEvent, System.Action<T> systemAction)
        {
            if (unityEvent == null) return;
            unityEvent.RemoveListener_New<T>(new UnityAction<T>(systemAction));
        }

#endregion Generic (T1)

#region Generic (T1,T2)

        /// <summary>
        /// Inspector에서 UnityEvent에 UnityAction이 추가된것을 확인가능하게끔 AddListener를 해주는 함수. <br/>Editor던 Runtime에서든 문제없이 사용가능
        /// <para/>Inspector에서 persistent call의 Method Name에 missing 이 표기되어 있을경우 빌드했을때 실행 안될 수도 있습니다.
        /// <para/>(조건1:missing이 표기되어있을때, 조건2:빌드후에 해당 UnityEvent를 가진 GameObject를 동적으로 복제 할 경우)
        /// <para/>익명메소드를 AddListener_New하는 경우는 지양하고 함수를 만들어 AddListener_New를 추천.
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <param name="call"></param>
        public static void AddListener_New<T1, T2>(this UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> call, bool isPrintWarningLog = true)
        {
            if (unityEvent == null) Debug.LogError(unityEvent + " UnityEvent is Null");
#if UNITY_EDITOR
            try
            {
                UnityEventTools.AddPersistentListener(unityEvent, call);
            }
            catch/* (Exception e)*/
            {
                unityEvent.AddListener(call);
                if (isPrintWarningLog)
                {
                    Debug.LogWarning("경고, 테스트 필수".SetColor(new Color().GetLightRed()) + "(무명메소드 혹은 Invoke()를 담을 경우 AddPersistentListener_Editor가 적용되지 않거나 Remove가 작동하지 않을 수 있습니다)/ 이벤트 기능은 작동할거임 다만, 인스펙터에 표시 안될수도있음\n" /*+ e.ToString()*/);
                }
            }
            return;
#else
            unityEvent.AddListener(call);
#endif
        }

        /// <summary>
        /// 이벤트에 제네릭함수를 등록할때 사용할것
        /// <para/>Inspector에서 persistent call의 Method Name에 missing 이 표기되어 있을경우 빌드했을때 실행 안될 수도 있습니다.
        /// <br/>(조건1:missing이 표기되어있을때, 조건2:빌드후에 해당 UnityEvent를 가진 GameObject를 동적으로 복제 할 경우)
        /// <para>익명메소드를 AddListener_New하는 경우는 지양하고 함수를 만들어 AddListener_New를 추천.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericEvent"></param>
        /// <param name="systemAction"></param>
        /// <param name="isPrintWarningLog"></param>
        public static void AddListener_NewGeneric<T1, T2>(this UnityEvent<T1, T2> genericEvent, System.Action<T1, T2> systemAction, bool isPrintWarningLog = false)
        {
            genericEvent.AddListener_New<T1, T2>(new UnityAction<T1, T2>(systemAction), isPrintWarningLog);
        }

        public static void RemoveListener_New<T1, T2>(this UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> call)
        {
            if (unityEvent == null) return;
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(unityEvent, call);
#endif
            unityEvent.RemoveListener(call); //무명메소드가 RemovePersistentListener로는 지워지지않는경우 대비(인스펙터에는 빈거로 남아있음)
        }

        /// <summary>
        /// 이벤트에 제네릭함수를 제거할때 사용할것
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericEvent"></param>
        /// <param name="systemAction"></param>
        /// <param name="isPrintWarningLog"></param>
        public static void RemoveListener_NewGeneric<T1, T2>(this UnityEvent<T1, T2> genericEvent, System.Action<T1, T2> systemAction)
        {
            if (genericEvent == null) return;
            genericEvent.RemoveListener_New<T1, T2>(new UnityAction<T1, T2>(systemAction));
        }

#endregion Generic (T1,T2)
    }
}