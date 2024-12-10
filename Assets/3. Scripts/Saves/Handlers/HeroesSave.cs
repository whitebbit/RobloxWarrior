using System;
using System.Linq;

namespace _3._Scripts.Saves.Handlers
{
    [Serializable]
    public class HeroesSave : ManySaveHandler<HeroSave>
    {
        public override bool IsSelected(HeroSave obj)
        {
            return selected.Exists(s => s.id == obj.id);
        }

        public bool IsSelected(string id)
        {
            return selected.Exists(s => s.id == id);
        }

        public event Action<HeroSave> OnSelect;

        public override void Select(HeroSave obj)
        {
            if (selected.Count == capacity) return;

            selected.Add(obj);
            OnSelect?.Invoke(obj);
        }

        public void Select(string obj)
        {
            if (!Unlocked(obj)) return;
            if (selected.Count == capacity) return;

            var item = unlocked.FirstOrDefault(s => s.id == obj);
            selected.Add(item);
            OnSelect?.Invoke(item);
        }

        public void Unselect(string obj)
        {
            if (!Unlocked(obj)) return;
            var item = unlocked.FirstOrDefault(s => s.id == obj);
            selected.Remove(item);
            OnSelect?.Invoke(item);
        }

        public override void Unlock(HeroSave obj)
        {
            if (Unlocked(obj)) return;

            unlocked.Add(obj);
        }

        public override bool Unlocked(HeroSave obj)
        {
            return unlocked.Exists(s => s.id == obj.id);
        }

        public bool Unlocked(string obj)
        {
            return unlocked.Exists(s => s.id == obj);
        }
    }
}