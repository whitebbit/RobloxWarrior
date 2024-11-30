﻿using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Extensions
{
    public static class ParticleExtensions
    {
        public static void SetState(this List<ParticleSystem> particleSystems, bool state)
        {
            foreach (var particleSystem in particleSystems)
            {
                if (state)
                    particleSystem.Play();
                else
                    particleSystem.Stop();
            }
            
        }
        public static void SetEmissionRateOverTime(this List<ParticleSystem> particleSystems, int emissionRate)
        {
            foreach (var particleSystem in particleSystems)
            {
                var emission = particleSystem.emission; 
                var rate = emission.rateOverTime;      
                rate.constant = emissionRate;          
                emission.rateOverTime = rate;
            }

        }
        
    }
}