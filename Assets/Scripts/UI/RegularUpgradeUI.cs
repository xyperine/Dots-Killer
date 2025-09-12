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
            nameLse.OnUpdateString.AddListener(UpdateTitle);
        }


        private void UpdateTitle(string localizedTitle)
        {
            _localizedTitle = localizedTitle;
        }


        private void Start()
        {
            nameLse.SetEntry(_regularUpgrades.GetNameEntryName(regularUpgrade.ID));
            descriptionLse.SetEntry(_regularUpgrades.GetDescriptionEntryName(regularUpgrade.ID));
            bonusText.text = _regularUpgrades.GetBonusText(regularUpgrade.ID, 0, 1, false, bonusColor);
        }


        private void Update()
        {
            bonusText.text = _regularUpgrades.GetBonusText(regularUpgrade.ID, regularUpgrade.Level, regularUpgrade.NextLevel, regularUpgrade.MaxedOut, bonusColor);
            titleText.SetText($"{_localizedTitle} ({regularUpgrade.Level})");
            //Debug.Log(_localizedTitle);
        }


        private void OnDisable()
        {
            nameLse.OnUpdateString.AddListener(UpdateTitle);
        }
    }
}