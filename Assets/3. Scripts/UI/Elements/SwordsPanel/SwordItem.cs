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
    public class SwordItem : CollectionItem<SwordSave, SwordItem>, IPoolable
    {
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private Image table;
        [SerializeField] private Transform deleteItem;
        [SerializeField] private List<Transform> stars;
        
        public SwordSave Save { get; private set; }
        public SwordConfig SwordConfig { get; private set; }
        public float SwordDamage => Save.GetDamage(SwordConfig.Damage);
        public bool ItemToDelete { get; private set; }
        protected override SwordItem Self => this;

        /// <summary>
        /// Инициализация элемента меча с данными сохранения.
        /// </summary>
        public override void Initialize(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);

            Save = save;
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
