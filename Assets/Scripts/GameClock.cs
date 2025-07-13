using System;
using UnityEngine;

namespace DotsKiller
{
    public class GameClock : MonoBehaviour
    {
        public DateTime GameStartedAt { get; private set; }

        public double UnscaledTimeInMilliseconds => DateTime.UtcNow.Subtract(GameStartedAt).TotalMilliseconds;


        private void Start()
        {
            GameStartedAt = DateTime.UtcNow;
        }
    }
}