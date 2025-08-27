using UnityEngine;
using Zenject;

namespace DotsKiller.UI.Popups
{
    public class PurgeResetPopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject popupObject;

        private Purge _purge;


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
        }


        public void Cancel()
        {
            popupObject.SetActive(false);
        }


        public void SetDoNotShowAgain(bool value)
        {
            int i = value ? 1 : 0;
            PlayerPrefs.SetInt(PlayerPrefsKeys.SHOW_PURGE_RESET_POPUP, i);
        }
    }
}