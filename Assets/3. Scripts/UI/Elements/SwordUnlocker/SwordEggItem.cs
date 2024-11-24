using System;
using _3._Scripts.Config;
using _3._Scripts.Config.Interfaces;
using _3._Scripts.Extensions;
using _3._Scripts.Swords.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements.SwordUnlocker
{
    public class SwordEggItem : MonoBehaviour, IInitializable<SwordConfig>
    {
        [SerializeField] private RawImage icon;
        [SerializeField] private TMP_Text chanceText;
        [SerializeField] private Image table;
        [SerializeField] private Transform blockItem;
        
        private Button _button;
        public bool DestroyOnGet {get; private set;}
        public string SwordId {get; private set;}
        private void Awake()
        {
            _button = GetComponent<Button>();
            Debug.Log(_button);
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            blockItem.gameObject.SetActive(DestroyOnGet);
        }

        private void OnClick()
        {
            DestroyOnGet = !DestroyOnGet;
            blockItem.gameObject.SetActive(DestroyOnGet);
        }
        
        public void Initialize(SwordConfig config)
        {
            var rarity = Configuration.Instance.GetRarityTable(config.Rarity);
            
            table.color = rarity.MainColor;
            chanceText.text = $"{config.Chance}%";
            icon.texture = config.Icon;

            SwordId = config.ID;
        }
    }
}