using System;
using System.Collections;
using _3._Scripts.Detectors.Interfaces;
using _3._Scripts.Gizmos;
using _3._Scripts.Gizmos.Enums;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Detectors.OverlapSystem.Base
{
    public abstract class OverlapDetector<T> : BaseDetector<T>
    {
        [SerializeField] private bool constantDetect;
        [Header("Mask")] [SerializeField] protected LayerMask searchLayer;
        [SerializeField] protected LayerMask obstacleLayer;

        [Header("Overlap area")] [SerializeField]
        protected Transform startPoint;

        [SerializeField] protected Vector3 offset;
        [Header("Obstacle")] [SerializeField] protected bool considerObstacles;

        [Header("Gizmos")] [SerializeField] protected CustomGizmos gizmos;

        protected readonly Collider[] OverlapResults = new Collider[32];
        private int _overlapResultsCount;

        public abstract float DetectAreaSize { get; }

        private IEnumerator _enumerator;

        public void DetectState(bool state)
        {
            if (!constantDetect) return;

            if (state)
            {
                _enumerator = FindTargetsWithDelay(.2f);
                StartCoroutine(_enumerator);
            }
            else
            {
                if (_enumerator != null)
                    StopCoroutine(_enumerator);
            }
        }

        private void InteractWithFoundedObjects()
        {
            for (var i = 0; i < _overlapResultsCount; i++)
            {
                if (!OverlapResults[i].TryGetComponent(out T findable))
                {
                    continue;
                }

                if (considerObstacles)
                {
                    var startPosition = startPoint.position;
                    var colliderPosition = OverlapResults[i].transform.position;
                    var hasObstacle = Physics.Linecast(startPosition, colliderPosition, obstacleLayer);
                    if (hasObstacle) continue;
                }

                CallOnFound(findable);
            }
        }

        public void SetStartPoint(Transform point) => startPoint = point;

        public override bool ObjectsDetected()
        {
            var position = startPoint.TransformPoint(offset);
            _overlapResultsCount = GetOverlapResult(position);

            return _overlapResultsCount > 0;
        }

        public void FindTargets()
        {
            if (ObjectsDetected()) InteractWithFoundedObjects();
            else CallOnFound(default);
        }

        private IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindTargets();
            }
        }

        protected abstract int GetOverlapResult(Vector3 position);
        protected abstract void DrawGizmos();

        private void OnDrawGizmos()
        {
            if (gizmos.Type == DrawGizmosType.Always)
                DrawGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmos.Type == DrawGizmosType.OnSelected)
                DrawGizmos();
        }
    }
}