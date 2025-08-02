using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class PurgeUI : MonoBehaviour
    {
        [SerializeField] private GameObject prestigeProgressBarObject;
        [SerializeField] private GameObject prestigeButtonObject;

        private Purge _purge;


        [Inject]
        public void Initialize(Purge purge)
        {
            _purge = purge;
        }
        

        private void Update()
        {
            prestigeProgressBarObject.SetActive(!_purge.Available);
            prestigeButtonObject.SetActive(_purge.Available);
        }
    }
}