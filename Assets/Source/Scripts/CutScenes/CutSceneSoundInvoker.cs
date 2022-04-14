using Support.Sound;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class CutSceneSoundInvoker : MonoBehaviour
    {
        [Inject] private readonly AudioManager _audioManager;

        public void PlaySpaceShipFlySound()
        {
            _audioManager.PlaySound(AudioName.environment_space_ship_fly_sound);
        }

        public void PlaySpaceAmbient()
        {
            _audioManager.PlaySound(AudioName.ambient_space);
        }

        public void StopSpaceAmbient()
        {
            _audioManager.StopAllSoundsWithName(AudioName.ambient_space);
        }
    }
}