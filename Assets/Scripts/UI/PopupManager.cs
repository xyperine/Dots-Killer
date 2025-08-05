using System;
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
            backgroundOverlay.SetActive(_activePopup is {activeInHierarchy: true});
        }
    }
    
    
    public enum PopupID
    {
        PurgeReset,
    }
}