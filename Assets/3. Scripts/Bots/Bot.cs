using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Detectors;
using _3._Scripts.Detectors.OverlapSystem.Base;
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

        private OverlapDetector<Player.Player> _playerDetector;
        private Transform _target;
        private UnitHealth _health;

        protected override void OnAwake()
        {
            _playerDetector = GetComponent<OverlapDetector<Player.Player>>();
            Dying = new BotDying(this);
            _health = new UnitHealth(0, Dying);

            _combat = GetComponent<BotCombat>();
            _animator = GetComponent<BotAnimator>();
            _movement = GetComponent<BotMovement>();
            _view = GetComponent<BotView>();
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
                var direction = _target.transform.position - transform.position;
                var trueDirection = new Vector2(direction.x, direction.z);
                _movement.Move(trueDirection);
            }
        }

        public void Initialize(BotConfig config)
        {
            Dying.IsDead = false;
            _target = null;

            _config = config;

            _animator.Initialize(_config);
            _combat.Initialize(_config);
            _movement.Initialize(_config);
            _view.Initialize(_config);
            
            _health.UpdateValues(_config.Health);
        }

        public void Upgrade(float damageIncrease, float healthIncrease)
        {
            var healthAmount = _config.Health * healthIncrease - _config.Health;

            Health.MaxHealth += healthAmount;
            _combat.Weapon.DamageIncrease = damageIncrease;
        }

        public void OnSpawn()
        {                   
            _playerDetector.DetectState(true);
            _playerDetector.OnFound += OnFound;
        }

        public void OnDespawn()
        {
            _playerDetector.OnFound -= OnFound;
            _playerDetector.DetectState(false);
        }
    }
}