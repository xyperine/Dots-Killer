using System;
using System.Collections.Generic;
using AYellowpaper;
using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float refreshCooldown;
        [SerializeField, Min(0)] private int entriesAmountToDisplay = 10;
        
        [SerializeField] private InterfaceReference<ILeaderboardManager> leaderboardManagerReference;

        [SerializeField] private TMP_Text[] entriesText;
        [SerializeField] private TMP_Text thisUserEntryText;

        private string _entryFormat;
        
        private ILeaderboardManager _leaderboardManager;

        private float _timeSinceLastUpdate;


        private void Awake()
        {
            _leaderboardManager = leaderboardManagerReference.Value;

            _entryFormat = "{0} {1} {2}";

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
                        string.Format(_entryFormat, entries[i].Rank, entries[i].Username, entries[i].Score);
                }
            }

            LeaderboardEntry entry = entries.Find(e => e.Mine);
            thisUserEntryText.text = string.Format(_entryFormat, entry.Rank, entry.Username, entry.Score);
        }
    }
}