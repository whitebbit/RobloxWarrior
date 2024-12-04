using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts
{
    public class RuntimeSwordIconRenderer : Singleton<RuntimeSwordIconRenderer>
    {
        [SerializeField] private float threshold = 25;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Color backgroundColor = Color.magenta;

        public Camera renderCamera;
        public RenderTexture renderTexture;
        public Transform swordTransform;

        private readonly Dictionary<string, Texture2D> _textureCache = new();
        private readonly Queue<string> _creationQueue = new();
        private bool _isProcessingQueue;

        public Texture2D GetTexture2D(string id)
        {
            // Проверяем, есть ли текстура в кэше
            if (_textureCache.TryGetValue(id, out var cachedTexture))
            {
                return cachedTexture;
            }

            // Если текстуры нет, добавляем её в очередь
            if (!_creationQueue.Contains(id))
            {
                _creationQueue.Enqueue(id);
            }

            // Если очередь не обрабатывается, запускаем обработку
            if (!_isProcessingQueue)
            {
                StartCoroutine(ProcessCreationQueue());
            }

            // Возвращаем временное значение (null или заглушка)
            return null;
        }

        private IEnumerator ProcessCreationQueue()
        {
            _isProcessingQueue = true;

            while (_creationQueue.Count > 0)
            {
                var id = _creationQueue.Dequeue();
                if (_textureCache.ContainsKey(id))
                {
                    continue;
                }

                // Создание текстуры
                var texture = CreateTexture2D(id);
                _textureCache.TryAdd(id, texture);

                // Ждём один кадр для избежания накладок
                yield return null;
            }

            _isProcessingQueue = false;
        }

        private Texture2D CreateTexture2D(string id)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(id);

            // Создаём временный объект для рендеринга
            var item = Instantiate(config.Prefab, swordTransform);

            // Подготавливаем объект для рендеринга
            item.Disable();
            item.gameObject.SetLayer(layerMask);
            item.transform.localPosition = Vector3.zero;

            // Настраиваем камеру
            renderCamera.cullingMask = layerMask;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = backgroundColor; // Устанавливаем цвет фона

            // Создаём временный RenderTexture
            var tempRT = new RenderTexture(renderTexture.width, renderTexture.height, 24, RenderTextureFormat.ARGB32);
            renderCamera.targetTexture = tempRT;

            // Рендеринг
            renderCamera.Render();

            // Читаем пиксели из временного RenderTexture
            RenderTexture.active = tempRT;
            var renderedTexture = new Texture2D(tempRT.width, tempRT.height, TextureFormat.RGBA32, false);
            renderedTexture.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            renderedTexture.Apply();

            // Очищаем RenderTexture
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            tempRT.Release();

            // Удаляем указанный фон
            var pixels = renderedTexture.GetPixels32();
            for (var i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                // Проверяем, насколько цвет пикселя близок к фоновому цвету
                if (Mathf.Abs(pixel.r / 255f - backgroundColor.r) <= threshold &&
                    Mathf.Abs(pixel.g / 255f - backgroundColor.g) <= threshold &&
                    Mathf.Abs(pixel.b / 255f - backgroundColor.b) <= threshold)
                {
                    pixel.a = 0; // Убираем пиксель
                    pixels[i] = pixel;
                }
            }

            renderedTexture.SetPixels32(pixels);
            renderedTexture.Apply();

            // Удаляем временные объекты
            Destroy(item.gameObject);

            return renderedTexture;
        }
    }
}