using UnityEngine;

namespace DotsKiller.UI
{
    public class ToggleMenuButton : MonoBehaviour
    {
        [SerializeField] private MenuUI menuUI;
        [SerializeField] private Transform closedPoint;
        [SerializeField] private Transform openPoint;
        [SerializeField] private Transform imageTransform;


        private void Start()
        {
            if (menuUI.IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }


        public void Toggle()
        {
            menuUI.Toggle();

            if (menuUI.IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
        
        
        public void Open()
        {
            transform.SetParent(openPoint, false);
            imageTransform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }


        public void Close()
        {
            transform.SetParent(closedPoint, false);
            imageTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}