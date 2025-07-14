using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace DotsKiller.UI
{
    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent nameLse;
        [SerializeField] private LocalizeStringEvent descriptionLse;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private Purchasable purchasable;
        
        private RegularUpgrades _regularUpgrades;

        
        public int ID { get; private set; }
        public int Level => purchasable.Amount;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }


        private void Start()
        {
            
            RegularUpgradeEntry entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            nameLse.SetEntry(_regularUpgrades.GetName(entry.ID));
            descriptionLse.SetEntry(_regularUpgrades.GetDescription(entry.ID));
            
            purchasable.SetPrice(entry.Price, entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(entry.MaxLevel);
            bonusText.text = _regularUpgrades.GetBonusText(entry.ID, 0, false);

            ID = entry.ID;

            if (GameStateHandler.Loaded)
            {
                if (GameStateHandler.State.RegularUpgradeLevels.ContainsKey(entry.ID))
                {
                    purchasable.Load(GameStateHandler.State.RegularUpgradeLevels[entry.ID]);
                }
            }
        }


        private void Update()
        {
            bonusText.text = _regularUpgrades.GetBonusText(ID, Level, purchasable.MaxedOut);
            
            if (!GameStateHandler.State.RegularUpgradeLevels.TryAdd(ID, Level))
            {
                GameStateHandler.State.RegularUpgradeLevels[ID] = Level;
            }
        }
    }
}