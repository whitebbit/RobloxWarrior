using System;
using System.Collections;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Units
{
    public abstract class UnitMovement<TAnimator> : MonoBehaviour where TAnimator : UnitAnimator
    {
        protected const float Gravity = -9.81f * 2.5f;
        private const float TurnSmoothTime = 0.1f;

        [SerializeField] private GroundChecker groundChecker;


        private CharacterController _characterController;
        private float _turnSmoothVelocity;

        protected Vector3 Velocity;
        protected TAnimator Animator;

        public bool IsGrounded => groundChecker.IsGrounded();
        protected abstract float Speed { get; }

        private void Awake()
        {
            Animator = GetComponent<TAnimator>();
            _characterController = GetComponent<CharacterController>();
        }
        
        private void Update()
        {
            if (Time.time - _timeToActivate < 0.02f) return;
            
            ResetVelocity();
            Fall();
            Animator.Grounded = IsGrounded;
        }

        private float _timeToActivate;

        private void OnEnable()
        {
            _timeToActivate = Time.time;
        }

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }
        
        public void Move(Vector2 direction)
        {
            var velocity = Mathf.Clamp(direction.magnitude, 0f, 1);

            if (IsGrounded)
                Animator.SetSpeed(velocity > 0 ? velocity + GetVelocityIncrease() : 0);

            if (!(velocity >= 0.01f)) return;

            var targetAngle = GetTargetAngle(direction);
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                TurnSmoothTime);
            var moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0, angle, 0);
            _characterController.Move(moveDirection * Speed * velocity * Time.deltaTime);
        }
        
        public void MoveInDirection(Vector3 direction, float distance, float speed)
        {
            StartCoroutine(MoveRoutine(direction, distance, speed));
        }

        private IEnumerator MoveRoutine(Vector3 direction, float distance, float speed)
        {
            var traveled = 0f; 

            var normalizedDirection = direction.normalized;

            while (traveled < distance)
            {
                var step = speed * Time.deltaTime;

                if (traveled + step > distance)
                {
                    step = distance - traveled;
                }

                _characterController.Move(normalizedDirection * step);

                traveled += step;

                yield return null;
            }
        }
        
        protected virtual float GetTargetAngle(Vector2 direction)
        {
            return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }

        protected virtual float GetVelocityIncrease()
        {
            return 0;
        }

        private void Fall()
        {
            Velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(Velocity * Time.deltaTime);
            Animator.DoFall();
        }

        private void ResetVelocity()
        {
            if (IsGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }
        }
    }
}