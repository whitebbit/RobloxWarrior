using System;
using _3._Scripts.Saves.Interfaces;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class HeroSave : ISavable
    {
        public string id;
        public int level;
        public string ID => id;
    }
}