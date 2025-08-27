using UnityEngine;
using Zenject;

namespace DotsKiller.UI.Popups
{
    public class RecalibrationResetPopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject popupObject;

        private Recalibration _recalibration;


        [Inject]
        public void Initialize(Recalibration recalibration)
        {
            _recalibration = recalibration;
        }
        
        
        public void Show()
        {
            popupObject.SetActive(true);
        }


        public void Confirm()
        {
            _recalibration.Perform();
            
            popupObject.SetActive(false);
        }


        public void Cancel()
        {
            popupObject.SetActive(false);
        }


        public void SetDoNotShowAgain(bool value)
        {
            int i = value ? 1 : 0;
            PlayerPrefs.SetInt(PlayerPrefsKeys.SHOW_RECALIBRATION_RESET_POPUP, i);
        }
    }
}