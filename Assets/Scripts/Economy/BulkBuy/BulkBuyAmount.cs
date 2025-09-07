using BreakInfinity;

namespace DotsKiller.Economy.BulkBuy
{
    public struct BulkBuyAmount
    {
        public BigDouble? Value { get; init; }
        public bool Max { get; init; }
        
        
        public BulkBuyAmount(BigDouble? value = null, bool max = false)
        {
            Value = value;
            Max = max;
        }
        
        
        public static BulkBuyAmount CreateAsNumber(BigDouble amount)
        {
            return new BulkBuyAmount(amount);
        }


        public static BulkBuyAmount CreateAsMax()
        {
            return new BulkBuyAmount(max: true);
        }
    }
}