using UnityEngine;
using Zenject;

namespace DotsKiller.UI
{
    public class GameOverUI : MonoBehaviour, IGameOverTarget
    {
        private PopupManager _popupManager;

        
        [Inject]
        public void Initialize(PopupManager popupManager)
        {
            _popupManager = popupManager;
        }


        public void OnGameOver()
        {
            _popupManager.Show(PopupID.EndReached);
        }
    }
}