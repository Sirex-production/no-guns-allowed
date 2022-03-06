using System.Collections;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(CameraTransiter))]
    public class ScreenShake : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float amplitudeGain;
        [SerializeField] [Min(0.0f)] private float frequencyGain;
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
                PlayerEventController.Instance.OnDashCancelled += StartScreenShake;

            _cameraTransiter.OnVirtualCameraChanged += OnVirtualCameraChanged;
            _effectsManager.OnEnemyKillEffectPlayed += StartScreenShake;
            _effectsManager.OnPlayerDeathEffectPlayed += StartScreenShake;
        }

        private void OnDestroy()
        {
            if(PlayerEventController.Instance != null)
                PlayerEventController.Instance.OnDashCancelled -= StartScreenShake;
            
            _cameraTransiter.OnVirtualCameraChanged -= OnVirtualCameraChanged;
            _effectsManager.OnEnemyKillEffectPlayed -= StartScreenShake;
            _effectsManager.OnPlayerDeathEffectPlayed -= StartScreenShake;
        }

        private void OnVirtualCameraChanged(CinemachineVirtualCamera virtualCamera)
        {
            _currentVirtualCamera = virtualCamera;
            
            _currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                frequencyGain;
        }

        private IEnumerator ScreenShakeRoutine()
        {
            var timeElapsed = 0.0f;
            var cinemachineBasicMultiChannelPerlin =
                _currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            while (true)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(amplitudeGain, 0.0f, timeElapsed / shakeDuration);

                timeElapsed += Time.deltaTime / Time.timeScale;
                if (timeElapsed >= shakeDuration)
                    break;

                yield return new WaitForEndOfFrame();
            }

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0.0f;
            ResetCameraTransform();
            yield return null;
        }

        private void ResetCameraTransform()
        {
            var transform1 = transform;
            transform1.position = _cameraPosition;
            transform1.rotation = _cameraRotation;
        }

        private void StartScreenShake()
        {
            if (!(Camera.main is { })) 
                Debug.LogError("MainCamera has not been found in the scene");

            StopAllCoroutines();
            StartCoroutine(ScreenShakeRoutine());
        }
    }
}