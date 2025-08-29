using System;
using System.Collections;
using BreakInfinity;
using DotsKiller.UI.Popups;
using DotsKiller.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
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

        private string _purgeString;
        private string _shardsString;

        private Purge _purge;
        private PopupManager _popupManager;
        private LocalizationAssetsHelper _localizationAssetsHelper;


        [Inject]
        public void Initialize(Purge purge, PopupManager popupManager, LocalizationAssetsHelper localizationAssetsHelper)
        {
            _purge = purge;
            _popupManager = popupManager;
            _localizationAssetsHelper = localizationAssetsHelper;
        }


        private void Awake()
        {
            _format = buttonText.text;
        }


        private void Start()
        {
            purgeLocalizedString.StringChanged += PurgeLocalizedStringOnStringChanged;

            AsyncOperationHandle<string> op = purgeLocalizedString.GetLocalizedStringAsync();

            _localizationAssetsHelper.GetLocalizedAsset(op, s => _purgeString = s);
        }


        private void PurgeLocalizedStringOnStringChanged(string value)
        {
            _purgeString = value;
        }


        private void Update()
        {
            button.interactable = _purge.Available;
            string purgeText = _purgeString;
            BigDouble shardsAmount = Formulas.CalculateShardsOnPurge();
            float pluralizationThreshold = 1000f;
            string shardsText = string.Empty;

            AsyncOperationHandle<string> op = shardsLocalizedString.GetLocalizedStringAsync(shardsAmount < pluralizationThreshold,
                shardsAmount.ToDouble());
            
            _localizationAssetsHelper.GetLocalizedAsset(op, s => shardsText = s);

            if (shardsAmount >= pluralizationThreshold)
            {
                shardsText = Formatting.DefaultFormat(shardsAmount) + " " + shardsText;
            }
            buttonText.text = string.Format(_format, purgeText, shardsText);
        }


        public void Perform()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.SHOW_PURGE_RESET_POPUP) == 1)
            {
                _purge.Perform();
                return;
            }

            _popupManager.Show(PopupID.PurgeReset);
        }


        private void OnDestroy()
        {
            purgeLocalizedString.StringChanged -= PurgeLocalizedStringOnStringChanged;
        }


        // Debug thing
        [Button]
        private void ClearPref()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.SHOW_PURGE_RESET_POPUP);
        }
    }
}