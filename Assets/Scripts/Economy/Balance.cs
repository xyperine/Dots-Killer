using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class Balance : MonoBehaviour
    {
        [SerializeField] private BigDouble startingPoints = BigDouble.Zero;

        private RegularUpgrades _regularUpgrades;
        private Milestones _milestones;

        private Dictionary<Currency, BigDouble> Currencies { get; } =
            EnumHelpers.EnumToDictionary<Currency, BigDouble>(BigDouble.Zero);

        public BigDouble Points => Currencies[Currency.Points];
        public BigDouble Shards => Currencies[Currency.Shards];
        
        public BigDouble TotalPoints { get; private set; }


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, Milestones milestones)
        {
            _regularUpgrades = regularUpgrades;
            _milestones = milestones;
        }
        
        
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
                BigDouble multiplier = _regularUpgrades.KillsFactor;
                multiplier *= _regularUpgrades.CleanFactor;
                multiplier *= _regularUpgrades.TimeFactor;
                multiplier *= _regularUpgrades.AccumulationFactor;
                multiplier *= _milestones.PointsIncomeMultiplier;
                multiplier *= _milestones.UpgradesFactor;

                BigDouble exponent = _regularUpgrades.GrowthExponent;

                amount = BigDouble.Pow(amount * multiplier, exponent);
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