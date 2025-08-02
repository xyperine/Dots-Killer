using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller
{
    [DisallowMultipleComponent]
    public class PurgeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private Purge _purge;


        [Inject]
        public void Initialize(Purge purge)
        {
            _purge = purge;
        }


        private void Update()
        {
            button.interactable = _purge.Available;
        }


        public void Perform()
        {
            _purge.Perform();
        }
    }
}