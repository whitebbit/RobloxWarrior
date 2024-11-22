using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Animations.Structs
{
    [Serializable]
    public struct AttackAnimation
    {
        public AnimationClip clip;
        public List<float> eventTimes;
    }
}