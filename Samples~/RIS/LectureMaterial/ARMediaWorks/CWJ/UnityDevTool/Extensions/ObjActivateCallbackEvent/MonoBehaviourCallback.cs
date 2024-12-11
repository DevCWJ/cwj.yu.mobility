using UnityEngine;
using UnityEngine.Events;
namespace CWJ
{
    public static class MonoBehaviourCallback_Unity_Utility
    {
        public static MonoBehaviourCallback GetMonoBehaviourEvent(this GameObject go)
        {
            return go.transform.GetOrAddComponent<MonoBehaviourCallback>();
        }
        public static MonoBehaviourCallback GetMonoBehaviourEvent(this Transform transform)
        {
            return transform.GetOrAddComponent<MonoBehaviourCallback>();
        }
        public static MonoBehaviourCallback GetMonoBehaviourEvent(this MonoBehaviour monoBehav)
        {
            return monoBehav.transform.GetOrAddComponent<MonoBehaviourCallback>();
        }

        public static TC GetGenericMonoBehaviourEvent<T, TC>(this GameObject go, T eventParamValue) where TC : _MonoBehaviourGenericCallback<T>
        {
            var callbacker = go.GetOrAddComponent<TC>();
            callbacker.eventParamValue = eventParamValue;
            return callbacker;
        }
    }

    public class MonoBehaviourCallback : MonoBehaviour
    {
        public UnityEvent_Transform awakeEvent = new UnityEvent_Transform();
        public UnityEvent_Transform startEvent = new UnityEvent_Transform();

        public UnityEvent_Transform onEnabledEvent = new UnityEvent_Transform();
        public UnityEvent_Transform onDisabledEvent = new UnityEvent_Transform();

        public UnityEvent_Transform onDestroyEvent = new UnityEvent_Transform();

        private void Awake() => awakeEvent.Invoke(transform);
        private void Start() => startEvent.Invoke(transform);
        private void OnEnable() => onEnabledEvent.Invoke(transform);
        private void OnDisable() => onDisabledEvent.Invoke(transform);
        private void OnDestroy() => onDestroyEvent.Invoke(transform);
    }

    /// <summary>
    /// generic MonoBehaviour 이므로 override 해서 선언해서 사용하기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _MonoBehaviourGenericCallback<T> : MonoBehaviour
    {
        public T eventParamValue;
        public UnityEvent<T> awakeEvent = new UnityEvent<T>();
        public UnityEvent<T> startEvent = new UnityEvent<T>();

        public UnityEvent<T> onEnabledEvent = new UnityEvent<T>();
        public UnityEvent<T> onDisabledEvent = new UnityEvent<T>();

        public UnityEvent<T> onDestroyEvent = new UnityEvent<T>();

        private void Awake() => awakeEvent.Invoke(eventParamValue);
        private void Start() => startEvent.Invoke(eventParamValue);
        private void OnEnable() => onEnabledEvent.Invoke(eventParamValue);
        private void OnDisable() => onDisabledEvent.Invoke(eventParamValue);
        private void OnDestroy() => onDestroyEvent.Invoke(eventParamValue);
    }
}