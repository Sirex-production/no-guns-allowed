using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Ingame.Graphics
{
    public class ScreenShake : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] [Min(0.0f)] private float amplitudeGain;
        [SerializeField] [Min(0.0f)] private float frequencyGain;
        [SerializeField] [Min(0.0f)] private float shakeDuration;

        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;


        private void Awake()
        {
            var transform1 = this.transform;
            var position = transform1.position;
            var rotation = transform1.rotation;
            _cameraPosition = new Vector3(position.x, position.y, position.z);
            _cameraRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                frequencyGain;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartScreenShake();
        }

        private IEnumerator ScreenShakeRoutine()
        {
            var timeElapsed = 0.0f;
            var cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            while (true)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(amplitudeGain, 0.0f, timeElapsed / shakeDuration);

                timeElapsed += Time.deltaTime;
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

        public void StartScreenShake()
        {
            StopAllCoroutines();
            StartCoroutine(ScreenShakeRoutine());
        }
    }
}