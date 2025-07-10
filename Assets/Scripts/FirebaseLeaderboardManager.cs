using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using DotsKiller.Economy;
using Firebase.Database;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DotsKiller
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
        
        private Balance _balance;
        
        private DatabaseReference _db;
        
        private string _userId;
        
        private string ConcreteEntryNameID => _userId;// $"{ENTRY_PATH}_{_userId}";


        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }


        private void Start()
        {
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

            await TryFetchUserDataAsync();
            
            _db.ValueChanged += OnValueChanged;
        }


        private void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            FetchUserDataAsync();
        }


        private async Task TryFetchUserDataAsync()
        {
            Task<DataSnapshot> task = _db.Child(ConcreteEntryNameID).GetValueAsync();

            await task;

            if (task.IsCanceled)
            {
                Debug.LogError($"Checking user canceled: {task.Exception}");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Checking user failed: {task.Exception}");
            }
            else if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot is {HasChildren: true})
                {
                    Debug.Log($"User {_userId} exists!");

                    await FetchUserDataAsync();
                }
                else
                {
                    Debug.LogError($"User {_userId} was not found!");

                    await PushDataAsync();

                    await FetchUserDataAsync();
                }
            }
        }


        [Button]
        private async Task PushDataAsync()
        {
            Task t1 = _db.Child(ConcreteEntryNameID).Child(USERNAME_PATH).SetValueAsync(username);
            Task t2 = _db.Child(ConcreteEntryNameID).Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).SetValueAsync(_balance.Points.Mantissa);
            Task t3 = _db.Child(ConcreteEntryNameID).Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).SetValueAsync(_balance.Points.Exponent);

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
            PushDataAsync();
        }


        private async Task FetchUserDataAsync()
        {
            Task<DataSnapshot> task = _db.Child(ConcreteEntryNameID).GetValueAsync();

            await task;

            if (task.IsCanceled)
            {
                Debug.LogError($"Fetching user data canceled: {task.Exception}");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Fetching user data failed: {task.Exception}");
            }
            else if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot is {HasChildren: true})
                {
                    Debug.Log($"User {_userId} exists!");

                    username = snapshot.Child(USERNAME_PATH).Value.ToString();
                    double mantissa = double.Parse(snapshot.Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).Value.ToString());
                    long exponent = (long) snapshot.Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).Value;
                    BigDouble score  = new BigDouble(mantissa, exponent);
                    
                    Debug.Log($"Fetched user {_userId} data!: Score {score}");
                }
                else
                {
                    Debug.LogError($"User {_userId} was not found!");
                }
            }
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


        private void OnDestroy()
        {
            _db.ValueChanged -= OnValueChanged;
        }
    }
}