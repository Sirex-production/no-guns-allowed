using System.Collections;
using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public class LethalObject : MonoBehaviour
    {
        private const float DAMAGE = 1;
        private const float COMPONENT_REMOVAL_POLL_FREQUENCY = 0.25f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox) ||
                hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController) 
                return;

            if (hitbox.AttachedActorStats.IsInvincible) 
                return;

            StartCoroutine(ComponentRemovalCoroutine());
            hitbox.TakeDamage(DAMAGE, DamageType.Obstacle);
        }

        private IEnumerator ComponentRemovalCoroutine()
        {
            var rigidBodyComponent = GetComponent<Rigidbody>();
            while (true)
            {
                if (!(rigidBodyComponent.velocity.sqrMagnitude < 0.01f))
                {
                    yield return new WaitForSeconds(COMPONENT_REMOVAL_POLL_FREQUENCY);
                    continue;
                }

                RemoveComponents();
                break;
            }
        }

        private void RemoveComponents()
        {
            Destroy(gameObject.GetComponent<Rigidbody>());

            Destroy(this);
        }
    }
}
