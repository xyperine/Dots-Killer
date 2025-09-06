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
                Active = true,
                Modes = new Dictionary<BulkBuyCategory, BulkBuyAmount>
                {
                    {category, amount},
                },
            };
        }
    }
}