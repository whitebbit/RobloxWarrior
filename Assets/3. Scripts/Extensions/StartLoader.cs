using System;
using System.Collections;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using YG;

namespace _3._Scripts.Extensions
{
    public class StartLoader : MonoBehaviour
    {
        private void Start()
        {
            LoadingScreen.Instance.ShowLoadingScreen(InitializeLocalization());
        }
        
        private IEnumerator InitializeLocalization()
        {
            yield return new WaitUntil(() => YandexGame.SDKEnabled);
            yield return LocalizationSettings.InitializationOperation;
            var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == GBGames.language);

            if (locale != null)
            {
                LocalizationSettings.SelectedLocale = locale;
            }

            if (!IsMemorySufficient())
                yield return FreeUpMemory();

            yield return LoadGameSceneAsync();
            yield return new WaitForSeconds(3f);
        }

        private IEnumerator LoadGameSceneAsync()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("MainScene");
            if (asyncOperation == null) yield break;
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private bool IsMemorySufficient()
        {
            GC.GetTotalMemory(false);
            var allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();

            const long memoryThreshold = 500 * 1024 * 1024; // 500 MB


            return allocatedMemory < memoryThreshold;
        }

        private static IEnumerator FreeUpMemory()
        {
            yield return Resources.UnloadUnusedAssets();

            yield return new WaitForSeconds(0.5f);

            UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        }
    }
}