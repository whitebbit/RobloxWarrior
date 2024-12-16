using System.Collections.Generic;
using _3._Scripts.Pool;
using _3._Scripts.Singleton;
using _3._Scripts.Sounds.Scriptables;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private SoundDatabase soundDatabase;
        private readonly Dictionary<string, AudioObject> _activeLoops = new();

        public void PlaySound(string id, bool loop = false, float volume = -1f, int repeatCount = 1)
        {
            var sound = soundDatabase.Sounds.Find(s => s.ID == id);
            if (sound == null)
            {
                Debug.LogWarning($"Sound with ID {id} not found");
                return;
            }

            var clip = sound.AudioClips[Random.Range(0, sound.AudioClips.Count)];
            var finalVolume = volume > 0 ? volume : sound.Volume;

            for (var i = 0; i < repeatCount; i++)
            {
                var obj = ObjectsPoolManager.Instance.Get<AudioObject>();
                obj.Initialize(clip, finalVolume, loop);
                if (loop)
                    _activeLoops[id] = obj;
            }
        }

        public void StopLoop(string id)
        {
            if (!_activeLoops.TryGetValue(id, out var source)) return;
            
            ObjectsPoolManager.Instance.Return(source);
            _activeLoops.Remove(id);
        }
    }
}