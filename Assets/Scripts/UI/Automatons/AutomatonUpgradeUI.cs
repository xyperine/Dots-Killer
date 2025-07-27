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
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private AutomatonUpgrade automatonUpgrade;
        [SerializeField] private Color bonusColor;

        private AutomatonUpgrades _automatonUpgrades;


        [Inject]
        public void Initialize(AutomatonUpgrades automatonUpgrades)
        {
            _automatonUpgrades = automatonUpgrades;
        }
        
        
        private void Start()
        {
            nameLse.SetEntry(_automatonUpgrades.GetNameTableEntryName(automatonUpgrade.ID));
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, 0, automatonUpgrade.MaxLevel, bonusColor);
        }

        
        private void Update()
        {
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, automatonUpgrade.Level, automatonUpgrade.MaxLevel, bonusColor);
        }
    }
}