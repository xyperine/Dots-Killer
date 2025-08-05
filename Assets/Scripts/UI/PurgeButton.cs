using DotsKiller.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller.UI
{
    [DisallowMultipleComponent]
    public class PurgeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;

        private string _format;

        private Purge _purge;
        private PopupManager _popupManager;


        [Inject]
        public void Initialize(Purge purge, PopupManager popupManager)
        {
            _purge = purge;
            _popupManager = popupManager;
        }


        private void Awake()
        {
            _format = buttonText.text;
        }


        private void Update()
        {
            button.interactable = _purge.Available;
            buttonText.text = string.Format(_format, Formatting.DefaultFormat(Formulas.CalculateShardsOnPurge()));
        }


        public void Perform()
        {
            if (PlayerPrefs.GetInt("ShowPurgeResetPopup") == 1)
            {
                _purge.Perform();
                return;
            }

            _popupManager.Show(PopupID.PurgeReset);
        }


        // Debug thing
        [Button]
        private void ClearPref()
        {
            PlayerPrefs.DeleteKey("ShowPurgeResetPopup");
        }
    }
}