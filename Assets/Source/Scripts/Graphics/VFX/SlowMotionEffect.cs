using System;
using System.Collections;
using Ingame;
using Support;
using UnityEngine;

namespace Ingame.Graphics.VFX
{
    public class SlowMotionEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float timeScale;
        [SerializeField] [Min(0)] private float lerpDuration;

        private float _defaultFixedDeltaTime;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Start()
        {
            PlayerEventController.Instance.OnSlowMotionEnter += DecreaseTimeScale;
            PlayerEventController.Instance.OnSlowMotionExit += ResetTimeScale;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnSlowMotionEnter -= DecreaseTimeScale;
            PlayerEventController.Instance.OnSlowMotionExit -= ResetTimeScale;
        }

        private void DecreaseTimeScale()
        {
            StartCoroutine(TimeScaleLerpRoutine());
        }

        private void ResetTimeScale()
        {
            StopAllCoroutines();

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = _defaultFixedDeltaTime;
        }


        private IEnumerator TimeScaleLerpRoutine()
        {
            var timeElapsed = 0.0f;
            while (Time.timeScale >= timeScale)
            {
                Time.timeScale = Mathf.Lerp(1.0f, timeScale, timeElapsed / lerpDuration);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;

                timeElapsed +=  Time.deltaTime / Time.timeScale;
                yield return null;
            }

            yield return null;
        }
    }
}