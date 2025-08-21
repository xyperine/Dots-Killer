using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.SaveSystem;
using DotsKiller.StatsLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.MilestonesLogic
{
    public class Milestones : MonoBehaviour, IRecalibrationTarget, IPurgeTarget
    {
        [SerializeField] private MilestonesSO milestonesSO;
        [SerializeField] private List<Milestone> milestones;

        private List<MilestoneEntry> _entries;

        private Stats _stats;
        
        public BigDouble PointsIncomeMultiplier { get; private set; } = BigDouble.One;
        public bool KillAutomatonUnlocked { get; private set; } = false;
        public bool PurchasingAutomatonUnlocked { get; private set; } = false;
        public BigDouble FirstUpgradeBoost { get; private set; } = BigDouble.One;
        public BigDouble UpgradesFactor { get; private set; } = BigDouble.One;
        public bool FreeLevelToUpgrades { get; private set; } = false;


        [Inject]
        public void Initialize(Stats stats)
        {
            _stats = stats;
        }


        private void Awake()
        {
            _entries = milestonesSO.SortedByThreshold;
        }


        private void Start()
        {
            foreach (Milestone milestone in milestones)
            {
                milestone.SetData();
            }

            if (GameStateHandler.Loaded)
            {
                foreach (int id in GameStateHandler.State.RecalibrationPersistentMilestones)
                {
                    milestones.Find(m => m.ID == id).Achieved = true;
                }
            }
        }


        private void Update()
        {
            foreach (Milestone milestone in milestones)
            {
                milestone.Achieved |= _stats.Kills >= milestone.Threshold;
                SetAppropriateValue(milestone.ID, milestone.Achieved);

                if (milestone.KeepOnRecalibration && milestone.Achieved)
                {
                    GameStateHandler.State.RecalibrationPersistentMilestones.Add(milestone.ID);
                }
            }
        }


        private void SetAppropriateValue(int id, bool achieved)
        {
            if (!achieved)
            {
                return;
            }

            switch (id)
            {
                case 0:
                    PointsIncomeMultiplier = 1.25d;
                    break;
                case 1:
                    KillAutomatonUnlocked = true;
                    break;
                case 2:
                    PurchasingAutomatonUnlocked = true;
                    break;
                case 3:
                    FirstUpgradeBoost = 2d;
                    break;
                case 4:
                    UpgradesFactor = _stats.RegularUpgradesBought + BigDouble.One;
                    break;
                case 5:
                    FreeLevelToUpgrades = true;
                    break;
            }
        }


        public string GetRewardText(int id)
        {
            return _entries.Find(e => e.ID == id).Reward;
        }


        public string GetThresholdText(int id, Color valueColor)
        {
            string colorTagStart = $"<color=#{ColorUtility.ToHtmlStringRGB(valueColor)}>";
            string colorTagEnd = "</color>";
            string thresholdValue = _entries.Find(e => e.ID == id).KillsThreshold.ToString();

            return $"{colorTagStart}{thresholdValue}{colorTagEnd} Kills";
        }


        public MilestoneEntry GetSorted(int i)
        {
            return milestonesSO.SortedByThreshold[i];
        }


        public bool IsAchieved(int id)
        {
            return
                milestones.Find(m => m.ID == id)
                    .Achieved; //_stats.Kills >= _entries.Find(e => e.ID == id).KillsThreshold;
        }
        

        public string GetRewardEntryName(int id)
        {
            string tableName = "Milestones";
            string entryName = $"Milestone{id + 1}";

            return string.Join('.', tableName, entryName, "Reward");
        }


        public void OnPurge()
        {
            foreach (Milestone milestone in milestones)
            {
                ResetMilestone(milestone);
            }
            
            GameStateHandler.State.RecalibrationPersistentMilestones.Clear();
        }


        private void ResetMilestone(Milestone milestone)
        {
            milestone.Achieved = false;
            
            switch (milestone.ID)
            {
                case 0:
                    PointsIncomeMultiplier = BigDouble.One;
                    break;
                case 1:
                    KillAutomatonUnlocked = false;
                    break;
                case 2:
                    PurchasingAutomatonUnlocked = false;
                    break;
                case 3:
                    FirstUpgradeBoost = BigDouble.One;
                    break;
                case 4:
                    UpgradesFactor = BigDouble.One;
                    break;
                case 5:
                    FreeLevelToUpgrades = false;
                    break;
            }
        }


        public void OnRecalibration()
        {
            Debug.Log("Recalibration: Milestones");

            foreach (Milestone milestone in milestones)
            {
                if (milestone.KeepOnRecalibration)
                {
                    continue;
                }

                ResetMilestone(milestone);
            }
        }
    }
}