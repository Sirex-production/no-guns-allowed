using NaughtyAttributes;
using UnityEngine;


namespace Support.Sound
{
    public class AudioManager : MonoBehaviour
    {
        [BoxGroup("References"), Required]
        [SerializeField] private AudioSource uiVfxAudioSource;
        [BoxGroup("References"), Required]
        [SerializeField] private AudioSource musicAudioSource;
        [Space]
        [BoxGroup("Data"), Required] 
        [SerializeField] private AudioData audioData;

        private UiVfxName _lastUsedUiVfxName = UiVfxName.None;
        private MusicName _lastUsedMusicName = MusicName.None;

        private void Awake()
        {
            uiVfxAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlayUiVfx(UiVfxName uiVfxName)
        {
            if(uiVfxName == UiVfxName.None)
                return;

            if (uiVfxName == _lastUsedUiVfxName)
            {
                uiVfxAudioSource.Play();
                return;
            }
            
            var uiAudioClip = audioData.GetUiFvx(uiVfxName);

            _lastUsedUiVfxName = uiVfxName;
            uiVfxAudioSource.clip = uiAudioClip;
            uiVfxAudioSource.Play();
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
    }
}