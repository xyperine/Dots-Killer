using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleWithSound : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Toggle toggle;
        
        private AudioManager _audioManager;


        [Inject]
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }


        private void OnValidate()
        {
            toggle = GetComponent<Toggle>();
        }


        private void OnEnable()
        {
            toggle.onValueChanged.AddListener(PlayOnClick);
        }


        public void PlayOnClick(bool _)
        {
            _audioManager.PlaySound(AudioID.ButtonClick);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!toggle.interactable)
            {
                return;
            }
            
            PlayOnHover();
        }


        public void PlayOnHover()
        {
            _audioManager.PlaySound(AudioID.ButtonHover);
        }


        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(PlayOnClick);
        }
    }
}