using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using Zenject;

namespace DotsKiller.MilestonesLogic
{
    public class MilestoneUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent rewardLse;
        [SerializeField] private LocalizeStringEvent thresholdLse;
        [SerializeField] private GameObject achievedFrameObject;
        [SerializeField] private Milestone milestone;
        [SerializeField] private Color thresholdValueColor;

        private Milestones _milestones;


        [Inject]
        public void Initialize(Milestones milestones)
        {
            _milestones = milestones;
        }


        private void Start()
        {
            LocalizedString thresholdLocalizedString = thresholdLse.StringReference;
            ((StringVariable) thresholdLocalizedString["threshold"]).Value = Formatting.DefaultFormat(milestone.Threshold);
            ((StringVariable) thresholdLocalizedString["color"]).Value = ColorUtility.ToHtmlStringRGB(thresholdValueColor);
            
            rewardLse.SetEntry(_milestones.GetRewardEntryName(milestone.ID));
        }


        private void Update()
        {
            achievedFrameObject.SetActive(_milestones.IsAchieved(milestone.ID));
        }
    }
}