using UnityEngine;

namespace Support.Sound
{
    [CreateAssetMenu(menuName = "Data/Support/AudioData", fileName = "NewAudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<UiVfxName, AudioClip> uiVfxClips;
        [SerializeField] private SerializableDictionary<MusicName, AudioClip> musicClips;
        
        public AudioClip GetUiFvx(UiVfxName uiVfxName)
        {
            return uiVfxClips[uiVfxName];
        }
        
        public AudioClip GetMusic(MusicName musicName)
        {
            return musicClips[musicName];
        }
    }
}