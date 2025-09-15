using UnityEditor;
using UnityEngine;

namespace DotsKiller
{
    public class PlatformBasedMaterialSwitch : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private Material webGLMaterial;
        [SerializeField] private Material pcMaterial;
        [SerializeField] private Material mobileMaterial;


        private void Awake()
        {
            Material material = default;
            
#if UNITY_STANDALONE
            material = pcMaterial;
#elif UNITY_WEBGL
            material = webGLMaterial;
#elif UNITY_ANDROID
            material = mobileMaterial;
#elif UNITY_EDITOR
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                material = pcMaterial;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
            {
                material = webGLMaterial;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                material = mobileMaterial;
            }
#else
            material = renderer.material;
#endif

            renderer.material = material;
        }
    }
}