using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.UI;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    [DisallowMultipleComponent]
    public class Purge : MonoBehaviour, IProgressProvider
    {
        [SerializeField] private BigDouble pointsThreshold = new BigDouble(1d, 100);

        private Balance _balance;
        private SignalBus _signalBus;

        public bool Available => _balance.Points >= pointsThreshold;

        public float Progress => Mathf.Clamp01((float) (BigDouble.Log10(_balance.Points) / BigDouble.Log10(pointsThreshold)));


        [Inject]
        public void Initialize(Balance balance, SignalBus signalBus)
        {
            _balance = balance;
            _signalBus = signalBus;
        }
        
        
        public void Perform()
        {
            Debug.Log("Purge performed!");
            // Fire prestige signal/event
            _signalBus.Fire<PurgePerformedSignal>();
        }
    }


    public class PurgePerformedSignal
    {
        
    }


    public interface IPurgeTarget
    {
        void OnPurge();
    }
}