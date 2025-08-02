using System;
using System.Collections.Generic;
using BreakInfinity;

namespace DotsKiller.SaveSystem
{
    public class GameState
    {
        public bool IsDirty { get; set; }
        
        public DateTime LastSeen { get; set; }
        public DateTime FirstTimePlayedAt { get; set; } = DateTime.MaxValue;

        public Dictionary<int, int> RegularUpgradeLevels { get; } = new Dictionary<int, int>();
        public Dictionary<int, int> AutomatonUpgradeLevels { get; } = new Dictionary<int, int>();
        
        public BigDouble Kills { get; set; }
        public BigDouble Purges { get; set; }
        
        public BigDouble Points { get; set; }
        public BigDouble Shards { get; set; }
        public BigDouble TotalPoints { get; set; }
    }
}