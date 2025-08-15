using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DotsKiller.UI
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject backgroundOverlay;
        [SerializeField, SerializedDictionary("ID", "Popup")] private SerializedDictionary<PopupID, GameObject> popupsMap;

        private GameObject _activePopup;
        

        public void Show(PopupID id)
        {
            _activePopup = popupsMap[id];
            _activePopup.SetActive(true);
        }


        private void Update()
        {
            backgroundOverlay.SetActive(_activePopup != null && _activePopup.activeInHierarchy);
        }
    }
    
    
    public enum PopupID
    {
        PurgeReset,
        RecalibrationReset,
    }


    public static class PlayerPrefsKeys
    {
        public const string SHOW_PURGE_RESET_POPUP = "ShowPurgeResetPopup";
        public const string SHOW_RECALIBRATION_RESET_POPUP = "ShowRecalibrationResetPopup";
    }
}