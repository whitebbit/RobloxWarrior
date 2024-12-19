using System;
using _3._Scripts.Quests;
using _3._Scripts.Quests.Enums;
using _3._Scripts.Quests.ScriptableObjects;
using UnityEngine;
using YG;

namespace _3._Scripts.UI.Extensions
{
    public class ButtonQuestEnabler : MonoBehaviour
    {
        [SerializeField] private string tutorialName;
        [SerializeField] private bool updateOnEnable;


        private void Start()
        {
            gameObject.SetActive(YG2.saves.Tutorials.ContainsKey(tutorialName) &&
                                 YG2.saves.Tutorials[tutorialName]);
        }

        private void OnEnable()
        {
            if (updateOnEnable)
                gameObject.SetActive(YG2.saves.Tutorials.ContainsKey(tutorialName) &&
                                     YG2.saves.Tutorials[tutorialName]);
        }
    }
}