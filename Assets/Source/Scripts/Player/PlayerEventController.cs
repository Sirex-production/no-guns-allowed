using System;
using MoreMountains.NiceVibrations;
using Support;
using UnityEngine;

namespace Ingame
{
    public class PlayerEventController : MonoSingleton<PlayerEventController>
    {
        private PlayerStatsController _statsController;
        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;
        
        public PlayerStatsController StatsController => _statsController;
        public PlayerMovementController MovementController => _movementController;
        public PlayerAnimationController AnimationController => _animationController;
        public PlayerData Data => _statsController.Data;

        public event Action<Vector3> OnDashPerformed;
        public event Action<Vector3> OnAim;
        public event Action<bool> OnFrozenChanged;
        public event Action OnDashStop;
        public event Action OnDashCancelled;
        public event Action<PlayerMovementController.SurfaceInteractionType> OnSurfaceInteraction;

        protected override void Awake()
        {
            base.Awake();

            _statsController = GetComponent<PlayerStatsController>();
            _movementController = GetComponent<PlayerMovementController>();
            _animationController = GetComponent<PlayerAnimationController>();
        }

        public void PerformDash(Vector3 dashingDirection)
        {
            VibrationController.Vibrate(HapticTypes.Selection);

            OnDashPerformed?.Invoke(dashingDirection);
        }

        public void Aim(Vector3 aimPos)
        {
            OnAim?.Invoke(aimPos);
        }

        public void StopDash()
        {
            OnDashStop?.Invoke();
        }

        public void CancelDash()
        {
            OnDashCancelled?.Invoke();
        }

        public void InteractWithSurface(PlayerMovementController.SurfaceInteractionType surfaceInteractionType)
        {
            OnSurfaceInteraction?.Invoke(surfaceInteractionType);
        }

        public void ChangeFrozen(bool isFrozen)
        {
            OnFrozenChanged?.Invoke(isFrozen);
        }
    }
}