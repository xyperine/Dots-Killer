using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;

namespace DotsKiller
{
    public interface ILeaderboardManager
    {
        string Username { get; }
        BigDouble Score { get; }
        
        public string UserID { get; }
        
        Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync();
    }
}