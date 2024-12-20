﻿using System;
using System.Linq;
using _3._Scripts.Animations;
using _3._Scripts.Animations.Structs;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;
using Animancer;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerAnimator : UnitAnimator
    {
        private float _jumpTime;

        private void Start()
        {
            Config = Configuration.Instance.Config.PlayerConfig.AnimationConfig;
        }

        public void DoJump()
        {
            if (!CanPlay()) return;

            _jumpTime = Time.time;
            MainLayer.Play(Config.JumpAnimation, FadeDuration);
        }

        protected override bool CanFall => Time.time - _jumpTime >= Config.JumpAnimation.length;
    }
}