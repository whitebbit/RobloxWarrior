using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Добавить на рабочий стол.
        /// </summary>
        public static void AddToHomeScreen()
        {
            if (!YandexGame.EnvironmentData.promptCanShow) return;
            if (YandexGame.savesData.promptDone) return;

            YandexGame.PromptShow();
        }
        
        /// <summary>
        /// Оценить игру.
        /// </summary>
        public static void Rate()
        {
            if(!YandexGame.EnvironmentData.reviewCanShow) return;
            
            YandexGame.ReviewShow(false);
        }
    }
}