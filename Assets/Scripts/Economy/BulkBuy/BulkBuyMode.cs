using BreakInfinity;

namespace DotsKiller.Economy.BulkBuy
{
    public struct BulkBuyMode
    {
        public BigDouble? Amount { get; init; }
        public bool Max { get; init; }
        
        
        public BulkBuyMode(BigDouble? amount = null, bool max = false)
        {
            Amount = amount;
            Max = max;
        }
        
        
        public static BulkBuyMode CreateAsNumber(BigDouble amount)
        {
            return new BulkBuyMode(amount);
        }


        public static BulkBuyMode CreateAsMax()
        {
            return new BulkBuyMode(max: true);
        }
    }
}