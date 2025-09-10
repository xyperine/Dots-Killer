using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller
{
    public class ButtonWithSound : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button button;
        
        private AudioManager _audioManager;


        [Inject]
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }


        private void OnEnable()
        {
            button.onClick.AddListener(PlayOnClick);
        }


        public void PlayOnClick()
        {
            _audioManager.PlaySound(AudioID.ButtonClick);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayOnHover();
        }


        public void PlayOnHover()
        {
            _audioManager.PlaySound(AudioID.ButtonHover);
        }


        private void OnDisable()
        {
            button.onClick.RemoveListener(PlayOnClick);
        }
    }
}