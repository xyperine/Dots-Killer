using System;
using BreakInfinity;

namespace DotsKiller.Economy
{
    public static class BulkBuyCalculation
    {
        public delegate BigDouble PriceCalculation(BigDouble ownedAmount);
        
        
        public static BulkBuy GetBulkBuyData(BigDouble alreadyOwned, BigDouble money, PriceCalculation calculatePrice, BigDouble? maxAmount = null)
        {
            BigDouble firstPrice = calculatePrice(alreadyOwned);
            if (money < firstPrice)
            {
                return new BulkBuy(BigDouble.Zero, firstPrice);
            }
            
            // Find the price we cannot afford
            BigDouble cantBuy = BigDouble.One;
            BigDouble nextPrice;
            do
            {
                cantBuy *= 2f;
                nextPrice = calculatePrice(alreadyOwned + cantBuy - 1);
            } while (money >= nextPrice);

            if (cantBuy == 2)
            {
                return new BulkBuy(BigDouble.One, firstPrice);
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

            if (maxAmount.HasValue)
            {
                canBuy = BigDouble.Min(canBuy, maxAmount.Value);
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
            
            return new BulkBuy(canBuy, totalPrice);
        }
    }
}