using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.Utility;
using UnityEngine;

namespace DotsKiller.Economy
{
    public class Balance : MonoBehaviour
    {
        [SerializeField] private BigDouble startingPoints = BigDouble.Zero;
        
        private Dictionary<Currency, BigDouble> Currencies { get; } =
            EnumHelpers.EnumToDictionary<Currency, BigDouble>(BigDouble.Zero);

        public BigDouble Points => Currencies[Currency.Points];
        public BigDouble Shards => Currencies[Currency.Shards];


        private void Awake()
        {
            Currencies[Currency.Points] = startingPoints;
        }


        public void Add(BigDouble amount, Currency currency)
        {
            if (amount <= BigDouble.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Currencies[currency] += amount;
        }
        
        
        public void Subtract(BigDouble amount, Currency currency)
        {
            if (amount <= BigDouble.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Currencies[currency] -= amount;
        }


        public bool IsAffordable(BigDouble amount, Currency currency)
        {
            return Currencies[currency] >= amount;
        }


        public BigDouble Available(Currency currency)
        {
            return Currencies[currency];
        }
    }
}