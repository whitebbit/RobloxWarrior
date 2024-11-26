using _3._Scripts.Config.Interfaces;
using _3._Scripts.Detectors.OverlapSystem.Base;
using _3._Scripts.Extensions;
using _3._Scripts.Pool;
using _3._Scripts.Units.Interfaces;
using _3._Scripts.Weapons.Interfaces;
using UnityEngine;

namespace _3._Scripts.Weapons.Base
{
    [RequireComponent(typeof(OverlapDetector<IWeaponVisitor>))]
    public abstract class Weapon<TConfig> : MonoBehaviour, IInitializable<TConfig> where TConfig : IWeaponConfig
    {
        [SerializeField] private bool useFloatingText;

        protected OverlapDetector<IWeaponVisitor> Detector;
        protected IWeaponConfig Config;

        public float AttackRange => Detector.DetectAreaSize;
        protected abstract float CritChance { get; }
        public virtual float DamageIncrease { get; set; }

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


        protected bool CanCrit()
        {
            var chance = Mathf.Clamp01(CritChance);
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
            var damage = GetDamage();
            var crit = CanCrit();
            var damageWithCrit = crit ? damage * 2 : damage;

            if (useFloatingText && obj != null)
            {
                var floatingText = ObjectsPoolManager.Instance.Get<FloatingText>();
                var objTransform = obj is MonoBehaviour behaviour ? behaviour.transform : null;
                var textPosition = Vector3.zero;
                
                if (objTransform != null)
                {
                    textPosition = objTransform.position + new Vector3(Random.Range(-3, 3), 2, 0) + objTransform.forward;
                }
                
                floatingText.Initialize($"{damageWithCrit}", textPosition);
                floatingText.SetColor(crit ? Color.red : Color.white);
            }

            obj?.Visit(damageWithCrit);
        }
    }
}