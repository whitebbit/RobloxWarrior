using _3._Scripts.Config.Scriptables;
using _3._Scripts.UI.Interfaces;
using UnityEngine;

namespace _3._Scripts.Player.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerClass", menuName = "Configs/Player/Class", order = 0)]
    public class PlayerClass : ConfigObject<Sprite>
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private Sprite icon;

        public Texture2D Texture => texture;
        public override Sprite Icon => icon;
    }
}