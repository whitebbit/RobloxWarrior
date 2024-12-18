using System.Collections.Generic;
using _3._Scripts.Pool;
using _3._Scripts.Singleton;
using _3._Scripts.Sounds.Scriptables;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private SoundDatabase soundDatabase;
        private readonly Dictionary<string, AudioObject> _activeLoops = new();

        public void PlaySound(string id, bool loop = false, float volume = -1f, int repeatCount = 1)
        {
            if (loop && _activeLoops.ContainsKey(id)) return;

            var sound = soundDatabase.Sounds.Find(s => s.ID == id);
            if (sound == null)
            {
                Debug.LogWarning($"Sound with ID {id} not found");
                return;
            }

            var clip = sound.AudioClips[Random.Range(0, sound.AudioClips.Count)];
            var finalVolume = volume > 0 ? volume : sound.Volume;

            var obj = ObjectsPoolManager.Instance.Get<AudioObject>();

            if (loop && _activeLoops.TryAdd(id, obj))
            {
                obj.Initialize(clip, 0, repeatCount, true);
                obj.FadeTransition(finalVolume, 0.5f);
                return;
            }

            obj.Initialize(clip, finalVolume, repeatCount, false);
        }

        public void StopLoop(string id)
        {
            if (!_activeLoops.TryGetValue(id, out var source)) return;

            source.FadeTransition(0, 0.25f).OnComplete(() => ObjectsPoolManager.Instance.Return(source));
            _activeLoops.Remove(id);
        }
    }
}