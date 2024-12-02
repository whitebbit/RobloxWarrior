namespace _3._Scripts.Quests.Interfaces
{
    public interface IQuestEvent
    {
        void OnEventRaised(object sender, QuestEventArgs args);

    }
}