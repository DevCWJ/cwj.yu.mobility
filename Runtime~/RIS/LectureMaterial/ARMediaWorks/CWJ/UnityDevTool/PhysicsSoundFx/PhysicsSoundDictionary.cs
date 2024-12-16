using System.Collections.Generic;

using UnityEngine;
using CWJ.Serializable;

namespace CWJ.PhysicsSoundFx
{
    [CreateAssetMenu(fileName = "Physics SoundFX Dictionary", menuName = "CWJ/Physics SoundFX/Physics SoundFX Dictionary", order = 2)]
    public class PhysicsSoundDictionary : ScriptableObject
    {
        // TODO Dictionary_Serializable
        //[SerializeField] private DictionaryStorePhysicsSoundFx physicsSoundStore = DictionaryStorePhysicsSoundFx.New<DictionaryStorePhysicsSoundFx>();
        //public Dictionary<UnityEngine.PhysicMaterial, PhysicsSoundArray> dictionary
        //{
        //    get => physicsSoundStore.dictionary;
        //    set => physicsSoundStore.dictionary = value;
        //}


        [SerializeField] private PhysicsSoundArray[] physicsSfx = new PhysicsSoundArray[0];

        [SerializeField] private AudioClip[] defaultClips = new AudioClip[0];
        private AudioClip[] currentClips;

        /// <summary>
        /// The current active array of audio clips as set by update active audio clips method.
        /// </summary>
        public AudioClip[] ActiveAudioClips => currentClips ?? defaultClips;

        /// <summary>
        /// This method allows you to update the cached array of active audio clips within this object. It can be accessed from the ActiveAudioClips property.
        /// </summary>
        /// <param name="material">The physics material that has been interacted with.</param>
        public void UpdateActiveAudioClips(PhysicMaterial material)
        {
            currentClips = FindAudioClipsFromMaterial(material);
        }

        /// <summary>
        /// This method allows you to get an array of audio clips that correspond to a physics material.
        /// </summary>
        /// <param name="material">The physics material that has been interacted with.</param>
        /// <returns>A corresponding array of possible audio clips.</returns>
        public AudioClip[] GetClipsFromMaterial(PhysicMaterial material) => FindAudioClipsFromMaterial(material);

        private AudioClip[] FindAudioClipsFromMaterial(PhysicMaterial material)
        {
            if (material == null)
            {
                return defaultClips;
            }

            AudioClip[] foundClips = null;
            for (var i = 0; i < physicsSfx.Length; i++)
            {
                if(material.name != physicsSfx[i].MaterialKey) { continue; }

                foundClips = physicsSfx[i].AudioClips;
                break;
            }

            return foundClips ?? defaultClips;
        }
    }
}
