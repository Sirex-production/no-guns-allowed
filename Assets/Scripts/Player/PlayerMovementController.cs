using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerStatsController))]
    public class PlayerMovementController : MonoBehaviour
    {
        private const float MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASH = .001f;
        
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
            ManageDash();
        }

        private void OnCollisionEnter(Collision other)
        {
            StopDash();
        }
        
        private void OnDestroy()
        {
            InputSystem.Instance.OnDirectionalSwipeAction -= Dash;
        }

        private void OnDrawGizmos()
        {
            if(_playerStatsController == null)
                return;
            
            Gizmos.DrawWireSphere(_dashStartPos, _playerStatsController.Data.MaximalDashDistance);
            Gizmos.color = Color.red;
        }

        private void ManageDash()
        {
            if(!_isDashing)
                return;
            
            if(Vector3.Distance(_dashStartPos, transform.position) > _playerStatsController.Data.MaximalDashDistance)
                StopDash();
            // else if(_rigidbody.velocity.magnitude < MINIMAL_SPEED_FOR_RIGIDBODY_TO_STOP_DASH)
            //     StopDash();
        }

        private void Dash(Vector2 dashDirection)
        {
            if(_isDashing)
                return;
            
            _isDashing = true;
            _dashStartPos = transform.position;
            dashDirection = dashDirection.normalized;
            _rigidbody.AddForce(dashDirection * _playerStatsController.Data.DashForce, ForceMode.Impulse);
        }

        private void StopDash()
        {
            if(!_isDashing)
                return;
            
            _rigidbody.velocity *= _playerStatsController.Data.VelocityScaleFactorAfterDash;
            _isDashing = false;
        }
    }
}