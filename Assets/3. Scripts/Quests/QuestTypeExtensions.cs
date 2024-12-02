using System;
using _3._Scripts.Quests.Enums;

namespace _3._Scripts.Quests
{
    public static class QuestTypeExtensions
    {
        public static string GetDescription(this QuestType type)
        {
            return type switch
            {
                QuestType.WavesPassed => "quest_waves_passed",
                QuestType.EnemyKills => "quest_enemy_kills",
                QuestType.OpeningEgg => "quest_opening_egg",
                QuestType.Rebirth => "quest_rebirth",
                QuestType.CompleteWave => "quest_complete_wave",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}