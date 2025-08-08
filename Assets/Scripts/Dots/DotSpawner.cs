using AYellowpaper;
using BreakInfinity;
using DotsKiller.StatsLogic;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;
using Range = DotsKiller.Utility.Range;

namespace DotsKiller.Dots
{
    public class DotSpawner : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IAreaProvider> areaProviderReference;
        [SerializeField] private AnimationCurve spawnsPerSecondCurve;
        [SerializeField] private Range spawnsPerSecondRange;
        [SerializeField] private BigDouble maxPoints;

        private float _spawnInterval;
        
        private float _timeSinceLastSpawn;
        
        private IAreaProvider _areaProvider;
        private Stats _stats;

        private Dot.Factory _factory;

        private bool ReadyToSpawn => _timeSinceLastSpawn >= _spawnInterval;
        public float SpawnRate => 1f / _spawnInterval;


        [Inject]
        public void Initialize(Dot.Factory factory, Stats stats)
        {
            _factory = factory;
            _stats = stats;
        }


        private void Start()
        {
            _areaProvider = areaProviderReference.Value;
        }


        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            
            float t = _stats.TotalPoints.Add(BigDouble.One).InverseLerpLog10(BigDouble.One, maxPoints);
            _spawnInterval = 1f / spawnsPerSecondRange.Lerp(spawnsPerSecondCurve.Evaluate(t));
            
            if (ReadyToSpawn)
            {
                Vector2 position = _areaProvider.RandomPoint;
                _factory.Create(position);

                _timeSinceLastSpawn = 0f;
            }
        }
    }
}