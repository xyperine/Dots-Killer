using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotsKiller
{
    public interface ILeaderboardManager
    {
        Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null);
    }
}