using Cinemachine;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        
        public void Look(Vector2 direction, float sensitivityX, float sensitivityY)
        {
            freeLookCamera.m_XAxis.Value += direction.x * sensitivityX * Time.deltaTime;
            freeLookCamera.m_YAxis.Value -= direction.y * sensitivityY * Time.deltaTime;
        }
        
    }
}