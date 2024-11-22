using System;
using _3._Scripts.Gizmos.Enums;
using UnityEngine;

namespace _3._Scripts.Gizmos
{
    [Serializable]
    public class CustomGizmos
    {
        [SerializeField] private DrawGizmosType type;
        [SerializeField] private Color color;

        public DrawGizmosType Type => type;

        public void DrawGizmos(Vector3 point, Vector3 size, Transform startPoint)
        {
            if (type == DrawGizmosType.Never) return;
            
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.matrix = startPoint.localToWorldMatrix;
            UnityEngine.Gizmos.DrawCube(point, size);
        }
        
        public void DrawGizmos(Vector3 point, float radius, Transform startPoint)
        {
            if (type == DrawGizmosType.Never) return;
            
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.matrix = startPoint.localToWorldMatrix;
            UnityEngine.Gizmos.DrawSphere(point, radius);
        }
    }
}