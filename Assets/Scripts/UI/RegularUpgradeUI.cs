using DotsKiller.RegularUpgrading;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller.UI
{
    public class RegularUpgradeUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent nameLse;
        [SerializeField] private LocalizeStringEvent descriptionLse;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private RegularUpgrade regularUpgrade;

        private RegularUpgrades _regularUpgrades;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }
        
        
        private void Start()
        {
            nameLse.SetEntry(_regularUpgrades.GetName(regularUpgrade.ID));
            descriptionLse.SetEntry(_regularUpgrades.GetDescription(regularUpgrade.ID));
            bonusText.text = _regularUpgrades.GetBonusText(regularUpgrade.ID, 0, false);
        }

        
        private void Update()
        {
            bonusText.text = _regularUpgrades.GetBonusText(regularUpgrade.ID, regularUpgrade.Level, regularUpgrade.MaxedOut);
        }
    }
}