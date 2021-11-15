using Extensions;
using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerEventController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Aim aim;

        private const float MINIMAL_DISTANCE_TO_PERFORM_DASH = 0f;
        private const float TIME_AFTER_DASH_WILL_BE_STOPPED = .15f;
        
        private Rigidbody _rigidbody;
        private Vector3 _initialDashPosition;
        private float _currentDashLength;
        private bool _isDashing;

        private Coroutine _stopDashCoroutine;

        public bool IsDashing => _isDashing;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            InputSystem.Instance.OnReleaseAction += Dash;
        }

        private void OnDestroy()
        {
            InputSystem.Instance.OnReleaseAction -= Dash;
        }

        private void OnCollisionEnter()
        {
            StopDash(); 
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
            if (aim == null || !PlayerEventController.Instance.StatsController.IsAbleToDash)
            {
                PlayerEventController.Instance.StatsController.TriggerOutOfChargesMessage();
                return;
            }
            
            if(Vector3.Distance(aim.transform.position, transform.position) < MINIMAL_DISTANCE_TO_PERFORM_DASH)
                return;
            
            var dashVector = aim.transform.position - transform.position;
            var dashDirection = dashVector.normalized;
            var impulseVelocityModifier = Mathf.InverseLerp(MINIMAL_DISTANCE_TO_PERFORM_DASH, PlayerEventController.Instance.Data.MaxDashDistance, dashVector.magnitude) > .3f ?
                1 : Mathf.InverseLerp(MINIMAL_DISTANCE_TO_PERFORM_DASH, PlayerEventController.Instance.Data.MaxDashDistance, dashVector.magnitude);
            var impulseVelocity = dashDirection * (impulseVelocityModifier * PlayerEventController.Instance.Data.DashForce);

            _initialDashPosition = transform.position;
            _currentDashLength = dashVector.magnitude;
            _isDashing = true;
            
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(impulseVelocity, ForceMode.Impulse);

            PlayerEventController.Instance.PerformDash(dashDirection);
            _stopDashCoroutine = this.WaitAndDo(TIME_AFTER_DASH_WILL_BE_STOPPED, StopDash);
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

            var dashingDirection = Vector3.Normalize(transform.position - _initialDashPosition);
            var afterDashForceModifier = Mathf.InverseLerp(0, PlayerEventController.Instance.Data.MaxDashDistance, _currentDashLength);
            
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(dashingDirection * (afterDashForceModifier * PlayerEventController.Instance.Data.AfterDashForce), ForceMode.Impulse);

            _initialDashPosition = transform.position;
            _currentDashLength = 0;
            _isDashing = false;
            
            PlayerEventController.Instance.StopDash();
        }
    }
}