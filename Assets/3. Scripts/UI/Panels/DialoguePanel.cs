using System;
using _3._Scripts.Localization;
using _3._Scripts.Quests;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class DialoguePanel : UIPanel
    {
        [SerializeField] private SlideTransition transition;
        [Space] [SerializeField] private LocalizeStringEvent text;
        [Space] [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;
        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        private Quest _currentQuest;
        private bool _inProgress;
        private event Action OnAccept;
        private event Action OnReward;

        public override void Initialize()
        {
            transition.SetStartPosition();

            InTransition = transition;
            OutTransition = transition;

            cancelButton.onClick.AddListener(() => Enabled = false);
        }

        public void OpenQuest(Quest quest, Action onReward, Action onAccept)
        {
            OnAccept = onAccept;
            OnReward = onReward;

            if (_currentQuest == null || !_inProgress)
                StartQuest(quest);
            else
            {
                if (_currentQuest.IsCompleted)
                {
                    GetReward();
                }
                else
                {
                    QuestInProgress();
                }
            }
        }


        private void StartQuest(Quest quest)
        {
            _currentQuest = quest;

            text.SetReference(quest.Type.GetDescription());
            text.SetVariable("value", quest.GoalText);

            acceptButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() =>
            {
                OnAccept?.Invoke();
                _currentQuest = quest;
                _inProgress = true;
                Enabled = false;
            });
        }

        private void QuestInProgress()
        {
            text.SetReference("complete_current_quest");

            acceptButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => { Enabled = false; });
        }

        private void GetReward()
        {
            _currentQuest = null;
            
            text.SetReference("quest_complete");

            acceptButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() =>
            {
                transition.AnimateOut().OnComplete(() =>
                {
                    _inProgress = false;
                    OnReward?.Invoke();
                    transition.AnimateIn();
                });
            });
        }
    }
}