using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Ingame.UI
{
    public class UiMainMenu : MonoBehaviour
    {
        [BoxGroup("References")] [SerializeField] private TMP_Text menuText;
        [Space]
        [BoxGroup("References")] [SerializeField] private CanvasGroup characterSectionCanvasGroup;
        [BoxGroup("References")] [SerializeField] private TMP_Text characterOutputText;
        [Space]
        [BoxGroup("References")] [SerializeField] private CanvasGroup startButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup optionsButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup developersButtonCanvasGroup;
        
        [BoxGroup("Animation settings")] [SerializeField] [Min(0)] private float characterSectionFadeAnimationTime = .5f;
        [Space]
        [BoxGroup("Animation settings")] [SerializeField] [Min(0)] private float letterSpawnDelayTime = .01f;
        [BoxGroup("Animation settings")] [SerializeField] [Min(0)] private float buttonFadeAnimationTime = .5f;
        [BoxGroup("Animation settings")] [SerializeField] [Min(0)] private float pauseBetweenFadingButtons = .5f;

        private void Start()
        {
            PlayAppearanceAnimation();
        }

        private void PlayAppearanceAnimation()
        {
            var menuTextContent = menuText.text;

            menuText.SetText("");
            characterSectionCanvasGroup.alpha = 0;
            startButtonCanvasGroup.alpha = 0;
            optionsButtonCanvasGroup.alpha = 0;
            developersButtonCanvasGroup.alpha = 0;
            
            startButtonCanvasGroup.SetGameObjectInactive();
            optionsButtonCanvasGroup.SetGameObjectInactive();
            developersButtonCanvasGroup.SetGameObjectInactive();
            
            this.SpawnTextCoroutine(menuText, menuTextContent, letterSpawnDelayTime, () =>
            {
                ShowButtons();
                ShowCharacterText();
            });
        }
        

        private void ShowButtons()
        {
            startButtonCanvasGroup.SetGameObjectActive();
            optionsButtonCanvasGroup.SetGameObjectActive();
            developersButtonCanvasGroup.SetGameObjectActive();
            
            startButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime);
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons, () => optionsButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime));
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons * 2, () => developersButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime));
        }

        private void ShowCharacterText()
        {
            var characterTextContent = characterOutputText.text;
            characterOutputText.SetText("");
            
            characterSectionCanvasGroup.DOFade(1, characterSectionFadeAnimationTime)
                .OnComplete(() => this.SpawnTextCoroutine(characterOutputText, characterTextContent, letterSpawnDelayTime));
        }
    }
}