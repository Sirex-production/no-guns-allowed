using System;
using System.Collections;
using Ingame;
using Support;
using UnityEngine;

namespace Ingame.Graphics.VFX
{
    public class SlowMotionEffect : MonoBehaviour
    {
        [SerializeField] private ChromaticAbberationController postProcessingEffect;
        [SerializeField] [Range(0.0f, 1.0f)] private float timeScale;
        [SerializeField] [Min(0)] private float lerpDuration;

        private bool _routineIsCalled;
        private float _timeElapsed; 
        private float _defaultFixedDeltaTime;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
            _routineIsCalled = false;
            _timeElapsed = 0.0f;
        }

        private void Start()
        {
            PlayerEventController.Instance.OnAim += SlowDownTime;
            PlayerEventController.Instance.OnDashPerformed += ResetTimeWhenDashPerformed;
            PlayerEventController.Instance.OnDashCancelled += ResetTimeOnRestartOrDashCancelled;
            GameController.Instance.OnLevelRestart += ResetTimeOnRestartOrDashCancelled;
            GameController.Instance.OnLevelEnded += ResetTimeOnLevelEnd;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnAim -= SlowDownTime;
            PlayerEventController.Instance.OnDashPerformed -= ResetTimeWhenDashPerformed;
            PlayerEventController.Instance.OnDashCancelled -= ResetTimeOnRestartOrDashCancelled;
            GameController.Instance.OnLevelRestart -= ResetTimeOnRestartOrDashCancelled;
            GameController.Instance.OnLevelEnded -= ResetTimeOnLevelEnd;
        }

        private void SlowDownTime(Vector3 _)
        {
            if(_routineIsCalled)
                return;
            
            postProcessingEffect.Modify();
            StartCoroutine(TimeSlowDownRoutine());
        }

        private void ResetTime()
        {
            StopAllCoroutines();
            postProcessingEffect.DoReset();
            _routineIsCalled = false;

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = _defaultFixedDeltaTime;
        }

        private void ResetTimeWhenDashPerformed(Vector3 _)
        {
            ResetTime();
        }

        private void ResetTimeOnLevelEnd(bool _)
        {
            ResetTime();
        }

        private void ResetTimeOnRestartOrDashCancelled()
        {
            ResetTime();
        }


        private IEnumerator TimeSlowDownRoutine()
        {
            _routineIsCalled = true;
            while (Time.timeScale >= timeScale)
            {
                Time.timeScale = Mathf.Lerp(1.0f, timeScale, _timeElapsed / lerpDuration);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;

                _timeElapsed +=  Time.deltaTime / Time.timeScale;
                yield return null;
            }

            yield return null;
        }
    }
}