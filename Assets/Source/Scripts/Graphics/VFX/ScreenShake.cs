using System.Collections;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(CameraTransiter))]
    public class ScreenShake : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float frequencyGain;

        [Header("On Dash Cancelled")]
        [SerializeField] [Min(0.0f)] private float amplitudeGain;
        [SerializeField] [Min(0.0f)] private float shakeDuration;

        [Inject] private EffectsManager _effectsManager;
        
        private CameraTransiter _cameraTransiter;
        private CinemachineVirtualCamera _currentVirtualCamera;
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;

        private void Awake()
        {
            _cameraTransiter = GetComponent<CameraTransiter>();
            
            var transformCopy = transform;
            var position = transformCopy.position;
            var rotation = transformCopy.rotation;
            _cameraPosition = new Vector3(position.x, position.y, position.z);
            _cameraRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }

        private void Start()
        {
            if(PlayerEventController.Instance != null)
                PlayerEventController.Instance.OnDashCancelled += OnDashCancelled;

            _cameraTransiter.OnVirtualCameraChanged += OnVirtualCameraChanged;
            _effectsManager.OnEnemyKillEffectPlayed += OnEnemyKillEffectPlayed;
            _effectsManager.OnPlayerDeathEffectPlayed += OnPlayerDeathEffectPlayed;
        }

        private void OnDestroy()
        {
            if(PlayerEventController.Instance != null)
                PlayerEventController.Instance.OnDashCancelled -= OnDashCancelled;
            
            _cameraTransiter.OnVirtualCameraChanged -= OnVirtualCameraChanged;
            _effectsManager.OnEnemyKillEffectPlayed -= OnEnemyKillEffectPlayed;
            _effectsManager.OnPlayerDeathEffectPlayed -= OnPlayerDeathEffectPlayed;
        }

        private IEnumerator ScreenShakeRoutine(float amplitude, float duration)
        {
            var timeElapsed = 0.0f;
            var cinemachineBasicMultiChannelPerlin =
                _currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            while (true)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(amplitude, 0.0f, timeElapsed / duration);

                timeElapsed += Time.deltaTime / Time.timeScale;
                if (timeElapsed >= duration)
                    break;

                yield return new WaitForEndOfFrame();
            }

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.0f;
            ResetCameraTransform();
            yield return null;
        }

        private void OnDashCancelled()
        {
            StartScreenShake(amplitudeGain, shakeDuration);
        }

        private void OnEnemyKillEffectPlayed()
        {
            StartScreenShake(amplitudeGain, shakeDuration);
        }

        private void OnPlayerDeathEffectPlayed()
        {
            StartScreenShake(amplitudeGain, shakeDuration);
        }

        private void StartScreenShake(float amplitude, float duration)
        {
            if (!(Camera.main is { })) 
                Debug.LogError("MainCamera has not been found in the scene");

            StopAllCoroutines();
            StartCoroutine(ScreenShakeRoutine(amplitude, duration));
        }

        private void OnVirtualCameraChanged(CinemachineVirtualCamera virtualCamera)
        {
            _currentVirtualCamera = virtualCamera;
            
            _currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                frequencyGain;
        }

        private void ResetCameraTransform()
        {
            var transform1 = transform;
            transform1.position = _cameraPosition;
            transform1.rotation = _cameraRotation;
        }
    }
}