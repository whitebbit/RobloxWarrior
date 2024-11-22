using _3._Scripts.Config.Interfaces;
using _3._Scripts.Detectors.OverlapSystem.Base;
using _3._Scripts.Units.Interfaces;
using _3._Scripts.Weapons.Interfaces;
using UnityEngine;

namespace _3._Scripts.Weapons.Base
{
    [RequireComponent(typeof(OverlapDetector<IWeaponVisitor>))]
    public abstract class Weapon<TConfig> : MonoBehaviour, IInitializable<TConfig> where TConfig : IWeaponConfig
    {
        protected OverlapDetector<IWeaponVisitor> Detector;
        protected IWeaponConfig Config;

        public float AttackRange => Detector.DetectAreaSize;
        protected void Awake()
        {
            Detector = GetComponent<OverlapDetector<IWeaponVisitor>>();
            OnAwake();
        }

        protected void Start()
        {
            Detector.OnFound += GetWeaponVisitors;
            OnStart();
        }

        public virtual void Initialize(TConfig config)
        {
            Config = config;
        }
        
        public abstract void Attack();
        protected abstract float GetDamage();

        public virtual float GetTrueDamage()
        {
            return 0;
        }

        protected bool CanCrit(float chance)
        {
            chance = Mathf.Clamp01(chance);
            return Random.value <= chance;
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        private void GetWeaponVisitors(IWeaponVisitor obj)
        {
            obj?.Visit(GetDamage());
        }
    }
}