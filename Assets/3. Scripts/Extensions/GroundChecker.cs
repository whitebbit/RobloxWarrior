using System;
using UnityEngine;

namespace _3._Scripts
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;
        
        public bool IsGrounded()
        {
            return Physics.CheckSphere(transform.position, groundDistance, groundMask);
        }

        private void OnDrawGizmos()
        {
            var color = Color.yellow;
            color.a = 0.5f;
            
            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawSphere(transform.position, groundDistance);
        }
    }
}