using System.Collections.Generic;
using _3._Scripts.Quests.Enums;
using _3._Scripts.Quests.Interfaces;

namespace _3._Scripts.Quests
{
    public class QuestEventManager
    {
        private static QuestEventManager _instance;
        public static QuestEventManager Instance => _instance ??= new QuestEventManager();

        private readonly Dictionary<QuestType, List<IQuestEvent>> _listeners = new();

        public void RegisterListener(QuestType eventType, IQuestEvent listener)
        {
            if (!_listeners.ContainsKey(eventType))
            {
                _listeners[eventType] = new List<IQuestEvent>();
            }
            _listeners[eventType].Add(listener);
        }

        public void UnregisterListener(QuestType eventType, IQuestEvent listener)
        {
            if (_listeners.TryGetValue(eventType, value: out var listener1))
            {
                listener1.Remove(listener);
            }
        }

        public void RaiseEvent(QuestType eventType, object eventData)
        {
            if (!_listeners.TryGetValue(eventType, out var listener1)) return;
            
            foreach (var listener in listener1)
            {
                listener.OnEventRaised(this, new QuestEventArgs(eventType, eventData));
            }
        }
    }

}