using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller
{
    // Maybe it's smarter to combine all these scripts into one like SelectableWithSound?
    [RequireComponent(typeof(Slider))]
    public class SliderWithSound : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
    {
        [SerializeField] private Slider slider;
                
        private AudioManager _audioManager;


        [Inject]
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        private void OnValidate()
        {
            slider = GetComponent<Slider>();
        }


        private void PlayOnClick()
        {
            _audioManager.PlaySound(AudioID.ButtonClick);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!slider.interactable)
            {
                return;
            }
            
            PlayOnHover();
        }


        public void PlayOnHover()
        {
            _audioManager.PlaySound(AudioID.ButtonHover);
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            PlayOnClick();
        }
    }
}