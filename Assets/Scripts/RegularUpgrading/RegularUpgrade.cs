using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller.RegularUpgrading
{
    public class RegularUpgrade : MonoBehaviour
    {
        [SerializeField] private Purchasable purchasable;
        
        private RegularUpgrades _regularUpgrades;

        public int ID { get; private set; }
        public int Level => purchasable.Amount;
        public bool MaxedOut => purchasable.MaxedOut;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }


        private void Start()
        {
            RegularUpgradeEntry entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(entry.Price, entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(entry.MaxLevel);

            ID = entry.ID;

            if (GameStateHandler.Loaded)
            {
                if (GameStateHandler.State.RegularUpgradeLevels.ContainsKey(entry.ID))
                {
                    purchasable.Load(GameStateHandler.State.RegularUpgradeLevels[entry.ID]);
                }
            }
        }


        public void Load()
        {
            RegularUpgradeEntry entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(entry.Price, entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(entry.MaxLevel);

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
            if (!GameStateHandler.State.RegularUpgradeLevels.TryAdd(ID, Level))
            {
                GameStateHandler.State.RegularUpgradeLevels[ID] = Level;
            }
        }
    }
}