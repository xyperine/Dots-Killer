using System.Collections.Generic;
using BreakInfinity;

namespace DotsKiller.Economy
{
    public struct BulkBuyProvider
    {
        public bool Active { get; init; }
        public Dictionary<BulkBuyCategory, BulkBuyAmount> Modes { get; init; }
    }


    public struct BulkBuyAmount
    {
        public BigDouble? Value { get; init; }
        public bool Max { get; init; }
        
        
        public BulkBuyAmount(BigDouble? value = null, bool max = false)
        {
            Value = value;
            Max = max;
        }
    }
}