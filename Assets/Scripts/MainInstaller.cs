using DotsKiller.Automatons.Upgrades;
using DotsKiller.Dots;
using DotsKiller.Economy;
using DotsKiller.MilestonesLogic;
using DotsKiller.RegularUpgrading;
using DotsKiller.StatsLogic;
using DotsKiller.UI;
using DotsKiller.Unlocking;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Balance balance;
        [SerializeField] private BalanceModifiersCalculator balanceModifiersCalculator;
        [SerializeField] private BulkBuyDetection bulkBuyDetection;
        [SerializeField] private RegularUpgrades regularUpgrades;
        [SerializeField] private AutomatonUpgrades automatonUpgrades;
        [SerializeField] private Milestones milestones;
        [SerializeField] private UnlockablesManager unlockablesManager;
        [SerializeField] private Stats stats;
        [SerializeField] private StatsTracker statsTracker;
        [SerializeField] private GameClock gameClock;
        [SerializeField] private DotSpawner dotSpawner;
        [SerializeField] private DotsTracker dotsTracker;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Purge purge;
        [SerializeField] private PopupManager popupManager;
        [SerializeField] private Recalibration recalibration;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Balance>().FromInstance(balance).AsSingle().NonLazy();
            
            Container.Bind<BalanceModifiersCalculator>().FromInstance(balanceModifiersCalculator).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<BulkBuyDetection>().FromInstance(bulkBuyDetection).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<RegularUpgrades>().FromInstance(regularUpgrades).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<AutomatonUpgrades>().FromInstance(automatonUpgrades).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<Milestones>().FromInstance(milestones).AsCached().NonLazy();
            
            Container.BindInterfacesAndSelfTo<UnlockablesManager>().FromInstance(unlockablesManager).AsCached().NonLazy();

            Container.Bind<Stats>().FromInstance(stats).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StatsTracker>().FromInstance(statsTracker).AsSingle().NonLazy();

            Container.Bind<GameClock>().FromInstance(gameClock).AsSingle().NonLazy();
            
            Container.Bind<DotSpawner>().FromInstance(dotSpawner).AsSingle().NonLazy();
            Container.BindFactory<Vector2, Dot, Dot.Factory>()
                .FromPoolableMemoryPool<Vector2, Dot, Dot.Pool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(dotPrefab)
                    .UnderTransformGroup("Dots"));
            Container.BindInterfacesAndSelfTo<DotsTracker>().FromInstance(dotsTracker).AsSingle().NonLazy();

            Container.Bind<Purge>().FromInstance(purge).AsSingle().NonLazy();
            
            Container.Bind<PopupManager>().FromInstance(popupManager).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<Recalibration>().FromInstance(recalibration).AsSingle().NonLazy();

            InstallSignals();
        }


        private void InstallSignals()
        {
            Container.DeclareSignal<PurgePerformedSignal>();
            Container.BindSignal<PurgePerformedSignal>().ToMethod<IPurgeTarget>(t => t.OnPurge).FromResolveAll();

            Container.DeclareSignal<RecalibrationResetSignal>();
            Container.BindSignal<RecalibrationResetSignal>().ToMethod<IRecalibrationTarget>(t => t.OnRecalibration)
                .FromResolveAll();
        }
    }
}