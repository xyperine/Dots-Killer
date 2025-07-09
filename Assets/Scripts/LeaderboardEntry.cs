using BreakInfinity;

namespace DotsKiller
{
    public struct LeaderboardEntry
    {
        public long Rank { get; init; }
        public string Username { get; init; }
        public BigDouble Score { get; init; }
        public bool Mine { get; init; }


        public LeaderboardEntry(long rank, string username, double scoreMantissa, long scoreExponent, bool mine) 
            : this(rank, username, new BigDouble(scoreMantissa, scoreExponent), mine)
        { }
        
        
        public LeaderboardEntry(long rank, string username, BigDouble score, bool mine)
        {
            Rank = rank;
            Username = username;
            Score = score;
            Mine = mine;
        }
    }
}