using System;
using System.Collections.Generic;
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
        [SerializeField, ShowIf(nameof(canBeBulkBought))] private BulkBuyCategory bulkBuyCategory;

        private Balance _balance;
        private IBulkBuyStateProvider _bulkBuyStateProvider;

        private bool _complexScaling;

        private BigDouble[] _priceScalingMultipliers;
        private BigDouble[] _priceIncreaseThresholds;
        
        public BigDouble Price { get; private set; }
        
        public int Amount { get; private set; }
        public int NextAmount
        {
            get
            {
                // BigDouble cast to double and then to int can be a problem
                int result = IsBulkBuyActive ? Amount + Mathf.Max((int) BulkBuyData.Amount.ToDouble(), 1) : Amount + 1;
                if (hasMaxAmount)
                {
                    result = Mathf.Min(result, MaxAmount);
                }

                return result;
            }
        }

        public int MaxAmount => maxAmount;
        public Currency Currency => currency;
        public bool IsAffordable => _balance.IsAffordable(Price, currency);
        public bool MaxedOut => hasMaxAmount && Amount >= maxAmount;

        public bool IsBulkBuyActive => canBeBulkBought && _bulkBuyStateProvider.Active;
        public BigDouble BulkPrice => BulkBuyData.Price;
        public BulkBuyData BulkBuyData => GetBulkBuyData();

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
            UpdatePrice();
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


        public void BulkPurchase(BulkBuyProvider provider)
        {
            if (!provider.Active)
            {
                return;
            }

            if (!GetRequestedAmount().Max && !GetRequestedAmount().Value.HasValue)
            {
                return;
            }
            
            BulkBuyData bb = GetBulkBuyData();

            if (!_balance.IsAffordable(bb.Price, currency))
            {
                return;
            }
            
            PerformPurchase(bb.Price, GetActualAmount(bb.Amount));

            BulkBuyAmount GetRequestedAmount()
            {
                return provider.Modes[bulkBuyCategory];
            }

            BigDouble GetActualAmount(BigDouble affordableAmount)
            {
                return GetRequestedAmount().Max ? affordableAmount : BigDouble.Min(affordableAmount, GetRequestedAmount().Value.GetValueOrDefault(BigDouble.One));
            }

            void PerformPurchase(BigDouble price, BigDouble amount)
            {
                _balance.Subtract(price, currency);

                Amount += (int) amount.ToDouble();
                UpdatePrice();
                
                BulkPurchased?.Invoke(amount);
            }
        }
        

        public void BulkPurchase()
        {
            if (MaxedOut)
            {
                return;
            }

            BulkBuyProvider p = new BulkBuyProvider
            {
                Active = true,
                Modes = new Dictionary<BulkBuyCategory, BulkBuyAmount>
                {
                    {BulkBuyCategory.RegularUpgrades, new BulkBuyAmount {Value = null, Max = true}},
                    {BulkBuyCategory.AutomatonUpgrades, new BulkBuyAmount {Value = null, Max = true}},
                },
            };
            
            BulkPurchase(p);
        }


        public void BulkPurchase(int amountLimit)
        {
            if (MaxedOut)
            {
                return;
            }

            BulkBuyProvider p = new BulkBuyProvider
            {
                Active = true,
                Modes = new Dictionary<BulkBuyCategory, BulkBuyAmount>
                {
                    {BulkBuyCategory.RegularUpgrades, new BulkBuyAmount {Value = amountLimit, Max = false}},
                    {BulkBuyCategory.AutomatonUpgrades, new BulkBuyAmount {Value = amountLimit, Max = false}},
                },
            };
            
            BulkPurchase(p);
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


        private BulkBuyData GetBulkBuyData()
        {
            return BulkBuyCalculation.GetBulkBuyData(Amount, _balance.Available(currency), CalculatePrice,
                hasMaxAmount ? maxAmount : null);
        }
    }
}