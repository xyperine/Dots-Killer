using System.Collections.Generic;
using UnityEngine;

namespace DotsKiller.Dots
{
    public class DotsTracker : MonoBehaviour, IRecalibrationTarget, IPurgeTarget
    {
        public List<Dot> ActiveDots { get; private set; } = new List<Dot>();

        public int AmountAlive => ActiveDots.Count;


        public void Register(Dot dot)
        {
            ActiveDots.Add(dot);
        }


        public Dot GetFirstAvailable()
        {
            return ActiveDots[0];
        }
        

        public void Unregister(Dot dot)
        {
            ActiveDots.Remove(dot);
        }


        public void OnPurge()
        {
            Debug.Log("Dots reset (all dead haha)");

            for (int i = ActiveDots.Count - 1; i >= 0; i--)
            {
                ActiveDots[i].Dispose();
                ActiveDots.RemoveAt(i);
            }
        }


        public void OnRecalibration()
        {
            Debug.Log("Recalibration: Dots Tracker");
            
            for (int i = ActiveDots.Count - 1; i >= 0; i--)
            {
                ActiveDots[i].Dispose();
                ActiveDots.RemoveAt(i);
            }
        }
    }
}