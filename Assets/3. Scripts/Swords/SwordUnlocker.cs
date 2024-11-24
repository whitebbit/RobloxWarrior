using System;
using System.Collections.Generic;
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
        [Tab("Main")] 
        [SerializeField] private Transform container;
        [SerializeField] private SwordEggItem prefab;

        [Tab("Buttons")] [SerializeField] private Button openButton;
        [SerializeField] private Button autoOpenButton;
        [SerializeField] private Button x3OpenButton;

        [SerializeField] private List<SwordConfig> _configs = new();

        private List<SwordEggItem> _items = new();
        
        private void Start()
        {
            Initialize(_configs);
            openButton.onClick.AddListener(OpenX1);
            x3OpenButton.onClick.AddListener(OpenX3);
        }

        public void Initialize(List<SwordConfig> configs)
        {
            _configs = configs;

            foreach (var config in _configs)
            {
                var item = Instantiate(prefab, container);
                item.Initialize(config);
                
                _items.Add(item);
            }
        }

        private void OpenX1()
        {
            Open(1);
        }
        private void OpenX3()
        {
            Open(3);
        }
        
        private void Open(int count)
        {
            var items = new List<SwordConfig>();
            
            for (var i = 0; i < count; i++)
            {
                items.Add(_configs.GetRandomElement());
            }

            foreach (var item in items)
            {
                GBGames.saves.SwordsSave.Unlock(new SwordSave(item.ID));
            }
            
            UIManager.Instance.SetScreen("3d", onOpenComplete: () =>
            {
                var panel = UIManager.Instance.GetPanel<SwordUnlockerPanel>();

                panel.Enabled = true;
                panel.StartUnlocking(null, items, () =>
                {
                    panel.Enabled = false;
                    UIManager.Instance.SetScreen("main");
                });
            });
        }
    }
}