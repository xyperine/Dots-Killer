using DotsKiller.Automatons;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace DotsKiller.UI.Automatons
{
    public class AutoKillFormatter : MonoBehaviour, IAutomatonFormatter
    {
        [SerializeField] private AutoKillAutomaton automaton;
        [SerializeField] private LocalizedStringTable unitsTable;

        private StringTable _table;
        private LocalizationAssetsHelper _localizationAssetsHelper;

        public string TickspeedFormatted => Formatting.DefaultFormat(automaton.Tickspeed);

        public string TickspeedUnitsFormatted => _table.GetEntry("Units.Sec").Value;
        public string TickspeedSeparator => "/";
        public string ActionsPerTickFormatted => Formatting.DefaultFormat(automaton.ActionsPerTick);
        public string ActionsPerTickUnitsFormatted => _table.GetEntry("Units.Tick").Value;
        public string ActionsPerTickSeparator => "/";

        
        [Inject]
        public void Initialize(LocalizationAssetsHelper localizationAssetsHelper)
        {
            _localizationAssetsHelper = localizationAssetsHelper;
        }
        

        private void Start()
        {
            unitsTable.TableChanged += OnTableChanged;
            
            AsyncOperationHandle<StringTable> op = unitsTable.GetTableAsync();
            _localizationAssetsHelper.GetLocalizedAsset(op, table => _table = table);
        }


        private void OnTableChanged(StringTable value)
        {
            _table = value;
        }


        private void OnDestroy()
        {
            unitsTable.TableChanged -= OnTableChanged;
        }
    }
}