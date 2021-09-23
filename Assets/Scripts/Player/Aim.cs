using Extensions;
using Support;
using UnityEngine;

namespace Ingame
{
    public class Aim : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform aimingOrigin;
        [Space]
        [SerializeField] [Range(0, 1)] private float sensitivity = 1f;
        
        private Vector3 _initialLocalPosition;

        private void Awake()
        {
            _initialLocalPosition = transform.localPosition;

            if (lineRenderer != null)
                lineRenderer.positionCount = 2;
        }

        private void Start()
        {
            InputSystem.Instance.OnReleaseAction += ResetLocalPosition;
            InputSystem.Instance.OnDragAction += Move;
        }

        private void OnDestroy()
        {
            InputSystem.Instance.OnReleaseAction -= ResetLocalPosition;
            InputSystem.Instance.OnDragAction -= Move;
        }

        private void Update()
        {
            DrawAimingLine();
        }

        private void DrawAimingLine()
        {
            if(lineRenderer == null || aimingOrigin == null)
                return;

            lineRenderer.SetPosition(0, aimingOrigin.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        private void Move(Vector2 movingDirection)
        { 
            var nextPos = transform.localPosition + (Vector3)movingDirection * sensitivity;

            transform.localPosition = Vector3.ClampMagnitude(nextPos, PlayerEventController.Instance.Data.MaxDashDistance);
        }

        private void ResetLocalPosition(Vector2 _)
        {
            this.DoAfterNextFrame(() => { transform.localPosition = _initialLocalPosition; });
        }
    }
}