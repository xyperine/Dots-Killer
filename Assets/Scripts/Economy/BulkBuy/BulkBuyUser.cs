using System.Collections.Generic;

namespace DotsKiller.Economy.BulkBuy
{
    public readonly struct BulkBuyUser
    {
        public Dictionary<BulkBuyCategory, BulkBuyMode> Modes { get; init; }


        public bool IsActive(BulkBuyCategory category)
        {
            return Modes.ContainsKey(category) && (Modes[category].Amount > 1 || Modes[category].Max);
        }
        

        public BulkBuyUser(Dictionary<BulkBuyCategory, BulkBuyMode> modes)
        {
            Modes = modes;
        }
    }
}