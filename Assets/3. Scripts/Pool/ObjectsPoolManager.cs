using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Pool.Interfaces;
using _3._Scripts.Singleton;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _3._Scripts.Pool
{
    public class ObjectsPoolManager : Singleton<ObjectsPoolManager>
    {
        [SerializeField] private List<PoolCategory> pools = new();

        private readonly Dictionary<Type, object> _poolDict = new();

        public List<PoolCategory> Pools
        {
            get => pools;
            set => pools = value;
        }

        private void OnValidate()
        {
            foreach (var pool in from pool in pools
                     where pool.Prefab != null
                     let poolableComponent = pool.Prefab.GetComponent<IPoolable>()
                     where poolableComponent == null
                     select pool)
            {
                Debug.LogError($"Prefab does not implement IPoolable: {pool.Prefab.name}");
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var pool in pools)
            {
                var poolableComponent = pool.Prefab.GetComponent<IPoolable>();
                if (poolableComponent != null)
                {
                    var type = poolableComponent.GetType();
                    CreateObjectPool(type, pool.Prefab, pool.InitialSize);
                }
                else
                {
                    Debug.LogError($"Prefab does not implement IPoolable: {pool.Prefab.name}");
                }
            }
        }

        private void CreateObjectPool(Type type, GameObject prefab, int initialSize)
        {
            var parent = new GameObject($"{type.Name}.Pool");
            var poolType = typeof(PoolObject<>).MakeGenericType(type);
            var constructor = poolType.GetConstructor(new[] { type, typeof(int), typeof(Transform) });
            var poolInstance = constructor?.Invoke(new object[]
                { prefab.GetComponent<IPoolable>(), initialSize, parent.transform });

            parent.transform.parent = transform;
            _poolDict[type] = poolInstance;
        }

        public T Get<T>() where T : MonoBehaviour, IPoolable
        {
            var type = typeof(T);
            if (!_poolDict.TryGetValue(type, out var value)) return default;
            var pool = (PoolObject<T>)value;
            return pool.Get();
        }

        public void Return<T>(T obj) where T : MonoBehaviour, IPoolable
        {
            var type = typeof(T);
            if (!_poolDict.TryGetValue(type, out var value)) return;
            var pool = (PoolObject<T>)value;
            pool.Return(obj);
        }
    }
}