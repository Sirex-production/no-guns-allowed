using System;
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
        [SerializeField] [Min(0)] private float maxAimDistance = 10f; 
        
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
            var nextPos = transform.localPosition + (Vector3)movingDirection;

            transform.localPosition = Vector3.ClampMagnitude(nextPos, maxAimDistance);
        }

        private void ResetLocalPosition(Vector2 _)
        {
            this.DoAfterNextFrame(() => { transform.localPosition = _initialLocalPosition; });
        }
    }
}