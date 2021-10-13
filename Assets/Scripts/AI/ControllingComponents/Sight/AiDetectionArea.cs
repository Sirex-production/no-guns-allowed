using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(Collider))]
    public class AiDetectionArea : MonoBehaviour
    {
        private AiSight _origin;

        public AiSight OriginSight
        {
            set
            {
                if (value != null)
                    _origin = value;
            }
        }

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ActorStats actor)) 
                _origin.Detect(actor);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

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

            if (attachedCollider is MeshCollider meshCollider)
            {
                Gizmos.DrawWireMesh(meshCollider.sharedMesh);
            }
        }
    }
}