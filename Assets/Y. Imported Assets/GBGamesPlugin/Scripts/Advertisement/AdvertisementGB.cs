using System;
using System.Collections;
using UnityEngine;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool NowAdsShow => YandexGame.nowAdsShow;
        
        #region Banner

        /// <summary>
        /// Показать баннер.
        /// </summary>
        public static void ShowBanner()
        {
            YandexGame.StickyAdActivity(true);
        }

        /// <summary>
        /// Скрыть баннер.
        /// </summary>
        public static void HideBanner()
        {
            YandexGame.StickyAdActivity(false);
        }

        #endregion

        #region Interstitial
        
        /// <summary>
        /// Показать межстраничную рекламу.
        /// </summary>
        public static void ShowInterstitial()
        {
            YandexGame.FullscreenShow();
        }

        private static void OnInterstitialStateChanged()
        {
            YandexGame.OpenFullAdEvent += () => PauseController.Pause(true);
            YandexGame.CloseFullAdEvent += () => PauseController.Pause(false);
            YandexGame.ErrorVideoEvent += () => PauseController.Pause(false);
        }

        #endregion

        #region Rewarded

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(Action onRewarded)
        {
            RewardedCallback = onRewarded;
            YandexGame.RewVideoShow(0);
        }

        private static event Action RewardedCallback;

        private static void OnRewardedStateChanged()
        {
            YandexGame.OpenVideoEvent += () => PauseController.Pause(true);
            YandexGame.CloseVideoEvent += () => PauseController.Pause(false);
            YandexGame.ErrorVideoEvent += () =>
            {
                RewardedCallback = null;
                PauseController.Pause(false);
            };
            YandexGame.RewardVideoEvent += _ =>
            {
                RewardedCallback = null;
                RewardedCallback?.Invoke();
            };
        }

        #endregion
    }
}