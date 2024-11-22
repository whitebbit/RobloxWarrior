using System;
using UnityEngine.Serialization;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class PlayerStatsSave
    {
        public float experience;
        public int level = 1;

        public int upgradePoints;

        public int healthPoints;
        public int attackPoints;
        public int speedPoints;
        public int critPoints;

        public int rebirthCounts;
        public int levelForRebirth;
        public int skillPoints;

        public void ResetStats()
        {
            healthPoints = 0;
            attackPoints = 0;
            speedPoints = 0;
            critPoints = 0;

            upgradePoints = 0;
        }
    }
}