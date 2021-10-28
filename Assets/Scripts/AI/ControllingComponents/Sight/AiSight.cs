using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class AiSight : MonoBehaviour
    {
        [SerializeField] private bool onlyPlayerCanBeDetected = true;
        [SerializeField] private List<AiDetectionArea> _detectionAreas = new List<AiDetectionArea>();

        private const float GIZMOS_SPHERE_RADIUS = .5f;
        
        private AiBehaviourController _aiBehaviourController;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            
            foreach (var detectionArea in _detectionAreas)
            {
                if(detectionArea == null)
                    return;

                detectionArea.OriginSight = this;
            }
        }

        private void OnValidate()
        {
            if(_detectionAreas == null || _detectionAreas.Count < 1)
                return;
            
            foreach (var detectionArea in _detectionAreas)
            {
                if(detectionArea == null)
                    return;

                detectionArea.OriginSight = this;
            }
        }

        private void OnDestroy()
        {
            if(_detectionAreas == null || _detectionAreas.Count < 1)
                return;
            
            foreach (var detectionArea in _detectionAreas)
            {
                if(detectionArea == null)
                    return;

                Destroy(detectionArea);
            }
        }

        private void OnDrawGizmos()
        {
            if(_detectionAreas == null || _detectionAreas.Count < 1)
                return;

            Gizmos.color = Color.red;
            foreach (var detectionArea in _detectionAreas)
            {
                if(detectionArea == null)
                    return;
                
                Gizmos.DrawLine(transform.position, detectionArea.transform.position);
                Gizmos.DrawSphere(detectionArea.transform.position, GIZMOS_SPHERE_RADIUS);
            }
        }

        public void Detect(ActorStats actorStats)
        {
            if(actorStats == _aiBehaviourController.AiActorStats)
                return;
            
            if (actorStats == PlayerEventController.Instance.StatsController)
                _aiBehaviourController.SpotEnemy();
            else if (!onlyPlayerCanBeDetected)
                _aiBehaviourController.SpotEnemy();
        }
    }
}