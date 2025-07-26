using UnityEngine;

namespace DotsKiller
{
    public class FramerateSetter : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool workInEditor;
#endif
        [SerializeField, Min(0)] private int targetFramerate = 60;
        
        
        private void Awake()
        {
            SetFramerate();
        }


        private void SetFramerate()
        {
#if UNITY_EDITOR
            if (!workInEditor)
            {
                return;
            }
#endif
            Application.targetFrameRate = targetFramerate;
        }
    }
}