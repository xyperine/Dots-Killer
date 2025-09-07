using System;
using BreakInfinity;
using NaughtyAttributes;
using UnityEngine;

namespace DotsKiller.Economy.BulkBuy
{
    [Serializable]
    public struct SerializableBulkBuyAmount
    {
        [SerializeField] private bool max;
        [SerializeField, HideIf(nameof(max))] private BigDouble value;


        public BulkBuyAmount CreateNonSerializable()
        {
            return new BulkBuyAmount(value, max);
        }
    }
}