using System;
using _3._Scripts.Currency;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class UserInfo : UIWidget
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

        protected override void OnOpen()
        {
            base.OnOpen();
            transform.localScale = Vector3.one;
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
            healthText.text = $"{arg1}/{arg2}";
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
            expText.text = $"{(int)(value * 100)}%";
        }

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
    }
}