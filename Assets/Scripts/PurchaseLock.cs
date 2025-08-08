using AYellowpaper;
using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using UnityEngine;

namespace DotsKiller
{
    public class PurchaseLock : MonoBehaviour
    {
        [SerializeField] private Purchasable lockPurchasable;
        [SerializeField] private GameObject purchasableLockOverlay;
        [SerializeField] private PurchaseLockID id;
        [SerializeField] private InterfaceReference<IPurchaseLockTarget> purchaseLockTargetReference;

        private IPurchaseLockTarget _purchaseLockTarget;


        private void Awake()
        {
            _purchaseLockTarget = purchaseLockTargetReference.Value;
        }


        private void OnEnable()
        {
            lockPurchasable.Purchased += OnPurchased;
        }


        private void Start()
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
            purchasableLockOverlay.SetActive(false);
            _purchaseLockTarget.Activate();
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