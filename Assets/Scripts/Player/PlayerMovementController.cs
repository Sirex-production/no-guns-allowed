using Extensions;
using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerStatsController))]
    public class PlayerMovementController : MonoBehaviour
    {
        private const float MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASHING = .001f;
        
        private Rigidbody _rigidbody;
        private PlayerStatsController _playerStatsController;

        private bool _isDashing = false;
        private Vector3 _dashStartPos;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerStatsController = GetComponent<PlayerStatsController>();
        }

        private void Start()
        {
            InputSystem.Instance.OnDirectionalSwipeAction += Dash;
        }

        private void Update()
        {
            ManageDashState();
        }

        private void OnDestroy()
        {
            InputSystem.Instance.OnDirectionalSwipeAction -= Dash;
        }
        
        private void OnDrawGizmos()
        {
            if(_playerStatsController == null)
                return;

            Gizmos.DrawWireSphere(!_isDashing ? transform.position : _dashStartPos, _playerStatsController.Data.MaximalDashDistance);
        }

        private void ManageDashState()
        {
            if(!_isDashing)
                return;
            
            var isDashDistanceWasPassed = Vector3.Distance(_dashStartPos, transform.position) > _playerStatsController.Data.MaximalDashDistance;
            var isInRestingState = _rigidbody.velocity.magnitude < MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASHING;

            if(isDashDistanceWasPassed || isInRestingState)
                StopDash();
        }

        private void Dash(Vector2 dashDirection)
        {
            if(_isDashing)
                return;
            
            _dashStartPos = transform.position;
            dashDirection = dashDirection.normalized;
            
            _rigidbody.AddForce(dashDirection * _playerStatsController.Data.DashForce, ForceMode.Impulse);
            
            this.DoAfterNextFrame(() => _isDashing = true);
        }

        private void StopDash()
        {
            if(!_isDashing)
                return;
            
            _rigidbody.velocity *= _playerStatsController.Data.VelocityScaleFactorAfterDash;
            
            this.DoAfterNextFrame(() => _isDashing = false);
        }
    }
}