using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Saves.Interfaces;

namespace _3._Scripts.Saves
{
    [Serializable]
    public abstract class ManySaveHandler<T> where T : ISavable
    {
        public int capacity = 1;
        public event Action<T> OnSelect;
        public List<T> selected = new();
        public List<T> unlocked = new();

        public bool IsSelected(string id)
        {
            return selected.Exists(s => s.ID == id);
        }

        public bool IsSelected(AbilitySave obj)
        {
            return selected.Exists(s => s.ID == obj.id);
        }

        public void Select(T obj)
        {
            if (selected.Count == capacity) return;

            selected.Add(obj);
            OnSelect?.Invoke(obj);
        }

        public void Select(string obj)
        {
            if (!Unlocked(obj)) return;
            if (selected.Count == capacity) return;

            var item = unlocked.FirstOrDefault(s => s.ID == obj);
            selected.Add(item);
            OnSelect?.Invoke(item);
        }

        public void Unlock(T obj)
        {
            if (Unlocked(obj)) return;

            unlocked.Add(obj);
        }

        public bool Unlocked(T obj) => unlocked.Exists(s => s.ID == obj.ID);

        public bool Unlocked(string id) => unlocked.Exists(a => a.ID == id);

        public void Unselect(string obj)
        {
            if (!Unlocked(obj)) return;

            var item = unlocked.FirstOrDefault(s => s.ID == obj);
            selected.Remove(item);
            OnSelect?.Invoke(item);
        }
        
        protected void InvokeOnSelect(T obj) => OnSelect?.Invoke(obj);
    }
}