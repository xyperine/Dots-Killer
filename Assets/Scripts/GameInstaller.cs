using DotsKiller.Dots;
using DotsKiller.Economy;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Balance balance;
        [SerializeField] private BulkBuyDetection bulkBuyDetection;
        [SerializeField] private DotSpawner dotSpawner;
        [SerializeField] private GameObject dotPrefab;
        
        
        public override void InstallBindings()
        {
            Container.Bind<Balance>().FromInstance(balance).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<BulkBuyDetection>().FromInstance(bulkBuyDetection).AsSingle().NonLazy();
            
            Container.Bind<DotSpawner>().FromInstance(dotSpawner).AsSingle().NonLazy();
            Container.BindFactory<Vector2, Dot, Dot.Factory>()
                .FromPoolableMemoryPool<Vector2, Dot, Dot.Pool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(dotPrefab)
                    .UnderTransformGroup("Dots"));
        }
    }
}