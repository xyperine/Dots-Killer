using BreakInfinity;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class StatsDebugUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointsMultText;
        [SerializeField] private TMP_Text killsFactorText;
        [SerializeField] private TMP_Text cleanFactorText;
        [SerializeField] private TMP_Text timeFactorText;
        [SerializeField] private TMP_Text accumulationFactorText;
        [SerializeField] private TMP_Text powerFarmExponentText;
        [SerializeField] private TMP_Text totalPointsPerKillText;
        [SerializeField] private TMP_Text killsText;

        private Stats _stats;
        private RegularUpgrades _regularUpgrades;


        [Inject]
        public void Initialize(Stats stats, RegularUpgrades regularUpgrades)
        {
            _stats = stats;
            _regularUpgrades = regularUpgrades;
        }
        

        private void Update()
        {
            pointsMultText.text = "Points per kill: " + Formatting.DefaultFormat(_regularUpgrades.PointsOnKill);
            killsFactorText.text = "Kills factor: " + Formatting.DefaultFormat(_regularUpgrades.KillsFactor);
            cleanFactorText.text = "Clean factor: " + Formatting.DefaultFormat(_regularUpgrades.CleanFactor);
            timeFactorText.text = "Time factor: " + Formatting.DefaultFormat(_regularUpgrades.TimeFactor);
            accumulationFactorText.text = "Accumulation factor: " + Formatting.DefaultFormat(_regularUpgrades.AccumulationFactor);
            powerFarmExponentText.text = "Power Farm Exponent: " + Formatting.DefaultFormat(_regularUpgrades.GrowthExponent);
            
            BigDouble multiplier = _regularUpgrades.KillsFactor *
                                   _regularUpgrades.CleanFactor * _regularUpgrades.TimeFactor *
                                   _regularUpgrades.AccumulationFactor;
            totalPointsPerKillText.text = "Total points per kill: " + Formatting.DefaultFormat(
                BigDouble.Pow((_regularUpgrades.PointsOnKill + BigDouble.One) * multiplier, _regularUpgrades.GrowthExponent));
            
            killsText.text = "Kills: " + Formatting.DefaultFormat(_stats.Kills);
        }
    }
}