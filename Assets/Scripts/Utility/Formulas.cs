using System;
using BreakInfinity;
using UnityEngine;

namespace DotsKiller.Utility
{
    public static class Formulas
    {
        public static BigDouble Limit { get; } = new BigDouble(1f, 1_000_000_000_000_000);


        public static BigDouble CalculateCleanFactor(int amountAlive, int level)
        {
            const int fullFieldThreshold = 5000;
            const int cleanFieldThreshold = 1;
            return Mathf.InverseLerp(Mathf.Log10(fullFieldThreshold), Mathf.Log10(cleanFieldThreshold + 1f),
                Mathf.Log10(amountAlive + 1f)) * level + BigDouble.One;
        }
        
        
        public static BigDouble CalculateTimeFactor(double seconds)
        {
            return Math.Pow(seconds / 100_000d, 0.6d) * 5f + BigDouble.One;
        }


        public static BigDouble CalculateShardsOnPurge()
        {
            return BigDouble.One;
        }
    }
}