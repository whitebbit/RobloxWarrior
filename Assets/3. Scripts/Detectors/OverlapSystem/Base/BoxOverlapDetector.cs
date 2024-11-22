using UnityEngine;
using VInspector;

namespace _3._Scripts.Detectors.OverlapSystem.Base
{
    public abstract class BoxOverlapDetector<T> : OverlapDetector<T>
    {
        [Tab("Box settings")] [SerializeField]
        private Vector3 size;

        public override float DetectAreaSize => size.magnitude;
        protected override int GetOverlapResult(Vector3 position)
        {
            return Physics.OverlapBoxNonAlloc(position, size / 2, OverlapResults, startPoint.rotation, searchLayer.value);
        }

        protected override void DrawGizmos()
        {
            gizmos.DrawGizmos(offset, size, startPoint);
        }
    }
}