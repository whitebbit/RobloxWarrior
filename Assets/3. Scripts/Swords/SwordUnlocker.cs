using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Extensions;
using _3._Scripts.Saves;
using _3._Scripts.Swords.Scriptables;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements.SwordUnlocker;
using _3._Scripts.UI.Panels;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Swords
{
    public class SwordUnlocker : MonoBehaviour
    {
        [Tab("Main")] [SerializeField] private Transform container;
        [SerializeField] private SwordEggItem prefab;

        [Tab("Buttons")] [SerializeField] private Button openButton;
        [SerializeField] private Button autoOpenButton;
        [SerializeField] private Button x3OpenButton;

        [SerializeField] private List<SwordConfig> _configs = new();

        private readonly List<SwordEggItem> _items = new();

        private bool _inProgress;
        private bool _autoOpen;

        private void Start()
        {
            Initialize(_configs);
            openButton.onClick.AddListener(() => Open(1));
            x3OpenButton.onClick.AddListener(() => Open(3));
            autoOpenButton.onClick.AddListener(AutoOpen);
        }

        public void Initialize(List<SwordConfig> configs)
        {
            _configs = configs;
            var orderByDescending = _configs.OrderByDescending(i => i.Chance);

            foreach (var config in orderByDescending)
            {
                var item = Instantiate(prefab, container);
                item.Initialize(config);

                _items.Add(item);
            }
        }

        private void AutoOpen()
        {
            _autoOpen = true;
            Open(1);
        }

        private void Open(int count)
        {
            if (_inProgress) return;

            _inProgress = true;

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
                GBGames.saves.SwordsSave.Unlock(new SwordSave(item.ID));
            }


            UIManager.Instance.SetScreen("3d", onOpenComplete: () =>
            {
                var panel = UIManager.Instance.GetPanel<SwordUnlockerPanel>();
                
                panel.Enabled = true;
                
                if (_autoOpen)
                    panel.EnableAutoOpen(() => _autoOpen = false);
                
                panel.StartUnlocking(null, items, () =>
                {
                    panel.Enabled = false;
                    UIManager.Instance.SetScreen("main", onOpenComplete: () =>
                    {
                        _inProgress = false;
                        if (_autoOpen)
                            Open(1);
                    });
                });
            });
        }
    }
}