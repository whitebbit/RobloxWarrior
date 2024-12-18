using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;

namespace _3._Scripts.UI.Panels
{
    public class OfferPanel : SimplePanel
    {
        [SerializeField] private PurchaseYG purchaseObject;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button cancelButton;

        private string _id;

        public override void Initialize()
        {
            base.Initialize();
            buyButton.onClick.AddListener(purchaseObject.BuyPurchase);
            cancelButton.onClick.AddListener(() => Enabled = false);

            YG2.onPurchaseSuccess += OnPurchaseSuccess;
            YG2.onPurchaseFailed += OnPurchaseSuccess;
        }

        public void UpdatePurchase(string id)
        {
            purchaseObject.UpdateEntries(id);
            _id = id;
        }

        private void OnPurchaseSuccess(string obj)
        {
            if (_id != obj) return;

            Enabled = false;
        }
    }
}