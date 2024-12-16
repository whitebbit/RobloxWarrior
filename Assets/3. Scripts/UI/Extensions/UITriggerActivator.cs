using System;
using _3._Scripts.UI.Panels;
using UnityEngine;

namespace _3._Scripts.UI.Extensions
{
    public class UITriggerActivator : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player _))
            {
                UIManager.Instance.GetPanel<HeroPanel>().Enabled = true;
            }
        }
    }
}