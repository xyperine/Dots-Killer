using DotsKiller.Automatons.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller.UI.Automatons
{
    public class AutomatonUpgradeUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent nameLse;
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
            nameLse.OnUpdateString.AddListener(UpdateTitle);
        }
        
        
        private void UpdateTitle(string localizedTitle)
        {
            _localizedTitle = localizedTitle;
        }


        private void Start()
        {
            nameLse.SetEntry(_automatonUpgrades.GetNameTableEntryName(automatonUpgrade.ID));
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, 0, 1, automatonUpgrade.MaxLevel, bonusColor);
        }

        
        private void Update()
        {
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, automatonUpgrade.Level, automatonUpgrade.NextLevel, automatonUpgrade.MaxLevel, bonusColor);
            titleText.SetText($"{_localizedTitle} ({automatonUpgrade.Level})");
        }
        
        
        private void OnDisable()
        {
            nameLse.OnUpdateString.RemoveListener(UpdateTitle);
        }
    }
}