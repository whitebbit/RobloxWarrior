using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public abstract class ManySaveHandler<T>
    {
        public int capacity = 1;
        
        public List<T> selected = new();
        public List<T> unlocked = new();

        public abstract bool IsSelected(T obj);
        public abstract void Select(T obj);

        public abstract void Unlock(T obj);
        public abstract bool Unlocked(T obj);
    }
}