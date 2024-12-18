using System;
using System.Collections.Generic;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Singleton;
using UnityEngine;
using YG;

namespace _3._Scripts.Purchases
{
    public class PurchaseController : Singleton<PurchaseController>
    {
        private readonly List<Purchase> _purchases = new()
        {
            new Purchase("x3_open", () =>
            {
                YG2.saves.maxEggToOpen = 3;
                YG2.SaveProgress();
            }),
            new Purchase("easy_craft", () =>
            {
                YG2.saves.swordsSave.requiredCountForMerge = 2;
                YG2.SaveProgress();
            }),
            new Purchase("extra_skill", () =>
            {
                YG2.saves.abilitiesSave.capacity = 3;
                YG2.SaveProgress();
            }),
            new Purchase("extra_hero", () =>
            {
                YG2.saves.heroesSave.capacity = 2;
                YG2.SaveProgress();
            }),
            new Purchase("80_rebirth", () =>
            {
                for (int i = 0; i < 80; i++)
                {
                    Player.Player.Instance.Stats.Rebirth();
                }
                YG2.SaveProgress();
            }),
            new Purchase("100_rebirth", () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Player.Player.Instance.Stats.Rebirth();
                }
                YG2.SaveProgress();
            }),
            new Purchase("100_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 100;
                YG2.SaveProgress();
            }),
            new Purchase("1000_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 1000;
                YG2.SaveProgress();
            }),
            new Purchase("5000_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 5000;
                YG2.SaveProgress();
            }),
            new Purchase("25000_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 25000;
                YG2.SaveProgress();
            }),
            new Purchase("50000_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 50000;
                YG2.SaveProgress();
            }),
            new Purchase("100000_gems", () =>
            {
                WalletManager.GetCurrency(CurrencyType.Crystal).Value += 100000;
                YG2.SaveProgress();
            }),
        };

        private void Start()
        {
            YG2.ConsumePurchases();
        }

        private void OnEnable()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
        }

        private void SuccessPurchased(string obj)
        {
            foreach (var purchase in _purchases)
            {
                purchase.PurchaseSuccess(obj);
            }
        }
    }
}