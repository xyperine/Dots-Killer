using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float refreshCooldown;
        [SerializeField, Min(0)] private int entriesAmountToDisplay = 10;
        [SerializeField] private string entryFormat = "{0} {1} {2}";

        [SerializeField] private TMP_Text[] entriesText;
        [SerializeField] private TMP_Text thisUserEntryText;

        private ILeaderboardManager _leaderboardManager;

        private float _timeSinceLastUpdate;


        [Inject]
        public void Initialize(ILeaderboardManager leaderboardManager)
        {
            _leaderboardManager = leaderboardManager;
        }


        private void Awake()
        {
            _timeSinceLastUpdate = float.MaxValue;
        }


        private void Update()
        {
            _timeSinceLastUpdate += Time.deltaTime;
            if (_timeSinceLastUpdate >= refreshCooldown)
            {
                _leaderboardManager.GetEntriesAsync(OnGetEntries);
                
                _timeSinceLastUpdate = 0f;
            }
        }


        private void OnGetEntries(IEnumerable<LeaderboardEntry> entriesCollection)
        {
            List<LeaderboardEntry> entries = new List<LeaderboardEntry>(entriesCollection);
            
            int length = Mathf.Min(entries.Count, entriesAmountToDisplay);
            if (entriesText.Length < length)
            {
                throw new Exception("Not enough text objects!");
            }

            for (int i = 0; i < entriesText.Length; i++)
            {
                entriesText[i].enabled = i < length;
                if (i < length)
                {
                    entriesText[i].text =
                        string.Format(entryFormat, entries[i].Rank, entries[i].Username, entries[i].Score);
                }
            }

            LeaderboardEntry entry = entries.Find(e => e.Mine);
            thisUserEntryText.text = string.Format(entryFormat, entry.Rank, entry.Username, entry.Score);
        }
    }
}