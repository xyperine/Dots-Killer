using System;
using UnityEngine;

namespace DotsKiller.SaveSystem
{
    public class GameStateHandler : MonoBehaviour
    {
        public static GameState State { get; private set; }
        public static bool Loaded { get; private set; }


        private void Awake()
        {
            State ??= new GameState();
        }


        public void Clear()
        {
            State = new GameState();
            Loaded = false;
        }


        public static void Load(GameState state)
        {
            State = state;
            Loaded = true;
        }


        private void OnDestroy()
        {
            State = null;
            Loaded = false;
        }
    }
}