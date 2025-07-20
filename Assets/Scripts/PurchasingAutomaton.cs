using DotsKiller.RegularUpgrading;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class PurchasingAutomaton : MonoBehaviour
    {
        [SerializeField] private float purchasesPerSecond;
        
        private RegularUpgrades _regularUpgrades;
        private AutomatonUpgrades _automatonUpgrades;
        
        private float _purchaseInterval;

        private float _timeSinceLastPurchase;

        public float PurchasesPerSecond => purchasesPerSecond;


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, AutomatonUpgrades automatonUpgrades)
        {
            _regularUpgrades = regularUpgrades;
            _automatonUpgrades = automatonUpgrades;
        }


        private void Awake()
        {
            _purchaseInterval = 1f / purchasesPerSecond;
        }


        private void Update()
        {
            _purchaseInterval = 1f / (purchasesPerSecond * 1f);
            
            _timeSinceLastPurchase += Time.deltaTime;
            if (_timeSinceLastPurchase >= _purchaseInterval)
            {
                Purchase();

                _timeSinceLastPurchase = 0f;
            }
        }


        private void Purchase()
        {
            _regularUpgrades.PurchaseAll();
        }


        public void SetStatus(bool value)
        {
            enabled = value;
        }
    }
}