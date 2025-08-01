using System;
using DotsKiller.Economy;
using DotsKiller.StatsLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.Dots
{
    public class Dot : MonoBehaviour, IPoolable<Vector2, IMemoryPool>, IDisposable
    {
        private KillReward _reward;
        private Clickable _clickable;
        private Stats _stats;
        private DotsTracker _dotsTracker;
        
        private IMemoryPool _pool;


        [Inject]
        public void Initialize(KillReward reward, Clickable clickable, Stats stats, DotsTracker dotsTracker)
        {
            _reward = reward;
            _clickable = clickable;
            _stats = stats;
            _dotsTracker = dotsTracker;
        }


        public void OnSpawned(Vector2 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            _dotsTracker.Register(this);
            
            _clickable.OnClicked += Die;
        }


        public void Die()
        {
            _reward.Give();
            _stats.Kills++;
            _dotsTracker.Unregister(this);
            
            Dispose();
        }


        public void OnDespawned()
        {
            _pool = null;
            
            _clickable.OnClicked -= Die;
        }


        public void Dispose()
        {
            _pool.Despawn(this);
        }


        public class Factory : PlaceholderFactory<Vector2, Dot>
        {
        
        }


        public class Pool : MonoPoolableMemoryPool<Vector2, IMemoryPool, Dot>
        {
            
        }
    }
}