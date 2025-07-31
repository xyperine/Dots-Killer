using BreakInfinity;
using DotsKiller.Dots;
using DotsKiller.Economy;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text spawnRateText;
        [SerializeField] private TMP_Text pointsMultText;
        [SerializeField] private TMP_Text killsFactorText;
        [SerializeField] private TMP_Text cleanFactorText;
        [SerializeField] private TMP_Text timeFactorText;
        [SerializeField] private TMP_Text accumulationFactorText;
        [SerializeField] private TMP_Text powerFarmExponentText;
        [SerializeField] private TMP_Text totalPointsPerKillText;
        [SerializeField] private TMP_Text milestone1MultiplierText;
        [SerializeField] private TMP_Text upgradesFactorText;
        [SerializeField] private TMP_Text killsText;

        private Stats _stats;
        private RegularUpgrades _regularUpgrades;
        private Milestones _milestones;
        private DotSpawner _dotSpawner;
        private BalanceModifiersCalculator _balanceModifiersCalculator;


        [Inject]
        public void Initialize(Stats stats, RegularUpgrades regularUpgrades, Milestones milestones, DotSpawner dotSpawner, BalanceModifiersCalculator balanceModifiersCalculator)
        {
            _stats = stats;
            _regularUpgrades = regularUpgrades;
            _milestones = milestones;
            _dotSpawner = dotSpawner;
            _balanceModifiersCalculator = balanceModifiersCalculator;
        }
        

        private void Update()
        {
            spawnRateText.text = _dotSpawner.SpawnRate.ToString("F2");
            
            pointsMultText.text = "Points per kill: " + Formatting.DefaultFormat(_regularUpgrades.PointsOnKill);
            killsFactorText.text = "Kills factor: " + Formatting.DefaultFormat(_regularUpgrades.KillsFactor);
            cleanFactorText.text = "Clean factor: " + Formatting.DefaultFormat(_regularUpgrades.CleanFactor);
            timeFactorText.text = "Time factor: " + Formatting.DefaultFormat(_regularUpgrades.TimeFactor);
            accumulationFactorText.text = "Accumulation factor: " + Formatting.DefaultFormat(_regularUpgrades.AccumulationFactor);
            powerFarmExponentText.text = "Power Farm Exponent: " + Formatting.DefaultFormat(_regularUpgrades.GrowthExponent);
            milestone1MultiplierText.text = "Milestone 1 Multiplier: " +
                                            Formatting.DefaultFormat(_milestones.PointsIncomeMultiplier);
            upgradesFactorText.text = "Upgrades factor: " + Formatting.DefaultFormat(_milestones.UpgradesFactor);

            totalPointsPerKillText.text = "Total points per kill: " +
                                          Formatting.DefaultFormat(
                                              _balanceModifiersCalculator.ApplyPointsModifiers(BigDouble.One +
                                                  _regularUpgrades.PointsOnKill));
            
            killsText.text = "Kills: " + Formatting.DefaultFormat(_stats.Kills);
        }
    }
}