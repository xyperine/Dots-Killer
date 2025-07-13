using System;
using System.Collections.Generic;

namespace DotsKiller.SaveSystem
{
    public class GameState
    {
        public bool IsDirty { get; set; }
        
        public DateTime LastSeen { get; set; }

        public Dictionary<int, int> RegularUpgradeLevels { get; } = new Dictionary<int, int>();
    }
}