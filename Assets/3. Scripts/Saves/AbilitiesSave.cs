using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Abilities.Scriptables;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class AbilitiesSave
    {
        public List<AbilitySave> abilities = new();

        public void Unlock(PlayerAbility playerAbility)
        {
            if (Unlocked(playerAbility.ID)) return;

            var data = new AbilitySave
            {
                id = playerAbility.ID,
                level = 0,
                maxLevel = 1
            };

            abilities.Add(data);
        }

        public AbilitySave Get(string id)
        {
            return !Unlocked(id) ? new AbilitySave { id = id } : abilities.FirstOrDefault(a => a.id == id);
        }

        public bool Unlocked(string id) => abilities.Exists(a => a.id == id);
    }
}