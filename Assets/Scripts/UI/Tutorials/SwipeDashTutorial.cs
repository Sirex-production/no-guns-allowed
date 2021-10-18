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
            _initialFingerScale = fingerImage.transform.localScale;
            _initialFingerPosition = fingerImage.transform.position;
        }

        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
        }

        private void OnDashPerformed(Vector3 _)
        {
            Complete();
        }

        public override void Activate()
        {
            DOTween.Sequence()
                .Append(fingerImage.transform.DOScale(_initialFingerScale * fingerScaleOffset, animationSpeed))
                .Append(fingerImage.transform.DOScale(_initialFingerScale, animationSpeed))
                .Append(fingerImage.transform.DOMove(_initialFingerPosition + Vector2.right * fingerDistanceOffset, animationSpeed))
                .Append(fingerImage.transform.DOMove(_initialFingerPosition + Vector2.left * fingerDistanceOffset, animationSpeed))
                .Append(fingerImage.transform.DOMove(_initialFingerPosition, animationSpeed))
                .SetLoops(-1);
        }

        protected override void Complete()
        {
            if(_animationSequence != null)
                _animationSequence.Kill();

            parentCanvasGroup.DOFade(0, animationSpeed);
        }
    }
}