using System.Collections.Generic;
using BreakInfinity;
using Dan.Models;

namespace DotsKiller.Leaderboards.WebGL
{
    public class WebGlRankComparer : IComparer<Entry>
    {
        /// <summary>
        /// Used to sort entries in DESCENDING order based on the BigDouble score value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Entry x, Entry y)
        {
            BigDouble xScore = BigDouble.Parse(x.Extra);
            BigDouble yScore = BigDouble.Parse(y.Extra);
            return yScore.CompareTo(xScore);
        }
    }
}