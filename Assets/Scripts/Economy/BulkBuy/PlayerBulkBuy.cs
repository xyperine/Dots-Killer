using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Economy.BulkBuy
{
    public class PlayerBulkBuy : MonoBehaviour
    {
        public BulkBuyUser User { get; } = new BulkBuyUser(new Dictionary<BulkBuyCategory, BulkBuyAmount>());


        public void SetCategoryAmount(BulkBuyCategory category, BulkBuyAmount amount)
        {
            if (!User.Modes.TryAdd(category, amount))
            {
                User.Modes[category] = amount;
            }
        }
    }
}