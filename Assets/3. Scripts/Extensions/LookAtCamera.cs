using UnityEngine;

namespace _3._Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _target;

        private void Start()
        {
            if (Camera.main == null) return;
            
            _target = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 targetDirection = _target.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(targetDirection);

            rotation = Quaternion.Euler(-rotation.eulerAngles.x, rotation.eulerAngles.y + 180,  rotation.eulerAngles.z); 

            transform.rotation = rotation;
        }
    }
}