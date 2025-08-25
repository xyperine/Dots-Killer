using System;
using DotsKiller.Automatons.Upgrades;
using UnityEngine;

namespace DotsKiller.Automatons
{
    //TODO: Move some data definition to sheets
    public abstract class Automaton : MonoBehaviour, IPurchaseLockTarget
    {
        [SerializeField] protected float ticksPerSecond;

        private bool _active;
        private float _previousFrameActionsRemainder;
        
        protected AutomatonUpgrades upgrades;

        protected float tickInterval;
        protected float timeSinceLastAction;
        
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
            if (!_active)
            {
                return;
            }
            
            tickInterval = CalculateTickInterval();

            Tickspeed = 1f / tickInterval;

            ActionsPerTick = CalculateActionsPerTick();
            
            timeSinceLastAction += Time.deltaTime;
            
            if (timeSinceLastAction >= tickInterval)
            {
                if (tickInterval < Time.deltaTime) // compensate for lag or when the tick happens faster than next frame
                {
                    float actionsThisFrame = _previousFrameActionsRemainder;
                    float ticksThisFrame = Time.deltaTime / tickInterval;
                    actionsThisFrame += ticksThisFrame * ActionsPerTick;
                    
                    PerformActions(Mathf.FloorToInt(actionsThisFrame));
                    
                    _previousFrameActionsRemainder = actionsThisFrame - Mathf.FloorToInt(actionsThisFrame);
                    
                    //Debug.Log($"The tick is too fast: {actionsThisFrame}");
                }
                else if (Math.Abs(ActionsPerTick - 1f) < 1e-6f) // if only 1 action per tick
                {
                    PerformAction();
                }
                else // if more or less than 1 action per tick and ticks are slower than frames
                {
                    float actionsThisFrame = _previousFrameActionsRemainder;
                    actionsThisFrame += ActionsPerTick;
                    
                    PerformActions(Mathf.FloorToInt(actionsThisFrame));
                    
                    _previousFrameActionsRemainder = actionsThisFrame - Mathf.FloorToInt(actionsThisFrame);
                    
                    //Debug.Log(actionsThisFrame);
                }

                timeSinceLastAction -= tickInterval;
            }
        }


        protected abstract float CalculateTickInterval();
        
        
        protected abstract float CalculateActionsPerTick();
        

        protected abstract void PerformAction();


        protected abstract void PerformActions(int amount);


        public void Activate()
        {
            _active = true;
            SetStatus(true);
        }
        

        public void SetStatus(bool value)
        {
            enabled = value;
        }
    }
}