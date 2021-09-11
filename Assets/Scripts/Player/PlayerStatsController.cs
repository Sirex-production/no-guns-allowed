using System;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController), typeof(PlayerAnimationController))]
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private PlayerData data;
        
        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;

        public PlayerData Data => data;
        public PlayerMovementController MovementController => _movementController;
        public PlayerAnimationController AnimationController => _animationController;

        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
            _animationController = GetComponent<PlayerAnimationController>();
        }
    }
}