using DotsKiller.Dots;
using DotsKiller.Economy;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using DotsKiller.Unlocking;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Balance balance;
        [SerializeField] private BulkBuyDetection bulkBuyDetection;
        [SerializeField] private RegularUpgrades regularUpgrades;
        [SerializeField] private Milestones milestones;
        [SerializeField] private UnlockablesManager unlockablesManager;
        [SerializeField] private Stats stats;
        [SerializeField] private GameClock gameClock;
        [SerializeField] private DotSpawner dotSpawner;
        [SerializeField] private DotsTracker dotsTracker;
        [SerializeField] private GameObject dotPrefab;
        
        
        public override void InstallBindings()
        {
            Container.Bind<Balance>().FromInstance(balance).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<BulkBuyDetection>().FromInstance(bulkBuyDetection).AsSingle().NonLazy();
            
            Container.Bind<RegularUpgrades>().FromInstance(regularUpgrades).AsSingle().NonLazy();

            Container.Bind<Milestones>().FromInstance(milestones).AsCached().NonLazy();
            
            Container.Bind<UnlockablesManager>().FromInstance(unlockablesManager).AsCached().NonLazy();

            Container.Bind<Stats>().FromInstance(stats).AsSingle().NonLazy();

            Container.Bind<GameClock>().FromInstance(gameClock).AsSingle().NonLazy();
            
            Container.Bind<DotSpawner>().FromInstance(dotSpawner).AsSingle().NonLazy();
            Container.BindFactory<Vector2, Dot, Dot.Factory>()
                .FromPoolableMemoryPool<Vector2, Dot, Dot.Pool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(dotPrefab)
                    .UnderTransformGroup("Dots"));
            Container.Bind<DotsTracker>().FromInstance(dotsTracker).AsSingle().NonLazy();
        }
    }
}