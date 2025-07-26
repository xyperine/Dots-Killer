using UnityEngine;

namespace DotsKiller
{
    //TODO: Move some data definition to sheets
    public abstract class Automaton : MonoBehaviour
    {
        [SerializeField] protected float ticksPerSecond;

        protected AutomatonUpgrades upgrades;

        protected float tickInterval;
        protected float timeSinceLastAction;
        
        private float _previousFrameActionsRemainder;

        public float Tickspeed { get; private set; }
        public float ActionsPerTick { get; private set; }
        
        public abstract AutomatonID ID { get; }
        public abstract string Name { get; }
        public abstract string FormattedActionsPerTick { get; }


        private void Awake()
        {
            tickInterval = 1f / ticksPerSecond;

            Tickspeed = ticksPerSecond;
        }

        
        private void Update()
        {
            tickInterval = CalculateTickInterval();

            Tickspeed = 1f / tickInterval;

            ActionsPerTick = CalculateActionsPerTick();
            
            timeSinceLastAction += Time.deltaTime;
            
            if (timeSinceLastAction >= tickInterval)
            {
                float actionsThisFrame = _previousFrameActionsRemainder;
                float ticksThisFrame = tickInterval < Time.deltaTime
                    ? Time.deltaTime / tickInterval
                    : 1f;
                actionsThisFrame += ticksThisFrame * ActionsPerTick;

                for (int i = Mathf.FloorToInt(actionsThisFrame); i >= 1; i--)
                {
                    PerformAction();
                }
                
                PerformActions(Mathf.FloorToInt(actionsThisFrame));
                
                _previousFrameActionsRemainder = actionsThisFrame - Mathf.FloorToInt(actionsThisFrame);

                timeSinceLastAction = 0f;
                
            }
        }


        protected abstract float CalculateTickInterval();
        
        
        protected abstract float CalculateActionsPerTick();
        

        protected abstract void PerformAction();


        protected abstract void PerformActions(int amount);


        public void SetStatus(bool value)
        {
            enabled = value;
        }
    }
}