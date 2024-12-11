using _3._Scripts.Bots.Enums;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class BotWeaponModel : MonoBehaviour
    {
        [SerializeField] private BotWeaponType type;
        public BotWeaponType Type => type;
    }
}