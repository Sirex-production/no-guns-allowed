using DG.Tweening;
using Extensions;
using MoreMountains.NiceVibrations;
using Support;
using TMPro;
using UnityEngine;

namespace Ingame.UI
{
    public class UiAfterLevelMenu : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float animationDuration = 1;
        [Space]
        [SerializeField] private CanvasGroup winScreenParentCanvas;
        [SerializeField] private TMP_Text winText;
        [Space]
        [SerializeField] private CanvasGroup looseScreenParentCanvas;
        [SerializeField] private TMP_Text loseText;

        private const float TEXT_ANIMATING_SCALE_MODIFIER = 1.2f;
        
        private Vector3 _initialWinTextScale;
        private Vector3 _initialLooseTextScale;

        private Sequence _animationSequence;

        private void Awake()
        {
            if(winText != null)
                _initialWinTextScale = winText.rectTransform.localScale;
            if(loseText != null)
                _initialLooseTextScale = loseText.rectTransform.localScale;
            
            winScreenParentCanvas.SetGameObjectInactive();
            looseScreenParentCanvas.SetGameObjectInactive();
        }

        private void Start()
        {
            GameController.Instance.OnLevelEnded += PlayAfterLevelMenuAnimation;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnLevelEnded -= PlayAfterLevelMenuAnimation;
            
            if(_animationSequence != null)
                _animationSequence.Kill();
        }

        private void PlayAfterLevelMenuAnimation(bool isVictory)
        {
            if(isVictory)
                ShowWinScreen();
            else
                ShowLooseScreen();
        }

        private void ShowWinScreen()
        {
            winScreenParentCanvas.alpha = 0;
            winText.rectTransform.localScale = Vector3.zero;
            winScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(winScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.Success)))
                .Append(winText.rectTransform.DOScale(_initialWinTextScale * TEXT_ANIMATING_SCALE_MODIFIER, animationDuration / 2))
                .Append(winText.rectTransform.DOScale(_initialWinTextScale, animationDuration));

        }

        private void ShowLooseScreen()
        {
            looseScreenParentCanvas.alpha = 0;
            loseText.rectTransform.localScale = Vector3.zero;
            looseScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(looseScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.Failure)))
                .Append(loseText.rectTransform.DOScale(_initialLooseTextScale * TEXT_ANIMATING_SCALE_MODIFIER, animationDuration / 2))
                .Append(loseText.rectTransform.DOScale(_initialLooseTextScale, animationDuration));
        }
    }
}