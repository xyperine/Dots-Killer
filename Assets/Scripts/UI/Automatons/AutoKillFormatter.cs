using DotsKiller.Automatons;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace DotsKiller.UI.Automatons
{
    public class AutoKillFormatter : MonoBehaviour, IAutomatonFormatter
    {
        [SerializeField] private AutoKillAutomaton automaton;
        [SerializeField] private LocalizedStringTable unitsTable;

        public string TickspeedFormatted => Formatting.DefaultFormat(automaton.Tickspeed);

        public string TickspeedUnitsFormatted => unitsTable.GetTable().GetEntry("Units.Sec").Value;
        public string TickspeedSeparator => "/";
        public string ActionsPerTickFormatted => Formatting.DefaultFormat(automaton.ActionsPerTick);
        public string ActionsPerTickUnitsFormatted => unitsTable.GetTable().GetEntry("Units.Tick").Value;
        public string ActionsPerTickSeparator => "/";
    }
}