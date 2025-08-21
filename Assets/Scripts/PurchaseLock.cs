using AYellowpaper;
using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class PurchaseLock : MonoBehaviour
    {
        [SerializeField] private Purchasable lockPurchasable;
        [SerializeField] private PurchaseLockID id;
        [SerializeField] private InterfaceReference<IPurchaseLockTarget> purchaseLockTargetReference;

        private IPurchaseLockTarget _purchaseLockTarget;
        private PurchaseLocks _purchaseLocks;
        
        public bool Unlocked { get; private set; }


        [Inject]
        public void Initialize(PurchaseLocks purchaseLocks)
        {
            _purchaseLocks = purchaseLocks;
        }
        

        private void Awake()
        {
            Setup();
        }


        public void Setup()
        {
            _purchaseLockTarget = purchaseLockTargetReference.Value;
            _purchaseLocks.Register(this);
        }


        private void OnEnable()
        {
            lockPurchasable.Purchased += OnPurchased;
        }


        private void Start()
        {
            Load();
        }


        public void Load()
        {
            if (GameStateHandler.Loaded)
            {
                if (GameStateHandler.State.UnlockedPurchasables.Contains(id))
                {
                    Unlock();
                }
            }
        }


        private void Unlock()
        {
            _purchaseLockTarget.Activate();
            Unlocked = true;
        }


        private void OnPurchased()
        {
            Unlock();
            
            GameStateHandler.State.UnlockedPurchasables.Add(id);
        }


        private void OnDisable()
        {
            lockPurchasable.Purchased -= OnPurchased;
        }
    }
}