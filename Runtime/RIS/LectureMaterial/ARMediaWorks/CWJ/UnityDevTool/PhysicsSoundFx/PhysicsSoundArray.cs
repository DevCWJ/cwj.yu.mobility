using System;
using UnityEngine;

namespace CWJ.PhysicsSoundFx
{
    [Serializable]
    [CreateAssetMenu(fileName = "Physics SoundFX KeyValuePair", menuName = "CWJ/Physics SoundFX/Physics SoundFX KeyValuePair", order = 2)]
    public class PhysicsSoundArray : ScriptableObject
    {
        [SerializeField] private PhysicMaterial _materialKey = null;

        public string MaterialKey => _materialKey ? _materialKey.name : null;

        [SerializeField]
        private AudioClip[] _audioClips = new AudioClip[0];

        public AudioClip[] AudioClips => _audioClips;
    }
}
