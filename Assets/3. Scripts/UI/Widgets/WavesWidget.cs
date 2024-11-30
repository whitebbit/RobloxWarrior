using _3._Scripts.Localization;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using _3._Scripts.Worlds;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Widgets
{
    public class WavesWidget: UIWidget
    {

        [SerializeField] private FadeTransition transition;
        [SerializeField] private LocalizeStringEvent waveText;
        [SerializeField] private Button exitButton;
        
        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        private int _waveNumber;
        public int WaveNumber
        {
            get => _waveNumber;
            set
            {
                _waveNumber = value;
                waveText.SetVariable("value", value);
            }
        }

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            
            exitButton.onClick.AddListener(OnClickExit);
        }
        
        
        private void OnClickExit()
        {
            WorldsManager.Instance.World.StopBattle();
        }
    }
}