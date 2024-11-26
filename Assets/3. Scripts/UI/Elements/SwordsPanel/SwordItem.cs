using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
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

        public int Uid => _save.uid;

        private Button _button;
        private SwordSave _save;

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
        }

        public void Initialize(SwordSave save)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(save.id);
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);

            table.color = rarity.MainColor;
            damageText.text = $"{save.GetDamage(config.Damage)}";
            icon.texture = config.Icon;

            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }

            for (int i = 0; i < save.starCount; i++)
            {
                stars[i].gameObject.SetActive(true);
            }

            _save = save;
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
    }
}