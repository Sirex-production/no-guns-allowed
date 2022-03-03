using DG.Tweening;
using Extensions;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class DashToKillTutorial : MonoTutorial
    {
        [SerializeField] private Image playerImage;
        [SerializeField] private Image enemyImage;
        [SerializeField] private Image bloodImage;
        [SerializeField] private CanvasGroup parentCanvas;
        [Space] 
        [SerializeField] [Min(0)] private float animationDuration = 1;
        [SerializeField] [Min(0)] private float dashAnimationDuration = .1f;
        [SerializeField] [Min(0)] private float bloodImageScaleMultiplier = 1.2f;

        [Inject] private GameController _gameController;
        [Inject] private TutorialsManager _tutorialsManager;
        
        private Vector3 _initialBloodImageScale;
        private Sequence _animationSequence;
        
        private void Awake()
        {
            _initialBloodImageScale = bloodImage.rectTransform.localScale;
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

        private void OnLevelRestart()
        {
            if(_animationSequence != null)
                _animationSequence.Kill();
            
            this.SetGameObjectInactive();
        }
        
        private void OnLevelEnd(bool _)
        {
            if(_animationSequence != null)
                _animationSequence.Kill();
            
            this.SetGameObjectInactive();
        }
        
        private void OnDashPerformed(Vector3 _)
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
            Complete();
        }

        private void Complete()
        {
            if(_animationSequence != null)
                _animationSequence.Kill();

            parentCanvas.DOFade(0, animationDuration).OnComplete(() => _tutorialsManager.ActivateNext());
        }

        public override void Activate()
        {
            playerImage.DOFade(0, 0);
            bloodImage.DOFade(0, 0);
            enemyImage.DOFade(0, 0);

            _animationSequence = DOTween.Sequence()
                .Append(playerImage.DOFade(1, animationDuration)) //Shows player
                .Append(enemyImage.DOFade(1, animationDuration)) //Shows enemy
                .Append(playerImage.rectTransform.DOLocalMove(enemyImage.rectTransform.localPosition, dashAnimationDuration)) //Dashes player to enemy
                .Append(enemyImage.DOFade(0, 0)) //Hides enemy
                .Append(bloodImage.DOFade(1, animationDuration)) //Shows blood
                .Append(bloodImage.rectTransform.DOScale(_initialBloodImageScale * bloodImageScaleMultiplier, animationDuration)) //Shows blood animation
                .Append(bloodImage.rectTransform.DOScale(_initialBloodImageScale, animationDuration)) //Shows blood animation
                .Append(playerImage.DOFade(0, animationDuration)//Hides player
                    .OnPlay(() => bloodImage.DOFade(0, animationDuration))) //Hides blood
                .SetLoops(-1);
        }
    }
}