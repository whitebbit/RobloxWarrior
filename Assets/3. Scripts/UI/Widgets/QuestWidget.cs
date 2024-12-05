using _3._Scripts.Localization;
using _3._Scripts.Quests;
using _3._Scripts.Quests.ScriptableObjects;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Widgets
{
    public class QuestWidget : UIWidget
    {
        [SerializeField] private SlideTransition transition;
        [Space] [SerializeField] private LocalizeStringEvent text;
        [SerializeField] private TMP_Text progressText;
        [Header("Completed")] [SerializeField] private Image completedImage;
        [SerializeField] private ParticleSystem particle;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public override void Initialize()
        {
            transition.SetStartPosition();

            InTransition = transition;
            OutTransition = transition;
        }

        public void SetQuest(Quest quest)
        {
            particle.gameObject.SetActive(false);
            completedImage.Fade(0);
            
            text.SetReference(quest.Type.GetDescription());
            text.SetVariable("value", quest.GoalText);

            progressText.text = quest.ProgressText;

            quest.OnProgressUpdate += OnProgressUpdate;
        }

        private void OnProgressUpdate(Quest obj)
        {
            progressText.text = obj.ProgressText;
            if (obj.IsCompleted)
                Complete();
        }

        private void Complete()
        {
            particle.gameObject.SetActive(true);
            completedImage.DOFade(1, 0.25f);
        }
    }
}