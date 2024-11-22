using System;
using YG;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static event Action GameVisibleStateCallback;
        public static event Action GameHiddenStateCallback;
        
        private static void OnVisibilityWindowGame(bool visible)
        {
            if (visible)
                GameVisibleStateCallback?.Invoke();
            else
                GameHiddenStateCallback?.Invoke();
        }
    }
}
