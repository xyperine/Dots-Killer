using System;
using System.Collections.Generic;
using DotsKiller.StatsLogic;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace DotsKiller.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private List<StatUI> statUIs;
        [SerializeField] private LocalizedStringTable unitsTable;

        private Stats _stats;
        private LocalizationAssetsHelper _localizationAssetsHelper;

        private string _playtimeString;


        [Inject]
        public void Initialize(Stats stats, LocalizationAssetsHelper localizationAssetsHelper)
        {
            _stats = stats;
            _localizationAssetsHelper = localizationAssetsHelper;
        }


        private void Update()
        {
            ConstructTotalPlaytimeString();

            statUIs[0].SetText("Kills", Formatting.DefaultFormat(_stats.Kills) + " <sprite=2 color=#DBDBDB>");
            statUIs[1].SetText("TotalPoints", Formatting.DefaultFormat(_stats.TotalPoints) + " <sprite=0 color=#DBDBDB>");
            statUIs[2].SetText("PointsPerKill", Formatting.DefaultFormat(_stats.PointsPerKill) + " <sprite=0 color=#DBDBDB>");
            statUIs[3].SetText("TotalPlaytime", _playtimeString);
            statUIs[4].SetText("Purges", Formatting.DefaultFormat(_stats.Purges) + " <sprite=3 color=#DBDBDB>");
            statUIs[5].SetText("PointsIncomeExponent", "^" + Formatting.DefaultFormat(_stats.PointsIncomeExponent));
        }


        private void ConstructTotalPlaytimeString()
        {
            TimeSpan playtime = _stats.TotalPlaytime;
            int days = playtime.Days;
            int hours = playtime.Hours;
            int minutes = playtime.Minutes;
            int seconds = playtime.Seconds;
            _localizationAssetsHelper.GetLocalizedAsset( unitsTable.GetTableAsync(), table =>
            {
                _playtimeString = $"{table.GetEntry("Units.Days").GetLocalizedString(days)} " +
                $"{table.GetEntry("Units.Hours").GetLocalizedString(hours)} " +
                $"{table.GetEntry("Units.Minutes").GetLocalizedString(minutes)} " +
                $"{table.GetEntry("Units.Seconds").GetLocalizedString(seconds)}";
            });
        }
    }
}