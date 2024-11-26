using System;
using _3._Scripts.Config;
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

        public float GetDamage(float damage)
        {
            var improveDamage = damage * (Configuration.Instance.Config.SwordCollectionConfig.StarDamageIncrease / 100) *
                                starCount;

            return damage + improveDamage;
        }
    }
}