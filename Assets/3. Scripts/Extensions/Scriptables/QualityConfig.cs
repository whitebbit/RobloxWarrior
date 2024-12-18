using System;
using _3._Scripts.Saves.Interfaces;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _3._Scripts.Extensions.Scriptables
{
    [CreateAssetMenu(fileName = "QualityType", menuName = "Configs/Quality/Type", order = 0)]
    public class QualityConfig : ScriptableObject, ISavable
    {
        [SerializeField] private string qualityName;
        [Space]
        [SerializeField] private bool usePostProcessing;
        [SerializeField] private string qualitySettingsName;
        [SerializeField] private UniversalRenderPipelineAsset asset;
        [SerializeField] private LightShadows shadowsType;

        public string ID => qualityName;

        public bool UsePostProcessing => usePostProcessing;

        public string QualitySettingsName => qualitySettingsName;

        public UniversalRenderPipelineAsset Asset => asset;

        public LightShadows ShadowsType => shadowsType;
    }
}