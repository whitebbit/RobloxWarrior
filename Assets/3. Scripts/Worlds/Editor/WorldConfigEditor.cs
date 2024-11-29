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
    private bool swordEggsFoldout = false;

    private SerializedProperty swordEggs;

    // Переменные для пагинации
    private int currentPage = 0;
    private const int itemsPerPage = 2;
    private int totalPages = 0;

    // Переменная для поиска волны по номеру
    private string searchWaveNumber = "";

    private void OnEnable()
    {
        waves = serializedObject.FindProperty("waves");
        swordEggs = serializedObject.FindProperty("swordEggs");
        totalPages = Mathf.CeilToInt((float)waves.arraySize / itemsPerPage); // Рассчитываем количество страниц
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Waves();
        SwordEggs();
        var worldPrefab = serializedObject.FindProperty("worldPrefab");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(worldPrefab, new GUIContent("World Prefab"));

        serializedObject.ApplyModifiedProperties();
    }

    private Vector2 _scrollPositionWave; // Переменная для сохранения позиции скролла

    private readonly Dictionary<int, bool> _waveFoldouts = new();

    private void Waves()
    {
        // Кнопка для добавления новой волны
        wavesFoldout = EditorGUILayout.Foldout(wavesFoldout, "Waves");
        if (!wavesFoldout) return; // Если список свернут, выходим

        EditorGUILayout.BeginVertical("box");

        // Поле для поиска
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search");
        searchWaveNumber = EditorGUILayout.TextField(searchWaveNumber);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Кнопки для добавления новой волны и очистки всех волн
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Wave", GUILayout.Height(30)))
        {
            waves.arraySize++;
            var newWave = waves.GetArrayElementAtIndex(waves.arraySize - 1);
            var bots = newWave.FindPropertyRelative("bots");
            bots.arraySize = 0;
        }

        if (GUILayout.Button("Clear Waves", GUILayout.Height(30)))
        {
            waves.ClearArray();
        }

        GUILayout.EndHorizontal();

        // Создаем список волн с учетом фильтрации
        var filteredWaves = new List<SerializedProperty>();
        for (var i = 0; i < waves.arraySize; i++)
        {
            if (string.IsNullOrEmpty(searchWaveNumber) || (i + 1).ToString() == searchWaveNumber)
            {
                filteredWaves.Add(waves.GetArrayElementAtIndex(i));
            }
        }

        if (waves.arraySize > 0)
        {
            _scrollPositionWave = EditorGUILayout.BeginScrollView(_scrollPositionWave);
        }

        for (var i = 0; i < filteredWaves.Count; i++)
        {
            var wave = filteredWaves[i];
            var bots = wave.FindPropertyRelative("bots");
            var damageIncrease = wave.FindPropertyRelative("damageIncrease");
            var healthIncrease = wave.FindPropertyRelative("healthIncrease");

            // Проверяем инициализацию foldout
            _waveFoldouts.TryAdd(i, true);

            // По умолчанию раскрыто
            // Отображаем foldout для волны
            EditorGUILayout.BeginHorizontal();

            var waveName = string.IsNullOrEmpty(searchWaveNumber) ? $"Wave {i + 1}" : $"Wave {searchWaveNumber}";
            _waveFoldouts[i] = EditorGUILayout.Foldout(_waveFoldouts[i], waveName, true, EditorStyles.foldout);
            if (GUILayout.Button("X", GUILayout.Height(20), GUILayout.Width(20)))
            {
                waves.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                _waveFoldouts.Remove(i); // Удаляем foldout для удалённой волны
                return;
            }

            EditorGUILayout.EndHorizontal();

            if (_waveFoldouts[i])
            {
                GUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(damageIncrease, new GUIContent("Damage Increase"));
                EditorGUILayout.PropertyField(healthIncrease, new GUIContent("Health Increase"));
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");
                if (!_scrollPosition.ContainsKey(i))
                {
                    _scrollPosition.Add(i, Vector2.zero);
                }

                if (bots.arraySize > 0)
                {
                    _scrollPosition[i] = EditorGUILayout.BeginScrollView(_scrollPosition[i], GUILayout.Height(170));

                    for (var j = 0; j < bots.arraySize; j++)
                    {
                        var botSpawnData = bots.GetArrayElementAtIndex(j);
                        var config = botSpawnData.FindPropertyRelative("config");
                        var count = botSpawnData.FindPropertyRelative("count");

                        GUILayout.BeginHorizontal("box");
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(config, new GUIContent("Config"));
                        EditorGUILayout.PropertyField(count, new GUIContent("Count"));
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
                if (GUILayout.Button("Add", GUILayout.Height(30)))
                {
                    bots.arraySize++;
                }

                if (GUILayout.Button("Clear", GUILayout.Height(30)))
                {
                    bots.arraySize = 0;
                }

                GUILayout.EndHorizontal();
            }

            if (i < filteredWaves.Count - 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        if (waves.arraySize > 0)
        {
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical();
    }


    private Vector2 _scrollPositionSwordEggs; // Переменная для сохранения позиции скролла

    private void SwordEggs()
    {
        swordEggsFoldout = EditorGUILayout.Foldout(swordEggsFoldout, "Sword Eggs");
        if (!swordEggsFoldout) return;

        GUILayout.BeginVertical("box");

        // Кнопки добавления и очистки
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Egg", GUILayout.Height(30)))
        {
            swordEggs.arraySize++;
        }

        if (GUILayout.Button("Clear Eggs", GUILayout.Height(30)))
        {
            swordEggs.ClearArray();
        }

        EditorGUILayout.EndHorizontal();

        // Вычисляем высоту области прокрутки

        // Создаем область с прокруткой, если объекты есть
        if (swordEggs.arraySize > 0)
        {
            _scrollPositionSwordEggs =
                EditorGUILayout.BeginScrollView(_scrollPositionSwordEggs);
        }

        // Отображение всех элементов списка SwordEggs
        for (int i = 0; i < swordEggs.arraySize; i++)
        {
            var swordEgg = swordEggs.GetArrayElementAtIndex(i);
            var price = swordEgg.FindPropertyRelative("price");
            var eggMaterial = swordEgg.FindPropertyRelative("eggMaterial");
            var swords = swordEgg.FindPropertyRelative("swords");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Egg {i + 1}", EditorStyles.boldLabel);

            // Поля для price и eggMaterial
            EditorGUILayout.PropertyField(price, new GUIContent("Price"));
            EditorGUILayout.PropertyField(eggMaterial, new GUIContent("Egg Material"));

            // Управление списком Swords
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Swords", EditorStyles.boldLabel);

            // Отображение списка мечей
            for (int j = 0; j < swords.arraySize; j++)
            {
                var sword = swords.GetArrayElementAtIndex(j);

                EditorGUILayout.BeginHorizontal();

                // Поле Sword Config
                EditorGUILayout.PropertyField(sword, new GUIContent($"Sword {j + 1}"), GUILayout.ExpandWidth(true));

                // Кнопка удаления справа от Sword Config
                if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    swords.DeleteArrayElementAtIndex(j);
                    serializedObject.ApplyModifiedProperties();
                    break; // Прерываем вложенный цикл
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();

            // Кнопка добавления нового меча
            if (GUILayout.Button("Add Sword", GUILayout.Height(25)))
            {
                swords.arraySize++;
            }

            // Кнопка удаления текущего Sword Egg
            if (GUILayout.Button("Delete Egg", GUILayout.Height(25)))
            {
                swordEggs.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                break; // Прерываем цикл после удаления
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            if (i < swordEggs.arraySize - 1)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        // Закрываем область с прокруткой, если она была создана
        if (swordEggs.arraySize > 0)
        {
            EditorGUILayout.EndScrollView();
        }

        GUILayout.EndVertical();
    }


    private readonly Dictionary<int, Vector2> _scrollPosition = new();
}