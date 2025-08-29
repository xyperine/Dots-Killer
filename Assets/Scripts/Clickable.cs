using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DotsKiller
{
    [RequireComponent(typeof(Collider2D))]
    public class Clickable : MonoBehaviour
    {
        [SerializeField] private Collider2D clickCollider;
        
        public event Action OnClicked;


        private void Update()
        {
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.value);
                if (clickCollider.OverlapPoint(clickPosition))
                {
                    Click();
                }
            }
        }
        

        public void Click()
        {
            Debug.Log("I'm clicked");

            OnClicked?.Invoke();
        }
    }
}