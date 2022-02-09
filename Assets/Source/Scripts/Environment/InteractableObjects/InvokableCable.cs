using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public class InvokableCable : Invokable
    {
        [SerializeField] private Rigidbody bodyToRelease;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox) ||
                hitbox.AttachedActorStats != PlayerEventController.Instance.StatsController) 
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
