using System;
using _3._Scripts.Config;
using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts.Worlds
{
    public class WorldsManager : Singleton<WorldsManager>
    {
        public World World { get; private set; }

        private void Start()
        {
            var worldConfig = Configuration.Instance.Config.Worlds[0];
        
            if(World != null)
                Destroy(World.gameObject);
            
            World = Instantiate(worldConfig.WorldPrefab, transform);

            World.transform.position = Vector3.zero;
            World.Initialize(worldConfig);
        }
    }
}