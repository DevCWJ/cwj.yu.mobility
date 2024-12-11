using System.Collections.Generic;

using UnityEngine;

namespace CWJ
{
    public class Example_ObjActivate_Unity : MonoBehaviour
    {
        public List<MonoBehaviourCallback> objectUnityListeners = new List<MonoBehaviourCallback>();

        private void Start()
        {
            foreach (Transform child in transform)
            {
                MonoBehaviourCallback objUnityListener = child.GetMonoBehaviourEvent();

                objUnityListener.onEnabledEvent.AddListener_New(PrintEnabled, false);
                objUnityListener.onDisabledEvent.AddListener_New(PrintDisabled, false);

                objectUnityListeners.Add(objUnityListener);
            }
        }

        private void PrintEnabled(Transform gameObject)
        {
            Debug.LogError(gameObject.name + " is Enabled");
        }

        private void PrintDisabled(Transform gameObject)
        {
            Debug.LogError(gameObject.name + " is Disabled");
        }
    }
}