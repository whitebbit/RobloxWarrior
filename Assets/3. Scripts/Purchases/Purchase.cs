using System;

namespace _3._Scripts.Purchases
{
    public class Purchase
    {
        private string _id;
        private event Action OnPurchase;

        public Purchase(string id, Action onPurchase)
        {
            _id = id;
            OnPurchase = onPurchase;
        }

        public void PurchaseSuccess(string id)
        {
            if (id != _id) return;
            OnPurchase?.Invoke();
        }
    }
}