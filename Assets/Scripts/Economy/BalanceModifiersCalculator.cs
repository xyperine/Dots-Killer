using BreakInfinity;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using DotsKiller.StatsLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class BalanceModifiersCalculator : MonoBehaviour
    {
        private RegularUpgrades _regularUpgrades;
        private Milestones _milestones;
        private Stats _stats;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, Milestones milestones, Stats stats)
        {
            _regularUpgrades = regularUpgrades;
            _milestones = milestones;
            _stats = stats;
        }
        
        
        public BigDouble ApplyPointsModifiers(BigDouble amount)
        {
            BigDouble multiplier = _regularUpgrades.KillsFactor;
            multiplier *= _regularUpgrades.CleanFactor;
            multiplier *= _regularUpgrades.TimeFactor;
            multiplier *= _regularUpgrades.AccumulationFactor;
            multiplier *= _milestones.PointsIncomeMultiplier;
            multiplier *= _milestones.UpgradesFactor;

            BigDouble exponent = _stats.PointsIncomeExponent;
            exponent *= _regularUpgrades.GrowthExponent;

            amount = BigDouble.Pow(amount * multiplier, exponent);
            return amount;
        }
    }
}
