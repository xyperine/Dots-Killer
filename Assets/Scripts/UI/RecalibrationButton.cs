using DotsKiller.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
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

        private Recalibration _recalibration;
        private PopupManager _popupManager;


        [Inject]
        public void Initialize(Recalibration recalibration, PopupManager popupManager)
        {
            _recalibration = recalibration;
            _popupManager = popupManager;
        }


        private void Awake()
        {
            _format = buttonText.text;
        }


        private void Update()
        {
            button.interactable = _recalibration.Available;
            button.gameObject.SetActive(_recalibration.Available);

            StringTable table = recalibrationTable.GetTable();
            buttonText.text = string.Format(_format,
                table.GetEntry(MULT_TABLE_ENTRY_KEY).Value,
                Formatting.DefaultFormat(_recalibration.CurrentMultiplier),
                Formatting.DefaultFormat(_recalibration.NextMultiplier),
                table.GetEntry(EXP_TABLE_ENTRY_KEY).Value,
                Formatting.DefaultFormat(_recalibration.CurrentExponent),
                Formatting.DefaultFormat(_recalibration.NextExponent));
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
        
        
        // Debug thing
        [Button]
        private void ClearPref()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.SHOW_RECALIBRATION_RESET_POPUP);
        }
    }
}