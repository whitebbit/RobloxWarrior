﻿using System;
using _3._Scripts.Player;
using _3._Scripts.UI.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Heroes
{
    [Serializable]
    public class PassiveEffect
    {
        [SerializeField] private ModificationType type;
        [SerializeField] private int points;

        public ModificationType Type => type;

        public int Points => points;
        public void ApplyEffect(PlayerStats stats)
        {
            stats.AddAdditionalPoints(type, points);
        }

        public void RemoveEffect(PlayerStats stats)
        {
            stats.ResetAdditionalPoints();
        }
    }
}