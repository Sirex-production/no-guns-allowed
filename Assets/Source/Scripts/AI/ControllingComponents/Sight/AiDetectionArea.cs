using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(Collider))]
    public class AiDetectionArea : MonoBehaviour
    {
        [SerializeField] private bool ignoreWallsDuringDetection = true;
        
        private AiSight _origin;
        private Dictionary<ActorStats, Coroutine> _detectionRoutines = new Dictionary<ActorStats, Coroutine>();

        private readonly WaitForSeconds PAUSE = new WaitForSeconds(.1f); 
        
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
            if(_origin == null || !_origin)
                return;
            
            if (other.TryGetComponent(out ActorStats actor))
                if(ignoreWallsDuringDetection)
                    _origin.Detect(actor);
                else
                    TryDetection(actor);
        }

        private void OnTriggerExit(Collider other)
        {
            if(_origin == null || !_origin || ignoreWallsDuringDetection)
                return;
            
            if (other.TryGetComponent(out ActorStats actor)) 
                StopDetection(actor);
        }

        private IEnumerator TryDetectionRoutine(ActorStats actorToDetect)
        {
            if (actorToDetect == null)
                yield break;

            var ignoreLayerMask = ~LayerMask.GetMask(
                "Ignore Detection Cast",
                "Ignore Collision With Dashing Player",
                "Breakable Object");

            while (true)
            {
                yield return PAUSE;

                if(actorToDetect == null || _origin == null)
                    yield break;
                    
                if (Physics.Linecast(actorToDetect.transform.position, _origin.transform.position, out _, ignoreLayerMask, QueryTriggerInteraction.Ignore))
                    continue;

                _origin.Detect(actorToDetect);
                StopDetection(actorToDetect);

                yield break;
            }
        }

        private void TryDetection(ActorStats actor)
        {
            _detectionRoutines.Add(actor, StartCoroutine(TryDetectionRoutine(actor)));
        }

        private void StopDetection(ActorStats actor)
        {
            if(!_detectionRoutines.ContainsKey(actor))
                return;
            
            StopCoroutine(_detectionRoutines[actor]);
            _detectionRoutines.Remove(actor);
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