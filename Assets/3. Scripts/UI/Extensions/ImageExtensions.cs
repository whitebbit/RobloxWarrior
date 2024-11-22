using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Extensions
{
    public static class ImageExtensions
    {
        public static void ScaleImage(this Image image)
        {
            var rectTransform = image.transform as RectTransform;
            var originalWidth = image.sprite.rect.width;
            var originalHeight = image.sprite.rect.height;

            if (rectTransform == null) return;

            var sizeDelta = rectTransform.sizeDelta;
            var currentWidth = sizeDelta.x;
            var currentHeight = sizeDelta.y;

            var aspectRatio = originalWidth / originalHeight;

            rectTransform.sizeDelta = currentWidth > currentHeight * aspectRatio
                ? new Vector2(currentHeight * aspectRatio, currentHeight)
                : new Vector2(currentWidth, currentWidth / aspectRatio);
        }

        public static void Fade(this Image image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }
}