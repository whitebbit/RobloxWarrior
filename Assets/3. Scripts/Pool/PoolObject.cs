using System;
using System.Collections.Generic;
using _3._Scripts.Pool.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _3._Scripts.Pool
{
    public class PoolObject<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;
        private readonly int _initialSize;
        private readonly Transform _parent;
        private readonly Queue<T> _poolQueue = new();

        public PoolObject(T prefab, int initialSize, Transform parent)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;

            InitializePool();
        }

        private void InitializePool()
        {
            for (var i = 0; i < _initialSize; i++)
            {
                var obj = SpawnObject();
                obj.gameObject.name += $"_{i + 1}";
            }
        }

        private T SpawnObject()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            obj.transform.localPosition = Vector3.zero;
            obj.gameObject.name = _prefab.name;

            obj.OnDespawn();
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
            return obj;
        }

        public T Get()
        {
            while (true)
            {
                if (_poolQueue.Count > 0)
                {
                    var obj = _poolQueue.Dequeue();
                    obj.transform.SetParent(null);
                    obj.gameObject.SetActive(true);
                    obj.OnSpawn();
                    return obj;
                }

                SpawnObject();
            }
        }

        public void Return(T obj)
        {
            obj.transform.SetParent(_parent);
            obj.OnDespawn();
            obj.gameObject.SetActive(false);

            _poolQueue.Enqueue(obj);
        }
    }
}