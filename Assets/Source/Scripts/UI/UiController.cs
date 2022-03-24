using System;
using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using Support;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class UiController : MonoBehaviour
    {
        [Required] 
        [SerializeField] private UiPlayerDashesController uiDashesController;
        [Required] 
        [SerializeField] private CanvasGroup gameMenuCanvasGroup;
        [Required] 
        [SerializeField] private CanvasGroup bordersCanvasGroup;
        [SerializeField] private Toggle[] uiVibrationOptionToggles;

        [Inject] private GameController _gameController;
        [Inject] private SaveLoadSystem _saveLoadSystem;

        private const float DISPLAY_FADE_ANIMATION_TIME = .1f;
        
        public event Action<string, LogDisplayType> OnLogMessageDisplayed;
        public event Action<Action> OnInteractableButtonShown;
        public event Action OnInteractableButtonHidden;

        public UiPlayerDashesController UiDashesController => uiDashesController;

        private void Start()
        {
            _gameController.OnGameplayStarted += ShowGameplayUi;

            HideGameplayUi();
            SetValuesToVibrationToggles();
        }

        private void OnDestroy()
        {
            _gameController.OnGameplayStarted -= ShowGameplayUi;
        }

        private void ShowGameplayUi()
        {
            gameMenuCanvasGroup.SetGameObjectActive();
            bordersCanvasGroup.SetGameObjectActive();
            
            bordersCanvasGroup.DOFade(1, DISPLAY_FADE_ANIMATION_TIME);
            gameMenuCanvasGroup.DOFade(1, DISPLAY_FADE_ANIMATION_TIME);
        }

        private void HideGameplayUi()
        {
            bordersCanvasGroup.DOFade(0, DISPLAY_FADE_ANIMATION_TIME)
                .OnComplete(() => bordersCanvasGroup.SetGameObjectInactive());;
            gameMenuCanvasGroup.DOFade(0, DISPLAY_FADE_ANIMATION_TIME)
                .OnComplete(() => gameMenuCanvasGroup.SetGameObjectInactive());
        }

        private void SetValuesToVibrationToggles()
        {
            var saveData = _saveLoadSystem.SaveData;
            
            foreach (var toggle in uiVibrationOptionToggles)
            {
                if(toggle == null)
                    continue;
                
                toggle.isOn = saveData.IsVibrationEnabled.Value;
            }
        }

        public void DisplayLogMessage(string message, LogDisplayType logDisplayType)
        {
            OnLogMessageDisplayed?.Invoke(message, logDisplayType);
        }

        public void ShowInteractableButton(Action onClick)
        {
            OnInteractableButtonShown?.Invoke(onClick);
        }

        public void HideInteractableButton()
        {
            OnInteractableButtonHidden?.Invoke();
        }
    }
}