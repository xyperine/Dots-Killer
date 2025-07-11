using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class LeaderboardInitialization : MonoBehaviour
    {
        private ILeaderboardManager _leaderboardManager;


        [Inject]
        public void Initialize(ILeaderboardManager leaderboardManager)
        {
            _leaderboardManager = leaderboardManager;
        }


        private void Awake()
        {
            _leaderboardManager.StartUp();
        }
    }
}