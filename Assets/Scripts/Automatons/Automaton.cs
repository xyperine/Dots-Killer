using DotsKiller.Automatons.Upgrades;
using UnityEngine;

namespace DotsKiller.Automatons
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
                if (tickInterval >= Time.deltaTime)
                {
                    PerformAction();
                }
                else
                {
                    float actionsThisFrame = _previousFrameActionsRemainder;
                    float ticksThisFrame = Time.deltaTime / tickInterval;
                    actionsThisFrame += ticksThisFrame * ActionsPerTick;
                    
                    PerformActions(Mathf.FloorToInt(actionsThisFrame));
                    
                    _previousFrameActionsRemainder = actionsThisFrame - Mathf.FloorToInt(actionsThisFrame);
                }
                
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