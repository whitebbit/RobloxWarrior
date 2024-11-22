using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public abstract class SaveHandler<T>
    {
        public T current;
        public List<T> unlocked = new();

        public abstract bool IsCurrent(T obj);
        public abstract void SetCurrent(T obj);
        
        public abstract void Unlock(T obj);
        public abstract bool Unlocked(T obj);
    }
}