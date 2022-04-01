using Ingame.AI;
using Ingame.Graphics;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class MonoInvokableCable : MonoInvokable
    {
        [SerializeField] private Rigidbody bodyToRelease;

        [Inject] private EffectsManager _effectsManager;
        
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

        public override void Invoke()
        {
            ReleaseObject();
        }

        private void ReleaseObject()
        {
            bodyToRelease.isKinematic = false;
            Destroy(this.gameObject);
        }
    }
}
