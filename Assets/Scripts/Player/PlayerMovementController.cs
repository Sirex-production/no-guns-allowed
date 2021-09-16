using Extensions;
using Support;
using UnityEngine;

namespace Ingame
{
    public class PlayerMovementController : MonoBehaviour
    {
        private const float MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASHING = .001f;

        private Rigidbody _rigidbody;
        private bool _isDashing = false;
        private Vector3 _dashStartPos;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            if(PlayerEventController.Instance == null)
                return;

            var maxDashDistance = PlayerEventController.Instance.StatsController.Data.MaximalDashDistance;

            Gizmos.DrawWireSphere(!_isDashing ? transform.position : _dashStartPos, maxDashDistance);
        }

        private void ManageDashState()
        {
            if(!_isDashing)
                return;

            var maximalDashDistance = PlayerEventController.Instance.StatsController.Data.MaximalDashDistance;
            var isDashDistanceWasPassed = Vector3.Distance(_dashStartPos, transform.position) > maximalDashDistance;
            var isInRestingState = _rigidbody.velocity.magnitude < MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASHING;

            if(isDashDistanceWasPassed || isInRestingState)
                StopDash();
        }

        private void Dash(Vector2 dashDirection)
        {
            // if(_isDashing)
            //     return;
            
            if(!PlayerEventController.Instance.StatsController.IsAbleToDash)
                return;

            var playerStatsController = PlayerEventController.Instance.StatsController;

            _dashStartPos = transform.position;
            dashDirection = dashDirection.normalized;
            
            _rigidbody.AddForce(dashDirection * playerStatsController.Data.DashForce, ForceMode.Impulse);
            
            PlayerEventController.Instance.PerformDash(dashDirection);

            this.DoAfterNextFrame(() => _isDashing = true);
        }

        private void StopDash()
        {
            if(!_isDashing)
                return;

            var velocityScaleAfterDash = PlayerEventController.Instance.StatsController.Data.VelocityScaleFactorAfterDash;
            
            _rigidbody.velocity *= velocityScaleAfterDash;
            
            this.DoAfterNextFrame(() => _isDashing = false);
        }
    }
}