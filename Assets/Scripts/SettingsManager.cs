using System;
using System.Collections;
using System.Runtime.InteropServices;
using DotsKiller.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using Zenject;

namespace DotsKiller
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer musicMixer;
        [SerializeField] private AudioMixer sfxMixer;

        private SaveLoadManager _saveLoadManager;

        public float SfxVolume { get; private set; } = 50;
        public float MusicVolume { get; private set; } = 50;
        public string LanguageCode { get; private set; } = "en"; // Default - english
        public float SecondsElapsedSinceLastSave => _saveLoadManager.SecondsSinceLastSave;

        public event Action ReloadRequested;
        
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern void reloadPage();
#endif


        [Inject]
        public void Initialize(SaveLoadManager saveLoadManager)
        {
            _saveLoadManager = saveLoadManager;
        }
        
        
        private void Start()
        {
            SfxVolume = PlayerPrefs.GetFloat(nameof(SfxVolume), SfxVolume);
            sfxMixer.SetFloat("Volume", RemapVolume(0f, 100f, SfxVolume));

            MusicVolume = PlayerPrefs.GetFloat(nameof(MusicVolume), MusicVolume);
            musicMixer.SetFloat("Volume", RemapVolume(0f, 100f, MusicVolume));
            
            LoadLanguage();
        }
        

        private void LoadLanguage()
        {
            StartCoroutine(WaitAndLoad());
            
            IEnumerator WaitAndLoad()
            {
                yield return LocalizationSettings.InitializationOperation;

                string defaultLanguageCode = LanguageCode;
                LanguageCode = PlayerPrefs.GetString(nameof(LanguageCode), defaultLanguageCode);
                Debug.Log(LanguageCode);
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(LanguageCode);
            }
        }
        
        
        public void ClearSave()
        {
            ReloadRequested?.Invoke();
            
            _saveLoadManager.ClearSave();

            SceneManager.LoadScene("Main"); // seems stable

#if PLATFORM_WEBGL
            Invoke(nameof(reloadPage), 0.5f);
#endif
        }
        

        public void SetSfxVolume(float value)
        {
            SfxVolume = value;
            sfxMixer.SetFloat("Volume", RemapVolume(0f, 100f, SfxVolume));
            
            Save();

            //Debug.Log($"New sfx volume: {SfxVolume}");
        }


        private float RemapVolume(float minA, float maxA, float a)
        {
            float t = Mathf.InverseLerp(minA, maxA, a);
            float output = Mathf.Lerp(0.0001f, 1f, t);
            output = Mathf.Log10(output) * 20f;
            return output;
        }
        
        
        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            musicMixer.SetFloat("Volume", RemapVolume(0f, 100f, MusicVolume));

            Save();

            //Debug.Log($"New music volume: {MusicVolume}");
        }
        
        
        public void SetLanguage(string code)
        {
            LanguageCode = code;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(LanguageCode);

            Save();
        }
        

        public void Save()
        {
            PlayerPrefs.SetFloat(nameof(SfxVolume), SfxVolume);
            PlayerPrefs.SetFloat(nameof(MusicVolume), MusicVolume);
            PlayerPrefs.SetString(nameof(LanguageCode), LanguageCode);
            
            Debug.Log(LanguageCode);
            
            _saveLoadManager.Save();
        }
    }
}