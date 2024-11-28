using System;
using System.Collections.Generic;
using _3._Scripts.Bots;
using _3._Scripts.Pool;
using _3._Scripts.Worlds.Scriptables;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Worlds
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField] private float distance; // Расстояние между ботами
        [SerializeField] private int maxBotsPerRow = 10; // Максимальное количество ботов в строке

        public List<Bot> SpawnEnemies(WaveData waveData)
        {
            var row = 0; // Индекс текущей строки
            var col = 0; // Индекс текущего столбца
            var list = new List<Bot>();

            // Перебор всех типов ботов в списке
            foreach (var botData in waveData.Bots)
            {
                // Получаем тип бота (config) и количество
                var botConfig = botData.Config;
                var botCount = botData.Count;

                // Определим, сколько строк и колонок будет для этой группы ботов
                var totalRows = Mathf.CeilToInt((float)botCount / maxBotsPerRow); // Считаем количество строк

                // Спавним ботов с расчетом их позиции вокруг стартовой точки
                for (var i = 0; i < botCount; i++)
                {
                    // Вычисляем позицию для текущего бота относительно стартовой точки
                    // Стартовая позиция (transform.position) будет центром, а позиции ботов будут от нее отступать.
                    var spawnPosition = transform.position + new Vector3(
                        (col - maxBotsPerRow / 2f) * distance, // Сдвиг по оси X
                        transform.position.y + botConfig.Size * 1.1f, // Высота остаётся постоянной
                        (row - totalRows / 2f) * distance // Сдвиг по оси Z
                    );

                    // Получаем бота из пула объектов
                    var bot = ObjectsPoolManager.Instance.Get<Bot>();

                    // Инициализируем бота
                    bot.Initialize(botConfig);
                    bot.Upgrade(waveData.DamageIncrease, waveData.HealthIncrease);
                    
                    // Перемещаем его на нужную позицию
                    bot.transform.position = spawnPosition;
                    bot.transform.eulerAngles = new Vector3(0, 180, 0);
                    list.Add(bot);
                    
                    // Увеличиваем индексы столбца и строки
                    col++;

                    // Если столбцы заполнены, переходим на следующую строку
                    if (col < maxBotsPerRow) continue;
                    col = 0;
                    row++;
                }
            }

            return list;
        }
    }
}