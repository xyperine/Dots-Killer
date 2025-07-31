using System;
using System.Collections.Generic;
using DotsKiller.Utility;
using TMPro;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> ts;

        private Stats _stats;


        [Inject]
        public void Initialize(Stats stats)
        {
            _stats = stats;
        }


        private void Update()
        {
            ts[0].text = $"Kills: {_stats.Kills}";
            ts[1].text = $"Total Points: {_stats.TotalPoints}";
            ts[2].text = $"Points Per Kill: {Formatting.DefaultFormat(_stats.PointsPerKill)}";
            var playtime = _stats.TotalPlaytime;
            ts[3].text = $"Total Playtime: {playtime:%d} days {playtime:%h} hours {playtime:%m} minutes {playtime:%s} seconds";
        }
    }
}