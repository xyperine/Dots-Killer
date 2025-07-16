using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Dots
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