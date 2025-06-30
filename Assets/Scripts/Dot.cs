using System;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class Dot : MonoBehaviour, IPoolable<Vector2, IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;
        

        public void OnSpawned(Vector2 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
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