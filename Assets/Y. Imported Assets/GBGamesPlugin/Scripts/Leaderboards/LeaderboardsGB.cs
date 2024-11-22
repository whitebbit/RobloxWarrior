using System.Collections.Generic;
using YG;
using YG.Utils.LB;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static Dictionary<string, LBData> _leaderboardData = new();

        /// <summary>
        /// Записать очки игрока.
        /// </summary>
        public static void NewLeaderboardScores(string leaderboardName, int score)
        {
            YandexGame.NewLeaderboardScores(leaderboardName, score);
            GetLeaderboard(leaderboardName);
        }

        /// <summary>
        /// Получение данных лидерборда. Последний параметр photoSizeLB может быть "nonePhoto", "small", "medium" и "large"
        /// </summary>
        public static void GetLeaderboard(string leaderboardName, int maxQuantityPlayers = 10, int quantityTop = 3,
            int quantityAround = 3, string photoSizeLb = "small")
        {
            YandexGame.GetLeaderboard(leaderboardName, maxQuantityPlayers, quantityTop, quantityAround, photoSizeLb);
        }

        public static LBData GetLeaderboardData(string leaderboardName)
        {
            return _leaderboardData.ContainsKey(leaderboardName) ? _leaderboardData[leaderboardName] : null;
        }

        private static void UpdateLeaderboardData(LBData data)
        {
            if (_leaderboardData.ContainsKey(data.technoName))
                _leaderboardData[data.technoName] = data;
            else
                _leaderboardData.Add(data.technoName, data);
        }
    }
}