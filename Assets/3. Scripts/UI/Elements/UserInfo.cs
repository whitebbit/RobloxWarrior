using System;
using _3._Scripts.Currency;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class UserInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text playerNameText;

        [Header("Health Bar")] [SerializeField]
        private Slider healthBar;

        [Header("Experience Bar")] [SerializeField]
        private Slider experienceBar;


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
        
        [Button]
        private void AddExperience()
        {
            Player.Player.Instance.Stats.Experience += Player.Player.Instance.Stats.ExperienceToLevelUp() * 5;
        }

        private void OnHealthChanged(float arg1, float arg2)
        {
            var value = arg1 / arg2;

            healthBar.DOValue(value, 0.15f);
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
        }
    }
}