using UnityEditor;
using UnityEngine;
using _3._Scripts.Worlds.Scriptables;
using System.Collections.Generic;

public class WavesEditorWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty waves;
    private int currentPage = 0;
    private const int itemsPerPage = 2;
    private int totalPages = 0;
    private string searchWaveNumber = "";

    // Состояние сворачивания всех волн
    private bool wavesFoldout = true;

    // Переменная для поиска
    private Vector2 scrollPosition;

    [MenuItem("Window/Waves Editor")]
    public static void OpenWindow()
    {
        WavesEditorWindow window = GetWindow<WavesEditorWindow>("Waves Editor");
        window.Show();
    }

    public void Initialize(SerializedObject serializedObject)
    {
        this.serializedObject = serializedObject;
        waves = serializedObject.FindProperty("waves");
        totalPages = Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Рассчитываем количество страниц
    }

    private void OnGUI()
    {
        serializedObject.Update();

        // Поиск по номеру волны
        GUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search");
        searchWaveNumber = EditorGUILayout.TextField(searchWaveNumber);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        // Кнопка для добавления новой волны
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

        // Фильтруем волны по поисковому запросу
        List<SerializedProperty> filteredWaves = new List<SerializedProperty>();
        for (int i = 0; i < waves.arraySize; i++)
        {
            if (string.IsNullOrEmpty(searchWaveNumber) || (i + 1).ToString() == searchWaveNumber)
            {
                filteredWaves.Add(waves.GetArrayElementAtIndex(i));
            }
        }

        // Отображаем список волн с учетом пагинации
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical("box");

        // Вычисляем, какие элементы отображать на текущей странице
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min((currentPage + 1) * itemsPerPage, filteredWaves.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            SerializedProperty wave = filteredWaves[i];
            SerializedProperty bots = wave.FindPropertyRelative("bots");
            SerializedProperty crystalAmount = wave.FindPropertyRelative("crystalAmount");

            // Отображаем информацию о текущей волне
            string waveLabel = $"Wave {i + 1}";
            if (!string.IsNullOrEmpty(searchWaveNumber))
            {
                waveLabel = $"Wave {searchWaveNumber}";
            }

            EditorGUILayout.LabelField(waveLabel, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(crystalAmount, new GUIContent("Crystals Amount"));

            // Отображаем список ботов для этой волны
            EditorGUILayout.BeginVertical();
            if (bots.arraySize > 0)
            {
                for (int j = 0; j < bots.arraySize; j++)
                {
                    SerializedProperty botSpawnData = bots.GetArrayElementAtIndex(j);
                    SerializedProperty config = botSpawnData.FindPropertyRelative("config");
                    SerializedProperty count = botSpawnData.FindPropertyRelative("count");

                    GUILayout.BeginHorizontal("box");
                    EditorGUILayout.PropertyField(config, new GUIContent("Config"));
                    EditorGUILayout.PropertyField(count, new GUIContent("Count"));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            // Кнопки для добавления нового бота или удаления текущей волны
            if (GUILayout.Button("Add Bot", GUILayout.Height(30)))
            {
                bots.arraySize++;
            }

            if (GUILayout.Button("Clear Bots", GUILayout.Height(30)))
            {
                bots.arraySize = 0;
            }

            if (GUILayout.Button("Delete Wave", GUILayout.Height(30)))
            {
                waves.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                totalPages = Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Пересчитываем количество страниц
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        // Пагинация
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Page {currentPage + 1} of {totalPages}");

        if (GUILayout.Button("<", GUILayout.Width(50)) && currentPage > 0)
        {
            currentPage--;
        }

        if (GUILayout.Button(">", GUILayout.Width(50)) && currentPage < totalPages - 1)
        {
            currentPage++;
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
