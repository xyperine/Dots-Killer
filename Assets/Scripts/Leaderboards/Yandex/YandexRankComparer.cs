using System.Collections.Generic;
using BreakInfinity;
using YG.Utils.LB;

namespace DotsKiller.Leaderboards.Yandex
{
    public class YandexRankComparer : IComparer<LBPlayerData>
    {
        /// <summary>
        /// Used to sort entries in DESCENDING order based on the BigDouble score value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(LBPlayerData x, LBPlayerData y)
        {
            if (ReferenceEquals(null, x))
            {
                return 1;
            }
            
            if (ReferenceEquals(null, y))
            {
                return -1;
            }
            
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            
            BigDouble xScore = BigDouble.Parse(x.extraData);
            BigDouble yScore = BigDouble.Parse(y.extraData);
            return yScore.CompareTo(xScore);
        }
    }
}