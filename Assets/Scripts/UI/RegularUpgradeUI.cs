using DotsKiller.RegularUpgrading;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller.UI
{
    public class RegularUpgradeUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent titleLse;
        [SerializeField] private LocalizeStringEvent descriptionLse;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private RegularUpgrade regularUpgrade;
        [SerializeField] private Color bonusColor;

        private RegularUpgrades _regularUpgrades;

        private string _localizedTitle;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
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
            titleLse.SetEntry(_regularUpgrades.GetTitleEntryName(regularUpgrade.ID));
            descriptionLse.SetEntry(_regularUpgrades.GetDescriptionEntryName(regularUpgrade.ID));
            bonusText.SetText(_regularUpgrades.GetBonusText(regularUpgrade.ID, 0, 1, false, bonusColor));
        }


        private void Update()
        {
            bonusText.SetText(_regularUpgrades.GetBonusText(regularUpgrade.ID, regularUpgrade.Level,
                regularUpgrade.NextLevel, regularUpgrade.MaxedOut, bonusColor));
            titleText.SetText($"{_localizedTitle} ({regularUpgrade.Level})");
        }


        private void OnDisable()
        {
            titleLse.OnUpdateString.RemoveListener(UpdateTitle);
        }
    }
}