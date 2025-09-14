#if !PLATFORM_WEBGL
using DotsKiller.Leaderboards.Firebase;
#endif
using DotsKiller.Leaderboards.WebGL;
using UnityEngine;
using Zenject;

namespace DotsKiller.Leaderboards
{
    public class LeaderboardInstaller : MonoInstaller
    {
        #if !PLATFORM_WEBGL
        [SerializeField] private FirebaseLeaderboardManager firebaseLeaderboardManager;
        #endif
        [SerializeField] private WebGlLeaderboardManager webGlLeaderboardManager;


        public override void InstallBindings()
        {
#if PLATFORM_WEBGL || PLATFORM_STANDALONE || UNITY_EDITOR
            Container.Bind<ILeaderboardManager>().FromInstance(webGlLeaderboardManager).AsSingle().NonLazy();
#elif UNITY_ANDROID
            Container.Bind<ILeaderboardManager>().FromInstance(firebaseLeaderboardManager).AsSingle().NonLazy();
#endif
        }
    }
}