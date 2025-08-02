using System.Collections.Generic;
using BreakInfinity;
using DotsKiller.StatsLogic;
using UnityEngine;
using Zenject;

namespace DotsKiller.MilestonesLogic
{
    public class Milestones : MonoBehaviour
    {
        [SerializeField] private MilestonesSO milestonesSO;
        [SerializeField] private List<Milestone> milestones;

        private List<MilestoneEntry> _entries;

        private Stats _stats;
        
        public BigDouble PointsIncomeMultiplier { get; private set; } = BigDouble.One;
        public bool AutomatonsUnlocked { get; private set; } = false;
        public bool GeneratorsUnlocked { get; private set; } = false;
        public BigDouble FirstUpgradeBoost { get; private set; } = BigDouble.One;
        public BigDouble UpgradesFactor { get; private set; } = BigDouble.One;
        public BigDouble GeneratorsBoost { get; private set; } = BigDouble.One;


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
                    AutomatonsUnlocked = true;
                    break;
                case 2:
                    GeneratorsUnlocked = true;
                    break;
                case 3:
                    FirstUpgradeBoost = 1.5d;
                    break;
                case 4:
                    UpgradesFactor = _stats.RegularUpgradesBought + BigDouble.One;
                    break;
                case 5:
                    GeneratorsBoost = BigDouble.One; // 10% of points per kill or smth
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
            AutomatonsUnlocked  = false;
            GeneratorsUnlocked = false;
            FirstUpgradeBoost = BigDouble.One;
            UpgradesFactor = BigDouble.One;
            GeneratorsBoost = BigDouble.One;
        }
    }
}