
using _3._Scripts.Weapons.Base;
using _3._Scripts.Weapons.Configs;

namespace _3._Scripts.Weapons
{
    public class Fist: Weapon<BaseWeaponConfig>
    {
        public override void Attack()
        {
            Detector.FindTargets();
        }

        protected override float GetDamage()
        {
            return Config.Damage;
        }
    }
}