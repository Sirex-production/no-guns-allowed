using DG.Tweening;
using Extensions;
using ModestTree;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class UiTutorial : MonoTutorial
    {
        [BoxGroup("References"), Required]
        [SerializeField] private Button targetButton;
        [BoxGroup("References"), Tooltip("Buttons that will be disabled during the active stage of the tutorial")]
        [SerializeField] private Button[] buttonsToDisable; 
        [BoxGroup("References"), Tooltip("Images that is covering other UI elements and focuses attention on the target ui element")]
        [SerializeField] private Image[] focusImages;
        [Space]
        [BoxGroup("Animation properties")]
        [SerializeField] private Color blinkingColor = Color.white;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float blinkingAnimationDuration = 1;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float focusImageFadeTime = .3f;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float focusImageTargetAlpha  = .7f;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float delayBeforeNextTutorial  = 1f;
        [Space]
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is activated")]
        [SerializeField] private string activateLogMessage;
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is completed")]
        [SerializeField] private string completeLogMessage;
        [BoxGroup("Game properties")]
        [SerializeField] private bool deactivateInputSystemOnActivate = false;
        [BoxGroup("Game properties")]
        [SerializeField] private bool activateNextTutorial = true;
        
        [Inject] private TutorialsManager _tutorialsManager;
        [Inject] private InputSystem _inputSystem;
        [Inject] private UiController _uiController;

        private Image _targetButtonImage;
        private Sequence _animationSequence;
        private Color _initialButtonImageColor;

        private void Awake()
        {
            _targetButtonImage = targetButton.image;
            _initialButtonImageColor = _targetButtonImage.color;
            TurnOffFocusImages(true);
        }

        public override void Activate()
        {
            if(deactivateInputSystemOnActivate)
                _inputSystem.Enabled = false;
            
            targetButton.onClick.AddListener(Complete);

            DisableButtons();
            TurnOnFocusImages();
            HighlightButton();
            
            if(activateLogMessage != null || !activateLogMessage.IsEmpty()) 
                _uiController.DisplayLogMessage(activateLogMessage, LogDisplayType.DisplayAndKeep);
        }
        
        public override void Complete()
        {
            EnableButtons();
            TurnOffFocusImages(true);
            
            if(completeLogMessage != null || !completeLogMessage.IsEmpty())
                _uiController.DisplayLogMessage(completeLogMessage, LogDisplayType.DisplayAndClear);
            
            targetButton.onClick.RemoveListener(Complete);
            _animationSequence.Kill();

            this.WaitAndDoCoroutine(delayBeforeNextTutorial, () =>
            {
                if(activateNextTutorial)
                    _tutorialsManager.ActivateNext();
                
                this.SetGameObjectInactive();
            });
        }
        
        private void TurnOffFocusImages(bool isDeactivatedWithoutAnimation)
        {
            if(focusImages == null || focusImages.IsEmpty())
                return;
            
            foreach (var focusImage in focusImages)
            {
                if(focusImage == null)
                    continue;

                if (isDeactivatedWithoutAnimation)
                {
                    var targetImageColor = focusImage.color;
                    targetImageColor.a = 0;
                    focusImage.color = targetImageColor;
                }
                else
                    _targetButtonImage.DOFade(0, focusImageFadeTime);

                focusImage.SetGameObjectInactive();
            }
        }

        private void TurnOnFocusImages()
        {
            if(focusImages == null || focusImages.IsEmpty())
                return;

            foreach (var focusImage in focusImages)
            {
                if(focusImage == null)
                    continue;
                
                focusImage.SetGameObjectActive();
                focusImage.DOFade(focusImageTargetAlpha, focusImageFadeTime);
            }
        }

        private void DisableButtons()
        {
            if(buttonsToDisable == null || buttonsToDisable.IsEmpty())
                return;

            foreach (var buttons in buttonsToDisable)
            {
                if(buttons == null)
                    continue;
                
                buttons.interactable = false;
            }
        }

        private void EnableButtons()
        {
            if(buttonsToDisable == null || buttonsToDisable.IsEmpty())
                return;

            foreach (var buttons in buttonsToDisable)
            {
                if(buttons == null)
                    continue;
                
                buttons.interactable = true;
            }
        }

        private void HighlightButton()
        {
            if (_animationSequence != null)
            {
                _animationSequence.Kill();
                _animationSequence = null;
            }
            
            _animationSequence = DOTween.Sequence()
                .Append(_targetButtonImage.DOColor(blinkingColor, blinkingAnimationDuration / 2))
                .Append(_targetButtonImage.DOColor(_initialButtonImageColor, blinkingAnimationDuration / 2))
                .SetLoops(-1)
                .OnKill(()=> _targetButtonImage.color = _initialButtonImageColor);
        }
    }
}