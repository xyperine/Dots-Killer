using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
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
            UpgradeEntry entry = _regularUpgrades.GetEntry(transform.GetSiblingIndex());
            
            titleText.text = entry.Title;
            descriptionText.text = entry.Description;
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