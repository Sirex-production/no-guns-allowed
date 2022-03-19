using Extensions;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerEventController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Aim aim;

        public enum SurfaceInteractionType {LandOnSurface, ReleaseFromSurface }

        private const float MINIMAL_DISTANCE_TO_PERFORM_DASH = 0f;
        private const float TIME_AFTER_DASH_WILL_BE_STOPPED = .15f;

        [Inject] private InputSystem _inputSystem;

        private PlayerEventController _playerEventController;
        private Rigidbody _rigidbody;
        private Vector3 _initialDashPosition;
        private float _currentDashLength;
        private bool _isFrozen;
        private bool _isDashing;

        private Coroutine _stopDashCoroutine;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _playerEventController = PlayerEventController.Instance;
            _inputSystem.OnReleaseAction += Dash;
        }

        private void OnDestroy()
        {
            _inputSystem.OnReleaseAction -= Dash;
        }

        private void OnCollisionEnter()
        {
            if(_playerEventController.Data.StopDashWhenCollidingWithEnvironment)
                StopDash();
        }

        private void OnCollisionStay(Collision other)
        {
            _playerEventController.InteractWithSurface(SurfaceInteractionType.LandOnSurface);
        }

        private void OnCollisionExit(Collision other)
        {
            _playerEventController.InteractWithSurface(SurfaceInteractionType.ReleaseFromSurface);
        }

        private void FixedUpdate()
        {
            if(_currentDashLength <= 0)
                return;

            if (Vector3.Distance(_initialDashPosition, transform.position) > _currentDashLength) 
                StopDash();
        }

        private void Dash(Vector2 _)
        {
            if (aim == null || !_playerEventController.StatsController.IsAbleToDash)
            {
                _playerEventController.CancelDash();
                return;
            }
            
            if(Vector3.Distance(aim.transform.position, transform.position) < MINIMAL_DISTANCE_TO_PERFORM_DASH)
                return;

            if (_isFrozen)
            {
                // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags (Unity documentation says it's an intended way to use this enum)
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                _isFrozen = false;
                _playerEventController.ChangeFrozen(_isFrozen);
            }

            var position = transform.position;
            var dashVector = aim.transform.position - position;
            var dashDirection = dashVector.normalized;
            var impulseVelocityModifier = Mathf.InverseLerp(MINIMAL_DISTANCE_TO_PERFORM_DASH, _playerEventController.Data.MaxDashDistance, dashVector.magnitude) > .3f ?
                1 : Mathf.InverseLerp(MINIMAL_DISTANCE_TO_PERFORM_DASH, _playerEventController.Data.MaxDashDistance, dashVector.magnitude);
            var impulseVelocity = dashDirection * (impulseVelocityModifier * _playerEventController.Data.DashForce);

            _initialDashPosition = position;
            _currentDashLength = dashVector.magnitude;
            _isDashing = true;
            
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(impulseVelocity, ForceMode.Impulse);

            gameObject.layer = LayerMask.NameToLayer("Player While Dashing");

            _playerEventController.PerformDash(dashDirection);
            _stopDashCoroutine = this.WaitAndDoCoroutine(TIME_AFTER_DASH_WILL_BE_STOPPED, StopDash);
        }

        private void StopDash()
        {
            if(!_isDashing)
                return;

            if (_stopDashCoroutine != null)
            {
                StopCoroutine(_stopDashCoroutine);
                _stopDashCoroutine = null;
            }

            var position = transform.position;
            var dashingDirection = Vector3.Normalize(position - _initialDashPosition);
            var afterDashForceModifier = Mathf.InverseLerp(0, _playerEventController.Data.MaxDashDistance, _currentDashLength);

            gameObject.layer = LayerMask.NameToLayer("Player Static");
            
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(dashingDirection * (afterDashForceModifier * _playerEventController.Data.AfterDashForce), ForceMode.Impulse);

            _initialDashPosition = position;
            _currentDashLength = 0;
            _isDashing = false;
            
            _playerEventController.StopDash();
        }

        public void FreezePlayer()
        {
            _isFrozen = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            _playerEventController.ChangeFrozen(_isFrozen);
        }
    }
}