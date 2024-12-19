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
            var configName = "";
            if (string.IsNullOrEmpty(YG2.saves.qualityName))
            {
                configName = YG2.envir.device == YG2.Device.Desktop ? "quality" : "performance";
            }
            else
            {
                configName = YG2.saves.qualityName;
            }

            var config =
                Configuration.Instance.Config.QualityConfigs.FirstOrDefault(q => q.ID == configName);

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
            yield return new WaitForSeconds(1f);
            for (var i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(1f);
                var fps = 1.0f / Time.deltaTime;
                if (fps > 30) continue;

                SetQuality(Configuration.Instance.Config.QualityConfigs.FirstOrDefault(q => q.ID == "performance"));
                break;
            }
        }
    }
}