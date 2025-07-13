using System.Collections.Generic;
using DotsKiller.Dots;
using UnityEngine;

namespace DotsKiller
{
    public class DotsTracker : MonoBehaviour
    {
        public List<Dot> ActiveDots { get; private set; } = new List<Dot>();

        public int AmountAlive => ActiveDots.Count;


        public void Register(Dot dot)
        {
            ActiveDots.Add(dot);
        }


        public void Unregister(Dot dot)
        {
            ActiveDots.Remove(dot);
        }
    }
}