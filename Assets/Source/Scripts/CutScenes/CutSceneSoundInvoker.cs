using Ingame.Sound;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class CutSceneSoundInvoker : MonoBehaviour
    {
        [Inject] private readonly LegacyAudioManager _legacyAudioManager;

        public void PlaySpaceShipFlySound()
        {
            _legacyAudioManager.PlaySound(AudioName.environment_space_ship_fly_sound);
        }

        public void PlaySpaceAmbient()
        {
            _legacyAudioManager.PlaySound(AudioName.ambient_space);
        }

        public void StopSpaceAmbient()
        {
            _legacyAudioManager.StopAllSoundsWithName(AudioName.ambient_space);
        }
    }
}