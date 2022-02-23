using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class LevelOverviewTutorial : MonoTutorial
    {
        [BoxGroup("References"), Required]
        [SerializeField] private Button levelOverviewButton;
        [BoxGroup("References"), Required] 
        [SerializeField] private Image levelOverviewButtonImage;
        [Space]
        [BoxGroup("Animation properties")]
        [SerializeField] private Color blinkingColor = Color.white;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float blinkingAnimationDuration = 1;

        [Inject] private TutorialsManager _tutorialsManager;
        
        private Sequence _animationSequence;
        private Color _initialButtonImageColor;

        private void Awake()
        {
            _initialButtonImageColor = levelOverviewButtonImage.color;
        }

        public override void Activate()
        {
            levelOverviewButton.onClick.AddListener(Complete);
            HighlightButton();
        }

        private void HighlightButton()
        {
            if (_animationSequence != null)
            {
                _animationSequence.Kill();
                _animationSequence = null;
            }
            
            _animationSequence = DOTween.Sequence()
                .Append(levelOverviewButtonImage.DOBlendableColor(blinkingColor, blinkingAnimationDuration / 2))
                .Append(levelOverviewButtonImage.DOBlendableColor(_initialButtonImageColor, blinkingAnimationDuration / 2))
                .OnKill(()=> levelOverviewButtonImage.color = _initialButtonImageColor)
                .SetLoops(-1);
        }

        private void Complete()
        {
            _animationSequence.Kill();
            _tutorialsManager.ActivateNext();
        }
    }
}