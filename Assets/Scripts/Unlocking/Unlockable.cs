using UnityEngine;
using Zenject;

namespace DotsKiller.Unlocking
{
    public class Unlockable : MonoBehaviour
    {
        [field: SerializeField] public UnlockableID ID { get; private set; }
        [SerializeField] private GameObject lockedObject;
        [SerializeField] private bool unlocked;

        private UnlockablesManager _unlockablesManager;
        
        
        [Inject]
        public void Initialize(UnlockablesManager unlockablesManager)
        {
            _unlockablesManager = unlockablesManager;
        }
        

        private void Start()
        {
            _unlockablesManager.Register(this);
            lockedObject.SetActive(unlocked);
        }


        public void Unlock()
        {
            unlocked = true;
            lockedObject.SetActive(true);
        }


        public void Lock()
        {
            unlocked = false;
            lockedObject.SetActive(false);
        }
    }
}