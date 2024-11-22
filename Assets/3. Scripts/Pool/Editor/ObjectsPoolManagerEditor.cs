using _3._Scripts.Pool.Interfaces;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace _3._Scripts.Pool.Editor
{
    //TODO: сделать +5 +10 +50 +100 для количества и добавить подпись
    [CustomEditor(typeof(ObjectsPoolManager))]
    public class ObjectsPoolManagerEditor : UnityEditor.Editor
    {
        private string searchQuery = ""; // Строка для хранения поискового запроса

        // Флаги для отслеживания направления сортировки
        private bool sortByNameAscending = true;
        private bool sortBySizeAscending = true;

        public override void OnInspectorGUI()
        {
            var container = (ObjectsPoolManager)target;

            // Панель поиска с улучшенным дизайном
            GUILayout.BeginVertical("box");
            GUILayout.Label("Search", EditorStyles.boldLabel);
            searchQuery = GUILayout.TextField(searchQuery, GetSearchFieldStyle());
            GUILayout.EndVertical();

            // Панель с кнопками для добавления, очистки и сортировки
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();

            // Кнопки "Add" и "Clear" слева
            GUIContent addButtonContent = new GUIContent("Add", "Click to add a new pool object");
            if (GUILayout.Button(addButtonContent, GUILayout.Height(35), GUILayout.MaxWidth(75)))
            {
                container.Pools.Insert(0, new PoolCategory()); // Вставляем новый объект в начало списка
            }

            GUIContent clearButtonContent = new GUIContent("Clear", "Click to clear all pool objects.");
            if (GUILayout.Button(clearButtonContent, GUILayout.Height(35), GUILayout.MaxWidth(75)))
            {
                container.Pools.Clear();
            }

            GUILayout.FlexibleSpace(); // Пространство между кнопками и сортировкой

            // Заголовок "Sort By:" и кнопки для сортировки списка
            GUILayout.Label("Sort By:", GUILayout.Height(35));

            // Кнопка сортировки по Name
            GUIContent nameSortButtonContent = new GUIContent(
                sortByNameAscending ? "Name ↑" : "Name ↓",
                "Click to sort the pools by Prefab Name in ascending or descending order."
            );
            if (GUILayout.Button(nameSortButtonContent, GUILayout.Height(35), GUILayout.MaxWidth(75)))
            {
                container.Pools = sortByNameAscending
                    ? container.Pools.OrderBy(pool => pool.Prefab != null ? pool.Prefab.name : string.Empty).ToList()
                    : container.Pools.OrderByDescending(pool => pool.Prefab != null ? pool.Prefab.name : string.Empty)
                        .ToList();

                // Переключаем флаг направления сортировки
                sortByNameAscending = !sortByNameAscending;
            }

            // Кнопка сортировки по Size
            GUIContent sizeSortButtonContent = new GUIContent(
                sortBySizeAscending ? "Size ↑" : "Size ↓",
                "Click to sort the pools by Initial Size in ascending or descending order."
            );
            if (GUILayout.Button(sizeSortButtonContent, GUILayout.Height(35), GUILayout.MaxWidth(75)))
            {
                container.Pools = sortBySizeAscending
                    ? container.Pools.OrderBy(pool => pool.InitialSize).ToList()
                    : container.Pools.OrderByDescending(pool => pool.InitialSize).ToList();

                // Переключаем флаг направления сортировки
                sortBySizeAscending = !sortBySizeAscending;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            List<string> prefabNames = container.Pools.Select(pool => pool.Prefab != null ? pool.Prefab.name : null).ToList();

            GUILayout.BeginVertical("box");
            for (var i = 0; i < container.Pools.Count; i++)
            {
                var poolItem = container.Pools[i];
                var prefabName = poolItem.Prefab != null ? poolItem.Prefab.name : "No Prefab";

                // Фильтрация элементов на основе поискового запроса
                if (!string.IsNullOrEmpty(searchQuery) && !prefabName.ToLower().Contains(searchQuery.ToLower()))
                {
                    continue; // Пропускаем элементы, не соответствующие поисковому запросу
                }

                List<string> errorMessages = new List<string>();

                GUILayout.BeginHorizontal();

                // Check for duplicate prefabs
                if (poolItem.Prefab != null && prefabNames.Count(x => x == prefabName) > 1)
                {
                    errorMessages.Add($"Duplicate prefab: {prefabName}");
                }

                
                // Превью префаба
                if (poolItem.Prefab != null)
                {
                    var previewTexture = AssetPreview.GetAssetPreview(poolItem.Prefab);
                    if (previewTexture != null)
                    {
                        GUILayout.Label(previewTexture, GUILayout.Width(75), GUILayout.Height(75));
                    }
                }
                else
                {
                    // Пустое место, если префаб не выбран, добавляем поддержку перетаскивания
                    GUILayout.BeginVertical(GUILayout.Width(75), GUILayout.Height(75));
                    GUIStyle emptyStyle = new GUIStyle(GUI.skin.box)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 12,
                        fontStyle = FontStyle.Italic,
                        normal = { textColor = Color.gray }
                    };
                    GUILayout.Label("Drag Prefab Here", emptyStyle, GUILayout.Width(75), GUILayout.Height(75));
                    GUILayout.EndVertical();

                    // Обработка перетаскивания
                    Rect dragAreaRect =
                        GUILayoutUtility.GetLastRect(); // Получаем прямоугольник области для перетаскивания

                    // Проверяем, что префаб перетаскивается в эту область
                    if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
                    {
                        if (dragAreaRect.Contains(Event.current.mousePosition)) // Если курсор в этой области
                        {
                            DragAndDrop.visualMode =
                                DragAndDropVisualMode.Generic; // Меняем визуальный режим перетаскивания

                            if (Event.current.type == EventType.DragPerform)
                            {
                                DragAndDrop.AcceptDrag(); // Принимаем перетаскиваемый объект

                                // Если это префаб, назначаем его в этот объект
                                if (DragAndDrop.objectReferences.Length > 0 &&
                                    DragAndDrop.objectReferences[0] is GameObject draggedPrefab)
                                {
                                    poolItem.Prefab = draggedPrefab; // Применяем префаб к текущему элементу
                                    Event.current.Use(); // Прекращаем дальнейшую обработку события
                                }
                            }
                        }
                        else
                        {
                            DragAndDrop.visualMode =
                                DragAndDropVisualMode.Rejected; // Отклоняем перетаскивание, если не в области
                        }
                    }
                }

                GUILayout.BeginVertical();

                // Название префаба над полем Initial Size
                GUILayout.Label(prefabName, EditorStyles.boldLabel);

                // Поле Initial Size с ползунком
                GUIContent sizeSliderContent = new GUIContent("Initial Size", "Set the initial size for the pool.");
                poolItem.InitialSize = EditorGUILayout.IntField(sizeSliderContent, poolItem.InitialSize /*, 1, 1000*/);
         

                // Горизонтальный контейнер для кнопок
                GUILayout.BeginHorizontal();

                // Кнопка для открытия префаба
                GUIContent openButtonContent =
                    new GUIContent("Open", "Click to open the selected prefab in the editor.");
                if (GUILayout.Button(openButtonContent, GUILayout.Height(36),
                        GUILayout.ExpandWidth(true))) // Используем GUILayout.ExpandWidth(true)
                {
                    if (poolItem.Prefab != null)
                    {
                        // Открываем префаб в редакторе
                        AssetDatabase.OpenAsset(poolItem.Prefab);
                    }
                    else
                    {
                        Debug.LogWarning("Prefab is null. Cannot open.");
                    }
                }

                // Кнопка для очистки текущего префаба
                GUIContent clearPrefabButtonContent =
                    new GUIContent("Remove", "Click to remove the current prefab.");
                if (GUILayout.Button(clearPrefabButtonContent, GUILayout.Height(36),
                        GUILayout.ExpandWidth(true))) // Используем GUILayout.ExpandWidth(true)
                {
                    poolItem.Prefab = null; // Очистка текущего префаба
                }

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                // Кнопка для удаления (крестик)
                GUIContent removeButtonContent = new GUIContent("X", "Click to remove this pool object");
                if (GUILayout.Button(removeButtonContent, GUILayout.Height(73), GUILayout.Width(35)))
                {
                    container.Pools.RemoveAt(i);
                    break; // Прерываем цикл, чтобы не перебирать удалённый элемент
                }

                GUILayout.EndHorizontal();

                // Если есть ошибки, выводим их перед полем Prefab
                if (errorMessages.Count > 0)
                {
                    foreach (var errorMessage in errorMessages)
                    {
                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    }
                }

                // Проверка на компонент IPoolable
                if (poolItem.Prefab != null)
                {
                    var myComponent = poolItem.Prefab.GetComponent<IPoolable>();
                    if (myComponent == null)
                    {
                        EditorGUILayout.HelpBox("Prefab does not implement IPoolable.", MessageType.Error);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Prefab is null.", MessageType.Error);
                }

                // Разделительная линия между элементами списка
                if (i < container.Pools.Count - 1)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
            }

            GUILayout.EndVertical();

            // Обновление объекта
            if (GUI.changed)
            {
                EditorUtility.SetDirty(container);
            }
        }

        // Стиль для поля поиска
        private GUIStyle GetSearchFieldStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 14,
                normal = { textColor = Color.black },
                border = new RectOffset(2, 2, 2, 2), // Добавляем рамку
                padding = new RectOffset(10, 10, 5, 5), // Паддинги для внутренностей поля
                margin = new RectOffset(0, 0, 5, 5) // Отступы снаружи
            };
            return style;
        }
    }
}