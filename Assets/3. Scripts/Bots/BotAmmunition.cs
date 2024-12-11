using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Bots.Enums;
using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotAmmunition : MonoBehaviour, IInitializable<BotConfig>
    {
        [SerializeField] private List<BotWeaponModel> weapons = new();
        
        public void Initialize(BotConfig config)
        {
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }

            if(config.UseShield)
                weapons.FirstOrDefault(w => w.Type == BotWeaponType.Shield)?.gameObject.SetActive(true);

            if (config.WeaponType == BotWeaponType.None) return;
            
            weapons.FirstOrDefault(w => w.Type == config.WeaponType)?.gameObject.SetActive(true);
        }
    }
}