using BreakInfinity;
using DotsKiller.SaveSystem;
using UnityEngine;

namespace DotsKiller
{
    public class Stats : MonoBehaviour
    {
        public BigDouble Kills { get; set; } = BigDouble.Zero;
        public int RegularUpgradesBought { get; set; } = 0;


        private void Start()
        {
            if (!GameStateHandler.Loaded)
            {
                return;
            }

            Kills = GameStateHandler.State.Kills;
        }


        private void Update()
        {
            GameStateHandler.State.Kills = Kills;
        }
    }
}