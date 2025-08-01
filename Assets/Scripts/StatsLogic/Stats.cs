using System;
using BreakInfinity;
using UnityEngine;

namespace DotsKiller.StatsLogic
{
    public class Stats : MonoBehaviour
    {
        public BigDouble Kills { get; set; } = BigDouble.Zero;
        public BigDouble PointsPerKill { get; set; } = BigDouble.One;
        public BigDouble TotalPoints { get; set; } = BigDouble.Zero;
        public TimeSpan TotalPlaytime { get; set; } = TimeSpan.Zero;
        
        public int RegularUpgradesBought { get; set; } = 0;
    }
}