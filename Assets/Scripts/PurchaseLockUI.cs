using UnityEngine;

namespace DotsKiller
{
    public class PurchaseLockUI : MonoBehaviour
    {
        [SerializeField] private PurchaseLock purchaseLock;
        [SerializeField] private GameObject purchasableLockOverlay;


        private void Update()
        {
            purchasableLockOverlay.SetActive(!purchaseLock.Unlocked);
        }
    }
}