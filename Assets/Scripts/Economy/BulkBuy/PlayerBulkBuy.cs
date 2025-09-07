using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Economy.BulkBuy
{
    public class PlayerBulkBuy : MonoBehaviour
    {
        public BulkBuyUser User { get; private set; }


        public void Recreate(BulkBuyCategory category, BulkBuyAmount amount)
        {
            User = new BulkBuyUser(amount.Value.Value > 1 || amount.Max,
                new Dictionary<BulkBuyCategory, BulkBuyAmount>
                {
                    {category, amount},
                });
        }
    }
}