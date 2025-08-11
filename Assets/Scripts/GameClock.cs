using System;
using DotsKiller.SaveSystem;
using UnityEngine;

namespace DotsKiller
{
    public class GameClock : MonoBehaviour
    {
        public DateTime GameStartedAt { get; private set; }
        public DateTime FirstTimePlayedAt { get; private set; }

        public double UnscaledTimeInMilliseconds => DateTime.UtcNow.Subtract(GameStartedAt).TotalMilliseconds;
        public double UnscaledTotalPlaytimeSeconds => DateTime.UtcNow.Subtract(FirstTimePlayedAt).TotalSeconds;


        private void Start()
        {
            GameStartedAt = DateTime.UtcNow;

            if (GameStateHandler.Loaded)
            {
                if (GameStateHandler.State.FirstTimePlayedAt == default)
                {
                    OnFirstLaunch();
                    return;
                }
                
                FirstTimePlayedAt = GameStateHandler.State.FirstTimePlayedAt;
            }
            else
            {
                OnFirstLaunch();
            }
        }


        private void OnFirstLaunch()
        {
            FirstTimePlayedAt = GameStartedAt;
            GameStateHandler.State.FirstTimePlayedAt = FirstTimePlayedAt;
        }
    }
}