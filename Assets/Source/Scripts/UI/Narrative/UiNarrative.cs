using System;
using System.Collections;
using DG.Tweening;
using Ingame.Sound;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UiNarrative : MonoBehaviour
    {
        [BoxGroup("References"), Required] 
        [SerializeField] private CanvasGroup parentCanvasGroup;
        [BoxGroup("References"), Required] 
        [SerializeField] private Image subtitlesBackgroundImage;
        [BoxGroup("References"), Required]
        [SerializeField] private TMP_Text headerText;
        [BoxGroup("References"), Required]
        [SerializeField] private TMP_Text subHeaderText;
        [BoxGroup("References"), Required]
        [SerializeField] private TMP_Text subtitlesText;
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup skipButtonCanvasGroup;
        [Space]
        [BoxGroup("Animation settings")]
        [SerializeField] [Range(0, 1f)] private float displayAnimationDuration = .1f;
        [Space]
        [BoxGroup("Animation settings")]
        [SerializeField] [Range(0, 1f)] private float headerTextSpawnSpeed = .5f;
        [BoxGroup("Animation settings")]
        [SerializeField] [Range(0, 1f)] private float subHeaderTextSpawnSpeed = .5f;
        [BoxGroup("Animation settings")]
        [SerializeField] [Range(0, 1f)] private float subtitlesTextSpawnSpeed = .5f;

        [Inject] private readonly GameController _gameController;
        [Inject] private readonly LegacyAudioManager _legacyAudioManager;

        private float _initialSubtitlesBackgroundAlpha;

        private Coroutine _modifyHeaderCoroutine;
        private Coroutine _modifySubHeaderCoroutine;
        private Coroutine _modifySubtitlesCoroutine;
        private Coroutine _dialogCoroutine;

        private void Awake()
        {
            _initialSubtitlesBackgroundAlpha = subtitlesBackgroundImage.color.a;
            
            headerText.SetText("");
            subHeaderText.SetText("");
            subtitlesText.SetText("");
        }

        private void Start()
        {
            subtitlesBackgroundImage.DOFade(0, 0);
            skipButtonCanvasGroup.alpha = 0;
            
            skipButtonCanvasGroup.SetGameObjectInactive();

            _gameController.OnLevelLoaded += OnLevelLoaded;
        }

        private void OnDestroy()
        {
            _gameController.OnLevelLoaded -= OnLevelLoaded;
        }

        private void OnLevelLoaded(int _)
        {
            this.SetGameObjectInactive();
        }

        private IEnumerator PlayDialogRoutine(DialogData dialogData, Action onComplete, Action onLetterSpawned)
        {
            var dialogPhrasesPairs = dialogData.Dialog;
            foreach (var dialogPhrasesPair in dialogPhrasesPairs)
            {
                if(dialogPhrasesPair == null || string.IsNullOrEmpty(dialogPhrasesPair.phrase))
                    continue;
                
                _legacyAudioManager.PlayRandomizedSound
                (
                    true,
                    AudioName.ui_letters_spawn_long_1,
                    AudioName.ui_letters_spawn_long_2,
                    AudioName.ui_letters_spawn_long_3
                );
                
                var phrase = dialogPhrasesPair.phrase;
                var pause = new WaitForSeconds(dialogPhrasesPair.pauseAfterPhrase);
                var isPhraseCompleted = false;
                
                PrintSubtitlesText(phrase, () => isPhraseCompleted = true, onLetterSpawned);

                yield return new WaitUntil(() => isPhraseCompleted);
                
                _legacyAudioManager.StopAllSoundsWithName
                (
                    AudioName.ui_letters_spawn_long_1,
                    AudioName.ui_letters_spawn_long_2,
                    AudioName.ui_letters_spawn_long_3
                );
                
                yield return pause;
            }
            
            onComplete?.Invoke();
            PrintSubtitlesText(" ");
            subtitlesBackgroundImage.DOFade(0, displayAnimationDuration / 2);
        }

        public void ShowNarrativeSection()
        {
            this.SetGameObjectActive();
            
            parentCanvasGroup.alpha = 0;
            parentCanvasGroup.DOFade(1, displayAnimationDuration);
        }

        public void HideNarrativeSection()
        {
            parentCanvasGroup.DOFade(0, displayAnimationDuration)
                .OnComplete(this.SetGameObjectInactive);
        }

        public void PrintHeaderText(string content)
        {
            if(_modifyHeaderCoroutine != null)
                StopCoroutine(_modifyHeaderCoroutine);
            
            headerText.SetText("");
            _legacyAudioManager.PlayRandomizedSound(true, AudioName.ui_letters_spawn_long_1);
            
            _modifyHeaderCoroutine = this.SpawnTextCoroutine(headerText, content, headerTextSpawnSpeed, 
                () => _legacyAudioManager.StopAllSoundsWithName(AudioName.ui_letters_spawn_long_1));
        }

        public void PrintSubHeaderText(string content)
        {
            if(_modifySubHeaderCoroutine != null)
                StopCoroutine(_modifySubHeaderCoroutine);
            
            subHeaderText.SetText("");
            _legacyAudioManager.PlayRandomizedSound(true, AudioName.ui_letters_spawn_long_2);
            
            _modifySubHeaderCoroutine = this.SpawnTextCoroutine(subHeaderText, content, subHeaderTextSpawnSpeed, 
                () => _legacyAudioManager.StopAllSoundsWithName(AudioName.ui_letters_spawn_long_2));
        }

        public void PrintSubtitlesText(string content, Action onEnd = null, Action onLetterSpawned = null)
        {
            if(_modifySubtitlesCoroutine != null)
                StopCoroutine(_modifySubtitlesCoroutine);

            subtitlesText.SetText("");
            
            _modifySubtitlesCoroutine = this.SpawnTextCoroutine(subtitlesText, content, subtitlesTextSpawnSpeed, onEnd, onLetterSpawned);
        }

        public void HideHeaderText()
        {
            if(_modifyHeaderCoroutine != null)
                StopCoroutine(_modifyHeaderCoroutine);
            
            _modifyHeaderCoroutine = this.DeleteTextLetterByLetterCoroutine(headerText, headerTextSpawnSpeed);
        }
        
        public void HideSubHeaderText()
        {
            if(_modifySubHeaderCoroutine != null)
                StopCoroutine(_modifySubHeaderCoroutine);
            
            _modifySubHeaderCoroutine = this.DeleteTextLetterByLetterCoroutine(subHeaderText, subHeaderTextSpawnSpeed);
        }

        public void PlayDialog(DialogData dialogData, Action onComplete = null, Action onLetterSpawned = null)
        {
            if (dialogData == null)
                return;

            if (_dialogCoroutine != null)
                StopCoroutine(_dialogCoroutine);

            subtitlesBackgroundImage.DOFade(_initialSubtitlesBackgroundAlpha, displayAnimationDuration / 2)
                .OnComplete(() => _dialogCoroutine = StartCoroutine(PlayDialogRoutine(dialogData, onComplete, onLetterSpawned)));
        }

        public void ShowSkipButton()
        {
            skipButtonCanvasGroup.SetGameObjectActive();
            skipButtonCanvasGroup.DOFade(1, displayAnimationDuration);
        }

        public void HideSkipButton()
        {
            skipButtonCanvasGroup.DOFade(0, displayAnimationDuration)
                .OnComplete(skipButtonCanvasGroup.SetGameObjectInactive);
        }
    }
}