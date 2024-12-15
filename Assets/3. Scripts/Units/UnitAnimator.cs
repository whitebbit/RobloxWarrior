using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using _3._Scripts.Animations.Structs;
using _3._Scripts.Player.Scriptables;
using Animancer;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.Units
{
    public abstract class UnitAnimator : MonoBehaviour
    {
        public bool Grounded { get; set; }

        private AnimancerComponent _animancer;

        private float _currentSpeed;


        private const float LerpSpeed = 7.5f;
        protected const float FadeDuration = .15f;

        private UnitAnimationConfig _config;
        private Animator _animator;

        protected virtual UnitAnimationConfig Config
        {
            get => _config;
            set
            {
                _animancer.enabled = false;
                _config = value;

                MainLayer.DestroyStates();
                AttackLayer.DestroyStates();

                AttackLayer.SetMask(_config.AttackMask);
                AttackLayer.IsAdditive = true;

                _movementAnimation = _config.MovementAnimation.Clone();
                _animancer.enabled = true;
            }
        }

        protected AnimancerLayer MainLayer => _animancer.Layers[0];
        private AnimancerLayer AttackLayer => _animancer.Layers[1];
        private AnimancerLayer AdditionalLayer => _animancer.Layers[2];

        private LinearMixerTransition _movementAnimation;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animancer = GetComponent<AnimancerComponent>();
        }

        public void DoAnimation(AnimationClip clip, Action onComplete = null, float speed = 1)
        {
            AdditionalLayer.DestroyStates();

            DOTween.To(() => MainLayer.Weight, x => MainLayer.Weight = x, 0, FadeDuration);
            DOTween.To(() => AttackLayer.Weight, x => AttackLayer.Weight = x, 0, FadeDuration);
            DOTween.To(() => AdditionalLayer.Weight, x => AttackLayer.Weight = x, 1, FadeDuration);

            var state = AdditionalLayer.Play(clip);
            var events = state.Events;
            state.Speed = speed;


            events.Clear();
            events.Add(0.9f, () =>
            {
                DOTween.To(() => MainLayer.Weight, x => MainLayer.Weight = x, 1, FadeDuration);
                DOTween.To(() => AttackLayer.Weight, x => AttackLayer.Weight = x, 1, FadeDuration);
                DOTween.To(() => AdditionalLayer.Weight, x => AdditionalLayer.Weight = x, 0, FadeDuration)
                    .OnComplete(() => onComplete?.Invoke());
            });
        }

        public void DoAttack(AttackAnimation attackAnimation, float attackSpeed, Action action)
        {
            if (!CanPlay()) return;

            // Проверяем, играет ли анимация и сбрасываем её
            if (AttackLayer.CurrentState != null && AttackLayer.CurrentState.Clip == attackAnimation.clip)
            {
                AttackLayer.Stop(); // Остановка текущей анимации
            }

            var state = AttackLayer.Play(attackAnimation.clip, FadeDuration);
            var events = state.Events;

            state.Speed = attackSpeed;

            events.Clear();
            foreach (var eventTime in attackAnimation.eventTimes.Where(_ => action != null))
            {
                events.Add(eventTime, action);
            }
        }

        public void SetSpeed(float targetSpeed)
        {
            if (!CanPlay()) return;

            if (!IsPlaying(Config.MovementAnimation))
            {
                MainLayer.Play(_movementAnimation, FadeDuration);
            }

            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * LerpSpeed);
            _movementAnimation.State.Parameter = _currentSpeed;
        }

        protected abstract bool CanFall { get; }

        public void DoFall()
        {
            if (!CanPlay()) return;

            if (!IsPlaying(Config.FallAnimation) && CanFall && !Grounded)
            {
                MainLayer.Play(Config.FallAnimation, FadeDuration);
            }
        }

        protected bool CanPlay() => _animancer != null;
        protected bool IsPlaying(AnimationClip clip) => _animancer.IsPlaying(clip);
        protected bool IsPlaying(IHasKey hasKey) => _animancer.IsPlaying(hasKey);
    }
}