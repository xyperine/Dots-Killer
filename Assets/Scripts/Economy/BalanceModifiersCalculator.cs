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
        private Recalibration _recalibration;

        public BigDouble PointsIncomeExponent => _recalibration.CurrentExponent * _regularUpgrades.GrowthExponent;
        

        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, Milestones milestones, Stats stats, Recalibration recalibration)
        {
            _regularUpgrades = regularUpgrades;
            _milestones = milestones;
            _stats = stats;
            _recalibration = recalibration;
        }
        
        
        public BigDouble ApplyPointsModifiers(BigDouble amount)
        {
            BigDouble multiplier = _regularUpgrades.KillsFactor;
            multiplier *= _regularUpgrades.CleanFactor;
            multiplier *= _regularUpgrades.TimeFactor;
            multiplier *= _regularUpgrades.AccumulationFactor;
            multiplier *= _milestones.PointsIncomeMultiplier;
            multiplier *= _milestones.UpgradesFactor;
            multiplier *= _recalibration.CurrentMultiplier;

            BigDouble exponent = PointsIncomeExponent;

            amount = BigDouble.Pow(amount * multiplier, exponent);
            return amount;
        }
    }
}
