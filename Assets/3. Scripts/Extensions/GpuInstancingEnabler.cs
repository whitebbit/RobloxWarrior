﻿using System;
using UnityEngine;

namespace _3._Scripts
{
    public class GpuInstancingEnabler: MonoBehaviour
    {
        private void Awake()
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}