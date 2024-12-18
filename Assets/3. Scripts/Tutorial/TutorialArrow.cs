using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialArrow : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;

        private Transform _target;
        private Transform _owner;

        private void Awake()
        {
            Disable();
        }

        public void Enable(Transform owner, Transform target)
        {
            line.gameObject.SetActive(true);
            _owner = owner;
            _target = target;
        }

        public void Disable()
        {
            line.gameObject.SetActive(false);
            _owner = null;
            _target = null;
        }

        private void Update()
        {
            if (_target == null || _owner == null) return;

            line.SetPosition(0, _owner.position);
            line.SetPosition(1, _target.position);
        }
    }
}