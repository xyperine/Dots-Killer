#if !PLATFORM_WEBGL
using System.Collections.Generic;
using BreakInfinity;
using Firebase.Database;

namespace DotsKiller.Leaderboards.Firebase
{
    public class FirebaseRankComparer : IComparer<DataSnapshot>
    {
        private const string SCORE_PATH = "score";
        private const string SCORE_MANTISSA_PATH = "mantissa";
        private const string SCORE_EXPONENT_PATH = "exponent";
        
        
        /// <summary>
        /// Used to sort entries in DESCENDING order based on the BigDouble score value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(DataSnapshot x, DataSnapshot y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(null, y))
            {
                return -1;
            }

            if (ReferenceEquals(null, x))
            {
                return 1;
            }
            
            double xMantissa = double.Parse(x.Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).Value.ToString());
            long xExponent = (long) x.Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).Value;
            BigDouble xScore = new BigDouble(xMantissa, xExponent);
            
            double yMantissa = double.Parse(y.Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).Value.ToString());
            long yExponent = (long) y.Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).Value;
            BigDouble yScore = new BigDouble(yMantissa, yExponent);

            return yScore.CompareTo(xScore);
        }
    }
}
#endif