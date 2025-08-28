using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using UnityEngine;

namespace DotsKiller
{
    public class FirebaseAuthenticationManager : MonoBehaviour
    {
        private const string WEB_CLIENT_ID = "598292494403-vp5c1uog5l4a41916ovlcr9qrith6hie.apps.googleusercontent.com";

        private GoogleSignInConfiguration _googleConfiguration;
        
        public string UserID { get; private set; }
        
        public bool Authorized { get; private set; }
        public bool Authorizing { get; private set; }


        private void Awake()
        {
            _googleConfiguration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = WEB_CLIENT_ID,
            };
        }


        public async Task AuthenticateAsync()
        {
            Authorizing = true;
            
#if UNITY_ANDROID
            GoogleSignIn.Configuration = _googleConfiguration;
            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
            await OnGoogleAuthFinished(signIn);
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

                UserID = result.User.UserId;
                Authorized = true;
                Authorizing = false;
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

                    UserID = task.Result.User.UserId;
                    Authorized = true;
                    Authorizing = false;
                }
            });
        }
    }
}