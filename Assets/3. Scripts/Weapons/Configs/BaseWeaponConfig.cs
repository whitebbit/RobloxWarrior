using _3._Scripts.Weapons.Interfaces;

namespace _3._Scripts.Weapons.Configs
{
    public class BaseWeaponConfig: IWeaponConfig
    {
        public BaseWeaponConfig(float damage)
        {
            Damage = damage;
        }

        public float Damage { get; }
    }
}