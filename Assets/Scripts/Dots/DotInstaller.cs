using DotsKiller.Economy;
using UnityEngine;
using Zenject;

namespace DotsKiller.Dots
{
    public class DotInstaller : MonoInstaller
    {
        [SerializeField] private Reward reward;
        [SerializeField] private Clickable clickable;
        
        
        public override void InstallBindings()
        {
            Container.Bind<Reward>().FromInstance(reward).AsSingle();
            Container.Bind<Clickable>().FromInstance(clickable).AsSingle();
        }
    }
}