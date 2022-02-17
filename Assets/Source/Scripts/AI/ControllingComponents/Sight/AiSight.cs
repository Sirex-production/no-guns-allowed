using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class AiSight : MonoBehaviour
    {
        [Required]
        [SerializeField] private Transform detectionAreasParent;
        [SerializeField] private List<AiDetectionArea> detectionAreas = new List<AiDetectionArea>();

        private const float GIZMOS_SPHERE_RADIUS = .5f;
        
        private AiBehaviourController _aiBehaviourController;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            
            foreach (var detectionArea in detectionAreas)
            {
                if(detectionArea == null)
                    return;

                detectionArea.OriginSight = this;
            }
        }

        private void OnValidate()
        {
            if (detectionAreasParent == null)
                detectionAreasParent = transform;
            
            if(detectionAreas == null || detectionAreas.Count < 1)
                return;

            for (var detectionAreaIndex = 0; detectionAreaIndex < detectionAreas.Count; detectionAreaIndex++)
            {
                if(detectionAreas[detectionAreaIndex] == null)
                    detectionAreas.RemoveAt(detectionAreaIndex);
                else
                    detectionAreas[detectionAreaIndex].OriginSight = this;
            }
        }

        private void OnDestroy()
        {
            if(detectionAreas == null || detectionAreas.Count < 1)
                return;
            
            foreach (var detectionArea in detectionAreas)
            {
                if(detectionArea == null)
                    return;

                Destroy(detectionArea);
            }
        }

        private void OnDrawGizmos()
        {
            if(detectionAreas == null || detectionAreas.Count < 1)
                return;

            Gizmos.color = Color.red;
            foreach (var detectionArea in detectionAreas)
            {
                if(detectionArea == null)
                    return;

                var detectionAreaCollider = detectionArea.GetComponent<Collider>();
                var detectionAreaPosition = detectionAreaCollider.bounds.center;
                
                Gizmos.DrawLine(transform.position, detectionAreaPosition);
                Gizmos.DrawSphere(detectionAreaPosition, GIZMOS_SPHERE_RADIUS);
            }
        }

        [Button("Add new detection area")]
        private void AddNewDetectionArea()
        {
            var detectionArea =
                new GameObject("DetectionArea", typeof(BoxCollider), typeof(AiDetectionArea))
                .GetComponent<AiDetectionArea>();

            detectionArea.transform.parent = detectionAreasParent;
            detectionArea.transform.position = transform.position;

            detectionAreas.Add(detectionArea);
        }

        public void Detect(ActorStats actorStats)
        {
            if(actorStats == _aiBehaviourController.AiActorStats)
                return;
            
            if(_aiBehaviourController.AiData.HostileSides.Contains(actorStats.ActorSide))
                _aiBehaviourController.SpotEnemy(actorStats);
        }
    }
}