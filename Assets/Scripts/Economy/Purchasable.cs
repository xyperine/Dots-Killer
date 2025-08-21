using System;
using BreakInfinity;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DotsKiller.Economy
{
    public class Purchasable : MonoBehaviour
    {
        [SerializeField] private BigDouble basePrice;
        [SerializeField] private BigDouble priceScaling = BigDouble.One;
        [SerializeField] private Currency currency;
        [SerializeField] private bool hasMaxAmount;
        [SerializeField, ShowIf(nameof(hasMaxAmount))] private int maxAmount;
        [SerializeField] private bool canBeBulkBought;
        
        private Balance _balance;
        private IBulkBuyStateProvider _bulkBuyStateProvider;

        private bool _complexScaling;

        private BigDouble[] _priceScalingMultipliers;
        private BigDouble[] _priceIncreaseThresholds;
        
        public BigDouble Price { get; private set; }
        
        public int Amount { get; private set; }
        public int MaxAmount => maxAmount;
        public Currency Currency => currency;
        public bool IsAffordable => _balance.IsAffordable(Price, currency);
        public bool MaxedOut => hasMaxAmount && Amount >= maxAmount;

        public bool IsBulkBuyActive => canBeBulkBought && _bulkBuyStateProvider.Active;
        public BigDouble BulkPrice => BulkBuyData.Price;
        public BulkBuy BulkBuyData => GetBulkBuyData();

        public event Action Purchasing;
        public event Action Purchased;

        public event Action<BigDouble> BulkPurchased;
        
        
        [Inject]
        public void Initialize(Balance balance, IBulkBuyStateProvider bulkBuyStateProvider)
        {
            _balance = balance;
            _bulkBuyStateProvider = bulkBuyStateProvider;
        }
        

        private void Awake()
        {
            Price = basePrice;
        }


        public void SetPrice(BigDouble price)
        {
            basePrice = price;
        }


        public void SetPrice(BigDouble price, BigDouble priceScaling)
        {
            basePrice = price;
            this.priceScaling = priceScaling;
        }
        
        
        public void SetPrice(BigDouble price, BigDouble priceScaling, Currency currency)
        {
            basePrice = price;
            this.priceScaling = priceScaling;
            this.currency = currency;

            Price = basePrice;
        }


        public void SetMaxAmount(int amount)
        {
            if (amount == 0)
            {
                hasMaxAmount = false;
                maxAmount = 0;
                return;
            }

            hasMaxAmount = true;
            maxAmount = amount;
        }


        /// <summary>
        /// Set scaling based on the price. Multipliers and thresholds arrays must be the same length.
        /// </summary>
        /// <param name="priceScalingMultipliers">The first element is always 1!</param>
        /// <param name="priceIncreaseThresholds"></param>
        public void SetComplexScaling(BigDouble[] priceScalingMultipliers, BigDouble[] priceIncreaseThresholds)
        {
            if (priceScalingMultipliers.Length != priceIncreaseThresholds.Length)
            {
                throw new ArgumentException();
            }

            if (priceScalingMultipliers[0] != BigDouble.One)
            {
                throw new ArgumentOutOfRangeException(nameof(priceScalingMultipliers));
            }

            _complexScaling = true;
            _priceScalingMultipliers = priceScalingMultipliers;
            _priceIncreaseThresholds = priceIncreaseThresholds;
        }


        public void Purchase()
        {
            if (IsBulkBuyActive)
            {
                BulkPurchase();
                return;
            }
            
            PurchaseSingle();
        }


        public void PurchaseSingle()
        {
            if (!_balance.IsAffordable(Price, currency))
            {
                return;
            }

            if (MaxedOut)
            {
                return;
            }

            Purchasing?.Invoke();

            _balance.Subtract(Price, currency);

            Amount++;
            UpdatePrice();

            Purchased?.Invoke();
        }


        public void BulkPurchase()
        {
            if (MaxedOut)
            {
                return;
            }

            BulkBuy bb = GetBulkBuyData();

            if (!_balance.IsAffordable(bb.Price, currency))
            {
                return;
            }

            _balance.Subtract(bb.Price, currency);

            Amount += (int) bb.Amount.ToDouble();
            UpdatePrice();
                
            BulkPurchased?.Invoke(bb.Amount);
        }


        public void BulkPurchase(int amountLimit)
        {
            if (MaxedOut)
            {
                return;
            }

            BulkBuy bb = GetBulkBuyData();

            if (!_balance.IsAffordable(bb.Price, currency))
            {
                return;
            }

            BigDouble amount = BigDouble.Min(bb.Amount, amountLimit);

            _balance.Subtract(bb.Price, currency);

            Amount += (int) amount.ToDouble();
            UpdatePrice();
                
            BulkPurchased?.Invoke(bb.Amount);
        }


        public void UpdatePrice()
        {
            Price = CalculatePrice(Amount);
        }


        private BigDouble CalculatePrice(BigDouble currentlyOwned)
        {
            if (!_complexScaling)
            {
                return basePrice * BigDouble.Pow(priceScaling, currentlyOwned);
            }

            for (int i = 0; i < _priceIncreaseThresholds.Length; i++)
            {
                BigDouble price = BigDouble.Pow(priceScaling * _priceScalingMultipliers[i], currentlyOwned).Multiply(basePrice);
                if (price < _priceIncreaseThresholds[i])
                {
                    return price;
                }
            }

            return BigDouble
                .Pow(priceScaling * _priceScalingMultipliers[^1], currentlyOwned)
                .Multiply(basePrice);
        }


        public void Sell()
        {
            Amount--;
            Price /= priceScaling;
            
            _balance.Add(Price * 0.5f, currency);
        }


        public void Clear()
        {
            Amount = 0;
            Price = basePrice;
        }


        public void Load(int amount)
        {
            Amount = amount;
            
            UpdatePrice();
        }


        private BulkBuy GetBulkBuyData()
        {
            return BulkBuyCalculation.GetBulkBuyData(Amount, _balance.Available(currency), CalculatePrice,
                hasMaxAmount ? maxAmount : null);
        }
    }
}