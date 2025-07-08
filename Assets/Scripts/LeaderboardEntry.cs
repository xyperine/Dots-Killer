using BreakInfinity;

namespace DotsKiller
{
    public struct LeaderboardEntry
    {
        public string UserID { get; init; }
        public long Rank { get; init; }
        public string Username { get; init; }
        public BigDouble Score { get; init; }


        public LeaderboardEntry(string userId, long rank, string username, double scoreMantissa, long scoreExponent) 
            : this(userId, rank, username, new BigDouble(scoreMantissa, scoreExponent))
        { }
        
        
        public LeaderboardEntry(string userId, long rank, string username, BigDouble score)
        {
            UserID = userId;
            Rank = rank;
            Username = username;
            Score = score;
        }
    }
}