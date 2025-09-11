using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DotsKiller.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField, SerializedDictionary("ID", "Source")]
        private SerializedDictionary<AudioID, AudioSource> sources;


        public void PlaySound(AudioID id)
        {
            sources[id].Play();
        }
    }
}