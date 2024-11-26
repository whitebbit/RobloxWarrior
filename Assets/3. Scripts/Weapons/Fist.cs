
using _3._Scripts.Weapons.Base;
using _3._Scripts.Weapons.Configs;

namespace _3._Scripts.Weapons
{
    public class Fist: Weapon<BaseWeaponConfig>
    {
        protected override float CritChance => 0;

        public override void Attack()
        {
            Detector.FindTargets();
        }

        protected override float GetDamage()
        {
            return Config.Damage * DamageIncrease;
        }
    }
}