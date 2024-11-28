using System;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Pool;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI.Extensions;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.SwordsPanel
{
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

        private void OnClick()
        {
            OnSelect?.Invoke(this);
        }

        public void Initialize(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);

            Save = save;
            Config = config;

            table.color = rarity.MainColor;
            damageText.text = $"{SwordDamage}";
            icon.texture = config.Icon;

            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }

            for (var i = 0; i < save.starCount; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        public void UpdateItem()
        {
            damageText.text = $"{SwordDamage}";
            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }

            for (var i = 0; i < Save.starCount; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        public void SetDeleteState(bool state)
        {
            ItemToDelete = state;
            deleteItem.gameObject.SetActive(state);
        }

        public void DisableFocus() => focusImage.gameObject.SetActive(false);

        public void SetCurrentFocus()
        {
            focusImage.gameObject.SetActive(true);
            focusImage.color = Color.yellow;
        }

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