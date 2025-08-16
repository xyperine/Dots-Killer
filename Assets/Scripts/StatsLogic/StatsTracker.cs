using System;
using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.RegularUpgrading;
using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller.StatsLogic
{
    public class StatsTracker : MonoBehaviour, IRecalibrationTarget, IPurgeTarget
    {
        [SerializeField] private Stats stats;
        
        private Balance _balance;
        private GameClock _gameClock;
        private RegularUpgrades _regularUpgrades;
        private BalanceModifiersCalculator _balanceModifiersCalculator;


        [Inject]
        public void Initialize(Balance balance, GameClock gameClock, RegularUpgrades regularUpgrades, 
            BalanceModifiersCalculator balanceModifiersCalculator)
        {
            _balance = balance;
            _gameClock = gameClock;
            _regularUpgrades = regularUpgrades;
            _balanceModifiersCalculator = balanceModifiersCalculator;
        }


        private void Start()
        {
            if (!GameStateHandler.Loaded)
            {
                return;
            }

            stats.Kills = GameStateHandler.State.Kills;
        }
        

        private void Update()
        {
            GameStateHandler.State.Kills = stats.Kills;

            stats.TotalPoints = _balance.TotalPoints;
            stats.TotalPlaytime = TimeSpan.FromSeconds(_gameClock.UnscaledTotalPlaytimeSeconds);

            BigDouble baseReward = BigDouble.One;
            stats.PointsPerKill =
                _balanceModifiersCalculator.ApplyPointsModifiers(baseReward + _regularUpgrades.PointsOnKill);

            stats.PointsIncomeExponent = _balanceModifiersCalculator.PointsIncomeExponent;
        }


        public void OnPurge()
        {
            Debug.Log("Stats reset");
            
            stats.Kills = BigDouble.Zero;
            stats.Purges++;
            
            GameStateHandler.State.Kills = stats.Kills;
            GameStateHandler.State.Purges = stats.Purges;
        }


        public void OnRecalibration()
        {
            Debug.Log("Recalibration: Stats");
            
            stats.Kills = BigDouble.Zero;
            stats.PointsIncomeExponent = _balanceModifiersCalculator.PointsIncomeExponent;

            GameStateHandler.State.Kills = stats.Kills;
        }
    }
}