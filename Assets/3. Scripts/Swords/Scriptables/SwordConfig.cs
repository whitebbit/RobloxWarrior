using _3._Scripts.Config.Scriptables;
using _3._Scripts.Extensions.Interfaces;
using _3._Scripts.UI.Enums;
using _3._Scripts.Weapons.Interfaces;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Swords.Scriptables
{
    [CreateAssetMenu(fileName = "SwordConfig", menuName = "Configs/Player/Sword/Sword Config", order = 0)]
    public class SwordConfig : ConfigObject<Texture2D>, IWeaponConfig, IRandomable
    {
        [Tab("Sword")] [SerializeField] private Sword prefab;
        [Space] [SerializeField] private Rarity rarity;
        [SerializeField] private float damage;
        [Space] 
        [SerializeField] private float dropChance;

        public float Damage => damage;
        public override Texture2D Icon => null;
        public Sword Prefab => prefab;
        public float Chance => dropChance;

        public Rarity Rarity => rarity;
    }
}