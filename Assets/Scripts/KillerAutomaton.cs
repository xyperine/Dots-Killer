using DotsKiller.Dots;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class KillerAutomaton : MonoBehaviour
    {
        [SerializeField] private float killsPerSecond;

        private DotsTracker _dotsTracker;
        private AutomatonUpgrades _automatonUpgrades;
        
        private float _killInterval;

        private float _timeSinceLastKill;

        public float KillsPerSecond => killsPerSecond;


        [Inject]
        public void Initialize(DotsTracker dotsTracker, AutomatonUpgrades automatonUpgrades)
        {
            _dotsTracker = dotsTracker;
            _automatonUpgrades = automatonUpgrades;
        }


        private void Awake()
        {
            _killInterval = 1f / killsPerSecond;
        }


        private void Update()
        {
            _killInterval = 1f / (killsPerSecond * _automatonUpgrades.KillsPerSecondMultiplier);
            
            _timeSinceLastKill += Time.deltaTime;
            if (_timeSinceLastKill >= _killInterval)
            {
                Kill();

                _timeSinceLastKill = 0f;
            }
        }


        private void Kill()
        {
            if (_dotsTracker.AmountAlive == 0)
            {
                return;
            }
            
            _dotsTracker.GetFirstAvailable().Die();
        }


        public void SetStatus(bool value)
        {
            enabled = value;
        }
    }
}