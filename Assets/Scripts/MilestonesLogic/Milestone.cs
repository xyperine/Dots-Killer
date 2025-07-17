using UnityEngine;
using Zenject;

namespace DotsKiller.MilestonesLogic
{
    public class Milestone : MonoBehaviour
    {
        private Milestones _milestones;
        
        public int ID { get; private set; }
        public int Threshold { get; private set; }


        [Inject]
        public void Initialize(Milestones milestones)
        {
            _milestones = milestones;
        }
        

        private void Awake()
        {
            SetData();
        }


        public void SetData()
        {
            MilestoneEntry entry = _milestones.GetSorted(transform.GetSiblingIndex());

            ID = entry.ID;
            Threshold = entry.KillsThreshold;
        }
    }
}