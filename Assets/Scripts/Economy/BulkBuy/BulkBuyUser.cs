using System.Collections.Generic;

namespace DotsKiller.Economy.BulkBuy
{
    public readonly struct BulkBuyUser
    {
        public Dictionary<BulkBuyCategory, BulkBuyAmount> Modes { get; init; }


        public bool IsActive(BulkBuyCategory category)
        {
            return Modes.ContainsKey(category) && (Modes[category].Value > 1 || Modes[category].Max);
        }
        

        public BulkBuyUser(Dictionary<BulkBuyCategory, BulkBuyAmount> modes)
        {
            Modes = modes;
        }
    }
}