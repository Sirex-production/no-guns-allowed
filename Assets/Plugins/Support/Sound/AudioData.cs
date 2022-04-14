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
        ui_out_of_charges,
        player_dash,
        player_sword_swing_1,
        environment_container_fall,
        environment_wall_destruction_1,
        environment_chain_break,
        player_sword_swing_2,
        player_sword_swing_3,
        player_sword_swing_4,
        environment_wall_destruction_2,
        environment_wall_destruction_3,
        environment_wall_destruction_4,
        ui_letters_spawn_long_1,
        ui_letters_spawn_long_2,
        ui_letters_spawn_long_3,
        environment_space_ship_fly_sound,
        ambient_space,
        ui_tv_on,
        ui_tv_off
    }
}