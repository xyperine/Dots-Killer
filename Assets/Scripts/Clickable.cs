using System;
using UnityEngine;

namespace DotsKiller
{
    [RequireComponent(typeof(Collider2D))]
    public class Clickable : MonoBehaviour
    {
        public event Action OnClicked;
        

        public void Click()
        {
            Debug.Log("I'm clicked");

            OnClicked?.Invoke();
        }
    }
}