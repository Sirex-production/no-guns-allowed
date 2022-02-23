using DG.Tweening;
using Extensions;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class SwipeToDashTutorial : MonoTutorial
    {
        [SerializeField] private CanvasGroup parentCanvasGroup;
        [SerializeField] private Image fingerImage;
        [Space]
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private float fingerScaleOffset = 2f;
        [SerializeField] private float fingerDistanceOffset = 30f;
        
        [Inject] private GameController _gameController;
        [Inject] private TutorialsManager _tiTutorialsManager;
        
        private Vector3 _initialFingerScale;
        private Vector2 _initialFingerPosition;
        private Sequence _animationSequence;

        private void Awake()
        {
            _initialFingerScale = fingerImage.rectTransform.localScale;
            _initialFingerPosition = fingerImage.rectTransform.position;
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
            if(_animationSequence != null)
                _animationSequence.Kill();
            if (parentCanvasGroup != null)
                parentCanvasGroup.DOFade(0, animationDuration).OnComplete(() => _tiTutorialsManager.ActivateNext());
        }

        public override void Activate()
        {
            parentCanvasGroup.alpha = 1;
            _animationSequence = DOTween.Sequence()
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale * fingerScaleOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale, animationDuration))
                .Append(fingerImage.rectTransform.DOMoveX(_initialFingerPosition.x + fingerDistanceOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOMoveX(_initialFingerPosition.x - fingerDistanceOffset, animationDuration))
                .Append(fingerImage.rectTransform.DOMoveX(_initialFingerPosition.x, animationDuration))
                .SetLoops(-1);
        }
    }
}