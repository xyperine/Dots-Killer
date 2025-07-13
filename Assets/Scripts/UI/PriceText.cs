using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.Utility;
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
            
            string priceText = Formatting.DefaultFormat(purchasable.IsBulkBuyActive
                ? purchasable.BulkPrice
                : purchasable.Price);
            string amountText = Formatting.DefaultFormat(purchasable.IsBulkBuyActive
                ? purchasable.BulkBuyData.Amount
                : BigDouble.One);

            text.text = string.Format(_format, priceText, amountText);
        }
    }
}