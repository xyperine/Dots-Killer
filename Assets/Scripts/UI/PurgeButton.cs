using BreakInfinity;
using DotsKiller.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller.UI
{
    [DisallowMultipleComponent]
    public class PurgeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private LocalizedString purgeLocalizedString;
        [SerializeField] private LocalizedString shardsLocalizedString;

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
            string purgeText = purgeLocalizedString.GetLocalizedString();
            BigDouble shardsAmount = Formulas.CalculateShardsOnPurge();
            float pluralizationThreshold = 1000f;
            string shardsText =
                shardsLocalizedString.GetLocalizedString(shardsAmount < pluralizationThreshold,
                    shardsAmount.ToDouble());
            if (shardsAmount >= pluralizationThreshold)
            {
                shardsText = Formatting.DefaultFormat(shardsAmount) + " " + shardsText;
            }
            buttonText.text = string.Format(_format, purgeText, shardsText);
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