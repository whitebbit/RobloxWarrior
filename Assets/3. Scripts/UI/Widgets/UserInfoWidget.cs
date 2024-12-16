using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

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


        [Button]
        private void AddExperience()
        {
            Player.Player.Instance.Stats.Experience += (float)Player.Player.Instance.Stats.ExperienceToLevelUp();
        }

        public override void Initialize()
        {
            transition.SetStartPosition();

            InTransition = transition;
            OutTransition = transition;

            Player.Player.Instance.Stats.OnExperienceChanged += OnExperienceChanged;
            Player.Player.Instance.Stats.OnLevelChange += OnLevelUp;
            Player.Player.Instance.Health.OnHealthChanged += OnHealthChanged;
        }

        private void Start()
        {
            playerNameText.text = GBGames.playerName;
            OnExperienceChanged(Player.Player.Instance.Stats.Experience);
            OnLevelUp(Player.Player.Instance.Stats.Level);
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            var value = currentHealth / maxHealth;

            healthBar.DOValue(value, 0.15f);
            healthText.text = $"{(int)currentHealth}/{(int)maxHealth}";
        }

        private void OnLevelUp(int level)
        {
            levelText.text = level.ToString();
        }

        private void OnExperienceChanged(float experience)
        {
            var toLevelUp = Player.Player.Instance.Stats.ExperienceToLevelUp();
            var value = experience / toLevelUp;

            experienceBar.DOValue((float)value, 0.15f);
            expText.text = $"{(int)experience}/{(int)toLevelUp}";
        }

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
    }
}