using System;

namespace DotsKiller.SaveSystem
{
    public class GameState
    {
        public bool IsDirty { get; set; }
        
        public DateTime LastSeen { get; set; }
    }
}