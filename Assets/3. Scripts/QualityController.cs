using System.Linq;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VInspector;

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
            GraphicsSettings.renderPipelineAsset = GBGames.deviceType switch
            {
                "mobile" => mobile,
                "desktop" => pc,
                _ => mobile
            };
            mainLight.shadows = GBGames.deviceType switch
            {
                "mobile" => LightShadows.None,
                "desktop" => LightShadows.Soft,
                _ => LightShadows.None
            };
            postProcessing.enabled = GBGames.deviceType switch
            {
                "mobile" => false,
                "desktop" => true,
                _ => false
            };
            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(GBGames.deviceType switch
            {
                "mobile" => "Mobile",
                "desktop" => "PC",
                _ => "Mobile"
            }));
        }
    }
}