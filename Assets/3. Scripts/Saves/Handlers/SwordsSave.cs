namespace _3._Scripts.Saves.Handlers
{
    public class SwordsSave : SaveHandler<SwordSave>
    {
        public override bool IsCurrent(SwordSave obj)
        {
            return obj.uid == current.uid;
        }

        public override void SetCurrent(SwordSave obj)
        {
            current = obj;
        }

        public override void Unlock(SwordSave obj)
        {
            if (Unlocked(obj)) return;
            
            unlocked.Add(obj);
        }

        public override bool Unlocked(SwordSave obj)
        {
            return unlocked.Exists(s => s.uid == obj.uid);
        }
    }
}