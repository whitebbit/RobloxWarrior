using UnityEngine;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static void Message(string message, LoggerState state = LoggerState.log)
        {
            if (!YandexGame.Instance.infoYG.debug) return;
            
            var prefix = state switch
            {
                LoggerState.log => "<color=green>[LOG]</color>",
                LoggerState.warning => "<color=yellow>[WARNING]</color>",
                LoggerState.error => "<color=red>[ERROR]</color>",
                _ => ""
            };
            Debug.Log($"{prefix} {message}");
        }
    }
}