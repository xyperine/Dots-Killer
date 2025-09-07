using System.Collections.Generic;
using BreakInfinity;

namespace DotsKiller.Economy.BulkBuy
{
    public class BulkBuyProcessor
    {
        private readonly BulkBuyCategory _category;
        
        public delegate BigDouble PriceCalculation(BigDouble ownedAmount);
        
        
        public BulkBuyProcessor(BulkBuyCategory category)
        {
            _category = category;
        }


        private BulkBuyMode GetRequestedMode(BulkBuyUser user)
        {
            return user.Modes[_category];
        }


        private BigDouble GetActualAmount(BulkBuyUser user, BigDouble affordableAmount)
        {
            return GetRequestedMode(user).Max
                ? affordableAmount
                : BigDouble.Min(affordableAmount, GetRequestedMode(user).Amount.GetValueOrDefault(BigDouble.One));
        }


        public BulkBuyData GetBulkBuyData(BulkBuyUser user, BigDouble alreadyOwned, BigDouble money, PriceCalculation calculatePrice, BigDouble? maxAmount = null)
        {
            BigDouble firstPrice = calculatePrice(alreadyOwned);
            if (money < firstPrice)
            {
                return new BulkBuyData(BigDouble.Zero, firstPrice);
            }
            
            // Find the price we cannot afford
            BigDouble cantBuy = BigDouble.One;
            BigDouble nextPrice;
            do
            {
                cantBuy *= 2f;
                nextPrice = calculatePrice(alreadyOwned + cantBuy - 1);
            } while (money >= nextPrice);
            
            // Makes more sense to do it with canBuy variable, but I do this instead for performance(is it worth?)
            if (maxAmount.HasValue)
            {
                cantBuy = BigDouble.Min(cantBuy, maxAmount.Value - alreadyOwned + BigDouble.One);
            }
            
            // If we can buy only 1
            if (cantBuy == 2)
            {
                return new BulkBuyData(BigDouble.One, firstPrice);
            }

            // Find the actual amount we can buy
            BigDouble canBuy = cantBuy / 2;
            while (cantBuy - canBuy > 1)
            {
                BigDouble middle = BigDouble.Floor((canBuy + cantBuy) / 2);
                if (money >= calculatePrice(alreadyOwned + middle - 1))
                {
                    canBuy = middle;
                }
                else
                {
                    cantBuy = middle;
                }
            }

            // Calculate the total price
            BigDouble highestAffordablePrice = calculatePrice(alreadyOwned + canBuy - 1);
            BigDouble smallerPrices = BigDouble.One;
            int loopCount = 0;
            for (BigDouble i = canBuy - 1; i >= 0; i--)
            {
                BigDouble newPrice = smallerPrices + calculatePrice(alreadyOwned + i - 1);
                if (newPrice == smallerPrices)
                {
                    break;
                }

                smallerPrices = newPrice;

                if (++loopCount > 1000)
                {
                    break;
                }
            }

            BigDouble totalPrice = highestAffordablePrice + smallerPrices;

            if (money < totalPrice)
            {
                --canBuy;
                totalPrice = smallerPrices;
            }

            canBuy = GetActualAmount(user, canBuy);
            return new BulkBuyData(canBuy, totalPrice);
        }


        public bool VerifyPurchase(BulkBuyUser user)
        {
            return user.IsActive(_category) &&
                   (GetRequestedMode(user).Max || GetRequestedMode(user).Amount.HasValue);
        }
        
        
        public BulkBuyUser ConstructAutomatonUser(int amountLimit)
        {
            return new BulkBuyUser(new Dictionary<BulkBuyCategory, BulkBuyMode>
            {
                {BulkBuyCategory.RegularUpgrades, BulkBuyMode.CreateAsNumber(amountLimit)},
                {BulkBuyCategory.AutomatonUpgrades, BulkBuyMode.CreateAsNumber(amountLimit)},
            });
        }
    }
}