using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Bots;
using _3._Scripts.Pool;
using UnityEngine;

namespace _3._Scripts.Worlds
{

    public class BotSpawner : MonoBehaviour
    {
        [SerializeField] private float distanceX; // Расстояние между ботами
        [SerializeField] private float distanceZ; // Расстояние между рядами
        [SerializeField] private List<int> rowSettings; // Настройки для количества ботов в рядах

        public List<Bot> SpawnEnemies(WaveData waveData)
        {
            var list = new List<Bot>();
            int totalBotsSpawned = 0; // Общее количество заспавненных ботов

            // Перебираем типы ботов
            foreach (var botData in waveData.Bots.OrderByDescending(b => b.Config.Health))
            {
                var botConfig = botData.Config;
                var botCount = botData.Count;

                for (int i = 0; i < botCount; i++)
                {
                    // Определяем текущий ряд
                    int currentRow = GetCurrentRow(totalBotsSpawned);

                    // Получаем количество ботов в текущем ряду
                    int botsInRow = GetBotsInRow(currentRow);

                    // Индекс бота в текущем ряду
                    int indexInRow = totalBotsSpawned % botsInRow;

                    // Центрируем ботов в ряду относительно середины
                    float offsetX = (indexInRow - (botsInRow - 1) / 2f) * distanceX;

                    // Определяем Z-смещение для текущего ряда
                    float offsetZ = -currentRow * distanceZ;

                    // Позиция спавна
                    Vector3 spawnPosition = transform.position + new Vector3(offsetX, transform.position.y + botConfig.Size * 1.1f, offsetZ);

                    // Получаем бота из пула
                    var bot = ObjectsPoolManager.Instance.Get<Bot>();

                    // Инициализируем бота
                    bot.Initialize(botConfig);
                   // bot.Upgrade(waveData.DamageIncrease, waveData.HealthIncrease);

                    // Устанавливаем позицию и ориентацию
                    bot.transform.position = spawnPosition;
                    bot.transform.eulerAngles = new Vector3(0, 180, 0);

                    // Добавляем бота в список
                    list.Add(bot);

                    totalBotsSpawned++;
                }
            }

            return list;
        }

        private int GetCurrentRow(int totalBotsSpawned)
        {
            int cumulativeBots = 0;

            for (int i = 0; i < rowSettings.Count; i++)
            {
                cumulativeBots += rowSettings[i];

                if (totalBotsSpawned < cumulativeBots)
                {
                    return i;
                }
            }

            // Если рядов недостаточно, возвращаем последний ряд
            return rowSettings.Count - 1;
        }

        private int GetBotsInRow(int row)
        {
            if (row < rowSettings.Count)
            {
                return rowSettings[row];
            }

            // Если настройка для ряда отсутствует, используем последний ряд
            return rowSettings[^1];
        }
    }
}
