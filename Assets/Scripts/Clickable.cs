using System;
using UnityEngine;

namespace DotsKiller
{
    [RequireComponent(typeof(Collider2D))]
    public class Clickable : MonoBehaviour
    {
        public event Action OnClicked;


        private void OnMouseDown()
        {
            Click();
        }


        public void Click()
        {
            OnClicked?.Invoke();
        }
    }
}