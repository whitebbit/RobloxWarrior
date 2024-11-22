using System.Collections.Generic;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static readonly Dictionary<string, string> PlayerWordTranslate = new()
        {
            {"ru", "Игрок"}, {"en", "Player"}, {"fr", "Joueur"}, {"it", "Giocatore"}, {"de", "Spieler"},
            {"es", "Jugador"},
            {"zh", "玩家 "}, {"pt", "Jogador"}, {"ko", "플레이어"}, {"ja", "プレイヤー"}, {"tr", "oyuncu"}, {"ar", "لاعب "},
            {"hi", "खिलाड़ी "}, {"id", "Pemain"},
        };
        
        /// <summary>
        /// Авторизован ли игрок в данный момент.
        /// </summary>
        public static bool isAuthorized => YandexGame.auth;

        /// <summary>
        /// ID игрока. Если авторизация поддерживается на платформе и игрок авторизован в данный момент – возвращает его ID на платформе, иначе — null.
        /// </summary>
        public static string playerID => YandexGame.playerId;

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public static string playerName =>
            string.IsNullOrEmpty(YandexGame.playerName)
                ? PlayerWordTranslate.GetValueOrDefault(language, PlayerWordTranslate["en"])
                : YandexGame.playerName;

        public static string playerPhoto => YandexGame.playerPhoto;
    }
}