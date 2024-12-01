using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Currency;
using _3._Scripts.Extensions;
using _3._Scripts.Extensions.Interfaces;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements.SwordUnlocker;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Transitions;
using _3._Scripts.UI.Widgets;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Swords
{
    public class SwordUnlocker : MonoBehaviour, IInteractive
    {
        [Tab("Main")] [SerializeField] private MeshRenderer eggModel;
        [SerializeField] private Transform eggTransform;
        [SerializeField] private TMP_Text priceText;
        
        [SerializeField] private ScaleTransition uiTransition;
        [Tab("UI")] [SerializeField] private Transform container;
        [SerializeField] private SwordEggItem prefab;

        [Tab("Buttons")] [SerializeField] private Button openButton;
        [SerializeField] private Button autoOpenButton;
        [SerializeField] private Button x3OpenButton;
        private List<SwordConfig> _configs = new();

        private readonly List<SwordEggItem> _items = new();

        private Material _eggMaterial;
        private bool _inProgress;
        private bool _autoOpen;

        private float _price;

        private void Start()
        {
            uiTransition.ForceOut();
            openButton.onClick.AddListener(() => Open(1));
            x3OpenButton.onClick.AddListener(() => Open(3));
            autoOpenButton.onClick.AddListener(AutoOpen);
        }

        public void Initialize(SwordUnlockerData data)
        {
            _configs = data.Swords;
            _eggMaterial = data.EggMaterial;
            _price = data.Price;

            var orderByDescending = _configs.OrderByDescending(i => i.Chance);

            foreach (var config in orderByDescending)
            {
                var item = Instantiate(prefab, container);
                item.Initialize(config);

                _items.Add(item);
            }

            eggModel.material = _eggMaterial;
            priceText.text = $"${_price}<sprite index=1>";
        }

        private void AutoOpen()
        {
            _autoOpen = true;
            Open(1);
        }

        private void Open(int count)
        {
            if (WalletManager.Crystals < _price * count)
            {
                var widget = UIManager.Instance.GetWidget<NotificationWidget>();
                widget.Enabled = true;
                widget.SetText("not_enought_crystals");
                return;
            }

            if (GBGames.saves.swordsSave.unlocked.Count + count > GBGames.saves.swordsSave.maxSwordsCount)
            {
                ShowNotification();
                return;
            }

            if (_inProgress)
                return;

            _inProgress = true;

            var items = UnlockSwords(count);

            ShowPanel(items);
        }

        private List<SwordConfig> UnlockSwords(int count)
        {
            var items = new List<SwordConfig>();

            for (var i = 0; i < count; i++)
            {
                items.Add(_configs.GetRandomElement());
            }

            foreach (var item in from item in items
                     let uiItem = _items.FirstOrDefault(i => i.SwordId == item.ID)
                     where uiItem != null && !uiItem.DestroyOnGet
                     select item)
            {
                GBGames.saves.swordsSave.Unlock(new SwordSave(item.ID));
            }

            return items;
        }

        private void ShowPanel(List<SwordConfig> items)
        {
            UIManager.Instance.SetScreen("3d", onOpenComplete: () =>
            {
                var panel = UIManager.Instance.GetPanel<SwordUnlockerPanel>();

                panel.Enabled = true;

                panel.StartUnlocking(_eggMaterial, items, () =>
                {
                    panel.Enabled = false;
                    UIManager.Instance.SetScreen("main", onOpenComplete: () =>
                    {
                        _inProgress = false;
                        if (_autoOpen)
                            Open(1);
                    });
                });

                if (_autoOpen)
                    panel.EnableAutoOpen(() => _autoOpen = false);
            });
        }

        private void ShowNotification()
        {
            var widget = UIManager.Instance.GetWidget<NotificationWidget>();

            widget.Enabled = true;
            widget.SetText("sword_cout_max");

            _autoOpen = false;
        }

        public void Interact()
        {
            eggTransform.DOScale(0, 0.25f);
            uiTransition.AnimateIn();
        }

        public void StopInteracting()
        {
            uiTransition.AnimateOut();
            eggTransform.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
        }
    }
}