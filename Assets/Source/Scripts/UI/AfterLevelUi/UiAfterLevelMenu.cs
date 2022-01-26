using DG.Tweening;
using Extensions;
using Support;
using Support.SLS;
using TMPro;
using UnityEngine;
using Zenject;

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

        [Inject] private GameController _gameController;
        [Inject] private AnalyticsWrapper _analyticsWrapper;
        [Inject] private SaveLoadSystem _saveLoadSystem;
        
        
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
            _gameController.OnLevelEnded += PlayAfterLevelMenuAnimation;
        }

        private void OnDestroy()
        {
            _gameController.OnLevelEnded -= PlayAfterLevelMenuAnimation;
            
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
            var winTextContent = GetGeneratedEndScreenText(_initialWinTextContent);

            _animationSequence = DOTween.Sequence()
                .Append(winScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(() => this.SpawnTextCoroutine(winText, winTextContent, lettersSpawnDelay)));
        }

        private void ShowLooseScreen()
        {
            looseScreenParentCanvas.alpha = 0;
            looseScreenParentCanvas.SetGameObjectActive();
            var looseTextContent = GetGeneratedEndScreenText(_initialLooseTextContent);

            _animationSequence = DOTween.Sequence()
                .Append(looseScreenParentCanvas.DOFade(1, animationDuration / 1.5f)
                    .OnComplete(() => this.SpawnTextCoroutine(loseText, looseTextContent, lettersSpawnDelay)));
        }

        private string GetGeneratedEndScreenText(string initialText)
        {
            if (string.IsNullOrEmpty(initialText) || _analyticsWrapper == null)
                return initialText; 
            
            var levelStatsPack = _analyticsWrapper.LevelStats.StatsPack;

            initialText = initialText
                .Replace("_PLAYER.DEATH.TYPE_", $"{levelStatsPack.deathDamageType}")
                .Replace("_ENEMY.KILLED_", $"{levelStatsPack.enemiesKilled}")
                .Replace("_LEVEL.TIME_", $"{levelStatsPack.timePassedFromTheBeginingOfTheLevel}")
                .Replace("_LEVEL.NUMBER_", $"{_saveLoadSystem.SaveData.CurrentLevelNumber.Value}");

            return initialText;
        }
    }
}