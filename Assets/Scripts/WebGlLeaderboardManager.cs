using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using Dan.Main;
using Dan.Models;
using DotsKiller.Economy;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class WebGlLeaderboardManager : MonoBehaviour, ILeaderboardManager
    {
        [SerializeField] private string username;
        
        private readonly WebGlRankComparer _rankComparer = new WebGlRankComparer();
        
        private LeaderboardReference _leaderboardReference;

        private Balance _balance;


        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }
        

        private void Awake()
        {
            _leaderboardReference = Leaderboards.Dots_Killer_leaderboard;
            
        }


        public bool EntryBelongsToThisUser(LeaderboardEntry entry)
        {
            bool b = false;
            _leaderboardReference.GetPersonalEntry(callback: e =>
            {
                b = entry.Equals(ConvertToEntry(e));
            });
            
            return b;
        }


        public async Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null)
        {
            LeaderboardEntry[] result = default;
            _leaderboardReference.GetEntries
            (
                callback: entries =>
                {
                    Array.Sort(entries, _rankComparer);
                    result = new LeaderboardEntry[entries.Length];
                    for (int i = 0; i < entries.Length; i++)
                    {
                        Entry entry = entries[i];
                        long rank = i + 1;
                        string username = entry.Username;
                        BigDouble score = BigDouble.Parse(entry.Extra);
            
                        result[i] = new LeaderboardEntry(rank, username, score, entry.IsMine());
                    }

                    successCallback?.Invoke(result);
                },
                errorCallback: Debug.LogError
            );
            
            return result;
        }


        private LeaderboardEntry ConvertToEntry(Entry e)
        {
            string username = e.Username;
            BigDouble score = BigDouble.Parse(e.Extra);
            
            return new LeaderboardEntry(e.Rank, username, score, e.IsMine());
        }


        [Button]
        public void SubmitScore()
        {
            _leaderboardReference.UploadNewEntry(username, 0, $"{_balance.Points}");
        }
    }
}