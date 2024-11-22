using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static SavesYG saves => YandexGame.savesData;
        
        #region Save

        ///<summary>
        /// Сохранение данных.
        /// </summary>
        public static void Save()
        {
           YandexGame.SaveProgress();
        }

        #endregion

        #region Delete

        public static void Delete()
        {
            YandexGame.ResetSaveProgress();
        }

        #endregion
    }
}