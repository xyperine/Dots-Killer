using UnityEngine;
using Zenject;

namespace DotsKiller.AudioSystem
{
    public class FieldAudio : MonoBehaviour
    {
        private AudioManager _audioManager;
        
        
        [Inject]
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }


        public void PlaySound()
        {
            _audioManager.PlaySound(AudioID.FieldClick);
        }
    }
}