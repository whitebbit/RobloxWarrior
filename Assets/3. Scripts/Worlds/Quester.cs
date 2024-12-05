﻿using System;
using _3._Scripts.Config;
using _3._Scripts.Extensions.Interfaces;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.Quests.ScriptableObjects.Quests;
using _3._Scripts.Saves;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Transitions;
using _3._Scripts.UI.Widgets;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Worlds
{
    public class Quester : MonoBehaviour, IInteractive
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private Button button;

        private WorldSave Save => GBGames.saves.worldSave;

        private Quest CurrentQuest =>
            Save.questSave.GetCurrentQuest(Save.worldName) >= WorldsManager.Instance.World.Quests.Count
                ? null
                : WorldsManager.Instance.World.Quests[Save.questSave.GetCurrentQuest(Save.worldName)];

  
        private QuestWidget Widget => UIManager.Instance.GetWidget<QuestWidget>();
        private DialoguePanel Panel => UIManager.Instance.GetPanel<DialoguePanel>();

        private void Start()
        {
            transition.ForceOut();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (CurrentQuest == null) return;

            Panel.Enabled = true;
            Panel.OpenQuest(CurrentQuest, CompleteQuest, ActivateQuest);
        }

        private void ActivateQuest()
        {
            if (CurrentQuest == null) return;

            CurrentQuest.Activate();

            Widget.Enabled = true;
            Widget.SetQuest(CurrentQuest);
        }

        private void CompleteQuest()
        {
            CurrentQuest.GetRewards();
            CurrentQuest.Deactivate();
            Save.questSave.SetNextQuest(Save.worldName);
            Widget.Enabled = false;
            
            Panel.OpenQuest(CurrentQuest, CompleteQuest, ActivateQuest);
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