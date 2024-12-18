using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Extensions.Interfaces;
using _3._Scripts.Quests.Enums;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.Saves;
using _3._Scripts.Tutorial;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Transitions;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Worlds;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _3._Scripts.Quests
{
    public class Quester : MonoBehaviour, IInteractive
    {
        [SerializeField] private ScaleTransition transition;
        [SerializeField] private Button button;
        [SerializeField] private List<QuestMark> marks = new();


        private static WorldSave Save => YG2.saves.worldSave;

        public static Quest CurrentQuest =>
            Save.questSave.GetCurrentQuest(Save.worldName) >= WorldsManager.Instance.World.Quests.Count
                ? null
                : WorldsManager.Instance.World.Quests[Save.questSave.GetCurrentQuest(Save.worldName)];


        private QuestWidget Widget => UIManager.Instance.GetWidget<QuestWidget>();
        private DialoguePanel Panel => UIManager.Instance.GetPanel<DialoguePanel>();

        private void SetMarkState(QuestMarkType type, bool state)
        {
            foreach (var mark in marks)
            {
                mark.gameObject.SetActive(false);
            }

            marks.FirstOrDefault(mark => mark.Type == type)?.gameObject.SetActive(state);
        }

        private void Start()
        {
            transition.ForceOut();
            button.onClick.AddListener(OnClick);
            SetMarkState(QuestMarkType.NewQuest, CurrentQuest != null);
        }

        private void OnClick()
        {
            if (CurrentQuest == null) return;

            Panel.Enabled = true;
            Panel.OpenQuest(CurrentQuest, CompleteQuest, ActivateQuest);
            TutorialManager.Instance.DisableStep("start");
            TutorialManager.Instance.DisableStep("after_battle");
        }

        private void ActivateQuest()
        {
            if (CurrentQuest == null) return;

            CurrentQuest.Activate();
            CurrentQuest.OnProgressUpdate += OnQuestUpdateProgress;
            Widget.Enabled = true;
            Widget.SetQuest(CurrentQuest);
            OnActivateQuest(CurrentQuest);
            SetMarkState(QuestMarkType.InProgress, true);
        }

        private void OnQuestUpdateProgress(Quest obj)
        {
            if (obj.IsCompleted)
                SetMarkState(QuestMarkType.Completed, true);
        }

        private void CompleteQuest()
        {
            CurrentQuest.GetRewards();
            CurrentQuest.Deactivate();
            Save.questSave.SetNextQuest(Save.worldName);
            Widget.Enabled = false;
            
            SetMarkState(QuestMarkType.NewQuest, CurrentQuest != null);
            Panel.OpenQuest(CurrentQuest, CompleteQuest, ActivateQuest);
            YG2.SaveProgress();
        }

        public void Interact()
        {
            transition.AnimateIn();
        }

        public void StopInteracting()
        {
            transition.AnimateOut();
        }

        private void OnActivateQuest(Quest quest)
        {
            switch (quest.Type)
            {
                case QuestType.WavesPassed:
                    
                    break;
                case QuestType.EnemyKills:
                    break;
                case QuestType.OpeningEgg:
                    TutorialManager.Instance.StartStep("eggs");
                    break;
                case QuestType.Rebirth:
                    break;
                case QuestType.CompleteWave:
                    TutorialManager.Instance.StartStep("battle");
                    break;
                case QuestType.SkillUpgrade:
                    break;
                case QuestType.GetHero:
                    TutorialManager.Instance.StartStep("hero");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}