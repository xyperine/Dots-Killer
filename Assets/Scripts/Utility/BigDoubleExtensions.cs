using System;
using BreakInfinity;

namespace DotsKiller.Utility
{
    public static class BigDoubleExtensions
    {
        public static float InverseLerp(this BigDouble value, BigDouble start, BigDouble end)
        {
            return (float) ((value - start) / (end - start)).ToDouble();
        }


        public static float InverseLerpLog10(this BigDouble value, BigDouble start, BigDouble end)
        {
            return InverseLerp(value.Log10(), start.Log10(), end.Log10());
        }


        public static double PositiveSafeLog10(this BigDouble value)
        {
            double actualValue = BigDouble.Log10(value + double.Epsilon);
            return Math.Max(actualValue, 0);
        }
        

        public static BigDouble Clamp(this BigDouble value, BigDouble a, BigDouble b)
        {
            BigDouble result;
            
            if (value < a)
            {
                result = a;
            }
            else if (value > b)
            {
                result = b;
            }
            else
            {
                result = value;
            }

            return result;
        }
    }
}