using System.Collections.Generic;
using _3._Scripts.Worlds.Scriptables;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldConfig))]
public class WorldConfigEditor : Editor
{
    private SerializedProperty waves;

    // Переменная для хранения состояния сворачивания/раскрытия всего списка waves
    private bool wavesFoldout = false;

    // Переменные для пагинации
    private int currentPage = 0;
    private const int itemsPerPage = 2;
    private int totalPages = 0;

    // Переменная для поиска волны по номеру
    private string searchWaveNumber = "";

    private void OnEnable()
    {
        waves = serializedObject.FindProperty("waves");
        totalPages = Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Рассчитываем количество страниц
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Waves();
        var worldPrefab = serializedObject.FindProperty("worldPrefab");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(worldPrefab, new GUIContent("World Prefab"));

        serializedObject.ApplyModifiedProperties();
    }

    private void Waves()
    {
        // Кнопка для добавления новой волны
        wavesFoldout = EditorGUILayout.Foldout(wavesFoldout, "Waves");
        if (!wavesFoldout) return; // Если список раскрыт
        // Поиск по номеру волны
        GUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search");
        searchWaveNumber = EditorGUILayout.TextField(searchWaveNumber);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Wave", GUILayout.Height(30)))
        {
            waves.arraySize++; // Увеличиваем размер списка
            var newWave = waves.GetArrayElementAtIndex(waves.arraySize - 1);
            var bots = newWave.FindPropertyRelative("bots");
            bots.arraySize = 0; // Новый список ботов для этой волны будет пустым
            totalPages = Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Пересчитываем количество страниц
        }

        // Кнопка для очистки всего списка
        if (GUILayout.Button("Clear Waves", GUILayout.Height(30)))
        {
            waves.ClearArray(); // Очистить весь список волн
            totalPages = 0; // Сбрасываем количество страниц
            currentPage = 0;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        // Фильтруем волны по поисковому запросу
        var filteredWaves = new List<SerializedProperty>();
        for (var i = 0; i < waves.arraySize; i++)
        {
            if (string.IsNullOrEmpty(searchWaveNumber) || (i + 1).ToString() == searchWaveNumber)
            {
                filteredWaves.Add(waves.GetArrayElementAtIndex(i));
            }
        }

        // Отображаем список волн с учетом пагинации
        GUILayout.BeginVertical("box");

        // Вычисляем, какие элементы отображать на текущей странице из отфильтрованного списка
        var startIndex = currentPage * itemsPerPage;
        var endIndex = Mathf.Min((currentPage + 1) * itemsPerPage, filteredWaves.Count);

        for (var i = startIndex; i < endIndex; i++)
        {
            // Получаем отфильтрованную волну
            var wave = filteredWaves[i];
            var bots = wave.FindPropertyRelative("bots");
            var damageIncrease = wave.FindPropertyRelative("damageIncrease");
            var healthIncrease = wave.FindPropertyRelative("healthIncrease");

            // Если мы ищем по номеру волны, отображаем это число в названии
            var waveLabel = $"Wave {i + 1}";
            if (!string.IsNullOrEmpty(searchWaveNumber))
            {
                waveLabel = $"Wave {searchWaveNumber}";
            }

            // Отображаем кнопку для удаления текущей волны
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(waveLabel, EditorStyles.boldLabel); // Отображаем правильный номер волны
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(damageIncrease, new GUIContent("Damage Increase"));
            EditorGUILayout.PropertyField(healthIncrease, new GUIContent("Health Increase"));
            GUILayout.EndVertical();

            // Отображаем список ботов для этой волны
            GUILayout.BeginVertical();

            // Ensure scroll position is initialized for each wave
            if (!_scrollPosition.ContainsKey(i))
            {
                _scrollPosition.Add(i, Vector2.zero);
            }

            if (bots.arraySize > 0)
            {
                _scrollPosition[i] = EditorGUILayout.BeginScrollView(_scrollPosition[i], GUILayout.Height(112));

                for (var j = 0; j < bots.arraySize; j++)
                {
                    var botSpawnData = bots.GetArrayElementAtIndex(j);
                    var config = botSpawnData.FindPropertyRelative("config");
                    var count = botSpawnData.FindPropertyRelative("count");

                    // Разделитель для каждого BotSpawnData
                    GUILayout.BeginHorizontal("box");
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(config, new GUIContent("Config"));
                    EditorGUILayout.PropertyField(count, new GUIContent("Count"));

                    // Кнопка для удаления конкретного бота
                    EditorGUILayout.EndVertical();
                    if (GUILayout.Button("Delete", GUILayout.Height(39)))
                    {
                        bots.DeleteArrayElementAtIndex(j);
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }

                EditorGUILayout.EndScrollView();
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            // Кнопка для добавления нового бота в текущую волну
            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                bots.arraySize++; // Увеличиваем количество ботов
            }

            if (GUILayout.Button("Clear", GUILayout.Height(30)))
            {
                bots.arraySize = 0; // Увеличиваем количество ботов
            }

            if (GUILayout.Button("Delete", GUILayout.Height(30)))
            {
                // Удаление волны из списка
                waves.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                totalPages =
                    Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Пересчитываем количество страниц
            }

            GUILayout.EndHorizontal();

            if (i < filteredWaves.Count - 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    
            }
        }

        // Пагинация
        GUILayout.EndVertical();
        if (!string.IsNullOrEmpty(searchWaveNumber)) return;
        GUILayout.BeginHorizontal("box");

        EditorGUILayout.LabelField($"Page {currentPage + 1} of {totalPages}");

        if (GUILayout.Button("<", GUILayout.Width(100)) && currentPage > 0)
        {
            currentPage--;
        }

        if (GUILayout.Button(">", GUILayout.Width(100)) && currentPage < totalPages - 1)
        {
            currentPage++;
        }

        GUILayout.EndHorizontal();
    }
    
    private readonly Dictionary<int, Vector2> _scrollPosition = new();
}