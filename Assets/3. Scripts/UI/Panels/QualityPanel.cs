using _3._Scripts.Extensions;
using _3._Scripts.Extensions.Scriptables;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Transitions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;

namespace _3._Scripts.UI.Panels
{
    public class QualityPanel : SimplePanel
    {
        [Space] [SerializeField] private Button qualityButton;
        [SerializeField] private Button performanceButton;
        [Space] [SerializeField] private QualityConfig qualityConfig;
        [SerializeField] private QualityConfig performanceConfig;
        [SerializeField] private QualityController controller;

    
        public override void Initialize()
        {
            base.Initialize();
            qualityButton.onClick.AddListener(() =>
            {
                controller.SetQuality(qualityConfig);
                Enabled = false;
            });
            performanceButton.onClick.AddListener(() =>
            {
                controller.SetQuality(performanceConfig);
                Enabled = false;
            });
        }
    }
}