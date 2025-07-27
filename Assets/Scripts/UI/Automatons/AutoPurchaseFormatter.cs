using DotsKiller.Automatons;
using DotsKiller.Automatons.Upgrades;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace DotsKiller.UI.Automatons
{
    //TODO: instead of polling table every time we can make a variable and update it when locale changes
    public class AutoPurchaseFormatter : MonoBehaviour, IAutomatonFormatter
    {
        [SerializeField] private AutoPurchaseAutomaton automaton;
        [SerializeField] private LocalizedStringTable unitsTable;

        private AutomatonUpgrades _upgrades;
            

        [Inject]
        public void Initialize(AutomatonUpgrades upgrades)
        {
            _upgrades = upgrades;
        }
            

        public string TickspeedFormatted => Formatting.DefaultFormat(automaton.Tickspeed);
        
        public string TickspeedUnitsFormatted => unitsTable.GetTable().GetEntry("Units.Sec").Value;
        public string TickspeedSeparator => "/";

        public string ActionsPerTickFormatted => _upgrades.AutoPurchaseAptMaxedOut
            ? unitsTable.GetTable().GetEntry("Units.All").Value.ToUpper()
            : Formatting.DefaultFormat(automaton.ActionsPerTick);

        public string ActionsPerTickUnitsFormatted => _upgrades.AutoPurchaseAptMaxedOut
            ? string.Empty
            : unitsTable.GetTable().GetEntry("Units.Tick").Value;

        public string ActionsPerTickSeparator => _upgrades.AutoPurchaseAptMaxedOut
            ? string.Empty
            : "/";
    }
}