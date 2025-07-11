using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;

namespace DotsKiller
{
    public interface ILeaderboardManager
    {
        void StartUp();
        Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null);
        void Submit(BigDouble score);
    }
}