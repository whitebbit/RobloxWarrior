using System;
using UnityEngine;
using YG;

namespace _3._Scripts.Purchases
{
    public class PurchaseDisabler : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private PurchaseYG purchase;
        private SavesYG Saves => YG2.saves;

        private void Start()
        {
            YG2.onPurchaseSuccess += CheckPurchases;
        }

        private void OnEnable()
        {
            CheckPurchases(id);
        }

        private void CheckPurchases(string purchaseId)
        {
            if (Check("x3_open") && Saves.maxEggToOpen >= 3)
            {
                gameObject.SetActive(false);
            }

            if (Check("easy_craft") && Saves.swordsSave.requiredCountForMerge < 3)
            {
                gameObject.SetActive(false);
            }

            if (Check("extra_skill") && Saves.abilitiesSave.capacity >= 3)
            {
                gameObject.SetActive(false);
            }
            
            if (Check("extra_hero") && Saves.heroesSave.capacity >= 2)
            {
                gameObject.SetActive(false);
            }
        }

        private bool Check(string purchaseId)
        {
            return (!string.IsNullOrEmpty(id) && id == purchaseId) || (purchase != null && purchase.id == purchaseId);
        }
    }
}