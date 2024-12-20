﻿using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Units;
using Animancer;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotAnimator : UnitAnimator, IInitializable<BotConfig>
    {
        protected override bool CanFall => !Grounded;

        public void Initialize(BotConfig config)
        {
            Config = config.AnimationConfig;
        }
    }
}