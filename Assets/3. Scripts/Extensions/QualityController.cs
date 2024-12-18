using System;
using System.Collections;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Extensions.Scriptables;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VInspector;
using YG;

namespace _3._Scripts.Extensions
{
    public class QualityController : MonoBehaviour
    {
        [SerializeField] private bool constantControl;
        [SerializeField] private Light mainLight;
        [SerializeField] private Volume postProcessing;
        
        private void Start()
        {
            var config =
                Configuration.Instance.Config.QualityConfigs.FirstOrDefault(q => q.ID == YG2.saves.qualityName);

            SetQuality(config);
            if (constantControl)
                StartCoroutine(ControlQuality());
        }

        public void SetQuality(QualityConfig config)
        {
            GraphicsSettings.renderPipelineAsset = config.Asset;
            if (mainLight)
                mainLight.shadows = config.ShadowsType;
            if (postProcessing)
                postProcessing.enabled = config.UsePostProcessing;
            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(config.QualitySettingsName));
            YG2.saves.qualityName = config.ID;
        }

        private IEnumerator ControlQuality()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(2.5f);
                var fps = 1.0f / Time.deltaTime;
                if (!(fps < 45)) continue;
                
                SetQuality(Configuration.Instance.Config.QualityConfigs.FirstOrDefault(q => q.ID == "performance"));
                break;
            }
        }
    }
}