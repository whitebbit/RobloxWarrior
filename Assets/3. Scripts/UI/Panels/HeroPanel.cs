using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Heroes.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.Saves.Handlers;
using _3._Scripts.UI.Elements.HeroPanel;
using _3._Scripts.UI.Elements.SwordsPanel;
using _3._Scripts.UI.Panels.Base;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class HeroPanel : CollectionPanel<HeroItem, HeroConfig>
    {
        [SerializeField] private HeroItem prefab;

        protected override HeroConfig CurrentConfig =>
            Save.selected.Count <= 0 ? null : Configuration.Instance.GetHero(Save.selected[0].id);

        private List<HeroConfig> Configs => Configuration.Instance.Config.Heroes;
        private HeroesSave Save => GBGames.saves.heroesSave;

        protected override void PopulateList()
        {
            foreach (var config in Configs)
            {
                var swordItem = Instantiate(prefab, container, true);
                swordItem.Initialize(config);
                swordItem.transform.localScale = Vector3.one;
                swordItem.OnSelect += OnItemSelected;

                if (Save.IsSelected(config.ID))
                {
                    swordItem.SetCurrentFocus();
                }

                Items.Add(swordItem);
            }
        }

        protected override bool ItsCurrentItem(HeroItem item)
        {
            return Save.IsSelected(item.Config.ID);
        }

        protected override void UpdateCapacityText()
        {
            capacityText.text = $"{Save.selected.Count}/{Save.capacity}";
        }

        protected override void EquipItem(HeroItem item)
        {
        }
    }
}