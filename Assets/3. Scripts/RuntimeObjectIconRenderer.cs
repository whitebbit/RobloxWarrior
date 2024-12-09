using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using UnityEngine;

namespace _3._Scripts
{
    public abstract class RuntimeObjectIconRenderer<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float threshold = 25;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Color backgroundColor = Color.magenta;

        [Header("Components")]
        [SerializeField] private Camera renderCamera;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] protected Transform itemTransform;
        
        private readonly Dictionary<string, Texture2D> _textureCache = new();
        private readonly Queue<string> _creationQueue = new();
        private bool _isProcessingQueue;

        public Texture2D GetTexture2D(string id)
        {
            if (_textureCache.TryGetValue(id, out var cachedTexture))
            {
                return cachedTexture;
            }

            if (!_creationQueue.Contains(id))
            {
                _creationQueue.Enqueue(id);
            }

            if (!_isProcessingQueue)
            {
                StartCoroutine(ProcessCreationQueue());
            }

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

                var texture = CreateTexture2D(id);
                _textureCache.TryAdd(id, texture);

                yield return null;
            }

            _isProcessingQueue = false;
        }

        protected abstract T SpawnItem(string id);
        protected abstract void OnRenderComplete(T item);

        private Texture2D CreateTexture2D(string id)
        {
            var item = SpawnItem(id);

            item.gameObject.SetLayer(layerMask);
            item.transform.localPosition = Vector3.zero;

            renderCamera.cullingMask = layerMask;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = backgroundColor; // Устанавливаем цвет фона

            var tempRT = new RenderTexture(renderTexture.width, renderTexture.height, 24, RenderTextureFormat.ARGB32);
            renderCamera.targetTexture = tempRT;

            renderCamera.Render();

            RenderTexture.active = tempRT;
            var renderedTexture = new Texture2D(tempRT.width, tempRT.height, TextureFormat.RGBA32, false);
            renderedTexture.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            renderedTexture.Apply();

            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            tempRT.Release();

            var pixels = renderedTexture.GetPixels32();
            for (var i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                if (Mathf.Abs(pixel.r / 255f - backgroundColor.r) <= threshold &&
                    Mathf.Abs(pixel.g / 255f - backgroundColor.g) <= threshold &&
                    Mathf.Abs(pixel.b / 255f - backgroundColor.b) <= threshold)
                {
                    pixel.a = 0;
                    pixels[i] = pixel;
                }
            }

            renderedTexture.SetPixels32(pixels);
            renderedTexture.Apply();

            OnRenderComplete(item);

            return renderedTexture;
        }
    }
}