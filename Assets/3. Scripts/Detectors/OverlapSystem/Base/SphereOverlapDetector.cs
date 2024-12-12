
using UnityEngine;
using VInspector;

namespace _3._Scripts.Detectors.OverlapSystem.Base
{
    public abstract class SphereOverlapDetector<T>: OverlapDetector<T>
    {
        [Header("Sphere settings")] 
        [SerializeField] private float radius;
        
        public override float DetectAreaSize => radius;

        protected override int GetOverlapResult(Vector3 position)
        {
            return Physics.OverlapSphereNonAlloc(position, radius, OverlapResults, searchLayer.value);
        }

        public void SetRadius(float r)
        {
            this.radius = r;
        }
        
        protected override void DrawGizmos()
        {
            gizmos.DrawGizmos(offset, radius, startPoint);
        }
    }
}