using NaughtyAttributes;
using UnityEngine;

namespace Support.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [Expandable] 
        [SerializeField] private SoundsData soundsData;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlaySound(string id)
        {
            _audioSource.clip = soundsData.GetSound(id);
            _audioSource.Play();
        }
    }
}