using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Aim aim;
        
        private Rigidbody _rigidbody;
        
        private Vector3 _initialDashPosition;
        private float _currentDashLength;
        private bool _isDashing;

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

        private void OnCollisionEnter(Collision other)
        {
            StopDash(); 
        }

        private void Update()
        {
            if(_currentDashLength <= 0)
                return;

            if (Vector3.Distance(_initialDashPosition, transform.position) > _currentDashLength) 
                StopDash();
        }

        private void Dash(Vector2 _)
        {
            if(aim == null || !PlayerEventController.Instance.StatsController.IsAbleToDash)
                return;
            
            var dashVector = aim.transform.position - transform.position;
            var dashDirection = dashVector.normalized;
            var impulseVelocity = dashDirection * PlayerEventController.Instance.Data.DashForce;

            _initialDashPosition = transform.position;
            _currentDashLength = dashVector.magnitude;
            _isDashing = true;
            
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(impulseVelocity, ForceMode.Impulse);

            PlayerEventController.Instance.PerformDash(dashDirection);
        }

        private void StopDash()
        {
            if(!_isDashing)
                return;
            
            var dashingDirection = Vector3.Normalize(transform.position - _initialDashPosition);
            
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(dashingDirection * PlayerEventController.Instance.Data.AfterDashForce, ForceMode.Impulse);

            _initialDashPosition = transform.position;
            _currentDashLength = 0;
            _isDashing = false;
        }
    }
}