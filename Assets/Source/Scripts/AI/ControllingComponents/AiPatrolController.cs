using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    [DisallowMultipleComponent]
    public class AiPatrolController : MonoBehaviour, IPatrolable
    {
        [Required]
        [SerializeField] private Transform patrollingPointsParent;
        [SerializeField] private List<Transform> patrolPoints = new List<Transform>();

        private const float GIZMOS_SPHERE_RADIUS = .5f;
        
        private AiBehaviourController _aiBehaviourController;
        private int _currentPatrolPointIndex = -1;
        private bool _isPatrolling = false;

        private void OnEnable()
        {
            MoveToNextPoint();
        }

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
        }

        private void OnValidate()
        {
            if (patrollingPointsParent == null)
                patrollingPointsParent = transform;

            for (var patrollingPointIndex = 0; patrollingPointIndex < patrolPoints.Count; patrollingPointIndex++)
                if (patrolPoints[patrollingPointIndex] == null)
                    patrolPoints.RemoveAt(patrollingPointIndex);
        }

        private void OnDrawGizmos()
        {
            if(patrolPoints == null || patrolPoints.Count < 1)
                return;

            var prevTransform = transform;
            
            Gizmos.color = Color.green;
            foreach (var patrolPoint in patrolPoints)
            {
                if(patrolPoint == null)
                    continue;
                
                Gizmos.DrawSphere(patrolPoint.position, GIZMOS_SPHERE_RADIUS);
                Gizmos.DrawLine(prevTransform.position, patrolPoint.position);
                
                patrolPoint.position = new Vector3(patrolPoint.position.x, patrolPoint.position.y, transform.position.z);

                prevTransform = patrolPoint;
            }
            
            if (_aiBehaviourController == null)
                _aiBehaviourController = GetComponent<AiBehaviourController>();
                
            if(_aiBehaviourController.AiData != null)
                if(_aiBehaviourController.AiData.IsLooped)
                    if(patrolPoints[0] != null && patrolPoints[patrolPoints.Count - 1] != null)
                        Gizmos.DrawLine(patrolPoints[0].position, patrolPoints[patrolPoints.Count - 1].position);
        }

        [Button("Add new patrolling point")]
        private void AddNewPatrollingPoint()
        {
            var patrollingPoint = new GameObject("PatrolPoint")
            {
                transform =
                {
                    parent = patrollingPointsParent,
                    position = transform.position
                }
            };

            patrolPoints.Add(patrollingPoint.transform);
        }

        private void MoveToNextPoint()
        {
            if (!_isPatrolling)
                return;

            if (patrolPoints == null || patrolPoints.Count < 1)
                return;

            _currentPatrolPointIndex++;

            if (_currentPatrolPointIndex >= patrolPoints.Count && !_aiBehaviourController.AiData.IsLooped)
                return;

            if (_currentPatrolPointIndex >= patrolPoints.Count && _aiBehaviourController.AiData.IsLooped)
                _currentPatrolPointIndex = 0;

            var actualPatrolPointTransform = patrolPoints[_currentPatrolPointIndex];
            var actualPatrolPointPos = actualPatrolPointTransform.position;
            var targetRotation = Quaternion.LookRotation(actualPatrolPointPos - transform.position);
            targetRotation.eulerAngles = Vector3.up * targetRotation.eulerAngles.y;


            _aiBehaviourController.AiMovementController.Rotate(targetRotation, _aiBehaviourController.AiData.RotationSpeed,
                () => _aiBehaviourController.AiMovementController.Follow(actualPatrolPointTransform,
                    _aiBehaviourController.AiData.Speed, MoveToNextPoint));

        }

        public void StartPatrolling()
        {
            _isPatrolling = true;
            MoveToNextPoint();
        }   

        public void StopPatrolling()
        {
            _isPatrolling = false;
            _aiBehaviourController.AiMovementController.StopMotion();
        }
    }
}