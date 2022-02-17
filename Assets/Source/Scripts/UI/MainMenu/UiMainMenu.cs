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
        [BoxGroup("References")] [SerializeField] private CanvasGroup continueButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup missionsButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup optionsButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup developersButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup feedbackButtonCanvasGroup;
        [BoxGroup("References")] [SerializeField] private CanvasGroup discordButtonCanvasGroup;
        
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
            continueButtonCanvasGroup.alpha = 0;
            missionsButtonCanvasGroup.alpha = 0;
            optionsButtonCanvasGroup.alpha = 0;
            developersButtonCanvasGroup.alpha = 0;
            feedbackButtonCanvasGroup.alpha = 0;
            discordButtonCanvasGroup.alpha = 0;
            
            continueButtonCanvasGroup.SetGameObjectInactive();
            missionsButtonCanvasGroup.SetGameObjectInactive();
            optionsButtonCanvasGroup.SetGameObjectInactive();
            developersButtonCanvasGroup.SetGameObjectInactive();
            feedbackButtonCanvasGroup.SetGameObjectInactive();
            discordButtonCanvasGroup.SetGameObjectInactive();
            
            this.SpawnTextCoroutine(menuText, menuTextContent, letterSpawnDelayTime, () =>
            {
                ShowButtons();
                ShowCharacterText();
            });
        }
        

        private void ShowButtons()
        {
            continueButtonCanvasGroup.SetGameObjectActive();
            missionsButtonCanvasGroup.SetGameObjectActive();
            optionsButtonCanvasGroup.SetGameObjectActive();
            developersButtonCanvasGroup.SetGameObjectActive();
            feedbackButtonCanvasGroup.SetGameObjectActive();
            discordButtonCanvasGroup.SetGameObjectActive();
            
            continueButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime);
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons, () => missionsButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime));
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons * 2, () => optionsButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime));
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons * 3, () => developersButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime));
            this.WaitAndDoCoroutine(pauseBetweenFadingButtons * 4, () =>
            {
                feedbackButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime);
                discordButtonCanvasGroup.DOFade(1, buttonFadeAnimationTime);
            });
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