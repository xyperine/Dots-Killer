using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DotsKiller.UI
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private Button button;


        private void Start()
        {
            bool onDesktop = Application.platform is RuntimePlatform.WindowsPlayer;
            bool showButton = onDesktop;
#if UNITY_EDITOR
            bool inEditor = Application.isEditor &&
                            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64; // the only supported standalone os now
            showButton |= inEditor;
#endif
            button.gameObject.SetActive(showButton);
        }


        public void Exit()
        {
            Application.Quit();
        }
    }
}