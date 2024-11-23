using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts
{
    public class WebGLSaver : MonoBehaviour
    {
        private void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Application.ExternalEval(@"
                window.onbeforeunload = function() {
                    SendMessage('WebGLSaver', 'SaveGameProgress');
                };
            ");
            }
        }

        public void SaveGameProgress()
        {
            GBGames.Save();
        }
    }
}