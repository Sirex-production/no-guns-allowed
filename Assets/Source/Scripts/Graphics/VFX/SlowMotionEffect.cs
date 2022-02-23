using System.Collections;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics.VFX
{
    public class SlowMotionEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float timeScale;
        [SerializeField] [Min(0)] private float lerpDuration;

        [Inject] private GameController _gameController;
        [Inject] private EffectsManager _effectsManager;
        
        private float _defaultFixedDeltaTime;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Start()
        {
            _gameController.OnLevelRestart += ResetTimeScale;
            
            if(PlayerEventController.Instance == null)
                return;
            _effectsManager.OnSlowMotionEnter += DecreaseTimeScale;
            _effectsManager.OnSlowMotionExit += ResetTimeScale;
        }

        private void OnDestroy()
        {
            _gameController.OnLevelRestart -= ResetTimeScale;
            
            if(PlayerEventController.Instance == null)
                return;
            _effectsManager.OnSlowMotionEnter -= DecreaseTimeScale;
            _effectsManager.OnSlowMotionExit -= ResetTimeScale;
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