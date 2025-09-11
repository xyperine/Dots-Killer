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
    }
}