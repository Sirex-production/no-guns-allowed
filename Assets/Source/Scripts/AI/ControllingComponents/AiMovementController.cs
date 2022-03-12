using System;
using System.Collections;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController), typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class AiMovementController : MonoBehaviour, IMovable
    {
        private const float DISTANCE_OFFSET = .1f;

        private AiBehaviourController _aiBehaviourController;
        private CharacterController _characterController;
        private Coroutine _moveCoroutine;
        private Coroutine _rotateCoroutine;
        private Coroutine _gravityCoroutine;
        private Coroutine _followCoroutine;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_aiBehaviourController.AiData.UseGravity)
                _characterController.Move(Physics.gravity * Time.deltaTime);

        }

        #region Routines
        private IEnumerator ApplyGravityRoutine()
        {
            while (_aiBehaviourController.AiData.UseGravity)
            {
                _characterController.Move(Physics.gravity * Time.fixedDeltaTime);

                yield return null;
            }
        }

        private IEnumerator FollowRoutine(Transform transformToFollow, float speed, Action onEnd)
        {
            bool IsPatrollingPointReached()
            {
                if (!_aiBehaviourController.AiData.UseGravity)
                    return Vector3.Distance(transform.position, transformToFollow.position) > DISTANCE_OFFSET;

                return Mathf.Abs(transformToFollow.position.x - transform.position.x) > DISTANCE_OFFSET;
            }

            while (IsPatrollingPointReached())
            {
                var velocity = Vector3.Normalize(transformToFollow.position - transform.position);
                velocity *= speed;
                if (_aiBehaviourController.AiData.UseGravity)
                    velocity.y = 0;
                velocity *= Time.deltaTime;

                _characterController.Move(velocity);
                
                yield return null;
            }

            // transform.position = transformToFollow.position;
            _gravityCoroutine ??= StartCoroutine(ApplyGravityRoutine());
            onEnd?.Invoke();
        }

        private IEnumerator MoveToRoutine(Vector3 destination, float speed, Action onEnd)
        {
            destination.z = transform.position.z;

            bool IsPatrollingPointReached()
            {
                if (!_aiBehaviourController.AiData.UseGravity)
                    return Vector3.Distance(transform.position, destination) > DISTANCE_OFFSET;

                return Mathf.Abs(destination.x - transform.position.x) > DISTANCE_OFFSET;
            }

            while (IsPatrollingPointReached())
            {
                var velocity = Vector3.Normalize(destination - transform.position);
                velocity *= speed;
                if (_aiBehaviourController.AiData.UseGravity)
                    velocity.y = 0;
                
                velocity *= Time.deltaTime;
                
                _characterController.Move(velocity);
                
                yield return null;
            }
            
            // transform.position = destination;
            _gravityCoroutine ??= StartCoroutine(ApplyGravityRoutine());
            onEnd?.Invoke();
        }

        private IEnumerator RotateRoutine(Quaternion rotation, float speed, Action onEnd)
        {
            while (Mathf.Abs(Quaternion.Dot(transform.rotation , rotation)) < 1 - .0001f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
            
                yield return null;
            }

            transform.rotation = rotation;
            
            onEnd?.Invoke();
        }
        #endregion

        public void Follow(Transform transformToFollow, float speed, Action onEnd = null)
        {
            if(_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            
            if (_followCoroutine != null) 
                StopCoroutine(_followCoroutine);
            
            if(_gravityCoroutine != null)
                StopCoroutine(_gravityCoroutine);
            
            _followCoroutine = StartCoroutine(FollowRoutine(transformToFollow, speed, onEnd));
            
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetWalking(true);
        }

        public void MoveTo(Vector3 destination, float speed, Action onEnd = null)
        {
            if(_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
            
            if (_followCoroutine != null) 
                StopCoroutine(_followCoroutine);
            
            if(_gravityCoroutine != null)
                StopCoroutine(_gravityCoroutine);
            
            _moveCoroutine = StartCoroutine(MoveToRoutine(destination, speed, onEnd));
            
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetWalking(true);
        }

        public void Rotate(Quaternion rotation, float speed, Action onEnd = null)
        {
            if(_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);

            _rotateCoroutine = StartCoroutine(RotateRoutine(rotation, speed, onEnd));
            
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetWalking(false);
        }

        public void StopMotion()
        {
            StopAllCoroutines();
            
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetWalking(false);
        }
    }
}