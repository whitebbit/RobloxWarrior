using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Animations;
using _3._Scripts.Animations.Structs;
using _3._Scripts.Weapons.Base;
using _3._Scripts.Weapons.Interfaces;
using UnityEngine;

namespace _3._Scripts.Units
{
    public abstract class UnitCombat<TWeapon, TConfig> : MonoBehaviour
        where TWeapon : Weapon<TConfig> where TConfig : IWeaponConfig
    {
        public abstract TWeapon Weapon { get; }
        protected abstract List<AttackAnimation> AttackAnimations { get; }
        protected abstract UnitAnimator Animator { get; set; }

        protected virtual float AttackSpeed => 1;

        private int _comboIndex;
        private bool _isAttacking;

        protected virtual bool CanAttack()
        {
            return !_isAttacking;
        }

        public void Attack()
        {
            if (!CanAttack()) return;

            StartCoroutine(PerformAttack());
        }


        protected virtual IEnumerator PerformAttack()
        {
            var attackAnimation = AttackAnimations[Mathf.Clamp(_comboIndex, 0, AttackAnimations.Count - 1)];
            var length = attackAnimation.clip.length / AttackSpeed;

            _isAttacking = true;

            Animator.DoAttack(attackAnimation, AttackSpeed, Weapon.Attack);

            yield return new WaitForSeconds(length - length * 0.3f);

            _comboIndex++;
            if (_comboIndex >= AttackAnimations.Count)
            {
                _comboIndex = 0;
            }

            _isAttacking = false;
        }
    }
}