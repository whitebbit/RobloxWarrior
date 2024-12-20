﻿#region License
/*
MIT License

Copyright(c) 2017-2020 Mattias Edlund

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

#if UNITY_2018_2 || UNITY_2018_3 || UNITY_2018_4 || UNITY_2019
#define UNITY_8UV_SUPPORT
#endif

#if UNITY_2017_3 || UNITY_2017_4 || UNITY_2018 || UNITY_2019
#define UNITY_MESH_INDEXFORMAT_SUPPORT
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_MESH_INDEXFORMAT_SUPPORT
using UnityEngine.Rendering;
#endif

namespace JustAssets.ColliderUtilityRuntime.MeshSimplifier.Utility
{
    /// <summary>
    /// Contains utility methods for meshes.
    /// </summary>
    public static class MeshUtils
    {
        #region Static Read-Only
        /// <summary>
        /// The count of supported UV channels.
        /// </summary>
#if UNITY_8UV_SUPPORT
        public static readonly int UVChannelCount = 8;
#else
        public static readonly int UVChannelCount = 4;
#endif
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new mesh.
        /// </summary>
        /// <param name="vertices">The mesh vertices.</param>
        /// <param name="indices">The mesh sub-mesh indices.</param>
        /// <param name="normals">The mesh normals.</param>
        /// <param name="tangents">The mesh tangents.</param>
        /// <param name="colors">The mesh colors.</param>
        /// <param name="boneWeights">The mesh bone-weights.</param>
        /// <param name="uvs2D">The mesh 2D UV sets.</param>
        /// <param name="uvs3D">The mesh 3D UV sets.</param>
        /// <param name="uvs4D">The mesh 4D UV sets.</param>
        /// <param name="bindposes">The mesh bindposes.</param>
        /// <returns>The created mesh.</returns>
        public static Mesh CreateMesh(Vector3[] vertices, int[][] indices, Vector3[] normals, Vector4[] tangents, Color[] colors, BoneWeight[] boneWeights, List<Vector2>[] uvs2D, List<Vector3>[] uvs3D, List<Vector4>[] uvs4D, Matrix4x4[] bindposes)
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");
            else if (indices == null)
                throw new ArgumentNullException("indices");

            Mesh newMesh = new Mesh();
            int subMeshCount = indices.Length;

#if UNITY_MESH_INDEXFORMAT_SUPPORT
            IndexFormat indexFormat;
            Vector2Int[] indexMinMax = MeshUtils.GetSubMeshIndexMinMax(indices, out indexFormat);
            newMesh.indexFormat = indexFormat;
#endif

            if (bindposes != null && bindposes.Length > 0)
            {
                newMesh.bindposes = bindposes;
            }

            newMesh.subMeshCount = subMeshCount;
            newMesh.vertices = vertices;
            if (normals != null && normals.Length > 0)
            {
                newMesh.normals = normals;
            }
            if (tangents != null && tangents.Length > 0)
            {
                newMesh.tangents = tangents;
            }
            if (colors != null && colors.Length > 0)
            {
                newMesh.colors = colors;
            }
            if (boneWeights != null && boneWeights.Length > 0)
            {
                newMesh.boneWeights = boneWeights;
            }

            if (uvs2D != null)
            {
                for (int uvChannel = 0; uvChannel < uvs2D.Length; uvChannel++)
                {
                    if (uvs2D[uvChannel] != null && uvs2D[uvChannel].Count > 0)
                    {
                        newMesh.SetUVs(uvChannel, uvs2D[uvChannel]);
                    }
                }
            }

            if (uvs3D != null)
            {
                for (int uvChannel = 0; uvChannel < uvs3D.Length; uvChannel++)
                {
                    if (uvs3D[uvChannel] != null && uvs3D[uvChannel].Count > 0)
                    {
                        newMesh.SetUVs(uvChannel, uvs3D[uvChannel]);
                    }
                }
            }

            if (uvs4D != null)
            {
                for (int uvChannel = 0; uvChannel < uvs4D.Length; uvChannel++)
                {
                    if (uvs4D[uvChannel] != null && uvs4D[uvChannel].Count > 0)
                    {
                        newMesh.SetUVs(uvChannel, uvs4D[uvChannel]);
                    }
                }
            }

            for (int subMeshIndex = 0; subMeshIndex < subMeshCount; subMeshIndex++)
            {
                int[] subMeshTriangles = indices[subMeshIndex];
#if UNITY_MESH_INDEXFORMAT_SUPPORT
                Vector2Int minMax = indexMinMax[subMeshIndex];
                if (indexFormat == UnityEngine.Rendering.IndexFormat.UInt16 && minMax.y > ushort.MaxValue)
                {
                    int baseVertex = minMax.x;
                    for (int index = 0; index < subMeshTriangles.Length; index++)
                    {
                        subMeshTriangles[index] -= baseVertex;
                    }
                    newMesh.SetTriangles(subMeshTriangles, subMeshIndex, false, baseVertex);
                }
                else
                {
                    newMesh.SetTriangles(subMeshTriangles, subMeshIndex, false, 0);
                }
#else
                newMesh.SetTriangles(subMeshTriangles, subMeshIndex, false);
#endif
            }

            newMesh.RecalculateBounds();
            return newMesh;
        }

        /// <summary>
        /// Returns the UV list for a specific mesh and UV channel.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="channel">The UV channel.</param>
        /// <returns>The UV list.</returns>
        public static List<Vector4> GetMeshUVs(Mesh mesh, int channel)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            else if (channel < 0 || channel >= UVChannelCount)
                throw new ArgumentOutOfRangeException("channel");

            List<Vector4> uvList = new List<Vector4>(mesh.vertexCount);
            mesh.GetUVs(channel, uvList);
            return uvList;
        }

        /// <summary>
        /// Returns the number of used UV components in a UV set.
        /// </summary>
        /// <param name="uvs">The UV set.</param>
        /// <returns>The number of used UV components.</returns>
        public static int GetUsedUVComponents(List<Vector4> uvs)
        {
            if (uvs == null || uvs.Count == 0)
                return 0;

            int usedComponents = 1;
            foreach (Vector4 uv in uvs)
            {
                if (usedComponents < 2 && uv.y != 0f)
                {
                    usedComponents = 2;
                }
                if (usedComponents < 3 && uv.z != 0f)
                {
                    usedComponents = 3;
                }
                if (usedComponents < 4 && uv.w != 0f)
                {
                    usedComponents = 4;
                    break;
                }
            }

            return usedComponents;
        }

        /// <summary>
        /// Converts a list of 4D UVs into 2D.
        /// </summary>
        /// <param name="uvs">The list of UVs.</param>
        /// <returns>The array of 2D UVs.</returns>
        public static Vector2[] ConvertUVsTo2D(List<Vector4> uvs)
        {
            if (uvs == null)
                return null;

            Vector2[] uv2D = new Vector2[uvs.Count];
            for (int i = 0; i < uv2D.Length; i++)
            {
                Vector4 uv = uvs[i];
                uv2D[i] = new Vector2(uv.x, uv.y);
            }
            return uv2D;
        }

        /// <summary>
        /// Converts a list of 4D UVs into 3D.
        /// </summary>
        /// <param name="uvs">The list of UVs.</param>
        /// <returns>The array of 3D UVs.</returns>
        public static Vector3[] ConvertUVsTo3D(List<Vector4> uvs)
        {
            if (uvs == null)
                return null;

            Vector3[] uv3D = new Vector3[uvs.Count];
            for (int i = 0; i < uv3D.Length; i++)
            {
                Vector4 uv = uvs[i];
                uv3D[i] = new Vector3(uv.x, uv.y, uv.z);
            }
            return uv3D;
        }

#if UNITY_MESH_INDEXFORMAT_SUPPORT
        /// <summary>
        /// Returns the minimum and maximum indices for each submesh along with the needed index format.
        /// </summary>
        /// <param name="indices">The indices for the submeshes.</param>
        /// <param name="indexFormat">The output index format.</param>
        /// <returns>The minimum and maximum indices for each submesh.</returns>
        public static Vector2Int[] GetSubMeshIndexMinMax(int[][] indices, out IndexFormat indexFormat)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");

            Vector2Int[] result = new Vector2Int[indices.Length];
            indexFormat = IndexFormat.UInt16;
            for (int subMeshIndex = 0; subMeshIndex < indices.Length; subMeshIndex++)
            {
                int minIndex, maxIndex;
                GetIndexMinMax(indices[subMeshIndex], out minIndex, out maxIndex);
                result[subMeshIndex] = new Vector2Int(minIndex, maxIndex);

                int indexRange = (maxIndex - minIndex);
                if (indexRange > ushort.MaxValue)
                {
                    indexFormat = IndexFormat.UInt32;
                }
            }
            return result;
        }
#endif
        #endregion

        #region Private Methods
        private static void GetIndexMinMax(int[] indices, out int minIndex, out int maxIndex)
        {
            if (indices == null || indices.Length == 0)
            {
                minIndex = maxIndex = 0;
                return;
            }

            minIndex = int.MaxValue;
            maxIndex = int.MinValue;

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] < minIndex)
                {
                    minIndex = indices[i];
                }
                if (indices[i] > maxIndex)
                {
                    maxIndex = indices[i];
                }
            }
        }
        #endregion
    }
}
