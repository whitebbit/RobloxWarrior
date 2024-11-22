#if UNITY_WEBGL
using System;
using System.Collections;
using UnityEngine;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames : MonoBehaviour
    {
        private static GBGames _instance;
        private static bool _inGame;

        private void Awake()
        {
            Singleton();
            Leaderboard();
            Advertisement();
            Game();
        }
        
        private void Singleton()
        {
            transform.SetParent(null);
            gameObject.name = "GBGames";

            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private static void Advertisement()
        {
            OnInterstitialStateChanged();
            OnRewardedStateChanged();
        }
        
        private static void Leaderboard()
        {
            YandexGame.onGetLeaderboard += UpdateLeaderboardData;
        }

        private static void Game()
        {
            YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;
        }
    }
}
#endif