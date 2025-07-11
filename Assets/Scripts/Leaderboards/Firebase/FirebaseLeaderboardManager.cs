using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using Firebase.Database;
using UnityEngine;

namespace DotsKiller.Leaderboards.Firebase
{
    public class FirebaseLeaderboardManager : MonoBehaviour, ILeaderboardManager
    {
        // 1. Get reference to database
        // 2. Subscribe to events if needed
        // 3. To set data - use push/setvalue
        // 4. To get data - verify that entry exists, then retrieve values and convert them to required types

        [SerializeField] private FirebaseAuthenticationManager authenticationManager;
        [SerializeField] private string username;

        private const string ENTRY_PATH = "Entry";
        private const string USERNAME_PATH = "username";
        private const string SCORE_PATH = "score";
        private const string SCORE_MANTISSA_PATH = "mantissa";
        private const string SCORE_EXPONENT_PATH = "exponent";
        
        private readonly FirebaseRankComparer _rankComparer = new FirebaseRankComparer();

        private DatabaseReference _db;
        
        private string _userId;
        
        private string ConcreteEntryNameID => _userId;// $"{ENTRY_PATH}_{_userId}";
        

        public void StartUp()
        {
            gameObject.SetActive(true);
            enabled = true;

            InitializeAsync();
        }
        

        private async Task InitializeAsync()
        {
            await authenticationManager.AuthenticateAsync();

            await InitializeDatabaseAsync();
        }


        private async Task InitializeDatabaseAsync()
        {
            _userId = authenticationManager.UserID;
            
            _db = FirebaseDatabase.DefaultInstance.GetReference("/Leaderboard");
        }


        private async Task PushDataAsync(BigDouble score)
        {
            Task t1 = _db.Child(ConcreteEntryNameID).Child(USERNAME_PATH).SetValueAsync(username);
            Task t2 = _db.Child(ConcreteEntryNameID).Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).SetValueAsync(score);
            Task t3 = _db.Child(ConcreteEntryNameID).Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).SetValueAsync(score);

            Task[] ts = {t1, t2, t3};
            await Task.WhenAll(ts);
            
            if (t1.IsCanceled)
            {
                Debug.LogError($"Pushing data cancelled: {t1.Exception}");
            }
            else if (t1.IsFaulted)
            {
                Debug.LogError($"Pushing data failed: {t1.Exception}");
            }
            else if (t1.IsCompletedSuccessfully)
            {
                Debug.Log($"User {username} data uploaded successfully!");
            }
        }


        public void Submit(BigDouble score)
        {
            PushDataAsync(score);
        }


        public async Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync(Action<IEnumerable<LeaderboardEntry>> successCallback = null)
        {
            if (_db == null)
            {
                return null;
            }

            DataSnapshot snapshot = await _db.GetValueAsync();
            LeaderboardEntry[] entries = new LeaderboardEntry[snapshot.ChildrenCount];
            List<DataSnapshot> children = new List<DataSnapshot>(snapshot.Children);
            children.Sort(_rankComparer);
            for (int i = 0; i < snapshot.ChildrenCount; i++)
            {
                DataSnapshot userSnapshot = children[i];
                string u = userSnapshot.Child(USERNAME_PATH).Value.ToString();
                double mantissa = double.Parse(userSnapshot.Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).Value.ToString());
                long exponent = (long) userSnapshot.Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).Value;
                BigDouble s = new BigDouble(mantissa, exponent);

                entries[i] = new LeaderboardEntry(i + 1, u, s, userSnapshot.Key == _userId);
            }
            
            successCallback?.Invoke(entries);

            return entries;
        }
    }
}