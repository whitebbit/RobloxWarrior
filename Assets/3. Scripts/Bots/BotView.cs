using System.Collections.Generic;
using _3._Scripts.Bots.Sciptables;
using _3._Scripts.Config.Interfaces;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotView : MonoBehaviour, IInitializable<BotConfig>
    {
        [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers;


        public void Initialize(BotConfig config)
        {
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.material = config.Skin;
            }

            transform.localScale = new Vector3(config.Size, config.Size, config.Size);
        }
    }
}