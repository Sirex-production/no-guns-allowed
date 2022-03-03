using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public class InvokableCable : Invokable
    {
        [SerializeField] private Rigidbody bodyToRelease;

        private void OnTriggerEnter(Collider other)
        {
            if(CheckConditionsToPass(other))
                ReleaseObject();
        }

        private void OnTriggerExit(Collider other)
        {
            if(CheckConditionsToPass(other))
                ReleaseObject();
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
            bodyToRelease.gameObject.AddComponent<LethalObject>();
            Destroy(this.gameObject);
        }
    }
}
