using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Возвращает тип девайса, с которого пользователь запустил игру. Возможные значения: Mobile, Tablet, Desktop, TV.
        /// </summary>
        public static string deviceType => YandexGame.EnvironmentData.deviceType;
    }
}