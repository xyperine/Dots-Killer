using DotsKiller.Automatons;
using DotsKiller.Automatons.Upgrades;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace DotsKiller.UI.Automatons
{
    public class AutoPurchaseFormatter : MonoBehaviour, IAutomatonFormatter
    {
        [SerializeField] private AutoPurchaseAutomaton automaton;
        [SerializeField] private LocalizedStringTable unitsTable;

        private StringTable _table;
        private AutomatonUpgrades _upgrades;
        private LocalizationAssetsHelper _localizationAssetsHelper;

        public string TickspeedFormatted => Formatting.DefaultFormat(automaton.Tickspeed);
        public string TickspeedUnitsFormatted => _table.GetEntry("Units.Sec").Value;
        public string TickspeedSeparator => "/";
        
        public string ActionsPerTickFormatted => _upgrades.AutoPurchaseAptMaxedOut
            ? _table.GetEntry("Units.All").Value.ToUpper()
            : Formatting.DefaultFormat(automaton.ActionsPerTick);
        
        public string ActionsPerTickUnitsFormatted => _upgrades.AutoPurchaseAptMaxedOut
            ? string.Empty
            : _table.GetEntry("Units.Tick").Value;
        
        public string ActionsPerTickSeparator => _upgrades.AutoPurchaseAptMaxedOut
            ? string.Empty
            : "/";


        [Inject]
        public void Initialize(AutomatonUpgrades upgrades, LocalizationAssetsHelper localizationAssetsHelper)
        {
            _upgrades = upgrades;
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