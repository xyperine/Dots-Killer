using BreakInfinity;
using DotsKiller.Economy;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class EndGoal : MonoBehaviour, IPurgeTarget
    {
        private enum EndGoalType
        {
            Points,
            Purge,
        }
        
        [SerializeField] private EndGoalType type;
        [SerializeField, ShowIf(nameof(type), EndGoalType.Points)] private BigDouble pointsGoal;

        private Balance _balance;
        private SignalBus _signalBus;
        

        [Inject]
        public void Initialize(Balance balance, SignalBus signalBus)
        {
            _balance = balance;
            _signalBus = signalBus;
        }
        

        private void Update()
        {
            if (type == EndGoalType.Points)
            {
                if (_balance.TotalPoints >= pointsGoal)
                {
                    OnGoalReached();
                }
            }
        }


        private void OnGoalReached()
        {
            _signalBus.Fire<GameOverSignal>();
            enabled = false;
        }


        public void OnPurge()
        {
            if (type == EndGoalType.Purge)
            {
                OnGoalReached();
            }
        }
    }


    public class GameOverSignal
    {
        
    }


    public interface IGameOverTarget
    {
        void OnGameOver();
    }
}