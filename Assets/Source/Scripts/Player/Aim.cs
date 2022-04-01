using Extensions;
using NaughtyAttributes;
using Support;
using Support.SLS;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class Aim : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform aimingOrigin;
        [Space] 
        [SerializeField] private bool obstaclesPreventAiming = true;
        [SerializeField] private bool isSaveLoadSystemIgnored = false;
        [ShowIf(nameof(isSaveLoadSystemIgnored))]
        [SerializeField] [Range(0, 10)] private float sensitivity = 5f;

        [Inject] private SaveLoadSystem _saveLoadSystem;
        [Inject] private InputSystem _inputSystem;

        private SpriteRenderer _spriteRenderer;
        private int ignoreRaycastLayers;
        private Vector3 _initialLocalPosition;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
            _initialLocalPosition = transform.localPosition;
            transform.parent = aimingOrigin;
         
            ignoreRaycastLayers = LayerMask.GetMask("Ignore Raycast") | 
                                  LayerMask.GetMask("Ignore Collision With Dashing Player") |
                                  LayerMask.GetMask("Ignore Collision With Dashing Player But Don't Ignore Detection Cast") |
                                  LayerMask.GetMask("Breakable Object");
            ignoreRaycastLayers = ~ignoreRaycastLayers;
            
            if (lineRenderer != null)
                lineRenderer.positionCount = 2;
        }

        private void Start()
        {
            _inputSystem.OnReleaseAction += OnReleaseAction;
            _inputSystem.OnDragAction += Move;
            _saveLoadSystem.SaveData.AimSensitivity.OnValueChanged += SetSensitivity;

            if(!isSaveLoadSystemIgnored)
                sensitivity = _saveLoadSystem.SaveData.AimSensitivity.Value;
        }

        private void OnDestroy()
        {
            _inputSystem.OnReleaseAction -= OnReleaseAction;
            _inputSystem.OnDragAction -= Move;
            _saveLoadSystem.SaveData.AimSensitivity.OnValueChanged -= SetSensitivity;
        }
        
        private void Update()
        {
            DrawAimingLine();
        }

        private void OnReleaseAction(Vector2 _)
        {
            ResetLocalPosition();
            _spriteRenderer.enabled = false;
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
            if(obstaclesPreventAiming)
                if(Physics.Linecast(aimingOrigin.transform.position, transform.position, out RaycastHit hit, ignoreRaycastLayers, QueryTriggerInteraction.Ignore))
                    transform.position = hit.point;

            var nextPos = transform.localPosition + (Vector3)movingDirection * sensitivity * Time.deltaTime / Time.timeScale;

            transform.localPosition = Vector3.ClampMagnitude(nextPos, PlayerEventController.Instance.Data.MaxDashDistance);
            _spriteRenderer.enabled = true;
            
            PlayerEventController.Instance.Aim(transform.position);
        }
        
        private void ResetLocalPosition()
        {
            this.DoAfterNextFrameCoroutine(() =>  transform.localPosition = _initialLocalPosition);
        }

        private void SetSensitivity(float sensitivity)
        {
            this.sensitivity = sensitivity;
        }
    }
}