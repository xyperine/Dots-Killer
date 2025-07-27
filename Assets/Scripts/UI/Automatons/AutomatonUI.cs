using AYellowpaper;
using DotsKiller.Automatons;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace DotsKiller.UI.Automatons
{
    public class AutomatonUI : MonoBehaviour
    {
        [SerializeField] private Automaton automaton;
        [SerializeField] private LocalizeStringEvent tickspeedLse;
        [SerializeField] private LocalizeStringEvent actionsPerTickLse;
        [SerializeField, RequireInterface(typeof(IAutomatonFormatter))] private MonoBehaviour formatterMono;

        private IAutomatonFormatter _formatter;
        

        private void Awake()
        {
            _formatter = formatterMono as IAutomatonFormatter;
        }


        private void Update()
        {
            LocalizedString tickspeedString = tickspeedLse.StringReference;
            ((StringVariable) tickspeedString["value"]).Value = _formatter.TickspeedFormatted;
            ((StringVariable) tickspeedString["units"]).Value = _formatter.TickspeedSeparator + _formatter.TickspeedUnitsFormatted;
            
            LocalizedString actionsPerTickString = actionsPerTickLse.StringReference;
            ((StringVariable) actionsPerTickString["value"]).Value = _formatter.ActionsPerTickFormatted;
            ((StringVariable) actionsPerTickString["units"]).Value = _formatter.ActionsPerTickSeparator + _formatter.ActionsPerTickUnitsFormatted;
        }
        

        public void SetStatus(bool value)
        {
            automaton.SetStatus(value);
        }
    }
}