using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class SwipeDashTutorial : Tutorial
    {
        [SerializeField] private CanvasGroup parentCanvasGroup;
        [SerializeField] private Image fingerImage;
        [Space]
        [SerializeField] private float animationSpeed = 1f;
        [SerializeField] private float fingerScaleOffset = 2f;
        [SerializeField] private float fingerDistanceOffset = 30f;

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
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        private void OnDashPerformed(Vector3 _)
        {
            Complete();
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        protected override void Complete()
        {
            if(_animationSequence != null)
                _animationSequence.Kill();
            if (parentCanvasGroup != null)
                parentCanvasGroup.DOFade(0, animationSpeed).OnComplete(base.Complete);
        }

        public override void Activate()
        {
            parentCanvasGroup.alpha = 1;
            _animationSequence = DOTween.Sequence()
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale * fingerScaleOffset, animationSpeed))
                .Append(fingerImage.rectTransform.DOScale(_initialFingerScale, animationSpeed))
                .Append(fingerImage.rectTransform.DOMove(_initialFingerPosition + Vector2.right * fingerDistanceOffset, animationSpeed))
                .Append(fingerImage.rectTransform.DOMove(_initialFingerPosition + Vector2.left * fingerDistanceOffset, animationSpeed))
                .Append(fingerImage.rectTransform.DOMove(_initialFingerPosition, animationSpeed))
                .SetLoops(-1);
        }
    }
}