using UnityEngine;

namespace Support.Sound
{
    [CreateAssetMenu(menuName = "Data/Support/AudioData", fileName = "NewAudioData")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<AudioName, AudioClip> audioClips;

        public AudioClip GetAudioClip(AudioName audioName)
        {
            if(audioClips.Contains(audioName))
                return audioClips[audioName];

            return null;
        }
    }
    
    public enum AudioName
    {
        none,
        ui_button_beep,
        ui_letters_spawn,
        player_dash,
        player_sword_swing,
        environment_container_fall,
        environment_wall_destruction,
        environment_chain_break
    }
}