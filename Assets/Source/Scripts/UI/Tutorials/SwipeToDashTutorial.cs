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
        [Space]
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is activated")]
        [SerializeField] private string activateLogMessage;
        [BoxGroup("Game properties"), Tooltip("Message that will be displayed to the LOG window when tutorial is completed")]
        [SerializeField] private string completeLogMessage;
        [Space]
        [BoxGroup("Tutorial properties")]
        [SerializeField] private bool activateNextTutorial = true;
        
        [Inject] private GameController _gameController;
        [Inject] private TutorialsManager _tutorialsManager;
        [Inject] private UiController _uiController;
        
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
            this.SetGameObjectInactive();
        }

        private void OnLevelRestart()
        {
            this.SetGameObjectInactive();
        }

        private void OnDashPerformed(Vector3 _)
        {
            Complete();
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        public override void Complete()
        {
            _uiController.DisplayLogMessage(completeLogMessage, LogDisplayType.DisplayAndKeep);
            
            if (parentCanvasGroup != null)
                parentCanvasGroup.DOFade(0, animationDuration).OnComplete(() =>
                {
                    if(activateNextTutorial)
                        _tutorialsManager.ActivateNext();
                });
        }

        public override void Activate()
        {
            _uiController.DisplayLogMessage(activateLogMessage, LogDisplayType.DisplayAndKeep);
            
            parentCanvasGroup.alpha = 1;
        }
    }
}