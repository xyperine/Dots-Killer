using DotsKiller.Automatons.Upgrades;
using DotsKiller.Dots;
using Zenject;

namespace DotsKiller.Automatons
{
    public class AutoKillAutomaton : Automaton
    {
        private DotsTracker _dotsTracker;

        public override AutomatonID ID => AutomatonID.Kill;
        public override string Name => "Auto Kill";


        [Inject]
        public void Initialize(DotsTracker dotsTracker, AutomatonUpgrades automatonUpgrades)
        {
            _dotsTracker = dotsTracker;
            upgrades = automatonUpgrades;
        }
        

        protected override float CalculateTickInterval()
        {
            return 1f / (ticksPerSecond * upgrades.KillsTickspeedMultiplier);
        }


        protected override float CalculateActionsPerTick()
        {
            return upgrades.KillsActionsPerTickMultiplier;
        }


        protected override void PerformAction()
        {
            if (_dotsTracker.AmountAlive == 0)
            {
                return;
            }

            _dotsTracker.GetFirstAvailable().Die();
        }


        protected override void PerformActions(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                PerformAction();
            }
        }
    }
}