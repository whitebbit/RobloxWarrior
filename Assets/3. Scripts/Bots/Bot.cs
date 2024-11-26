using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Detectors;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Units;
using _3._Scripts.Units.Interfaces;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class Bot : Unit, IInitializable<BotConfig>, IPoolable
    {
        public override UnitHealth Health => _health;
        public BotDying Dying { get; private set; }

        private BotConfig _config;
        private BotMovement _movement;
        private BotCombat _combat;
        private BotAnimator _animator;
        private BotView _view;

        private BaseDetector<Player.Player> _playerDetector;
        private Transform _target;
        private UnitHealth _health;

        protected override void OnAwake()
        {
            base.OnAwake();

            _playerDetector = GetComponent<BaseDetector<Player.Player>>();
            Dying = new BotDying(this);
            _health = new UnitHealth(100, Dying);

            _combat = GetComponent<BotCombat>();
            _animator = GetComponent<BotAnimator>();
            _movement = GetComponent<BotMovement>();
            _view = GetComponent<BotView>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _playerDetector.OnFound += OnFound;
        }

        private void OnFound(Player.Player obj)
        {
            if (obj != null)
                _target = obj.transform;
        }

        private void Update()
        {
            if (_target == null)
            {
                _movement.Move(Vector2.zero);
                return;
            }

            if (Vector3.Distance(transform.position, _target.transform.position) <= _combat.Weapon.AttackRange)
            {
                _movement.Move(Vector2.zero);
                _combat.Attack();
            }
            else
            {
                var direction = (_target.transform.position - transform.position);
                var trueDirection = new Vector2(direction.x, direction.z);
                _movement.Move(trueDirection);
            }
        }

        public void Initialize(BotConfig config)
        {
            _config = config;

            _animator.Initialize(_config);
            _combat.Initialize(_config);
            _movement.Initialize(_config);
            _view.Initialize(_config);
        }

        public void Upgrade(float damageIncrease, float healthIncrease)
        {
            var healthAmount = _config.Health * healthIncrease - _config.Health;

            Health.IncreaseMaxHealth(healthAmount);
            _combat.Weapon.DamageIncrease = damageIncrease;
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
            if (_config == null) return;

            Health.Health = _config.Health;
            Dying.IsDead = false;
        }
    }
}