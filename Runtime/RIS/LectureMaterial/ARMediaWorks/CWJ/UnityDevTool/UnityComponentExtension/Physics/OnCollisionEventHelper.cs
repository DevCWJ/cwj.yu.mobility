using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    [System.Serializable] public class OnCollisionEvent : UnityEngine.Events.UnityEvent<Transform, Collision> { }

    public class OnCollisionEventHelper : MonoBehaviour
    {
        public new Collider collider;
        public OnCollisionEvent onCollisionEnter = new OnCollisionEvent();
        public OnCollisionEvent onCollisionStay = new OnCollisionEvent();
        public OnCollisionEvent onCollisionExit = new OnCollisionEvent();

        private void Awake()
        {
            collider = transform.GetOrAddComponent_New<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter?.Invoke(transform, collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            onCollisionStay?.Invoke(transform, collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            onCollisionExit?.Invoke(transform, collision);
        }

        private void OnEnable()
        {
            collider.enabled = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
        }
    }

}