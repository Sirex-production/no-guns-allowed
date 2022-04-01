using System;
using Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    public class ShootingEnemyAnimator : MonoBehaviour
    {
        [Required] 
        [SerializeField] private Animator animator;

        private AiBehaviourController _aiBehaviourController;
        
        private void Start()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            
            SetWalkingSpeed(_aiBehaviourController.AiData.Speed);
        }

        public void Shoot()
        {
            animator.SetTrigger("Shoot");
            this.DoAfterNextFrameCoroutine(() => animator.ResetTrigger("Shoot"));
        }

        public void SetWalking(bool isWalking)
        {
            animator.SetBool("IsWalking", isWalking);
        }

        public void SetCombat(bool isInCombat)
        {
            animator.SetBool("IsInCombat", isInCombat);
        }

        public void SetWalkingSpeed(float walkingSpeed)
        {
            animator.SetFloat("WalkingSpeed", walkingSpeed);
        }
    }
}