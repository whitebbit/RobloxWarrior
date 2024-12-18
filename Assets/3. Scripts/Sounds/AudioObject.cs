using System;
using System.Collections;
using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    public class AudioObject : MonoBehaviour, IPoolable
    {
        private AudioSource _audioSource;
        private AudioClip _clip;
        private int _repeatCount;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(AudioClip clip, float volume, int repeatCount = 1, bool loop = false)
        {
            _repeatCount = repeatCount;
            _clip = clip;

            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.loop = loop;

            if (!loop)
                StartCoroutine(DelayDisable());
            else
                _audioSource.Play();
        }

        private IEnumerator DelayDisable()
        {
            for (var i = 0; i < _repeatCount; i++)
            {
                _audioSource.PlayOneShot(_clip);
                yield return new WaitForSeconds(_audioSource.clip.length);
            }

            ObjectsPoolManager.Instance.Return(this);
        }

        public Tween FadeTransition(float volume, float duration)
        {
            return DOTween.To(() => _audioSource.volume, x => _audioSource.volume = x, volume, duration);
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
            _audioSource.Stop();
            StopAllCoroutines();
        }
    }
}