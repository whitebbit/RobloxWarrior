using System.Collections.Generic;
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
        [SerializeField] private List<ParticleSystem> starParticles;
        
        protected override float CritChance => Player.Player.Instance.Stats.CritImprovement;
        private SwordSave _save;

        protected override void OnAwake()
        {
            base.OnAwake();
            starParticles.SetState(false);
        }
        
        public void Disable()
        {
            starParticles.SetState(false);
            enabled = false;
            Detector.enabled = false;
            trail.enabled = false;
        }

        public override void Initialize(SwordConfig config)
        {
            base.Initialize(config);

            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);
            Detector.SetStartPoint(Player.Player.Instance.transform);
            trail.SetColor(rarity.MainColor);
        }

        public void SetSave(SwordSave save)
        {
            _save = save;
        }

        public void SetStars(int starAmount)
        {
            if (starAmount <= 0)
            {
                starParticles.SetState(false);
                return;
            }

            starParticles.SetEmissionRateOverTime(starAmount);
            starParticles.SetState(true);
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
            return _save.GetDamage(Config.Damage);
        }
    }
}