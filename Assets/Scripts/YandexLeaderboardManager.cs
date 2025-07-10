using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace DotsKiller
{
    public class YandexLeaderboardManager : MonoBehaviour, ILeaderboardManager
    {
        [SerializeField] private string lbName;

        private readonly YandexRankComparer _rankComparer = new YandexRankComparer();

        private IEnumerable<LeaderboardEntry> _entries;
        private Action<IEnumerable<LeaderboardEntry>> _queuedCallback;
        
        
        private void OnEnable()
        {
            YG2.onGetLeaderboard += OnGetLeaderboardData;
            YG2.GetLeaderboard(lbName);
        }


        private void OnGetLeaderboardData(LBData lbData)
        {
            LBPlayerData[] players = lbData.players;
            Array.Sort(players, _rankComparer);
            LeaderboardEntry[] entries = new LeaderboardEntry[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                LBPlayerData playerData = players[i];
                long rank = i + 1;
                string username = playerData.name;
                BigDouble score = BigDouble.Parse(playerData.extraData);

                entries[i] = new LeaderboardEntry(rank, username, score, playerData.uniqueID == YG2.player.id);
            }

            _entries = entries;
            
            _queuedCallback?.Invoke(entries);
        }


        public async Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null)
        {
            _queuedCallback = successCallback;
            YG2.GetLeaderboard(lbName);

            return _entries;
        }

        
        public void Submit(BigDouble score)
        {
            YG2.SetLeaderboard(lbName, 0, score.ToString());
        }
        

        private void OnDisable()
        {
            _queuedCallback = null;
            YG2.onGetLeaderboard -= OnGetLeaderboardData;
        }
    }
}