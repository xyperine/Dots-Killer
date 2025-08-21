using System;
using System.Collections.Generic;
using DotsKiller.MilestonesLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.Unlocking
{
    public class UnlockablesManager : MonoBehaviour, IRecalibrationTarget, IPurgeTarget
    {
        private readonly List<Unlockable> _unlockables = new List<Unlockable>();

        private Milestones _milestones;
        
        
        [Inject]
        private void Initialize(Milestones milestones)
        {
            _milestones = milestones;
        }
        

        public void Register(Unlockable unlockable)
        {
            _unlockables.Add(unlockable);
        }


        private void Update()
        {
            for (int i = 0; i < _unlockables.Count; i++)
            {
                Unlockable unlockable = _unlockables[i];
                bool conditionMet = unlockable.ID switch
                {
                    UnlockableID.KillAutomaton => _milestones.KillAutomatonUnlocked,
                    UnlockableID.PurchasingAutomaton => _milestones.PurchasingAutomatonUnlocked,
                    UnlockableID.AutomatonsTab => _milestones.KillAutomatonUnlocked ||
                                                  _milestones.PurchasingAutomatonUnlocked,
                    _ => throw new ArgumentOutOfRangeException(),
                };

                if (conditionMet)
                {
                    unlockable.Unlock();
                }
            }
        }


        public void OnPurge()
        {
            for (int i = 0; i < _unlockables.Count; i++)
            {
                Unlockable unlockable = _unlockables[i];
                unlockable.Lock();
            }
        }


        public void OnRecalibration()
        {
            Debug.Log("Recalibration: Unlockables Manager");
            
            for (int i = 0; i < _unlockables.Count; i++)
            {
                Unlockable unlockable = _unlockables[i];
                unlockable.Lock();
            }
        }
    }
}