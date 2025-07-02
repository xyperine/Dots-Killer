using System;
using DotsKiller.Economy;
using UnityEngine;
using Zenject;

namespace DotsKiller.Dots
{
    public class Dot : MonoBehaviour, IPoolable<Vector2, IMemoryPool>, IDisposable
    {
        [SerializeField] private Reward reward;
        
        private IMemoryPool _pool;
        

        public void OnSpawned(Vector2 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
        }


        private void OnMouseDown()
        {
            reward.Give();
            Dispose();
        }


        public void OnDespawned()
        {
            _pool = null;
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