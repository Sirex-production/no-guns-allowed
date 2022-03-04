using UnityEngine;

namespace Support.Sound
{
    [CreateAssetMenu(menuName = "Data/Support/SoundData", fileName = "NewSoundData")]
    public class SoundsData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<string, AudioClip> musicClips;
        [SerializeField] private SerializableDictionary<string, AudioClip> soundClips;

        public AudioClip GetMusic(string id)
        {
            return musicClips[id];
        }
        
        public AudioClip GetSound(string id)
        {
            return soundClips[id];
        }
    }
}