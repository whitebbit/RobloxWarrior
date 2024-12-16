using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    [System.Serializable]
    public class SoundData
    {
        [SerializeField] private string id;
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField, Range(0f, 1f)] private float volume = 1.0f;

        public string ID => id;
        public List<AudioClip> AudioClips => audioClips;
        public float Volume => volume;
    }
}