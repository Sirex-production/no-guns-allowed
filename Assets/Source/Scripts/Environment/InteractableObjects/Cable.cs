using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public class Cable : MonoBehaviour
    {
        [SerializeField] private Rigidbody bodyToRelease;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HitBox hitbox) &&
                hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController)
            {
                bodyToRelease.isKinematic = false;
                bodyToRelease.gameObject.AddComponent<LethalObject>();
                Destroy(this.gameObject);
            }
        }
    }
}
