using System;
using BreakInfinity;
using DotsKiller.Economy;
using DotsKiller.SaveSystem;
using DotsKiller.Utility;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class Recalibration : MonoBehaviour
    {
        [SerializeField] private BigDouble pointsThreshold = 1e12;
        
        private Balance _balance;
        private SignalBus _signalBus;

        public bool Available => _balance.Points >= pointsThreshold && NextExponent > CurrentExponent;

        //public float Progress => Mathf.Clamp01((float) (BigDouble.Log10(_balance.Points) / BigDouble.Log10(pointsThreshold)));
        
        public BigDouble CurrentExponent { get; private set; } = BigDouble.One;
        public BigDouble NextExponent => Formulas.CalculateRecalibrationExponent(_balance.TotalPoints, pointsThreshold);


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
            }
        }


        public void Perform()
        {
            Debug.Log("Recalibration performed!");
            CurrentExponent = Formulas.CalculateRecalibrationExponent(_balance.TotalPoints, pointsThreshold);
            GameStateHandler.State.RecalibrationExponent = CurrentExponent;
            _signalBus.Fire<RecalibrationResetSignal>();
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