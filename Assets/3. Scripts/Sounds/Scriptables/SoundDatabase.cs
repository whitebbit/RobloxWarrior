using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Sounds.Scriptables
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Configs/Sounds/Database", order = 0)]
    public class SoundDatabase : ScriptableObject
    {
        [SerializeField] private List<SoundData> _sounds;

        public List<SoundData> Sounds => _sounds;
    }
}