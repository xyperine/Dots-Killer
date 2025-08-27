using DotsKiller.UI.Popups;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private LanguageButtons languageButtons;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private LocalizeStringEvent lastSavedLse;
        
        private SettingsManager _settingsManager;
        private PopupManager _popupManager;


        [Inject]
        public void Initialize(PopupManager popupManager, SettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _popupManager = popupManager;
        }
        

        private void OnEnable()
        {
            string languageCode = _settingsManager.LanguageCode;
            languageButtons.SetActive(languageCode);
        }


        public void SetSfxVolume(float value)
        {
            _settingsManager.SetSfxVolume(value);
        }


        public void SetMusicVolume(float value)
        {
            _settingsManager.SetMusicVolume(value);
        }


        public void Save()
        {
            _settingsManager.Save();
        }


        public void ClearSave()
        {
            _popupManager.Show(PopupID.ClearSave);
        }


        public void SetLanguage(string code)
        {
            _settingsManager.SetLanguage(code);
            
            languageButtons.SetActive(code);
        }


        private void Update()
        {
            sfxSlider.value = _settingsManager.SfxVolume;
            musicSlider.value = _settingsManager.MusicVolume;

            LocalizedString localizedString = lastSavedLse.StringReference;
            ((StringVariable) localizedString["seconds"]).Value =
                _settingsManager.SecondsElapsedSinceLastSave.ToString("F2");
        }
    }
}