using System;
using _3._Scripts.Pool.Interfaces;
using UnityEngine;

namespace _3._Scripts.Pool
{
    [Serializable]
    public class PoolCategory
    {
        [SerializeField] private GameObject prefab;
        [SerializeField, Range(1, 1000)] private int initialSize;
        
        public GameObject Prefab { get=> prefab; set => prefab = value; }
        public int InitialSize { get=> initialSize; set => initialSize = value; }
    }
}