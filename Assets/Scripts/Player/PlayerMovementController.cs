using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Aim aim;
        
        private Rigidbody _rigidbody;

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

        private void Dash(Vector2 _)
        {
            if(aim == null || !PlayerEventController.Instance.StatsController.IsAbleToDash)
                return;
            
            var dashDirection = Vector3.Normalize(aim.transform.position - transform.position);
            var impulseVelocity = dashDirection * PlayerEventController.Instance.Data.DashForce;

            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(impulseVelocity, ForceMode.Impulse);
            
            PlayerEventController.Instance.PerformDash(dashDirection);
        }

        private void StopDash()
        {
            
        }
    }
}