using NaughtyAttributes;
using UnityEngine;

namespace Support.Sound
{
    public class AudioManager : MonoBehaviour
    {
        [BoxGroup("References"), Required]
        [SerializeField] private AudioSource uiSfxAudioSource;
        [BoxGroup("References"), Required]
        [SerializeField] private AudioSource musicAudioSource;
        [Space]
        [BoxGroup("Data"), Required, Expandable] 
        [SerializeField] private AudioData audioData;

        private UiSfxName _lastUsedUiSfxName = UiSfxName.None;
        private MusicName _lastUsedMusicName = MusicName.None;

        private void Awake()
        {
            uiSfxAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlayUiSfx(UiSfxName uiSfxName, bool isLooped = false)
        {
            if(uiSfxName == UiSfxName.None)
                return;

            if (uiSfxName == _lastUsedUiSfxName)
            {
                uiSfxAudioSource.Play();
                return;
            }
            
            var uiAudioClip = audioData.GetUiFvx(uiSfxName);

            _lastUsedUiSfxName = uiSfxName;
            uiSfxAudioSource.clip = uiAudioClip;
            uiSfxAudioSource.loop = isLooped;
            uiSfxAudioSource.Play();
        }

        public void StopUiSfx()
        {
            uiSfxAudioSource.Stop();
        }

        public void PlayMusic(MusicName musicName)
        {
            if(musicName == MusicName.None)
                return;

            if (musicName == _lastUsedMusicName)
            {
                musicAudioSource.Play();
                return;
            }
            
            var musicAudioClip = audioData.GetMusic(musicName);

            _lastUsedMusicName = musicName;
            musicAudioSource.clip = musicAudioClip;
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }
    }
}