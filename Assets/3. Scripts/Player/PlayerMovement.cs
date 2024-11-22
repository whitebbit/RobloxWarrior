using System;
using _3._Scripts.Config;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;
using Cinemachine;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public sealed class PlayerMovement : UnitMovement<PlayerAnimator>
    {
        private Transform _camera;

        private MovementConfig Config => Configuration.Instance.Config.PlayerConfig.MovementConfig;
        protected override float Speed => Config.BaseSpeed + Player.Instance.Stats.SpeedImprovement;

        private void Start()
        {
            if (Camera.main is not null) _camera = Camera.main.transform;
        }

        protected override float GetTargetAngle(Vector2 direction)
        {
            return base.GetTargetAngle(direction) + _camera.eulerAngles.y;
        }

        protected override float GetVelocityIncrease()
        {
            return (Speed - Config.BaseSpeed) / Config.BaseSpeed;
        }

        public void Jump()
        {
            if (!IsGrounded) return;

            Velocity.y = Mathf.Sqrt(Config.JumpHeight * -2 * Gravity);
            Animator.DoJump();
        }
        
        
    }
}