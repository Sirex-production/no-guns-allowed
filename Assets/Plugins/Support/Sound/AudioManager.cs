using System.Collections.Generic;
using System.Linq;
using ModestTree;
using NaughtyAttributes;
using UnityEngine;

namespace Support.Sound
{
    public sealed class AudioManager : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int numberOfAudioSources;
        [Space]
        [BoxGroup("Data"), Required] //Do not add Expandable attribute. It leads to the inspector bugs 
        [SerializeField] private AudioData audioData;

        private List<AudioPair> _audio;

        private void Awake()
        {
            _audio = new List<AudioPair>(numberOfAudioSources);

            for (int i = 0; i < numberOfAudioSources; i++) 
                _audio.Add(new AudioPair{audioName = AudioName.none, audioSource = gameObject.AddComponent<AudioSource>()});
        }
        
        public void PlaySound(AudioName audioName, bool isLopped = false)
        {
            if(audioName == AudioName.none)
                return;
            
            var audioClip = audioData.GetAudioClip(audioName);
            
            if(audioClip == null)
                return;
            
            var audioPair = _audio.First(pair => !pair.audioSource.isPlaying);
            var audioSource = audioPair.audioSource;
            
            audioPair.audioName = audioName;
            audioSource.clip = audioClip;
            audioSource.loop = isLopped;
            audioSource.Play();
        }

        public void StopAllSoundsWithName(params AudioName[] audioNames)
        {
            if(audioNames == null || audioNames.IsEmpty())
                return;
            
            foreach (var pair in _audio.Where(pair => audioNames.Contains(pair.audioName)))
            {
                var audioSource = pair.audioSource;
                
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
    }

    internal sealed class AudioPair
    {
        public AudioName audioName;
        public AudioSource audioSource;
    }
}