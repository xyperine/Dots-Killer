using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.StatsLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.MilestonesLogic
{
    public class Milestones : MonoBehaviour, IRecalibrationTarget
    {
        [SerializeField] private MilestonesSO milestonesSO;
        [SerializeField] private List<Milestone> milestones;

        private List<MilestoneEntry> _entries;

        private Stats _stats;
        
        public BigDouble PointsIncomeMultiplier { get; private set; } = BigDouble.One;
        public bool KillAutomatonUnlocked { get; private set; } = false;
        public bool PurchasingAutomatonUnlocked { get; private set; } = false; // Old generators unlocked
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
        }


        private void Update()
        {
            foreach (Milestone milestone in milestones)
            {
                bool achieved = _stats.Kills >= milestone.Threshold;
                SetAppropriateValue(milestone.ID, achieved);
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
            return _stats.Kills >= _entries.Find(e => e.ID == id).KillsThreshold;
        }


        public string GetRewardEntryName(int id)
        {
            string tableName = "Milestones";
            string entryName = $"Milestone{id + 1}";

            return string.Join('.', tableName, entryName, "Reward");
        }


        public void OnPurge()
        {
            PointsIncomeMultiplier = BigDouble.One;
            KillAutomatonUnlocked  = false;
            PurchasingAutomatonUnlocked = false;
            FirstUpgradeBoost = BigDouble.One;
            UpgradesFactor = BigDouble.One;
            FreeLevelToUpgrades = false;
        }


        public void OnRecalibration()
        {
            Debug.Log("Recalibration: Milestones");
            
            PointsIncomeMultiplier = BigDouble.One;
            KillAutomatonUnlocked  = false;
            PurchasingAutomatonUnlocked = false;
            FirstUpgradeBoost = BigDouble.One;
            UpgradesFactor = BigDouble.One;
            FreeLevelToUpgrades = false;
        }
    }
}