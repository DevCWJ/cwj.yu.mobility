using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemStopCallbackComp : MonoBehaviour
    {
        public UnityEvent_GameObject stopCallback = new UnityEvent_GameObject();

        private void Awake()
        {
            var particle = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            stopCallback?.Invoke(gameObject);
        }
    } 

}