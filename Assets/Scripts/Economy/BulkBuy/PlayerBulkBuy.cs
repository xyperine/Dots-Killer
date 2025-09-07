using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Economy.BulkBuy
{
    public class PlayerBulkBuy : MonoBehaviour
    {
        public BulkBuyUser User { get; } = new BulkBuyUser(new Dictionary<BulkBuyCategory, BulkBuyMode>());


        public void SetCategoryMode(BulkBuyCategory category, BulkBuyMode mode)
        {
            if (!User.Modes.TryAdd(category, mode))
            {
                User.Modes[category] = mode;
            }
        }
    }
}