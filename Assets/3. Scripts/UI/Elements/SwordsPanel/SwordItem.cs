using System;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.SwordsPanel
{
    /// <summary>
    /// Элемент меча, представляющий информацию о мечах в панели.
    /// </summary>
    public class SwordItem : MonoBehaviour, IPoolable, IInitializable<SwordSave>
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private Image table;
        [SerializeField] private Transform deleteItem;
        [SerializeField] private List<Transform> stars;
        [SerializeField] private Image focusImage;

        private Button _button;

        public SwordSave Save { get; private set; }
        public SwordConfig Config { get; private set; }
        public float SwordDamage => Save.GetDamage(Config.Damage);
        public bool ItemToDelete { get; private set; }
        public event Action<SwordItem> OnSelect;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Обработчик клика по мечу.
        /// </summary>
        private void OnClick()
        {
            OnSelect?.Invoke(this);
        }

        /// <summary>
        /// Инициализация элемента меча с данными сохранения.
        /// </summary>
        public void Initialize(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);

            Save = save;
            Config = config;

            // Установка визуальных характеристик
            table.color = rarity.MainColor;
            damageText.text = $"{SwordDamage}";
            icon.texture = config.Icon;

            // Обновление звезд
            for (var i = 0; i < stars.Count; i++)
            {
                stars[i].gameObject.SetActive(i < save.starCount);
            }
        }

        /// <summary>
        /// Обновление данных меча (например, после объединения).
        /// </summary>
        public void UpdateItem()
        {
            damageText.text = $"{SwordDamage}";

            // Обновление звезд
            for (var i = 0; i < stars.Count; i++)
            {
                stars[i].gameObject.SetActive(i < Save.starCount);
            }
        }

        /// <summary>
        /// Установка состояния для удаления элемента.
        /// </summary>
        public void SetDeleteState(bool state)
        {
            ItemToDelete = state;
            deleteItem.gameObject.SetActive(state);
        }

        /// <summary>
        /// Отключить фокус.
        /// </summary>
        public void DisableFocus() => focusImage.gameObject.SetActive(false);

        /// <summary>
        /// Установить фокус как текущий (желтый цвет).
        /// </summary>
        public void SetCurrentFocus()
        {
            focusImage.gameObject.SetActive(true);
            focusImage.color = Color.yellow;
        }

        /// <summary>
        /// Установить фокус как выбранный (зеленый цвет).
        /// </summary>
        public void SetSelectedFocus()
        {
            focusImage.gameObject.SetActive(true);
            focusImage.color = Color.green;
        }

        public void OnSpawn()
        {
            GBGames.saves.swordsSave.OnDelete += SwordsSaveOnDelete;
        }

        private void SwordsSaveOnDelete(SwordSave obj)
        {
            // Проверка, удаляем ли текущий меч
            if (obj.uid != Save.uid) return;
            ObjectsPoolManager.Instance.Return(this);
        }

        public void OnDespawn()
        {
            GBGames.saves.swordsSave.OnDelete -= SwordsSaveOnDelete;
            DisableFocus();
            deleteItem.gameObject.SetActive(false);
        }
    }
}
