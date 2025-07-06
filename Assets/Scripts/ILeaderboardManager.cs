using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using Firebase.Database;
using UnityEngine;

namespace DotsKiller
{
    public interface ILeaderboardManager
    {
        string Username { get; }
        BigDouble Score { get; }
        
        Task<LeaderboardEntry[]> GetEntriesAsync();
    }


    public struct LeaderboardEntry
    {
        public long Rank { get; init; }
        public string Username { get; init; }
        public BigDouble Score { get; init; }


        public LeaderboardEntry(long rank, string username, double scoreMantissa, long scoreExponent) 
            : this(rank, username, new BigDouble(scoreMantissa, scoreExponent))
        { }
        
        
        public LeaderboardEntry(long rank, string username, BigDouble score)
        {
            Rank = rank;
            Username = username;
            Score = score;
        }
    }


    public class RankComparer : IComparer<DataSnapshot>
    {
        private const string SCORE_PATH = "score";
        private const string SCORE_MANTISSA_PATH = "mantissa";
        private const string SCORE_EXPONENT_PATH = "exponent";
        
        
        /// <summary>
        /// Used to sort entries in DESCENDING order based on the score value
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