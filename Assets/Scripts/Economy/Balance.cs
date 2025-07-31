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
        [SerializeField] private BalanceModifiersCalculator balanceModifiersCalculator;
        
        private Dictionary<Currency, BigDouble> Currencies { get; } =
            EnumHelpers.EnumToDictionary<Currency, BigDouble>(BigDouble.Zero);

        public BigDouble Points => Currencies[Currency.Points];
        public BigDouble Shards => Currencies[Currency.Shards];
        
        public BigDouble TotalPoints { get; private set; }


        private void Awake()
        {
            Currencies[Currency.Points] = startingPoints;
            TotalPoints = startingPoints;
        }


        public void Add(BigDouble amount, Currency currency)
        {
            if (amount <= BigDouble.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }
            
            if (currency == Currency.Points)
            {
                amount = balanceModifiersCalculator.ApplyPointsModifiers(amount);
            }

            Currencies[currency] += amount;

            if (currency == Currency.Points)
            {
                TotalPoints += amount;
            }
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