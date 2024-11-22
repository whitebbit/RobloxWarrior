using UnityEngine;

namespace _3._Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _target; 

        private void Start()
        {
            if (Camera.main != null) _target = Camera.main.transform; 
        }

        private void Update()
        {
            transform.LookAt(_target);
        }
    }
}