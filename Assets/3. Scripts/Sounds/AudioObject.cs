using System;
using _3._Scripts.Pool.Interfaces;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    public class AudioObject: MonoBehaviour, IPoolable
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(AudioClip clip, float volume, bool loop)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.loop = loop;
            _audioSource.Play();
        }
        
        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
            _audioSource.Stop();
        }
    }
}