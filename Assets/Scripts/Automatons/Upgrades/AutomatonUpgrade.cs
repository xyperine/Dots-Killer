using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller.Automatons.Upgrades
{
    public class AutomatonUpgrade : MonoBehaviour
    {
        [SerializeField] private Purchasable purchasable;

        [SerializeField] private Transform automatonUiTransform;

        private AutomatonUpgrades _automatonUpgrades;

        private AutomatonUpgradeEntry _entry;

        public int ID { get; private set; }
        public int Level => purchasable.Amount;
        public int NextLevel => purchasable.NextAmount;
        public int MaxLevel => purchasable.MaxAmount;
        public bool MaxedOut => purchasable.MaxedOut;


        [Inject]
        public void Initialize(AutomatonUpgrades automatonUpgrades)
        {
            _automatonUpgrades = automatonUpgrades;
        }


        private void Awake()
        {
            AutomatonID automatonID = (AutomatonID) automatonUiTransform.GetSiblingIndex();
            _entry = _automatonUpgrades.GetSorted(transform.GetSiblingIndex(), automatonID);
            
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

            AutomatonID automatonID = (AutomatonID) automatonUiTransform.GetSiblingIndex();
            _entry = _automatonUpgrades.GetSorted(transform.GetSiblingIndex(), automatonID);

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
            Save();
        }


        private void Save()
        {
            if (!GameStateHandler.State.AutomatonUpgradeLevels.TryAdd(ID, Level))
            {
                GameStateHandler.State.AutomatonUpgradeLevels[ID] = Level;
            }
        }


        public void OnPrestige()
        {
            purchasable.Clear();
            
            Save();
        }
    }
}