using System.Collections.Generic;
using _3._Scripts.Pool;
using _3._Scripts.UI.Elements.SwordsPanel;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class SwordsPanel : UIPanel
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private Transform container;
        
        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }
        
        private List<SwordItem> _items = new();
        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }

        protected override void OnOpen()
        {
            foreach (var save in GBGames.saves.SwordsSave.unlocked)
            {
                var item = ObjectsPoolManager.Instance.Get<SwordItem>();
                
                item.Initialize(save);
                item.transform.SetParent(container);
                item.transform.localScale = Vector3.one;
                
                _items.Add(item);
            }
            base.OnOpen();
        }

        protected override void OnClose()
        {
            base.OnClose();
            foreach (var item in _items)
            {
                ObjectsPoolManager.Instance.Return(item);
            }
        }
    }
}