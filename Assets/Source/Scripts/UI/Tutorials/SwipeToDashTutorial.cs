using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class SwipeToDashTutorial : MonoTutorial
    {
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup parentCanvasGroup;
        [BoxGroup("References"), Required]
        [SerializeField] private Image fingerImage;
        [Space]
        [BoxGroup("Animation options")]
        [SerializeField] private float animationDuration = 1f;
        [BoxGroup("Animation options")]
        [SerializeField] private float fingerScaleOffset = 2f;
        [BoxGroup("Animation options")]
        [SerializeField] private float fingerDistanceOffset = 30f;
        [Space]
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is activated")]
        [SerializeField] private string activateLogMessage;
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is completed")]
        [SerializeField] private string completeLogMessage;
        [Space]
        [BoxGroup("Tutorial properties")]
        [SerializeField] private bool activateNextTutorialWhenCurrentIsComplete = true;
        
        [Inject] private GameController _gameController;
        [Inject] private TutorialsManager _tutorialsManager;
        [Inject] private UiController _uiController;
        
        private Vector3 _initialFingerScale;
        private Vector2 _initialFingerPosition;
        private Sequence _animationSequence;

        private void Awake()
        {
            _initialFingerScale = fingerImage.rectTransform.localScale;
            _initialFingerPosition = fingerImage.rectTransform.localPosition;
        }

        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            _gameController.OnLevelEnded += OnLevelEnd;
            _gameController.OnLevelRestart += OnLevelRestart;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
            _gameController.OnLevelEnded -= OnLevelEnd;
            _gameController.OnLevelRestart -= OnLevelRestart;
        }

        private void OnLevelEnd(bool _)
        {
            if(_animationSequence != null)
                _animationSequence.Kill();
            
            this.SetGameObjectInactive();
        }

        private void OnLevelRestart()
        {
            if(_animationSequence != null)
                _animationSequence.Kill();
            
            this.SetGameObjectInactive();
        }

        private void OnDashPerformed(Vector3 _)
        {
            Complete();
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        private void Complete()
        {
            _uiController.DisplayLogMessage(completeLogMessage, LogDisplayType.DisplayAndKeep);

            if(_animationSequence != null)
                _animationSequence.Kill();
            if (parentCanvasGroup != null)
                parentCanvasGroup.DOFade(0, animationDuration).OnComplete(() =>
                {
                    if(activateNextTutorialWhenCurrentIsComplete)
                        _tutorialsManager.ActivateNext();
                });
        }

        public override void Activate()
        {
            _uiController.DisplayLogMessage(activateLogMessage, LogDisplayType.DisplayAndKeep);
            
            parentCanvasGroup.alpha = 1;
            _animationSequence = DOTween.Sequence()
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale * fingerScaleOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale, animationDuration))
                .Append(fingerImage.rectTransform.DOLocalMoveX(_initialFingerPosition.x + fingerDistanceOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOLocalMoveX(_initialFingerPosition.x - fingerDistanceOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOLocalMoveX(_initialFingerPosition.x, animationDuration))
                .SetLoops(-1);
        }
    }
}