using DotsKiller.Automatons.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller.UI.Automatons
{
    public class AutomatonUpgradeUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent titleLse;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private AutomatonUpgrade automatonUpgrade;
        [SerializeField] private Color bonusColor;

        private AutomatonUpgrades _automatonUpgrades;
        
        private string _localizedTitle;


        [Inject]
        public void Initialize(AutomatonUpgrades automatonUpgrades)
        {
            _automatonUpgrades = automatonUpgrades;
        }
        
        
        private void OnEnable()
        {
            titleLse.OnUpdateString.AddListener(UpdateTitle);
        }
        
        
        private void UpdateTitle(string localizedTitle)
        {
            _localizedTitle = localizedTitle;
        }


        private void Start()
        {
            titleLse.SetEntry(_automatonUpgrades.GetTitleTableEntryName(automatonUpgrade.ID));
            bonusText.SetText(_automatonUpgrades.GetBonusText(automatonUpgrade.ID, 0, 1, automatonUpgrade.MaxLevel,
                bonusColor));
        }

        
        private void Update()
        {
            bonusText.SetText(_automatonUpgrades.GetBonusText(automatonUpgrade.ID, automatonUpgrade.Level,
                automatonUpgrade.NextLevel, automatonUpgrade.MaxLevel, bonusColor));
            titleText.SetText($"{_localizedTitle} ({automatonUpgrade.Level})");
        }
        
        
        private void OnDisable()
        {
            titleLse.OnUpdateString.RemoveListener(UpdateTitle);
        }
    }
}