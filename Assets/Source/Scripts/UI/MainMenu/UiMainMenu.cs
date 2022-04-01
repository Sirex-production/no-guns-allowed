using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using Support;
using Support.Sound;
using TMPro;
using UnityEngine;
using Zenject;

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

        [Inject] private InputSystem _inputSystem;
        [Inject] private AudioManager _audioManager;

        private const float INITIAL_SOUND_DELAY = .3f;
        
        private string _initialMenuTextContent;
        private string _initialCharacterTextContent;

        private void Awake()
        {
            _initialMenuTextContent = menuText.text;
            _initialCharacterTextContent = characterOutputText.text;
        }

        private void Start()
        {
            PlayAppearanceAnimation();
            _inputSystem.OnTouchAction += OnTouch;
        }

        private void OnDestroy()
        {
            _inputSystem.OnTouchAction -= OnTouch;
        }

        private void OnTouch(Vector2 _)
        {
            SkipAppearanceAnimation();
        }

        private void SkipAppearanceAnimation()
        {
            StopAllCoroutines();
            _audioManager.StopUiSfx();
            
            menuText.SetText(_initialMenuTextContent);
            characterOutputText.SetText(_initialCharacterTextContent);
            
            characterSectionCanvasGroup.SetGameObjectActive();
            continueButtonCanvasGroup.SetGameObjectActive();
            missionsButtonCanvasGroup.SetGameObjectActive();
            optionsButtonCanvasGroup.SetGameObjectActive();
            developersButtonCanvasGroup.SetGameObjectActive();
            feedbackButtonCanvasGroup.SetGameObjectActive();
            discordButtonCanvasGroup.SetGameObjectActive();

            characterSectionCanvasGroup.alpha = 1;
            continueButtonCanvasGroup.alpha = 1;
            missionsButtonCanvasGroup.alpha = 1;
            optionsButtonCanvasGroup.alpha = 1;
            developersButtonCanvasGroup.alpha = 1;
            feedbackButtonCanvasGroup.alpha = 1;
            discordButtonCanvasGroup.alpha = 1;
        }

        private void PlayAppearanceAnimation()
        {
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
            
            this.WaitAndDoCoroutine(INITIAL_SOUND_DELAY, ()=>_audioManager.PlayUiSfx(UiSfxName.LettersBeep1, true));
            this.SpawnTextCoroutine(menuText, _initialMenuTextContent, letterSpawnDelayTime, () =>
            {
                _audioManager.StopUiSfx();
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
            characterSectionCanvasGroup.SetGameObjectActive();
            
            var characterTextContent = characterOutputText.text;
            characterOutputText.SetText("");
            
            characterSectionCanvasGroup.DOFade(1, characterSectionFadeAnimationTime)
                .OnComplete(() =>
                {
                    _audioManager.PlayUiSfx(UiSfxName.LettersBeep1, true);
                    this.SpawnTextCoroutine(characterOutputText, characterTextContent, letterSpawnDelayTime, () => _audioManager.StopUiSfx());
                });
        }
    }
}