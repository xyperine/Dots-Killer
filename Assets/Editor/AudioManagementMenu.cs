using DotsKiller.AudioSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.Editor
{
    public static class AudioManagementMenu
    {
        [MenuItem("Tools/Audio Management/Add Audio To All Buttons On Scene")]
        private static void AddAudioToAllButtonsOnScene()
        {
            Button[] buttonsOnScene = GameObject.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (Button button in buttonsOnScene)
            {
                if (!button.TryGetComponent(out ButtonWithSound _))
                {
                    Undo.AddComponent<ButtonWithSound>(button.gameObject);
                }
            }
        }
        
        
        [MenuItem("Tools/Audio Management/Add Audio To All Toggles On Scene")]
        private static void AddAudioToAllTogglesOnScene()
        {
            Toggle[] togglesOnScene = GameObject.FindObjectsByType<Toggle>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (Toggle toggle in togglesOnScene)
            {
                if (!toggle.TryGetComponent(out ToggleWithSound _))
                {
                    Undo.AddComponent<ToggleWithSound>(toggle.gameObject);
                }
            }
        }
        
        
        [MenuItem("Tools/Audio Management/Add Audio To All Sliders On Scene")]
        private static void AddAudioToAllSlidersOnScene()
        {
            Slider[] slidersOnScene = GameObject.FindObjectsByType<Slider>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (Slider slider in slidersOnScene)
            {
                if (!slider.TryGetComponent(out SliderWithSound _))
                {
                    Undo.AddComponent<SliderWithSound>(slider.gameObject);
                }
            }
        }
    }
}