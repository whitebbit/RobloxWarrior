using _3._Scripts.Player;
using _3._Scripts.UI.Elements.RebirthPanel;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class RebirthPanel : UIPanel
    {
        [SerializeField] private SlideTransition transition;
        [Space] 
        [SerializeField] private RebirthItem upgradeItem;
        [SerializeField] private RebirthItem currentItem;
        [Space] [SerializeField] private Slider progressSlider;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Button rebirthButton;
        
        private PlayerStats Stats => Player.Player.Instance.Stats;
        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
        public override void Initialize()
        {
            transition.SetStartPosition();
            
            InTransition = transition;
            OutTransition = transition;
                
            rebirthButton.onClick.AddListener(OnClick);
            Stats.OnExperienceChanged += _=> UpdateView();
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            UpdateView();
        }

        private void UpdateView()
        {
            upgradeItem.Initialize(Stats.RebirthCount + 1);
            currentItem.Initialize(Stats.RebirthCount);

            progressSlider.value = Stats.Level / (float)Stats.LevelForRebirth;
            progressText.text = $"{Stats.Level}/{Stats.LevelForRebirth}";
        }

        private void OnClick()
        {
            if(Stats.Level < Stats.LevelForRebirth) return; 
            
            GameEvents.Rebirth();
            Stats.Rebirth();
            UpdateView();
        }
    }
}