using System;
using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerStatsController))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerAnimationController))]
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

        protected override void Awake()
        {
            base.Awake();

            _statsController = GetComponent<PlayerStatsController>();
            _movementController = GetComponent<PlayerMovementController>();
            _animationController = GetComponent<PlayerAnimationController>();
        }

        public void PerformDash(Vector3 dashingDirection)
        {
            OnDashPerformed?.Invoke(dashingDirection);
        }
    }
}