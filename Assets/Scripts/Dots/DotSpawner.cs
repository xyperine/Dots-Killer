using AYellowpaper;
using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Range = DotsKiller.Utility.Range;

namespace DotsKiller.Dots
{
    public class DotSpawner : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IAreaProvider> areaProviderReference;
        [SerializeField] private AnimationCurve spawnIntervalCurve;
        [SerializeField] private Range spawnIntervalRange;
        [SerializeField] private BigDouble maxPoints;

        private float _spawnInterval;
        
        private float _timeSinceLastSpawn;
        
        private IAreaProvider _areaProvider;
        private Balance _balance;

        private Dot.Factory _factory;

        private bool ReadyToSpawn => _timeSinceLastSpawn >= _spawnInterval;


        [Inject]
        public void Initialize(Dot.Factory factory, Balance balance)
        {
            _factory = factory;
            _balance = balance;
        }


        private void Start()
        {
            _areaProvider = areaProviderReference.Value;
        }


        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            
            float t = _balance.Points.Add(BigDouble.One).InverseLerpLog10(BigDouble.One, maxPoints);
            _spawnInterval = spawnIntervalRange.Lerp(spawnIntervalCurve.Evaluate(t));
            
            if (ReadyToSpawn)
            {
                Vector2 position = (Vector2) _areaProvider.Center + _areaProvider.Extents * Random.insideUnitCircle;
                _factory.Create(position);

                _timeSinceLastSpawn = 0f;
            }
        }
    }
}