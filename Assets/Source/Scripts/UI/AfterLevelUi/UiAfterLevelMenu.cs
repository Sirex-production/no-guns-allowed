using DG.Tweening;
using Extensions;
using Support;
using TMPro;
using UnityEngine;

namespace Ingame.UI
{
    public class UiAfterLevelMenu : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float animationDuration = 1;
        [SerializeField] [Min(0)] private float lettersSpawnDelay = .01f;
        [Space]
        [SerializeField] private CanvasGroup winScreenParentCanvas;
        [SerializeField] private TMP_Text winText;
        [Space]
        [SerializeField] private CanvasGroup looseScreenParentCanvas;
        [SerializeField] private TMP_Text loseText;
        [Space] 
        [SerializeField] private UiLevelTransition uiLevelTransition;

        private const float TEXT_ANIMATING_SCALE_MODIFIER = 1.2f;
        
        private string _initialWinTextContent;
        private string _initialLooseTextContent;

        private Sequence _animationSequence;

        private void Awake()
        {
            if (winText != null)
            {
                _initialWinTextContent = winText.text;
                winText.SetText("");
            }

            if (loseText != null)
            {
                _initialLooseTextContent = loseText.text;
                loseText.SetText("");
            }

            if(uiLevelTransition != null)
                uiLevelTransition.SetGameObjectActive();
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
            winScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(winScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(() => this.SpawnTextCoroutine(winText, _initialWinTextContent, lettersSpawnDelay)));
        }

        private void ShowLooseScreen()
        {
            looseScreenParentCanvas.alpha = 0;
            looseScreenParentCanvas.SetGameObjectActive();

            _animationSequence = DOTween.Sequence()
                .Append(looseScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(() => this.SpawnTextCoroutine(loseText, _initialLooseTextContent, lettersSpawnDelay)));
        }
    }
}