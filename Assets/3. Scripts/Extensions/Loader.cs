using System;
using System.Collections;
using _3._Scripts.Saves;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace _3._Scripts
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        private void Start()
        {
            GBGamesOnSaveLoadedCallback();
        }

        private void GBGamesOnSaveLoadedCallback()
        {
            StartCoroutine(InitializeLocalization());
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

            if (!IsMemorySufficient()) yield return FreeUpMemory();

            yield return LoadGameSceneAsync();
        }

        private IEnumerator LoadGameSceneAsync()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("MainScene");
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                var progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                if (progressBar != null)
                    progressBar.value = progress;

                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private bool IsMemorySufficient()
        {
            // Проверьте текущее использование памяти
            var totalMemory = GC.GetTotalMemory(false);
            var allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();

            // Установите порог, после которого будет попытка освободить память (например, 500MB)
            const long memoryThreshold = 500 * 1024 * 1024; // 500 MB

            Debug.Log($"Total Managed Memory: {totalMemory}, Total Allocated Memory: {allocatedMemory}");

            return allocatedMemory < memoryThreshold;
        }

        private IEnumerator FreeUpMemory()
        {
            // Выгружаем неиспользуемые ресурсы
            yield return Resources.UnloadUnusedAssets();
            // Запускаем сборщик мусора
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            // Подождите некоторое время, чтобы убедиться, что все очистки завершены
            yield return new WaitForSeconds(0.5f);

            // Повторно проверяем использование памяти
            var allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            Debug.Log($"Memory after cleanup: {allocatedMemory}");
        }
    }
}