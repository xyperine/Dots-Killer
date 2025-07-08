using System.Collections.Generic;
using System.Threading.Tasks;
using BreakInfinity;
using DotsKiller.Economy;
using Firebase.Auth;
using Firebase.Database;
using Google;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    // TODO: Move authorization logic into a separate class
    // TODO: Optimize async calls, right now it generates too much garbage
    public class FirebaseLeaderboardManager : MonoBehaviour, ILeaderboardManager
    {
        // 1. Get reference to database
        // 2. Subscribe to events if needed
        // 3. To set data - use push/setvalue
        // 4. To get data - verify that entry exists, then retrieve values and convert them to required types

        [SerializeField] private string username;

        private const string ENTRY_PATH = "Entry";
        private const string USERNAME_PATH = "username";
        private const string SCORE_PATH = "score";
        private const string SCORE_MANTISSA_PATH = "mantissa";
        private const string SCORE_EXPONENT_PATH = "exponent";

        private const string WEB_CLIENT_ID = "598292494403-vp5c1uog5l4a41916ovlcr9qrith6hie.apps.googleusercontent.com";

        private readonly RankComparer _rankComparer = new RankComparer();
        
        private Balance _balance;
        
        private DatabaseReference _db;

        private GoogleSignInConfiguration _googleConfiguration;

        private string _userId;
        
        private string ConcreteEntryNameID => _userId;// $"{ENTRY_PATH}_{_userId}";
        
        public string Username { get; private set; }
        public BigDouble Score { get; private set; }
        public string UserID => _userId;


        public async Task<IEnumerable<LeaderboardEntry>> GetEntriesAsync()
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

                entries[i] = new LeaderboardEntry(userSnapshot.Key, i + 1, u, s);
            }

            return entries;
        }


        [Inject]
        public void Initialize(Balance balance)
        {
            _balance = balance;
        }


        private void Awake()
        {
            Username = username;
            Score = _balance.Points;
            
            _googleConfiguration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = WEB_CLIENT_ID,
            };
        }


        private void Start()
        {
            InitializeAsync();
        }


        private async Task InitializeAsync()
        {
            await AuthenticateAsync();

            await InitializeDatabaseAsync();
        }
        
        
        private async Task AuthenticateAsync()
        {
#if UNITY_ANDROID
            GoogleSignIn.Configuration = _googleConfiguration;
            await GoogleSignIn.DefaultInstance.SignIn();
            await OnGoogleAuthFinished();
#elif UNITY_EDITOR
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            await auth.SignInAnonymouslyAsync().ContinueWith(task => 
            {
                if (task.IsCanceled) {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                _userId = result.User.UserId;
            });
#endif
        }


        private async Task OnGoogleAuthFinished(Task<GoogleSignInUser> task)
        {
            if (task.IsCanceled)
            {
                Debug.LogError($"Google authorization canceled: {task.Exception}");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Google authorization failed: {task.Exception}");
            }
            else if (task.IsCompletedSuccessfully)
            {
                Debug.Log("Google sign in successful!");
                
                await PerformFirebaseSignIn(task.Result);
            }
        }


        private async Task PerformFirebaseSignIn(GoogleSignInUser googleSignInUser)
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            string googleIdToken = googleSignInUser.IdToken;
            Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);

            await auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError($"Firebase authentication canceled: {task.Exception}");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"Firebase authentication failed: {task.Exception}");
                }
                else if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Firebase authentication successful!");

                    _userId = task.Result.User.UserId;
                }
            });
        }


        private void OnDestroy()
        {
            _db.ValueChanged -= OnValueChanged;
        }

        
        private async Task InitializeDatabaseAsync()
        {
            _db = FirebaseDatabase.DefaultInstance.GetReference("/Leaderboard");

            await CheckIfUserExistsAsync();
            
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
        

        [Button]
        public void SignInAsync()
        {
            CheckIfUserExistsAsync();
        }
        

        private async Task CheckIfUserExistsAsync()
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

                    Username = snapshot.Child(USERNAME_PATH).Value.ToString();
                    double mantissa = double.Parse(snapshot.Child(SCORE_PATH).Child(SCORE_MANTISSA_PATH).Value.ToString());
                    long exponent = (long) snapshot.Child(SCORE_PATH).Child(SCORE_EXPONENT_PATH).Value;
                    Score = new BigDouble(mantissa, exponent);
                    
                    Debug.Log($"Fetched user {_userId} data!: Score {Score}");
                }
                else
                {
                    Debug.LogError($"User {_userId} was not found!");
                }
            }
        }
    }
}