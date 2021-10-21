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
        [SerializeField] private TMP_Text looseText;

        private const float TEXT_ANIMATIN_SCALE_MODIFIER = 1.2f;
        
        private Vector3 _initialWinTextScale;
        private Vector3 _initialLooseTextScale;

        private Sequence _animationSequence;

        private void Awake()
        {
            _initialWinTextScale = winText.rectTransform.localScale;
            _initialLooseTextScale = looseText.rectTransform.localScale;
            
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

        public void ShowWinScreen()
        {
            winScreenParentCanvas.alpha = 0;
            winText.rectTransform.localScale = Vector3.zero;
            winScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(winScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.SoftImpact)))
                .Append(winText.rectTransform.DOScale(_initialWinTextScale * TEXT_ANIMATIN_SCALE_MODIFIER, animationDuration / 2))
                .Append(winText.rectTransform.DOScale(_initialWinTextScale, animationDuration)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.Success)));

        }

        public void ShowLooseScreen()
        {
            looseScreenParentCanvas.alpha = 0;
            looseText.rectTransform.localScale = Vector3.zero;
            looseScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(looseScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.SoftImpact)))
                .Append(looseText.rectTransform.DOScale(_initialLooseTextScale * TEXT_ANIMATIN_SCALE_MODIFIER, animationDuration / 2))
                .Append(looseText.rectTransform.DOScale(_initialLooseTextScale, animationDuration)
                    .OnComplete(()=>VibrationController.Vibrate(HapticTypes.HeavyImpact)));
        }
    }
}