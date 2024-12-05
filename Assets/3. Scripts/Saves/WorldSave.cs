using System;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class WorldSave
    {
        public string worldName = "";
        public QuestSave questSave = new();
    }
}