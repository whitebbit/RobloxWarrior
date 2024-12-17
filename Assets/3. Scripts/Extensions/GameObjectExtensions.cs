using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayer(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = layer;
            }
        }

        public static void SetLayer(this GameObject gameObject, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);

            gameObject.layer = layer;
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = layer;
            }
        }

        public static void SetLayer(this GameObject gameObject, LayerMask layer)
        {
            // Get the layer index from the LayerMask
            var layerIndex = Mathf.RoundToInt(Mathf.Log(layer.value, 2)); // Extract the first set bit of the LayerMask

            gameObject.layer = layerIndex;

            // Recursively set the layer for all child objects
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = layerIndex;
            }
        }

        public static List<T> FindObjectsInChildren<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            var foundObjects = new List<T>();
            FindObjectsRecursively(gameObject.transform, foundObjects);
            return foundObjects;
        }

        private static void FindObjectsRecursively<T>(Transform parent, List<T> foundObjects) where T : MonoBehaviour
        {
            var component = parent.GetComponent<T>();
            if (component != null)
            {
                foundObjects.Add(component);
            }

            foreach (Transform child in parent)
            {
                FindObjectsRecursively(child, foundObjects);
            }
        }
    }
}