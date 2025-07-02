using DotsKiller.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.UI
{
    public class PurchasableUI : MonoBehaviour
    {
        [SerializeField] private Purchasable purchasable;
        [SerializeField] private Button purchaseButton;


        private void Update()
        {
            purchaseButton.interactable = purchasable.IsAffordable && !purchasable.MaxedOut;
        }
    }
}