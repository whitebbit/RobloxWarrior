using System.Collections;
using _3._Scripts.Config;
using _3._Scripts.Extensions;
using _3._Scripts.Localization;
using _3._Scripts.Player;
using _3._Scripts.Player.Enums;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.UI.Elements.PlayerCreator;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Transitions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;
using YG;

namespace _3._Scripts.UI.Panels
{
    public class PlayerCreatorPanel : SimplePanel
    {
        [SerializeField] private PlayerCreator creator;
        [SerializeField] private TMP_Text playerName;
        [SerializeField] private Button playButton;

        [Tab("Current Class")] [SerializeField]
        private Image currentClassImage;

        [SerializeField] private LocalizeStringEvent currentClassTitle;

        [Tab("Gender Buttons")] [SerializeField]
        private GenderButton maleGenderButton;

        [SerializeField] private GenderButton femaleGenderButton;

        [Tab("Class Buttons")] [SerializeField]
        private ClassButton classButtonPrefab;

        [SerializeField] private Transform classContainer;


        private GenderButton _currentGenderButton;
        private ClassButton _currentClassButton;

        public override void Initialize()
        {
            base.Initialize();

            InitializeGenderButtons();
            PopulateClassList();

            playButton.onClick.AddListener(() =>
            {
                creator.LoadDefault();
                LoadingScreen.Instance.ShowLoadingScreen(LoadGame());
            });
            playerName.text = YG2.player.name;
        }

        private void InitializeGenderButtons()
        {
            maleGenderButton.OnClick += GenderOnClick;
            femaleGenderButton.OnClick += GenderOnClick;
            GenderOnClick(PlayerGender.Male, maleGenderButton);
        }

        private void PopulateClassList()
        {
            var i = 0;
            foreach (var config in Configuration.Instance.Config.Classes)
            {
                var obj = Instantiate(classButtonPrefab, classContainer);
                obj.Initialize(config);
                obj.OnClick += ClassOnClick;

                if (i != 0) continue;

                ClassOnClick(config, obj);
                i += 1;
            }
        }

        private void ClassOnClick(PlayerClass arg1, ClassButton arg2)
        {
            _currentClassButton?.SelectedState(false);
            _currentClassButton = arg2;
            _currentClassButton?.SelectedState(true);

            creator.ChangeClass(arg1);
            currentClassTitle.SetReference(arg1.TitleID);
            currentClassImage.sprite = arg1.Icon;
            currentClassImage.ScaleImage();
        }

        private void GenderOnClick(PlayerGender type, GenderButton obj)
        {
            _currentGenderButton?.SelectedState(false);
            _currentGenderButton = obj;
            _currentGenderButton?.SelectedState(true);

            creator.ChangeGender(type);
        }

        private IEnumerator LoadGame()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("MainScene");
            if (asyncOperation == null) yield break;
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            yield return new WaitForSeconds(2);
        }
    }
}