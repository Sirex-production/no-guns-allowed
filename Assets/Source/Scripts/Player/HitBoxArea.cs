using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class HitBoxArea : MonoBehaviour
    {
        [Required][SerializeField] private ActorStats attachedStatsController;

        private float GIZMOS_SPHERE_SIZE = .3f;
        
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBoxArea enemyHitBox) || enemyHitBox.IsAttachedToGivenActor(attachedStatsController))
                return;

            if (attachedStatsController.IsInvincible) 
                enemyHitBox.TakeDamage(attachedStatsController.CurrentDamage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, GIZMOS_SPHERE_SIZE);
            
            if(attachedStatsController == null)
                return;
            
            Gizmos.DrawLine(transform.position, attachedStatsController.transform.position);
            
            var attachedCollider = GetComponent<Collider>();
            
            if (attachedCollider is SphereCollider sphereCollider)
            {
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
                return;
            }
            if (attachedCollider is BoxCollider boxCollider)
            {
                Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
                return;
            }
            if (attachedCollider is CapsuleCollider capsuleCollider)
            {
                Gizmos.DrawWireCube(capsuleCollider.bounds.center, capsuleCollider.bounds.size);
                return;
            }
            if (attachedCollider is MeshCollider meshCollider)
                Gizmos.DrawWireMesh(meshCollider.sharedMesh);
        }

        private bool IsAttachedToGivenActor(ActorStats actorStatsToCheck)
        {
            return attachedStatsController == actorStatsToCheck;
        }

        private void TakeDamage(float amountOfDamage)
        {
            attachedStatsController.TakeDamage(amountOfDamage);
        }
    }
}