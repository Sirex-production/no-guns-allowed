using Ingame.AI;
using Ingame.Graphics;
using Support.Sound;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class MonoInvokableCable : MonoInvokable
    {
        [SerializeField] private Rigidbody bodyToRelease;

        [Inject] private EffectsManager _effectsManager;
        [Inject] private AudioManager _audioManager;
        
        public override void Invoke()
        {
            ReleaseObject();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!CheckConditionsToPass(other))
                return;
            
            ReleaseObject();
            _effectsManager.PlayPlayerAttackEffect();
        }

        private void OnTriggerExit(Collider other)
        {
            if(!CheckConditionsToPass(other))
                return;
                
            ReleaseObject();
            _effectsManager.PlayPlayerAttackEffect();
        }

        private bool CheckConditionsToPass(Component other)
        {
            if (!other.TryGetComponent(out HitBox hitbox)) 
                return false;

            if(!hitbox.AttachedActorStats.TryGetComponent(out PlayerEventController controller) ||
               !controller.MovementController.IsDashing)
                return false;

            return true;
        }

        private void ReleaseObject()
        {
            _audioManager.PlaySound(AudioName.environment_chain_break);
            
            bodyToRelease.isKinematic = false;
            Destroy(gameObject);
        }
    }
}
