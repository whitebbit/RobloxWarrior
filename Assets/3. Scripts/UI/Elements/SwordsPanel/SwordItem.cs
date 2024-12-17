using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _3._Scripts.UI.Elements.SwordsPanel
{
    /// <summary>
    /// Элемент меча, представляющий информацию о мечах в панели.
    /// </summary>
    public class SwordItem : CollectionItem<SwordSave, SwordItem, RawImage>, IPoolable
    {
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private Image table;
        [SerializeField] private Transform deleteItem;
        [SerializeField] private List<Transform> stars;

        public SwordConfig SwordConfig { get; private set; }
        public float SwordDamage => Config.GetDamage(SwordConfig.Damage);
        public bool ItemToDelete { get; private set; }
        protected override SwordItem Self => this;

        /// <summary>
        /// Инициализация элемента меча с данными сохранения.
        /// </summary>
        public override void Initialize(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);

            Config = save;
            SwordConfig = config;

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
                stars[i].gameObject.SetActive(i < Config.starCount);
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

        public void OnSpawn()
        {
            ResetOnSelect();
            YG2.saves.swordsSave.OnDelete += SwordsSaveOnDelete;
        }

        private void SwordsSaveOnDelete(SwordSave obj)
        {
            // Проверка, удаляем ли текущий меч
            if (obj.uid != Config.uid) return;
            ObjectsPoolManager.Instance.Return(this);
        }

        public void OnDespawn()
        {
            YG2.saves.swordsSave.OnDelete -= SwordsSaveOnDelete;
            DisableFocus();
            deleteItem.gameObject.SetActive(false);
        }
    }
}