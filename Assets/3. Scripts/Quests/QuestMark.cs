using _3._Scripts.Quests.Enums;
using UnityEngine;

namespace _3._Scripts.Quests
{
    public class QuestMark: MonoBehaviour
    {
        [SerializeField] private QuestMarkType type;
        public QuestMarkType Type => type;
        
        
    }
}