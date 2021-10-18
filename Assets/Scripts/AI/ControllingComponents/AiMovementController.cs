using System;
using System.Collections;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class AiMovementController : MonoBehaviour, IMovable
    {
        [SerializeField] private bool useGravity;

        private const float DISTANCE_OFFSET = .1f;
        
        private CharacterController _characterController;
        private Coroutine _moveCoroutine;
        private Coroutine _rotateCoroutine;
        private Coroutine _gravityCoroutine;
        private Coroutine _followCoroutine;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        #region Routines
        private IEnumerator ApplyGravityRoutine()
        {
            while (useGravity)
            {
                _characterController.Move(Physics.gravity * Time.fixedDeltaTime);

                yield return null;
            }
        }

        private IEnumerator FollowRoutine(Transform transformToFollow, float speed, Action onEnd)
        {
            bool IsPatrollingPointReached()
            {
                if (!useGravity)
                    return Vector3.Distance(transform.position, transformToFollow.position) > DISTANCE_OFFSET;

                return Mathf.Abs(transformToFollow.position.x - transform.position.x) > DISTANCE_OFFSET;
            }

            while (IsPatrollingPointReached())
            {
                var velocity = Vector3.Normalize(transformToFollow.position - transform.position);
                velocity *= speed;
                if (useGravity)
                    velocity += Physics.gravity;
                velocity *= Time.deltaTime;

                _characterController.Move(velocity);
                
                yield return null;
            }

            transform.position = transformToFollow.position;
            _gravityCoroutine ??= StartCoroutine(ApplyGravityRoutine());
            onEnd?.Invoke();
        }

        private IEnumerator MoveToRoutine(Vector3 destination, float speed, Action onEnd)
        {
            destination.z = transform.position.z;
            
            bool IsPatrollingPointReached()
            {
                if (!useGravity)
                    return Vector3.Distance(transform.position, destination) > DISTANCE_OFFSET;

                return Mathf.Abs(destination.x - transform.position.x) > DISTANCE_OFFSET;
            }

            while (IsPatrollingPointReached())
            {
                var velocity = Vector3.Normalize(destination - transform.position);
                velocity *= speed;
                if(useGravity)
                    velocity += Physics.gravity;
                velocity *= Time.deltaTime;
                
                _characterController.Move(velocity);
                
                yield return null;
            }

            transform.position = destination;
            _gravityCoroutine ??= StartCoroutine(ApplyGravityRoutine());
            onEnd?.Invoke();
        }

        private IEnumerator RotateRoutine(Vector3 rotation, float speed)
        {
            while (transform.eulerAngles != rotation)
            {
                var rotationVelocity = Vector3.Normalize(rotation - transform.eulerAngles);
                rotationVelocity *= speed * Time.deltaTime;

                transform.Rotate(rotationVelocity);
                
                yield return null;
            }
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
        }

        public void Rotate(Vector3 rotation, float speed)
        {
            if(_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);

            _rotateCoroutine = StartCoroutine(RotateRoutine(rotation, speed));
        }

        public void StopMotion()
        {
            StopAllCoroutines();
        }
    }
}