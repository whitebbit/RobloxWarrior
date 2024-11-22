/*using System.Collections.Generic;
using System.Linq;
using _3._Scripts.UI.Elements.ShopSlots;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Scriptable.Shop;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public abstract class ShopPanel<T, T1> : SimplePanel where T : ShopItem where T1 : BaseShopSlot
    {
        [Tab("Components")] 
        [SerializeField] private Transform container;
        [SerializeField] private T1 prefab;

        private readonly List<T1> _shopSlots = new();
        protected abstract IEnumerable<T> ShopItems();

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            SpawnItems();
            SetSlotsState();
        }

        protected override void OnOpen()
        {
            SetSlotsState();
        }

       
        protected virtual void OnSpawnItems(T1 slot, T data)
        {
        }

        private void SpawnItems()
        {
            var items = ShopItems().ToList();
            foreach (var item in items)
            {
                var obj = Instantiate(prefab, container);
                obj.SetView(item);
                obj.SetAction(() => OnClick(item.ID));
                OnSpawnItems(obj, item);
                _shopSlots.Add(obj);
            }
        }
        

        protected void SetSlotsState()
        {
            foreach (var slot in _shopSlots)
            {
                if (!ItemUnlocked(slot.Data.ID))
                    slot.Lock();
                else
                {
                    if (IsSelected(slot.Data.ID))
                        slot.Select();
                    else
                        slot.Unselect();
                }
            }
        }

        private void OnClick(string id)
        {
            if (ItemUnlocked(id))
            {
                Select(id);
            }
            else
            {
                Buy(id);
            }
        }

        protected T1 GetSlot(string id) => _shopSlots.FirstOrDefault(s => s.Data.ID == id);
        protected abstract bool ItemUnlocked(string id);
        protected abstract bool IsSelected(string id);
        protected abstract bool Select(string id);
        protected abstract bool Buy(string id);
    }
}*/