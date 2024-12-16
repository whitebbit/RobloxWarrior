using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Sounds.Scriptables
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Configs/Sounds/Database", order = 0)]
    public class SoundDatabase : ScriptableObject
    {
        [SerializeField] private List<SoundData> sounds;

        public List<SoundData> Sounds => sounds;
    }
}