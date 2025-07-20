using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class AutomatonUpgrade : MonoBehaviour
    {
        [SerializeField] private Purchasable purchasable;
        
        private AutomatonUpgrades _automatonUpgrades;

        private AutomatonUpgradeEntry _entry;

        public int ID { get; private set; }
        public int Level => purchasable.Amount;
        public bool MaxedOut => purchasable.MaxedOut;


        [Inject]
        public void Initialize(AutomatonUpgrades automatonUpgrades)
        {
            _automatonUpgrades = automatonUpgrades;
        }


        private void Awake()
        {
            _entry = _automatonUpgrades.GetSorted(transform.GetSiblingIndex());
            
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
            
            _entry = _automatonUpgrades.GetSorted(transform.GetSiblingIndex());
            
            purchasable.SetPrice(_entry.Price, _entry.PriceScaling, Currency.Points);
            purchasable.SetMaxAmount(_entry.MaxLevel);

            ID = _entry.ID;

            if (GameStateHandler.State.AutomatonUpgradeLevels.ContainsKey(_entry.ID))
            {
                purchasable.Load(GameStateHandler.State.AutomatonUpgradeLevels[_entry.ID]);
            }
        }
        

        private void Update()
        {
            if (!GameStateHandler.State.AutomatonUpgradeLevels.TryAdd(ID, Level))
            {
                GameStateHandler.State.AutomatonUpgradeLevels[ID] = Level;
            }
        }
    }
}