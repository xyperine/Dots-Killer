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
        public bool MaxedOut => purchasable.MaxedOut;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades)
        {
            _regularUpgrades = regularUpgrades;
        }


        private void Awake()
        {
            _entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(_entry.Price, _entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(_entry.MaxLevel);

            ID = _entry.ID;
        }


        private void Start()
        {
            Load();
        }


        public void Load()
        {
            if (!GameStateHandler.Loaded)
            {
                return;
            }
            
            _entry = _regularUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(_entry.Price, _entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(_entry.MaxLevel);

            ID = _entry.ID;

            if (GameStateHandler.State.RegularUpgradeLevels.ContainsKey(_entry.ID))
            {
                purchasable.Load(GameStateHandler.State.RegularUpgradeLevels[_entry.ID]);
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