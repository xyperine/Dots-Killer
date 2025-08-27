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

        private RegularUpgradeEntry _entry;

        public int ID { get; private set; }
        public int Level => purchasable.Amount;
        public int NextLevel => purchasable.NextAmount;
        public bool MaxedOut => purchasable.MaxedOut;
        public bool IsAffordable => purchasable.IsAffordable;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }
        

        public void SetData()
        {
            _entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(_entry.Price, _entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(_entry.MaxLevel);

            ID = _entry.ID;
        }
        

        public void Load()
        {
            if (!GameStateHandler.Loaded)
            {
                return;
            }

            if (GameStateHandler.State.RegularUpgradeLevels.ContainsKey(_entry.ID))
            {
                purchasable.Load(GameStateHandler.State.RegularUpgradeLevels[_entry.ID]);
            }
        }
        

        private void Update()
        {
            Save();
        }


        public void Save()
        {
            if (!GameStateHandler.State.RegularUpgradeLevels.TryAdd(ID, Level))
            {
                GameStateHandler.State.RegularUpgradeLevels[ID] = Level;
            }
        }

        
        public void Purchase()
        {
            purchasable.PurchaseSingle();
        }


        public void PurchaseInBulk()
        {
            purchasable.BulkPurchase();
        }
        
        
        public void PurchaseInBulk(int amount)
        {
            purchasable.BulkPurchase(amount);
        }


        public void OnPrestige()
        {
            purchasable.Clear();
            
            Save();
        }
    }
}