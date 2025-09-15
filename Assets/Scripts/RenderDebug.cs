using UnityEngine;

namespace DotsKiller
{
    public class RenderDebug : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject field;
        
        
        public void ToggleBackground()
        {
            ToggleGameObject(background);
        }


        public void ToggleField()
        {
            ToggleGameObject(field);
        }


        private void ToggleGameObject(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}