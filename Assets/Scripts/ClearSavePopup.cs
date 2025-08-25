using DotsKiller.SaveSystem;
using UnityEngine;
using Zenject;

namespace DotsKiller
{
    public class ClearSavePopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupObject;

        private SettingsManager _settingsManager;


        [Inject]
        public void Initialize(SettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        
        
        public void Show()
        {
            popupObject.SetActive(true);
        }


        public void Confirm()
        {
            _settingsManager.ClearSave();
            
            popupObject.SetActive(false);
        }


        public void Cancel()
        {
            popupObject.SetActive(false);
        }
    }
}