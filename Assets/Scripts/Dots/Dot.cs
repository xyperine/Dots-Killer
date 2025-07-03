using System;
using DotsKiller.Economy;
using UnityEngine;
using Zenject;

namespace DotsKiller.Dots
{
    public class Dot : MonoBehaviour, IPoolable<Vector2, IMemoryPool>, IDisposable
    {
        private Reward _reward;
        private Clickable _clickable;
        
        private IMemoryPool _pool;


        [Inject]
        public void Initialize(Reward reward, Clickable clickable)
        {
            _reward = reward;
            _clickable = clickable;
        }


        public void OnSpawned(Vector2 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            
            _clickable.OnClicked += Die;
        }


        public void Die()
        {
            _reward.Give();
            
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