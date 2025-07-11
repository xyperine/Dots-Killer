using DotsKiller.Leaderboards.Firebase;
using DotsKiller.Leaderboards.WebGL;
using DotsKiller.Leaderboards.Yandex;
using UnityEngine;
using Zenject;

namespace DotsKiller.Leaderboards
{
    public class LeaderboardInstaller : MonoInstaller
    {
        [SerializeField] private FirebaseLeaderboardManager firebaseLeaderboardManager;
        [SerializeField] private WebGlLeaderboardManager webGlLeaderboardManager;
        [SerializeField] private YandexLeaderboardManager yandexLeaderboardManager;
        
        
        public override void InstallBindings()
        {
#if YandexGamesPlatform_yg
            Container.Bind<ILeaderboardManager>().FromInstance(yandexLeaderboardManager).AsSingle().NonLazy();
#elif PLATFORM_WEBGL || UNITY_EDITOR
            Container.Bind<ILeaderboardManager>().FromInstance(webGlLeaderboardManager).AsSingle().NonLazy();
#elif UNITY_ANDROID
            Container.Bind<ILeaderboardManager>().FromInstance(firebaseLeaderboardManager).AsSingle().NonLazy();
#endif
        }
    }
}