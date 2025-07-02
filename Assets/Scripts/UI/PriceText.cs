using DotsKiller.Economy;
using TMPro;
using UnityEngine;

namespace DotsKiller.UI
{
    public class PriceText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Purchasable purchasable;

        private string _format;


        private void Awake()
        {
            _format = text.text;
        }


        private void Update()
        {
            if (purchasable.MaxedOut)
            {
                text.text = "MAX";
                return;
            }
            
            string priceText = purchasable.IsBulkBuyActive
                ? purchasable.BulkPrice.ToString()
                : purchasable.Price.ToString();
            string amountText = purchasable.IsBulkBuyActive
                ? purchasable.BulkBuyData.Amount.ToString()
                : 1.ToString();

            text.text = string.Format(_format, priceText, amountText);
        }
    }
}