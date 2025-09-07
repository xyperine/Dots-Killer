using System;
using System.Collections.Generic;
using BreakInfinity;
using NaughtyAttributes;
using UnityEngine;

namespace DotsKiller.Economy
{
    public struct BulkBuyProvider
    {
        public bool Active { get; init; }
        public Dictionary<BulkBuyCategory, BulkBuyAmount> Modes { get; init; }


        public BulkBuyProvider(bool active, Dictionary<BulkBuyCategory, BulkBuyAmount> modes)
        {
            Active = active;
            Modes = modes;
        }
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
        
        
        public static BulkBuyAmount CreateAsNumber(BigDouble amount)
        {
            return new BulkBuyAmount(amount);
        }


        public static BulkBuyAmount CreateAsMax()
        {
            return new BulkBuyAmount(max: true);
        }
    }


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