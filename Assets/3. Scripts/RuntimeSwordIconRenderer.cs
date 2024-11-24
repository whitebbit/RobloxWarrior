using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts
{
    public class RuntimeSwordIconRenderer : Singleton<RuntimeSwordIconRenderer>
    {
        [SerializeField] private float greenThreshold = 25;
        [SerializeField] private LayerMask layerMask;
        
        public Camera renderCamera;
        public RenderTexture renderTexture;
        public Transform swordTransform;
 
        private readonly Dictionary<string, Texture2D> _textureCache = new();

        public Texture2D GetTexture2D(string id, Material material = null)
        {
            return _textureCache.TryGetValue(id, out var value) ? value : CreateTexture2D(id, material);
        }
        
        private Texture2D CreateTexture2D(string id, Material material)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(id);
            var item = Instantiate(config.Prefab, swordTransform);
            
            item.Disable();
            item.gameObject.SetLayer(layerMask);
            
            renderCamera.cullingMask = layerMask;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = new Color(0, 1, 0, 1); // Полностью прозрачный фон
            
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
                if (pixel.g <= pixel.r + greenThreshold || pixel.g <= pixel.b + greenThreshold) continue;
                pixel.a = 0; 
                pixels[i] = pixel;
            }

            renderedTexture.SetPixels32(pixels);
            renderedTexture.Apply();
            
            _textureCache.TryAdd(id, renderedTexture);
            
            Destroy(item.gameObject);
            return renderedTexture;
        }
    }
}