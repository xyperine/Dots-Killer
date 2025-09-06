using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Economy
{
    public class BulkBuyProfile : MonoBehaviour
    {
        public BulkBuyProvider Provider { get; private set; }


        public void Recreate(BulkBuyCategory category, BulkBuyAmount amount)
        {
            Provider = new BulkBuyProvider
            {
                Active = amount.Value.Value > 1 || amount.Max,
                Modes = new Dictionary<BulkBuyCategory, BulkBuyAmount>
                {
                    {category, amount},
                },
            };
        }
    }
}