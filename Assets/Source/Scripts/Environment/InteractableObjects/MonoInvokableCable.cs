using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public class MonoInvokableCable : MonoInvokable
    {
        [SerializeField] private Rigidbody bodyToRelease;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox)) 
                return;

            if(!hitbox.AttachedActorStats.TryGetComponent(out PlayerEventController controller) ||
               !controller.MovementController.IsDashing)
                return;

            ReleaseObject();
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
