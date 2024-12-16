using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VInspector;
using YG;

namespace _3._Scripts
{
    public class QualityController : MonoBehaviour
    {
        [Tab("Assets")] [SerializeField] private UniversalRenderPipelineAsset pc;
        [SerializeField] private UniversalRenderPipelineAsset mobile;
        [Tab("Components")] [SerializeField] private Light mainLight;
        [SerializeField] private Volume postProcessing;

        private void Start()
        {
            GraphicsSettings.renderPipelineAsset = YG2.envir.device switch
            {
                YG2.Device.Desktop => pc,
                YG2.Device.Mobile => mobile,
                YG2.Device.Tablet => mobile,
                YG2.Device.TV => mobile,
                _ => mobile
            };
            mainLight.shadows = YG2.envir.device switch
            {
                YG2.Device.Desktop => LightShadows.Soft,
                YG2.Device.Mobile => LightShadows.None,
                YG2.Device.Tablet => LightShadows.None,
                YG2.Device.TV => LightShadows.None,
                _ => LightShadows.None
            };
            postProcessing.enabled = YG2.envir.device switch
            {
                YG2.Device.Desktop => true,
                YG2.Device.Mobile => false,
                YG2.Device.Tablet => false,
                YG2.Device.TV => false,
                _ => false
            };
            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(YG2.envir.device switch
            {
                YG2.Device.Desktop => "PC",
                YG2.Device.Mobile => "Mobile",
                YG2.Device.Tablet => "Mobile",
                YG2.Device.TV => "Mobile",
                _ => "Mobile"
            }));
        }
    }
}