using System;
using _3._Scripts.Quests.Enums;

namespace _3._Scripts.Quests
{
    public class QuestEventArgs : EventArgs
    {
        public QuestType EventType { get; }
        public object EventData { get; }

        public QuestEventArgs(QuestType eventType, object eventData)
        {
            EventType = eventType;
            EventData = eventData;
        }
    }
}