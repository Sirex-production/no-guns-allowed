using System;
using System.Collections;
using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }

        private IEnumerator PlayDialogRoutine(DialogData dialogData)
        {
            var dialogPhrasesPairs = dialogData.Dialog;
            
            foreach (var dialogPhrasesPair in dialogPhrasesPairs)
            {
                if(dialogPhrasesPair == null || string.IsNullOrEmpty(dialogPhrasesPair.phrase))
                    continue;
                
                var phrase = dialogPhrasesPair.phrase;
                var pause = new WaitForSeconds(dialogPhrasesPair.pauseAfterPhrase);
                var isPhraseCompleted = false;
                
                PrintSubtitlesText(phrase, () => isPhraseCompleted = true);

                yield return new WaitUntil( () => isPhraseCompleted);
                yield return pause;
            }
            
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
            
            _modifyHeaderCoroutine = this.SpawnTextCoroutine(headerText, content, headerTextSpawnSpeed);
        }

        public void PrintSubHeaderText(string content)
        {
            if(_modifySubHeaderCoroutine != null)
                StopCoroutine(_modifySubHeaderCoroutine);
            
            subHeaderText.SetText("");    
            
            _modifySubHeaderCoroutine = this.SpawnTextCoroutine(subHeaderText, content, subHeaderTextSpawnSpeed);
        }

        public void PrintSubtitlesText(string content, Action onEnd = null)
        {
            if(_modifySubtitlesCoroutine != null)
                StopCoroutine(_modifySubtitlesCoroutine);

            subtitlesText.SetText("");
            
            _modifySubtitlesCoroutine = this.SpawnTextCoroutine(subtitlesText, content, subtitlesTextSpawnSpeed, onEnd);
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

        public void PlayDialog(DialogData dialogData)
        {
            if (dialogData == null)
                return;

            if (_dialogCoroutine != null)
                StopCoroutine(_dialogCoroutine);

            subtitlesBackgroundImage.DOFade(_initialSubtitlesBackgroundAlpha, displayAnimationDuration / 2)
                .OnComplete(() => _dialogCoroutine = StartCoroutine(PlayDialogRoutine(dialogData)));
        }
    }
}