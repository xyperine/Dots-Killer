using DotsKiller.RegularUpgrading;
using Zenject;

namespace DotsKiller
{
    public class PurchasingAutomaton : Automaton
    {
        private RegularUpgrades _regularUpgrades;

        public override AutomatonID ID => AutomatonID.Purchase;
        public override string Name => "Auto Purchase";


        [Inject]
        public void Initialize(RegularUpgrades regularUpgrades, AutomatonUpgrades automatonUpgrades)
        {
            _regularUpgrades = regularUpgrades;
            upgrades = automatonUpgrades;
        }
        

        protected override float CalculateTickInterval()
        {
            return 1f / (ticksPerSecond * upgrades.PurchasesTickspeedMultiplier);
        }


        protected override float CalculateActionsPerTick()
        {
            return upgrades.PurchasesActionsPerTickMultiplier;
        }


        protected override void PerformAction()
        {
            _regularUpgrades.PurchaseAll();
        }


        protected override void PerformActions(int amount)
        {
            if (upgrades.AutoPurchaseAptMaxedOut)
            {
                _regularUpgrades.PurchaseAllInBulk();
                return;
            }
            
            _regularUpgrades.PurchaseAllInBulk(amount);
        }
    }
}