using System;
using UnityEngine;

namespace DotsKiller.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject tabsPanelObject;
        [SerializeField] private GameObject pagesPanelObject;
        [SerializeField] private bool isOpenByDefault; // debug

        public bool IsOpen { get; private set; }


        private void Awake()
        {
            IsOpen = isOpenByDefault;
        }


        private void Start()
        {
            tabsPanelObject.SetActive(IsOpen);
            pagesPanelObject.SetActive(IsOpen);
        }


        public void Toggle()
        {
            IsOpen = !IsOpen;
            
            tabsPanelObject.SetActive(IsOpen);
            pagesPanelObject.SetActive(IsOpen);
        }
    }
}