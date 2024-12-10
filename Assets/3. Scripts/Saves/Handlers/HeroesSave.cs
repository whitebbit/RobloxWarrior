using System;

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
        
        public override void Select(HeroSave obj)
        {
            if (selected.Count == capacity) return;
            
            selected.Add(obj);
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
    }
}