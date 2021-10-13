using System.Collections.Generic;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class AiPatrolController : MonoBehaviour
    {
        [SerializeField] private bool isLooped;
        [SerializeField] private List<Transform> patrolPoints;

        private const float GIZMOS_SPHERE_RADIUS = .5f;
        
        private AiBehaviourController _aiBehaviourController;
        private int _currentPatrolPointIndex = -1;
        private bool _isPatrolling = false;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
        }

        private void OnDrawGizmos()
        {
            if(patrolPoints == null || patrolPoints.Count < 1)
                return;

            Transform prevTransform = transform;
            
            foreach (var patrolPoint in patrolPoints)
            {
                if(patrolPoint == null)
                    continue;

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(patrolPoint.position, GIZMOS_SPHERE_RADIUS);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(prevTransform.position, patrolPoint.position);
                
                patrolPoint.position = new Vector3(patrolPoint.position.x, patrolPoint.position.y, transform.position.z);

                prevTransform = patrolPoint;
            }
            
            if(isLooped)
                Gizmos.DrawLine(patrolPoints[0].position, patrolPoints[patrolPoints.Count - 1].position);
        }
        
        private void MoveToNextPoint()
        {
            if(!_isPatrolling)
                return;
            
            _currentPatrolPointIndex++;
            
            if(_currentPatrolPointIndex >= patrolPoints.Count && !isLooped)
                return;
            
            if (_currentPatrolPointIndex >= patrolPoints.Count && isLooped)
                _currentPatrolPointIndex = 0;

            _aiBehaviourController.AiMovementController.Follow(patrolPoints[_currentPatrolPointIndex], 10, MoveToNextPoint); //todo reduce hardcode with AI stats
        }

        public void StartPatrolling()
        {
            _isPatrolling = true;
            MoveToNextPoint();
        }   

        public void StopPatrolling()
        {
            _isPatrolling = false;
            _aiBehaviourController.AiMovementController.StopMoving();
        }
    }
}