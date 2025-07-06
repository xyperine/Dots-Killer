using AYellowpaper;
using TMPro;
using UnityEngine;

namespace DotsKiller
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ILeaderboardManager> leaderboardManagerReference;

        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text scoreText;

        [SerializeField] private TMP_Text[] entriesText;

        private string _entryFormat;
        
        private ILeaderboardManager _leaderboardManager;


        private void Awake()
        {
            _leaderboardManager = leaderboardManagerReference.Value;

            _entryFormat = "{0} {1} {2}";
        }


        private void Update()
        {
            usernameText.text = _leaderboardManager.Username;
            scoreText.text = _leaderboardManager.Score.ToString();

            UpdateEntriesAsync();
        }


        private async void UpdateEntriesAsync()
        {
            LeaderboardEntry[] entries = await _leaderboardManager.GetEntriesAsync();

            if (entries == null)
            {
                return;
            }

            int length = Mathf.Min(entries.Length, entriesText.Length);

            for (int i = 0; i < length; i++)
            {
                entriesText[i].text =
                    string.Format(_entryFormat, entries[i].Rank, entries[i].Username, entries[i].Score);
            }
        }
    }
}