using System;
using _3._Scripts.Saves.Interfaces;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class AbilitySave : ISavable
    {
        public string id;
        public int level;
        public int maxLevel = 1;
        public string ID => id;
    }
}