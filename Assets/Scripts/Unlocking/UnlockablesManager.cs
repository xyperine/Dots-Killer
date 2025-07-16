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
                    case UnlockableID.Automatons:
                        if (_milestones.AutomatonsUnlocked)
                        {
                            unlockable.Unlock();
                        }
                        break;
                    case UnlockableID.Generators:
                        if (_milestones.GeneratorsUnlocked)
                        {
                            unlockable.Unlock();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}