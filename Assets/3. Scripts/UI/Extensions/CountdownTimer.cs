using System;
using _3._Scripts.Sounds;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _3._Scripts.UI.Extensions
{
    [Serializable]
    public class CountdownTimer
    {
        [SerializeField] private TMP_Text timerText;
        private int _countdownValue;

        private event Action CountdownComplete;
        private Sequence _countdownSequence;
        public void StartCountdown(Action countdownComplete, int countdownValue = 3)
        {
            _countdownSequence = DOTween.Sequence();
            _countdownValue = countdownValue;
            UpdateTimerText();
            CountdownComplete = countdownComplete;
        
            for (var i = _countdownValue; i > 0; i--)
            {
                _countdownSequence.Append(timerText.DOFade(1, 0.25f)) // Появление текста
                    .Append(timerText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBack)) // Увеличение текста
                    .AppendInterval(0.25f) // Пауза перед исчезновением
                    .Append(timerText.DOFade(0, 0.25f)) // Исчезновение текста
                    .Join(timerText.transform.DOScale(1f, 0.25f).SetEase(Ease.InBack)) // Возвращение к исходному размеру
                    .AppendCallback(() => _countdownValue--) // Уменьшение значения таймера
                    .AppendCallback(UpdateTimerText); // Обновление текста
            }

            _countdownSequence.Append(timerText.DOFade(1, 0.25f)) // Появление нуля
                .Append(timerText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBack))
                .AppendInterval(0.5f)
                .Append(timerText.DOFade(0, 0.25f)) // Исчезновение нуля
                .Join(timerText.transform.DOScale(1f, 0.25f).SetEase(Ease.InBack))
                .AppendCallback(OnCountdownComplete); // Завершение таймера
        }

        public void StopCountdown()
        {
            _countdownSequence?.Pause();
            _countdownSequence?.Kill();
            timerText.DOFade(0, 0.25f);
        }
        
        private void UpdateTimerText()
        {
            timerText.text = _countdownValue.ToString();
            AudioManager.Instance.PlaySound("timer");
        }

        private void OnCountdownComplete()
        {
            timerText.text = "0";
            CountdownComplete?.Invoke();
        }
    }
}