using Extensions;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public class UiLevelTransition : MonoBehaviour
    {
        [Required] 
        [SerializeField] private Animator levelTransitionAnimator;

        [Inject] private GameController _gameController;
        [Inject] private LevelManager _levelManager;
        
        private const float TIME_OFFSET_AFTER_ANIMATION_FOR_SCENE_TRANSITION = .2f;

        private void Start()
        {
            _gameController.OnNextLevelLoaded += OnNextLevelLoad;
            _gameController.OnLevelLoaded += OnLevelLoaded;
            _gameController.OnLevelRestart += OnLevelRestart;
            _gameController.OnLastLevelFromStaveLoaded += OnLastLevelFromSaveLoaded;

            PlayOpenAnimation();
        }


        private void OnDestroy()
        {
            _gameController.OnNextLevelLoaded -= OnNextLevelLoad;
            _gameController.OnLevelLoaded -= OnLevelLoaded;
            _gameController.OnLevelRestart -= OnLevelRestart;
            _gameController.OnLastLevelFromStaveLoaded -= OnLastLevelFromSaveLoaded; }

        private void OnNextLevelLoad()
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION_FOR_SCENE_TRANSITION, () => _levelManager.LoadNextLevel());
        }

        private void OnLevelLoaded(int levelNumber)
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION_FOR_SCENE_TRANSITION, () => _levelManager.LoadLevel(levelNumber));
        }

        private void OnLevelRestart()
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION_FOR_SCENE_TRANSITION, () => _levelManager.RestartLevel());
        }
        
        private void OnLastLevelFromSaveLoaded()
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION_FOR_SCENE_TRANSITION, () => _levelManager.LoadLastLevelFromSave());
        }

        private void PlayOpenAnimation()
        {
            levelTransitionAnimator.SetTrigger("Open");
            this.DoAfterNextFrameCoroutine(() => levelTransitionAnimator.ResetTrigger("Open"));
        }

        private void PlayCloseAnimation()
        {
            levelTransitionAnimator.SetTrigger("Close");
            this.DoAfterNextFrameCoroutine(() => levelTransitionAnimator.ResetTrigger("Close"));
        }
    }
}