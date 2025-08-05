using System;
using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class PurgeResetPopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject popupObject;

        private Purge _purge;
        private Action _confirmAction;


        [Inject]
        public void Initialize(Purge purge)
        {
            _purge = purge;
        }
        
        
        public void Show()
        {
            popupObject.SetActive(true);
        }


        public void Confirm()
        {
            _purge.Perform();
            
            popupObject.SetActive(false);
            
            _confirmAction?.Invoke();
            _confirmAction = null;
        }


        public void Cancel()
        {
            popupObject.SetActive(false);
            _confirmAction = null;
        }


        public void SetDoNotShowAgain(bool value)
        {
            int i = value ? 1 : 0;
            PlayerPrefs.SetInt("ShowPurgeResetPopup", i);
        }
    }
}