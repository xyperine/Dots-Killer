#if !PLATFORM_WEBGL
using DotsKiller.Leaderboards.Firebase;
#endif
using DotsKiller.Leaderboards.WebGL;
using DotsKiller.Leaderboards.Yandex;
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
        [SerializeField] private YandexLeaderboardManager yandexLeaderboardManager;
        
        
        public override void InstallBindings()
        {
#if YandexGamesPlatform_yg
            Container.Bind<ILeaderboardManager>().FromInstance(yandexLeaderboardManager).AsSingle().NonLazy();
#elif PLATFORM_WEBGL || PLATFORM_STANDALONE || UNITY_EDITOR
            Container.Bind<ILeaderboardManager>().FromInstance(webGlLeaderboardManager).AsSingle().NonLazy();
#elif UNITY_ANDROID
            Container.Bind<ILeaderboardManager>().FromInstance(firebaseLeaderboardManager).AsSingle().NonLazy();
#endif
        }
    }
}