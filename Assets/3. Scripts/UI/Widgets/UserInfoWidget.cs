using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Widgets
{
    public class UserInfoWidget : UIWidget
    {
        [SerializeField] private SlideTransition transition;

        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text playerNameText;

        [Header("Health Bar")] [SerializeField]
        private Slider healthBar;

        [SerializeField] private TMP_Text healthText;

        [Header("Experience Bar")] [SerializeField]
        private Slider experienceBar;

        [SerializeField] private TMP_Text expText;

        public override void Initialize()
        {
            transition.SetStartPosition();

            InTransition = transition;
            OutTransition = transition;
        }

        private void Start()
        {
            playerNameText.text = GBGames.playerName;
            OnExperienceChanged(Player.Player.Instance.Stats.Experience);
            OnLevelUp(Player.Player.Instance.Stats.Level);
        }

        private void OnEnable()
        {
            Player.Player.Instance.Stats.OnExperienceChanged += OnExperienceChanged;
            Player.Player.Instance.Stats.OnLevelChange += OnLevelUp;
            Player.Player.Instance.Health.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float arg1, float arg2)
        {
            var value = arg1 / arg2;

            healthBar.DOValue(value, 0.15f);
            healthText.text = $"{(int)arg1}/{(int)arg2}";
        }

        private void OnLevelUp(int obj)
        {
            levelText.text = obj.ToString();
        }

        private void OnExperienceChanged(float obj)
        {
            var toLevelUp = Player.Player.Instance.Stats.ExperienceToLevelUp();
            var value = obj / toLevelUp;

            experienceBar.DOValue(value, 0.15f);
            expText.text = $"{(int)obj}/{(int)toLevelUp}";
        }

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
    }
}