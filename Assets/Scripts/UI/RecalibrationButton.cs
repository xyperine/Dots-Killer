using System;
using System.Collections;
using DotsKiller.UI.Popups;
using DotsKiller.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller.UI
{
    [DisallowMultipleComponent]
    public class RecalibrationButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private LocalizedStringTable recalibrationTable;

        private string _format;

        private const string MULT_TABLE_ENTRY_KEY = "Recalibration.Button.Mult";
        private const string EXP_TABLE_ENTRY_KEY = "Recalibration.Button.Exp";

        private StringTable _table;

        private Recalibration _recalibration;
        private PopupManager _popupManager;
        private LocalizationAssetsHelper _localizationAssetsHelper;


        [Inject]
        public void Initialize(Recalibration recalibration, PopupManager popupManager, LocalizationAssetsHelper localizationAssetsHelper)
        {
            _recalibration = recalibration;
            _popupManager = popupManager;
            _localizationAssetsHelper = localizationAssetsHelper;
        }


        private void Awake()
        {
            _format = buttonText.text;
        }


        private void Start()
        {
            recalibrationTable.TableChanged += RecalibrationTableOnTableChanged;

            AsyncOperationHandle<StringTable> op = recalibrationTable.GetTableAsync();
            
            _localizationAssetsHelper.GetLocalizedAsset(op, table => _table = table);
        }


        private void RecalibrationTableOnTableChanged(StringTable value)
        {
            _table = value;
        }


        private void Update()
        {
            button.interactable = _recalibration.Available;
            button.gameObject.SetActive(_recalibration.Available);
            
            if (_table != null)
            {
                buttonText.text = string.Format(_format,
                    _table.GetEntry(MULT_TABLE_ENTRY_KEY).Value,
                    Formatting.DefaultFormat(_recalibration.CurrentMultiplier),
                    Formatting.DefaultFormat(_recalibration.NextMultiplier),
                    _table.GetEntry(EXP_TABLE_ENTRY_KEY).Value,
                    Formatting.DefaultFormat(_recalibration.CurrentExponent),
                    Formatting.DefaultFormat(_recalibration.NextExponent));
            }
        }


        public void Perform()
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.SHOW_RECALIBRATION_RESET_POPUP) == 1)
            {
                _recalibration.Perform();
            }
            else
            {
                _popupManager.Show(PopupID.RecalibrationReset);
            }
        }
        
        
        private void OnDestroy()
        {
            recalibrationTable.TableChanged -= RecalibrationTableOnTableChanged;
        }


        // Debug thing
        [Button]
        private void ClearPref()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.SHOW_RECALIBRATION_RESET_POPUP);
        }
    }
}