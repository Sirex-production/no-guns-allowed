using DG.Tweening;
using Extensions;
using ModestTree;
using NaughtyAttributes;
using Support;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class DashToKillTutorial : MonoTutorial
    {
        [BoxGroup("References"), Required]
        [SerializeField] private Image playerImage;
        [BoxGroup("References"), Required]
        [SerializeField] private Image enemyImage;
        [BoxGroup("References"), Required]
        [SerializeField] private Image bloodImage;
        [BoxGroup("References"), Required]
        [SerializeField] private CanvasGroup parentCanvas;
        [Space] 
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float animationDuration = 1;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float dashAnimationDuration = .1f;
        [BoxGroup("Animation properties")]
        [SerializeField] [Min(0)] private float bloodImageScaleMultiplier = 1.2f;
        [Space]
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is activated")]
        [SerializeField] private string activateLogMessage;
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is completed")]
        [SerializeField] private string completeLogMessage;

        [Inject] private GameController _gameController;
        [Inject] private TutorialsManager _tutorialsManager;
        [Inject] private UiController _uiController;
        
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

        public override void Complete()
        {
            if(activateLogMessage != null || !activateLogMessage.IsEmpty()) 
                _uiController.DisplayLogMessage(completeLogMessage, LogDisplayType.DisplayAndKeep);
            
            if(_animationSequence != null)
                _animationSequence.Kill();

            parentCanvas.DOFade(0, animationDuration).OnComplete(() => _tutorialsManager.ActivateNext());
        }

        public override void Activate()
        {
            if(activateLogMessage != null || !activateLogMessage.IsEmpty()) 
                _uiController.DisplayLogMessage(activateLogMessage, LogDisplayType.DisplayAndKeep);
            
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