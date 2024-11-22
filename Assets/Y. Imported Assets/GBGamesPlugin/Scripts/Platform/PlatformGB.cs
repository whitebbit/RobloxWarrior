using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Если платформа предоставляет данные об языке пользователя — то это будет язык, который установлен у пользователя на платформе. Если не предоставляет — это будет язык браузера пользователя. Формат: ISO 639-1. Пример: ru, en.
        /// </summary>
        public static string language => YandexGame.EnvironmentData.language;

        /// <summary>
        /// С помощью данного параметра можно в ссылку на игру встраивать какую-либо вспомогательную информацию.
        /// </summary>
        public static string payload => YandexGame.EnvironmentData.payload;

        /// <summary>
        /// Информация о домене.
        /// </summary>
        public static string domain => YandexGame.EnvironmentData.domain;

        /// <summary>
        /// Игра загрузилась, все загрузочные экраны прошли, игрок может взаимодействовать с игрой. Yandex
        /// </summary>
        public static void GameReady()
        {
            YandexGame.GameReadyAPI();
            Message($"GameReady");
        }
        
        /// <summary>
        /// Начался геймплей. Например, игрок зашёл в уровень с главного меню. CrazyGames
        /// </summary>
        public static void GameplayStart()
        {
            YandexGame.GameplayStart();
            Message($"GameplayStart");
        }

        /// <summary>
        /// Геймплей закончился/приостановился. Например, при выходе с уровня в главное меню, открытии меню паузы и т.д. CrazyGames
        /// </summary>
        public static void GameplayStop()
        {
            YandexGame.GameplayStop();
            Message($"GameplayStop");
        }
        
    }
}