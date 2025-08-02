using System;
using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.SaveSystem;
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


        private void Start()
        {
            if (GameStateHandler.Loaded)
            {
                Currencies[Currency.Points] = GameStateHandler.State.Points;
                Currencies[Currency.Shards] = GameStateHandler.State.Shards;
                TotalPoints = GameStateHandler.State.TotalPoints;
            }
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


        public void OnPurge()
        {
            Debug.Log("Balance reset");

            Currencies[Currency.Points] = startingPoints;
            //Currencies[Currency.Shards] = SOME AMOUNT;
            TotalPoints = Points;

            GameStateHandler.State.Points = Points;
            GameStateHandler.State.TotalPoints = TotalPoints;
        }


        private void Update()
        {
            GameStateHandler.State.Points = Points;
            GameStateHandler.State.Shards = Shards;
            GameStateHandler.State.TotalPoints = TotalPoints;
        }
    }
}