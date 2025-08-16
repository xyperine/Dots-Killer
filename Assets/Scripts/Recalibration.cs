using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class Recalibration : MonoBehaviour, IPurgeTarget
    {
        [SerializeField] private BigDouble pointsThreshold = 1e12;
        
        private Balance _balance;
        private SignalBus _signalBus;

        private BigDouble _lastRecalibrationPoints;
        
        public bool Available => _balance.Points >= pointsThreshold && _balance.TotalPoints > _lastRecalibrationPoints;

        //public float Progress => Mathf.Clamp01((float) (BigDouble.Log10(_balance.Points) / BigDouble.Log10(pointsThreshold)));

        public BigDouble CurrentExponent { get; private set; } = BigDouble.One;
        public BigDouble NextExponent => Formulas.CalculateRecalibrationExponent(_balance.TotalPoints, pointsThreshold);
        public BigDouble CurrentMultiplier { get; private set; } = BigDouble.One;
        public BigDouble NextMultiplier =>
            Formulas.CalculateRecalibrationMultiplier(_balance.TotalPoints, pointsThreshold);


        [Inject]
        public void Initialize(Balance balance, SignalBus signalBus)
        {
            _balance = balance;
            _signalBus = signalBus;
        }


        private void Start()
        {
            if (GameStateHandler.Loaded)
            {
                CurrentExponent = GameStateHandler.State.RecalibrationExponent;
                CurrentMultiplier = GameStateHandler.State.RecalibrationMultiplier;
            }
        }


        public void Perform()
        {
            Debug.Log("Recalibration performed!");
            
            _lastRecalibrationPoints = _balance.TotalPoints;
            CurrentExponent = Formulas.CalculateRecalibrationExponent(_balance.TotalPoints, pointsThreshold);
            CurrentMultiplier =  Formulas.CalculateRecalibrationMultiplier(_balance.TotalPoints, pointsThreshold);
            
            GameStateHandler.State.RecalibrationMultiplier = CurrentMultiplier;
            GameStateHandler.State.RecalibrationExponent = CurrentExponent;
            
            _signalBus.Fire<RecalibrationResetSignal>();
        }


        public void OnPurge()
        {
            _lastRecalibrationPoints = BigDouble.Zero;
            CurrentExponent = BigDouble.One;
            CurrentMultiplier = BigDouble.One;
            
            GameStateHandler.State.RecalibrationMultiplier = CurrentMultiplier;
            GameStateHandler.State.RecalibrationExponent = CurrentExponent;
        }
    }


    public class RecalibrationResetSignal
    {
        
    }


    public interface IRecalibrationTarget
    {
        void OnRecalibration();
    }
}