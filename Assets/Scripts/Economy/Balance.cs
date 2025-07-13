using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class Balance : MonoBehaviour
    {
        [SerializeField] private BigDouble startingPoints = BigDouble.Zero;

        private RegularUpgrades _regularUpgrades;
        
        private Dictionary<Currency, BigDouble> Currencies { get; } =
            EnumHelpers.EnumToDictionary<Currency, BigDouble>(BigDouble.Zero);

        public BigDouble Points => Currencies[Currency.Points];
        public BigDouble Shards => Currencies[Currency.Shards];


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }
        
        
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

            if (currency == Currency.Points)
            {
                BigDouble multiplier = _regularUpgrades.KillsFactor;
                multiplier *= _regularUpgrades.CleanFactor;
                multiplier *= _regularUpgrades.TimeFactor;
                
                amount *= multiplier;
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