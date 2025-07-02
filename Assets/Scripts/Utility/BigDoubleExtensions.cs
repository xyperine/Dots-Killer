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
    }
}