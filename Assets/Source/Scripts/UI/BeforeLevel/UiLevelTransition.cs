using Extensions;
using NaughtyAttributes;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class UiLevelTransition : MonoBehaviour
    {
        [Required] [SerializeField] private Animator levelTransitionAnimator;

        private const float TIME_OFFSET_AFTER_ANIMATION = .1f;
        
        private void Start()
        {
            GameController.Instance.OnNextLevelLoaded += OnNextLevelLoad;
            GameController.Instance.OnLevelRestart += OnLevelRestart;
            
            PlayOpenAnimation();
        }

        private void OnDestroy()
        {
            GameController.Instance.OnNextLevelLoaded -= OnNextLevelLoad;
            GameController.Instance.OnLevelRestart -= OnLevelRestart;
        }
        
        private void OnNextLevelLoad()
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION, () => LevelManager.Instance.LoadNextLevel());
        }

        private void OnLevelRestart()
        {
            PlayCloseAnimation();
            var currentState = levelTransitionAnimator.GetCurrentAnimatorStateInfo(0);
            this.WaitAndDoCoroutine(currentState.length + TIME_OFFSET_AFTER_ANIMATION, () => LevelManager.Instance.RestartLevel());
        }

        private void PlayOpenAnimation()
        {
            if(levelTransitionAnimator == null)
                return;
            
            levelTransitionAnimator.SetTrigger("Open");
            this.DoAfterNextFrameCoroutine(() => levelTransitionAnimator.ResetTrigger("Open"));
        }

        private void PlayCloseAnimation()
        {
            if(levelTransitionAnimator == null)
                return;
            
            levelTransitionAnimator.SetTrigger("Close");
            this.DoAfterNextFrameCoroutine(() => levelTransitionAnimator.ResetTrigger("Close"));
        }
    }
}