using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private DotSpawner dotsSpawner;
        [SerializeField] private GameObject dotPrefab;
        
        
        public override void InstallBindings()
        {
            Container.Bind<DotSpawner>().FromInstance(dotsSpawner).AsSingle().NonLazy();
            Container.BindFactory<Vector2, Dot, Dot.Factory>()
                .FromPoolableMemoryPool<Vector2, Dot, Dot.Pool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(dotPrefab)
                    .UnderTransformGroup("Dots"));
        }
    }
}