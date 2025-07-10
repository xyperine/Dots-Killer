using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;

namespace DotsKiller
{
    public interface ILeaderboardManager
    {
        Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null);
        void Submit(BigDouble score);
    }
}