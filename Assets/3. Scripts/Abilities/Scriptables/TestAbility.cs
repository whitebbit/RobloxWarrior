using UnityEngine;

namespace _3._Scripts.Abilities.Scriptables
{
    [CreateAssetMenu(fileName = "TestAbility", menuName = "Configs/Player/Abilities/Test", order = 0)]
    public class TestAbility : Ability
    {
        protected override void PerformAbility()
        {
            Debug.Log(DamagePercent);
        }
    }
}