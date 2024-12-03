using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Extensions;
using _3._Scripts.Swords.Scriptables;
using UnityEngine;

namespace _3._Scripts.Config.Scriptables
{
    [CreateAssetMenu(fileName = "SwordCollectionConfig", menuName = "Configs/Player/Sword/Sword Collection Config", order = 0)]
    public class SwordCollectionConfig : ScriptableObject
    {
        [SerializeField] private List<SwordConfig> swords = new();
        [Space]
        [SerializeField] private float starDamageIncrease;

        [SerializeField] private RandomType unlockerRandomType;
        
        public List<SwordConfig> Swords => swords;

        public RandomType UnlockerRandomType => unlockerRandomType;
        public float StarDamageIncrease => starDamageIncrease;
        public SwordConfig GetSword(string id) => swords.FirstOrDefault(s => s.ID == id);
        
        
    }
}