using _3._Scripts.Config;
using _3._Scripts.Heroes;
using _3._Scripts.Swords;
using UnityEngine;

namespace _3._Scripts
{
    public class RuntimeHeroIconRenderer : RuntimeObjectIconRenderer<HeroModel>
    {
        #region Singleton

        public static RuntimeHeroIconRenderer Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var instances = FindObjectsOfType<RuntimeHeroIconRenderer>();
                var count = instances.Length;
                switch (count)
                {
                    case <= 0:
                        return _instance = new GameObject().AddComponent<RuntimeHeroIconRenderer>();
                    case 1:
                        return _instance = instances[0];
                }

                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);

                return _instance = instances[0];
            }
        }

        private static RuntimeHeroIconRenderer _instance;

        #endregion

        protected override HeroModel SpawnItem(string id)
        {
            var config = Configuration.Instance.GetHero(id);
            var item = Instantiate(config.ModelPrefab, itemTransform);
            return item;
        }

        protected override void CleanupItem(HeroModel item)
        {
            Destroy(item.gameObject);
        }
        
    }
}