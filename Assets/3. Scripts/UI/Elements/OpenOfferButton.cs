using System;
using _3._Scripts.Extensions.Interfaces;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class OpenOfferButton : MonoBehaviour, IInteractive
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private string id;
        [SerializeField] private Button button;


        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                var p = UIManager.Instance.GetPanel<OfferPanel>();
                p.Enabled = true;
                p.UpdatePurchase(id);
            });
            transition.ForceOut();
        }

        public void Interact()
        {
            transition.AnimateIn();
        }

        public void StopInteracting()
        {
            transition.AnimateOut();
        }
    }
}