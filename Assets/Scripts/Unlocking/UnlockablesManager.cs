using System;
using System.Collections.Generic;
using DotsKiller.MilestonesLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.Unlocking
{
    public class UnlockablesManager : MonoBehaviour
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
                switch (unlockable.ID)
                {
                    case UnlockableID.KillAutomaton:
                        if (_milestones.KillAutomatonUnlocked)
                        {
                            unlockable.Unlock();
                        }
                        break;
                    case UnlockableID.PurchasingAutomaton:
                        if (_milestones.PurchasingAutomatonUnlocked)
                        {
                            unlockable.Unlock();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public void OnPurge()
        {
            for (int i = 0; i < _unlockables.Count; i++)
            {
                Unlockable unlockable = _unlockables[i];
                switch (unlockable.ID)
                {
                    case UnlockableID.KillAutomaton:
                        unlockable.Lock();
                        break;
                    case UnlockableID.PurchasingAutomaton:
                        unlockable.Lock();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}