using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(Collider))]
    public class HitBox : MonoBehaviour
    {
        [Required][SerializeField] protected ActorStats attachedStatsController;
        
        private float GIZMOS_SPHERE_SIZE = .3f;
        
        public ActorStats AttachedActorStats => attachedStatsController;

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
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
        
        public void TakeDamage(float amountOfDamage, DamageType damageType)
        {
            attachedStatsController.TakeDamage(amountOfDamage, damageType);
        }
    }
}