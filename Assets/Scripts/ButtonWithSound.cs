using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DotsKiller
{
    [RequireComponent(typeof(Button))]
    public class ButtonWithSound : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button button;
        
        private AudioManager _audioManager;


        [Inject]
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }


        private void OnValidate()
        {
            button = GetComponent<Button>();
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
            if (!button.interactable)
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
            button.onClick.RemoveListener(PlayOnClick);
        }
    }
}