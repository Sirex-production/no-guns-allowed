using UnityEngine;

namespace Support.Sound
{
    [CreateAssetMenu(menuName = "Data/Support/AudioData", fileName = "NewAudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<UiSfxName, AudioClip> uiVfxClips;
        [SerializeField] private SerializableDictionary<MusicName, AudioClip> musicClips;
        
        public AudioClip GetUiFvx(UiSfxName uiSfxName)
        {
            return uiVfxClips[uiSfxName];
        }
        
        public AudioClip GetMusic(MusicName musicName)
        {
            return musicClips[musicName];
        }
    }
}