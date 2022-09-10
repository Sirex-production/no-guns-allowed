using Support.Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerEventController))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [Required] [SerializeField] private Animator playerAnimator;

        private void Start()
        {
            PlayerEventController.Instance.OnAim += OnAim;
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            PlayerEventController.Instance.OnSurfaceInteraction += OnSurfaceInteraction;
            PlayerEventController.Instance.OnDashPerformed += ChangeLookDirection;
            PlayerEventController.Instance.OnFrozenChanged += SetFrozen;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnAim -= OnAim;
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
            PlayerEventController.Instance.OnSurfaceInteraction -= OnSurfaceInteraction;
            PlayerEventController.Instance.OnDashPerformed -= ChangeLookDirection;
            PlayerEventController.Instance.OnFrozenChanged -= SetFrozen;
        }

        private void OnAim(Vector3 aimPos)
        {
            SetIsAiming(aimPos.magnitude > 0);
        }

        private void OnDashPerformed(Vector3 _)
        {
            PlayAttackAnimation();
            SetIsAiming(false);
        }

        private void OnSurfaceInteraction(PlayerMovementController.SurfaceInteractionType surfaceInteractionType)
        {
            switch (surfaceInteractionType)
            {
                case PlayerMovementController.SurfaceInteractionType.LandOnSurface:
                    SetIsFalling(false);
                    break;
                case PlayerMovementController.SurfaceInteractionType.ReleaseFromSurface:
                    SetIsFalling(true);
                    break;
            }
        }

        private void ChangeLookDirection(Vector3 lookDirection)
        {
            if(lookDirection.x == 0)
                return;

            var playerLocalScale = playerAnimator.transform.localScale;
            playerLocalScale.z = Mathf.Abs(playerLocalScale.z) * Mathf.Sign(lookDirection.x);
            
            playerAnimator.transform.localScale = playerLocalScale;
        }

        private void PlayAttackAnimation()
        {
            playerAnimator.SetTrigger("Attack");
            this.DoAfterNextFrameCoroutine(() => playerAnimator.ResetTrigger("Attack"));
        }

        private void SetIsAiming(bool isAiming)
        {
            playerAnimator.SetBool("IsAiming", isAiming);
        }

        private void SetIsFalling(bool isFalling)
        {
            playerAnimator.SetBool("IsFalling", isFalling);
        }

        private void SetFrozen(bool isFrozen)
        {
            playerAnimator.SetBool("IsFrozen", isFrozen);
        }
    }
}