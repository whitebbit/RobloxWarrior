using System;
using UnityEngine;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class SwordSave
    {
        public int uid;
        public string id;
        public int starCount;

        public SwordSave(string id)
        {
            this.id = id;
            uid = $"{id}_{Time.time}".GetHashCode();
        }
        
        
    }
}