using UnityEngine;

namespace GBGamesPlugin
{
    public static class PauseController
    {
        private static bool _audioPause;
        private static float _timeScale = 1;
        private static CursorLockMode _cursorLockMode = CursorLockMode.None;
        private static bool _cursorVisible = true;

        public static void Pause(bool state)
        {
            if (state)
            {
                GBGames.GameplayStop();
                
                _audioPause = AudioListener.pause;
                _timeScale = Time.timeScale;
                _cursorLockMode = Cursor.lockState;
                _cursorVisible = Cursor.visible;

                AudioListener.pause = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = _timeScale;
                AudioListener.pause = _audioPause;
                Cursor.lockState = _cursorLockMode;
                Cursor.visible = _cursorVisible;
                
                GBGames.GameplayStart();
            }
        }
    }
}