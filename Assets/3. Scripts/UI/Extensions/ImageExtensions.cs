using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Extensions
{
    public static class ImageExtensions
    {
        public static void ScaleImage(this Image image)
        {
            float originalWidth = image.sprite.rect.width;
            float originalHeight = image.sprite.rect.height;

            // Получаем текущие размеры RectTransform
            RectTransform rectTransform = image.transform as RectTransform;
            float currentWidth = rectTransform.rect.width;
            float currentHeight = rectTransform.rect.height;

            // Сохраняем исходное соотношение сторон спрайта
            float originalAspectRatio = originalWidth / originalHeight;

            // Чтобы сохранить пропорции, мы подбираем новый размер для RectTransform
            float newWidth = currentHeight * originalAspectRatio;

            // Устанавливаем новый размер RectTransform с сохранением пропорций
            rectTransform.sizeDelta = new Vector2(newWidth, currentHeight);
        }

        public static void Fade(this Image image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
        
        public static void Fade(this TMP_Text text, float value)
        {
            var color = text.color;
            color.a = value;
            text.color = color;
        }
    }
}