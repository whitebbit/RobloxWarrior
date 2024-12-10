using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Extensions;
using UnityEngine;

namespace _3._Scripts
{
    public abstract class RuntimeObjectIconRenderer<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float threshold = 0.01f; // Порог для определения фона
        [SerializeField] private LayerMask renderLayer; // Слой рендера
        [SerializeField] private Color backgroundColor = Color.magenta;

        [Header("Components")] [SerializeField]
        private Camera renderCamera;

        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] protected Transform itemTransform;

        private readonly Dictionary<string, Texture2D> _textureCache = new();
        private readonly Queue<string> _creationQueue = new();
        private bool _isProcessingQueue;

        /// <summary>
        /// Возвращает текстуру объекта по ID. Если текстуры нет, она добавляется в очередь на создание.
        /// </summary>
        public Texture2D GetTexture(string id)
        {
            if (_textureCache.TryGetValue(id, out var cachedTexture))
                return cachedTexture;

            if (!_creationQueue.Contains(id))
            {
                _creationQueue.Enqueue(id);
                if (!_isProcessingQueue)
                    StartCoroutine(ProcessQueue());
            }

            return null;
        }

        private IEnumerator ProcessQueue()
        {
            _isProcessingQueue = true;

            while (_creationQueue.Count > 0)
            {
                string id = _creationQueue.Dequeue();
                if (!_textureCache.ContainsKey(id))
                {
                    var texture = CreateIconTexture(id);
                    if (texture != null)
                        _textureCache[id] = texture;
                }

                yield return null;
            }

            _isProcessingQueue = false;
        }

        private Texture2D CreateIconTexture(string id)
        {
            T item = SpawnItem(id);

            SetupItemForRendering(item);
            SetupCamera();

            RenderTexture tempRT = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 24,
                RenderTextureFormat.ARGB32);
            renderCamera.targetTexture = tempRT;
            renderCamera.Render();

            Texture2D texture = ExtractTexture(tempRT);
            ApplyTransparency(texture);

            RenderTexture.ReleaseTemporary(tempRT);
            renderCamera.targetTexture = null;

            CleanupItem(item);
            return texture;
        }

        private void SetupItemForRendering(T item)
        {
            item.gameObject.SetLayer(renderLayer);
            item.transform.localPosition = Vector3.zero;
        }

        private void SetupCamera()
        {
            renderCamera.cullingMask = renderLayer;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = backgroundColor;
        }

        private Texture2D ExtractTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = null;
            return texture;
        }

        private void ApplyTransparency(Texture2D texture)
        {
            var pixels = texture.GetPixels32();
            Color32 bgColor32 = backgroundColor; // Преобразование цвета для сравнения

            for (int i = 0; i < pixels.Length; i++)
            {
                if (IsBackgroundColor(pixels[i], bgColor32))
                    pixels[i].a = 0; // Устанавливаем прозрачность
            }

            texture.SetPixels32(pixels);
            texture.Apply();
        }

        private bool IsBackgroundColor(Color32 pixel, Color32 bgColor)
        {
            return Mathf.Abs(pixel.r - bgColor.r) / 255f <= threshold &&
                   Mathf.Abs(pixel.g - bgColor.g) / 255f <= threshold &&
                   Mathf.Abs(pixel.b - bgColor.b) / 255f <= threshold;
        }

        protected abstract T SpawnItem(string id);
        protected abstract void CleanupItem(T item);
    }
}