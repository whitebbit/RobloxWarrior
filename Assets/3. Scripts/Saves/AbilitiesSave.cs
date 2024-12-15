using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class AbilitiesSave : ManySaveHandler<AbilitySave>
    {
        public void Unlock(PlayerAbility playerAbility)
        {
            if (Unlocked(playerAbility.ID)) return;

            var data = new AbilitySave
            {
                id = playerAbility.ID,
                level = 0,
                maxLevel = 1
            };

            unlocked.Add(data);
            InvokeOnSelect(data);
        }

        public void Upgrade(PlayerAbility playerAbility)
        {
            if (!Unlocked(playerAbility.ID)) return;
            
            var item = unlocked.FirstOrDefault(x => x.id == playerAbility.ID);
           
            if (item != null) 
                item.level++;
        }
        
        public AbilitySave Get(string id)
        {
            return !Unlocked(id) ? new AbilitySave { id = id } : unlocked.FirstOrDefault(a => a.id == id);
        }
        
    }
}