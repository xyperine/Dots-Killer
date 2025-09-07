using System;
using BreakInfinity;
using NaughtyAttributes;
using UnityEngine;

namespace DotsKiller.Economy.BulkBuy
{
    [Serializable]
    public struct SerializableBulkBuyMode
    {
        [SerializeField] private bool max;
        [SerializeField, HideIf(nameof(max))] private BigDouble amount;


        public BulkBuyMode CreateNonSerializable()
        {
            return new BulkBuyMode(amount, max);
        }
    }
}