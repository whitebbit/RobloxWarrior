﻿using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Pool;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.Weapons.Base;
using UnityEngine;

namespace _3._Scripts.Swords
{
    public class Sword : Weapon<SwordConfig>
    {
        [SerializeField] private MeleeWeaponTrail trail;
        protected override float CritChance => Player.Player.Instance.Stats.CritImprovement;
        private SwordSave _save;

        protected override void OnStart()
        {
            base.OnStart();
            Detector.SetStartPoint(Player.Player.Instance.transform);
        }

        public void Disable()
        {
            enabled = false;
            Detector.enabled = false;
            trail.enabled = false;
        }
        
        public void SetSave(SwordSave save)
        {
            _save = save;
        }


        public override void Attack()
        {
            Detector.FindTargets();
        }

        protected override float GetDamage()
        {
            var damage = Player.Player.Instance.GetDamage(GetTrueDamage());
            return damage;
        }

        public override float GetTrueDamage()
        {
            var improveDamage = Config.Damage * (Configuration.Instance.Config.SwordCollectionConfig.StarDamageIncrease / 100) *
                                _save.starCount;

            return Config.Damage + improveDamage;
        }
    }
}