using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Player.Enums;
using UnityEngine;
using YG;

namespace _3._Scripts.Player
{
    public class PlayerView : MonoBehaviour
    {
        private static readonly int HeadTexture = Shader.PropertyToID("_Head_Texture");
        private static readonly int BodyTexture = Shader.PropertyToID("_Body_Texture");
        [SerializeField] private Material characterMaterial;
        [SerializeField] private Texture2D male;
        [SerializeField] private Texture2D female;

        private void Start()
        {
            var gender = YG2.saves.characterSave.gender;
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

            var config =
                Configuration.Instance.Config.Classes.FirstOrDefault(c => c.ID == YG2.saves.characterSave.classID);
            if (config != null) characterMaterial.SetTexture(BodyTexture, config.Texture);
        }
    }
}