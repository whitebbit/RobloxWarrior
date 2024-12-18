using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Player.Enums;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.UI.Panels;
using Animancer;
using UnityEngine;
using VInspector;
using YG;

namespace _3._Scripts.Player
{
    public class PlayerCreator : MonoBehaviour
    {
        private static readonly int HeadTexture = Shader.PropertyToID("_Head_Texture");
        private static readonly int BodyTexture = Shader.PropertyToID("_Body_Texture");

        [Tab("Animation")] [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private AnimationClip clip;

        [Tab("View")] [SerializeField] [Header("Base")]
        private Material characterMaterial;

        [Header("Gender")] [SerializeField] private Texture2D male;
        [SerializeField] private Texture2D female;
        [Tab("UI")] [SerializeField] private QualityPanel qualityPanel;
        [SerializeField] private PlayerCreatorPanel creatorPanel;

        private void Start()
        {
            animancer.Play(clip);
            InitializeUI();
        }

        private void InitializeUI()
        {
            qualityPanel.Initialize();
            creatorPanel.Initialize();
            creatorPanel.Enabled = true;
            qualityPanel.Enabled = true;
        }

        public void ChangeGender(PlayerGender gender)
        {
            switch (gender)
            {
                case PlayerGender.Male:
                    characterMaterial.SetTexture(HeadTexture, male);
                    break;
                case PlayerGender.Female:
                    characterMaterial.SetTexture(HeadTexture, female);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
            
            YG2.saves.characterSave.gender = gender;
        }

        public void ChangeClass(PlayerClass playerClass)
        {
            characterMaterial.SetTexture(BodyTexture, playerClass.Texture);
            YG2.saves.characterSave.classID = playerClass.ID;
        }

        public void LoadDefault()
        {
            var sword = new SwordSave(Configuration.Instance.Config.SwordCollectionConfig.Swords[0].ID);

            YG2.saves.swordsSave.Unlock(sword);
            YG2.saves.swordsSave.SetCurrent(YG2.saves.swordsSave.unlocked.FirstOrDefault(s => s.uid == sword.uid));
            YG2.saves.abilitiesSave.capacity = 2;
            YG2.saves.worldSave.worldName = Configuration.Instance.Config.Worlds[0].WorldName;

            YG2.saves.defaultLoaded = true;
            YG2.SaveProgress();
        }
    }
}