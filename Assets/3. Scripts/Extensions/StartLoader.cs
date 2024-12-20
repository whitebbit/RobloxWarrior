using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using YG;

namespace _3._Scripts.Extensions
{
    public class StartLoader : MonoBehaviour
    {
        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            LoadingScreen.Instance.ShowLoadingScreen(InitializeLocalization(), YG2.GameReadyAPI);
        }

        private IEnumerator InitializeLocalization()
        {
            yield return new WaitUntil(() => YG2.isSDKEnabled);
            yield return LocalizationSettings.InitializationOperation;
            var locale =
                LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == YG2.envir.language);

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
            var asyncOperation = SceneManager.LoadSceneAsync(YG2.saves.defaultLoaded ? "MainScene" : "FirstLoad");
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

        private IEnumerator FreeUpMemory()
        {
            yield return Resources.UnloadUnusedAssets();

            yield return new WaitForSeconds(0.5f);

            UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        }
    }
}