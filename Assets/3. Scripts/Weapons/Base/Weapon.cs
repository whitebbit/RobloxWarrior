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
        public virtual float DamageIncrease { get; set; } = 1;
        private Transform _owner;

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

        public void SetOwner(Transform owner) => _owner = owner;

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

            UseFloatingText(obj, damageWithCrit, crit);

            obj?.Visit(damageWithCrit);
        }

        private void UseFloatingText(IWeaponVisitor obj, float damageWithCrit, bool crit)
        {
            if (!useFloatingText || obj == null) return;

            var floatingText = ObjectsPoolManager.Instance.Get<FloatingText>();
            var textPosition = Vector3.zero;

            if (_owner != null)
            {
                textPosition = _owner.position + new Vector3(Random.Range(-3f, 3f), Random.Range(2f, 3f), 0) -
                               transform.forward * Random.Range(0.5f, 1f);
            }

            floatingText.Initialize($"{damageWithCrit}", textPosition);
            floatingText.SetColor(crit ? Color.red : Color.white);
        }
    }
}