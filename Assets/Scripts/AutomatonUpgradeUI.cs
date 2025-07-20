using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller
{
    public class AutomatonUpgradeUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent nameLse;
        [SerializeField] private LocalizeStringEvent descriptionLse;
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
            nameLse.SetEntry(_automatonUpgrades.GetNameEntryName(automatonUpgrade.ID));
            descriptionLse.SetEntry(_automatonUpgrades.GetDescriptionEntryName(automatonUpgrade.ID));
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, 0, false, bonusColor);
        }

        
        private void Update()
        {
            bonusText.text = _automatonUpgrades.GetBonusText(automatonUpgrade.ID, automatonUpgrade.Level, automatonUpgrade.MaxedOut, bonusColor);
        }
    }
}