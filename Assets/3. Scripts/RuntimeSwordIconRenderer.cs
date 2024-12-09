using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Singleton;
using _3._Scripts.Swords;
using UnityEngine;

namespace _3._Scripts
{
    public class RuntimeSwordIconRenderer : RuntimeObjectIconRenderer<Sword>
    {

        #region Singleton

        public static RuntimeSwordIconRenderer Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var instances = FindObjectsOfType<RuntimeSwordIconRenderer>();
                var count = instances.Length;
                switch (count)
                {
                    case <= 0:
                        return _instance = new GameObject().AddComponent<RuntimeSwordIconRenderer>();
                    case 1:
                        return _instance = instances[0];
                }

                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);

                return _instance = instances[0];
            }
        }

        private static RuntimeSwordIconRenderer _instance;

        #endregion

        protected override Sword SpawnItem(string id)
        {
            var config = Configuration.Instance.Config.SwordCollectionConfig.GetSword(id);
            var item = Instantiate(config.Prefab, itemTransform);

            item.Disable();
            return item;
        }

        protected override void OnRenderComplete(Sword item)
        {
            Destroy(item.gameObject);
        }
    }
}