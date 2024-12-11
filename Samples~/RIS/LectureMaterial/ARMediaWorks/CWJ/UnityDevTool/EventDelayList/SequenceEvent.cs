using System;

using UnityEngine.Events;

namespace CWJ
{
    [Serializable]
    public struct SequenceEvent
    {
        public float delay;
        public UnityEvent unityEvent;
    }
}