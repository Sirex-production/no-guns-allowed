using System;
using UnityEngine;

namespace Ingame.Sound
{
    [CreateAssetMenu(menuName = "Data/Support/AudioData", fileName = "NewAudioData")]
    public class LegacyAudioData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<AudioName, LegacyAudioClipSettings> audioClips;

        public LegacyAudioClipSettings GetAudioClip(AudioName audioName)
        {
            if(audioClips.Contains(audioName))
                return audioClips[audioName];

            return null;
        }
    }

    [Serializable]
    public class LegacyAudioClipSettings
    {
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
        [Range(-3, 3)]
        public float pitch = 1;
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
        ui_tv_off,
        ui_detection
    }
}