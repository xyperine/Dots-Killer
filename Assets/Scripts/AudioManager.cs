using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DotsKiller
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


    public enum AudioID
    {
        FieldClick,
        DotKill,
        ButtonClick,
        ButtonHover,
        PointsGainedActively,
    }
}