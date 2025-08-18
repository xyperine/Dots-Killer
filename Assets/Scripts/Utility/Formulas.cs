using System;
using BreakInfinity;
using UnityEngine;

namespace DotsKiller.Utility
{
    public static class Formulas
    {
        public static BigDouble Limit { get; } = new BigDouble(1f, 1_000_000_000_000_000);


        public static BigDouble CalculateRecalibrationExponent(BigDouble totalPoints, BigDouble recalibrationPointsThreshold)
        {
            BigDouble adjustedPoints = BigDouble.Max(BigDouble.One, (totalPoints*1000).Divide(recalibrationPointsThreshold));
            BigDouble x = BigDouble.Max(BigDouble.One, BigDouble.Log(adjustedPoints, 1000));
            BigDouble x1 = BigDouble.Log10(x) + BigDouble.One;
            BigDouble x2 = BigDouble.Pow(x1, 1f);

            adjustedPoints = BigDouble.Max(BigDouble.One, totalPoints.Divide(recalibrationPointsThreshold));
            x = BigDouble.Max(BigDouble.One, BigDouble.Log2(adjustedPoints));
            x1 = BigDouble.Log10(x) + BigDouble.One;
            return x2;
        }


        public static BigDouble CalculateRecalibrationMultiplier(BigDouble totalPoints, BigDouble recalibrationPointsThreshold)
        {
            BigDouble adjustedPoints = BigDouble.Max(BigDouble.One, totalPoints.Divide(recalibrationPointsThreshold));
            BigDouble l = BigDouble.Log10(adjustedPoints);
            double e = 4d;
            double a = 8d;
            BigDouble y = BigDouble.Pow(l, e) * a + BigDouble.One;
            return y;
        }


        public static BigDouble CalculateCleanFactor(int amountAlive, int level)
        {
            const int fullFieldThreshold = 5000;
            const int cleanFieldThreshold = 1;
            return Mathf.InverseLerp(Mathf.Log10(fullFieldThreshold), Mathf.Log10(cleanFieldThreshold + 1f),
                Mathf.Log10(amountAlive + 1f)) * level*level + BigDouble.One;
        }


        public static BigDouble CalculateTimeFactor(double seconds, int level)
        {
            return Math.Pow(seconds / 80_000d, 0.6d) * 6d * level + BigDouble.One;
        }


        public static BigDouble CalculateShardsOnPurge()
        {
            return new BigDouble(1, 2931);
        }
    }
}