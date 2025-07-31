using BreakInfinity;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class BalanceModifiersCalculator : MonoBehaviour
    {
        private RegularUpgrades _regularUpgrades;
        private Milestones _milestones;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, Milestones milestones)
        {
            _regularUpgrades = regularUpgrades;
            _milestones = milestones;
        }
        
        
        public BigDouble ApplyPointsModifiers(BigDouble amount)
        {
            BigDouble multiplier = _regularUpgrades.KillsFactor;
            multiplier *= _regularUpgrades.CleanFactor;
            multiplier *= _regularUpgrades.TimeFactor;
            multiplier *= _regularUpgrades.AccumulationFactor;
            multiplier *= _milestones.PointsIncomeMultiplier;
            multiplier *= _milestones.UpgradesFactor;

            BigDouble exponent = _regularUpgrades.GrowthExponent;

            amount = BigDouble.Pow(amount * multiplier, exponent);
            return amount;
        }
    }
}
