using System.Collections.Generic;

namespace DotsKiller.Economy.BulkBuy
{
    public struct BulkBuyUser
    {
        public bool Active { get; init; }
        public Dictionary<BulkBuyCategory, BulkBuyAmount> Modes { get; init; }


        public BulkBuyUser(bool active, Dictionary<BulkBuyCategory, BulkBuyAmount> modes)
        {
            Active = active;
            Modes = modes;
        }
    }
}