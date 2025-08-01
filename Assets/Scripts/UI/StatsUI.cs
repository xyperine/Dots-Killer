using System;
using System.Collections.Generic;
using DotsKiller.StatsLogic;
using DotsKiller.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Zenject;

namespace DotsKiller.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private List<StatUI> statUIs;
        [SerializeField] private LocalizedStringTable unitsTable;

        private Stats _stats;


        [Inject]
        public void Initialize(Stats stats)
        {
            _stats = stats;
        }


        private void Update()
        {
            statUIs[0].SetText("Kills", Formatting.DefaultFormat(_stats.Kills));
            statUIs[1].SetText("TotalPoints", Formatting.DefaultFormat(_stats.TotalPoints));
            statUIs[2].SetText("PointsPerKill", Formatting.DefaultFormat(_stats.PointsPerKill));
            statUIs[3].SetText("TotalPlaytime", ConstructTotalPlaytimeString());
        }


        private string ConstructTotalPlaytimeString()
        {
            TimeSpan playtime = _stats.TotalPlaytime;
            int days = playtime.Days;
            int hours = playtime.Hours;
            int minutes = playtime.Minutes;
            int seconds = playtime.Seconds;
            StringTable t = unitsTable.GetTable();
            return $"{t.GetEntry("Units.Days").GetLocalizedString(days)} " +
                   $"{t.GetEntry("Units.Hours").GetLocalizedString(hours)} " +
                   $"{t.GetEntry("Units.Minutes").GetLocalizedString(minutes)} " +
                   $"{t.GetEntry("Units.Seconds").GetLocalizedString(seconds)}";
        }
    }
}