using System.Collections;
using Extensions;
using Ingame.Graphics;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class SlowMotionController : MonoSingleton<SlowMotionController>
    {
        [BoxGroup("Player stats"), Tooltip("How much slow-motion does player have as a resource in seconds")]
        [SerializeField] private float slowMotionPool;
        [BoxGroup("Player stats"), Tooltip("The minimum amount of slow-motion player has to restore before ")]
        [SerializeField] private float slowMotionThreshold;
        [Space]
        [BoxGroup("Killing effect")]
        [SerializeField] private float killingEffectSlowMotionDuration = .01f;
        [BoxGroup("Killing effect")]
        [SerializeField] private float timeScaleDuringKillingEffect = .05f;
        [BoxGroup("Player death effect")]
        [SerializeField] private float playerDeathSlowMotionEffectDuration = .01f;
        [BoxGroup("Player death effect")]
        [SerializeField] private float timeScaleDuringPlayerDeathEffect = .05f;
        
        [Inject] private GameController _gameController;
        [Inject] private EffectsManager _effectsManager;
        
        private enum State
        {
            Default, //No slow-motion, No aiming
            InSlowMotion, //Slow-motion, Aiming
            OutOfSlowMotion //No slow-motion, Aiming
        }
        private State _state;
        private float _timeRemaining;
        private bool _outOfTime; //Is true whenever player fully consumes cooldown bar, and stays true until it restores to the threshold

        public float SlowMotionPool => slowMotionPool;
        public float TimeRemaining => _timeRemaining;
        public bool OutOfTime => _outOfTime;

        private void Start()
        {
            _outOfTime = false;
            _state = State.Default;
            _timeRemaining = slowMotionPool;

            PlayerEventController.Instance.OnAim += CallForSlowMotion;
            PlayerEventController.Instance.OnDashCancelled += ReturnToDefaultState;
            PlayerEventController.Instance.OnDashPerformed += ReturnToDefaultStateOnDashPerformed;
            _gameController.OnLevelRestart += ReturnToDefaultState;
            _gameController.OnLevelEnded += ReturnToDefaultStateOnLevelEnd;
            _effectsManager.OnEnemyKillEffectPlayed += PlayKillingSlowMoEffect;
            _effectsManager.OnPlayerDeathEffectPlayed += PlayPlayerDeathSlowMotionEffect;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnAim -= CallForSlowMotion;
            PlayerEventController.Instance.OnDashCancelled -= ReturnToDefaultState;
            PlayerEventController.Instance.OnDashPerformed -= ReturnToDefaultStateOnDashPerformed;
            _gameController.OnLevelRestart -= ReturnToDefaultState;
            _gameController.OnLevelEnded -= ReturnToDefaultStateOnLevelEnd;
            _effectsManager.OnEnemyKillEffectPlayed -= PlayKillingSlowMoEffect;
            _effectsManager.OnPlayerDeathEffectPlayed -= PlayPlayerDeathSlowMotionEffect;
        }

        private IEnumerator TimerRoutine()
        {
            while (_timeRemaining >= 0.0f)
            {
                _timeRemaining -= Time.deltaTime / Time.timeScale;
                yield return null;
            }
            _timeRemaining = Mathf.Clamp(_timeRemaining, 0.0f, slowMotionPool);

            if(_state != State.InSlowMotion)
                this.SafeDebug($"State is: {_state}, should be {State.InSlowMotion}", LogType.Error);

            _outOfTime = true;
            _state = State.OutOfSlowMotion;
            InvokeCooldown();
            _effectsManager.ExitSlowMotion();
        }

        private IEnumerator CooldownRoutine()
        {
            if(_state == State.InSlowMotion)
                Debug.LogError($"State should not be in: {_state}");

            while (_timeRemaining <= slowMotionPool)
            {
                _timeRemaining += Time.deltaTime / Time.timeScale;
                GetCooldownState();

                yield return null;
            }

            //_outOfTime = false;
            _timeRemaining = Mathf.Clamp(_timeRemaining, 0.0f, slowMotionPool);
        }

        private void InvokeTimer()
        {
            StartCoroutine(TimerRoutine());
        }

        private void InvokeCooldown()
        {
            StartCoroutine(CooldownRoutine());
        }

        private void GetCooldownState()
        {
            if (_timeRemaining >= slowMotionThreshold)
                _outOfTime = false;
        }

        private void CallForSlowMotion(Vector3 _)
        {
            if (_state != State.Default || _outOfTime) 
                return;

            _state = State.InSlowMotion;

            StopAllCoroutines();
            InvokeTimer();
            _effectsManager.EnterSlowMotion();
        }

        private void ReturnToDefaultStateOnLevelEnd(bool _)
        {
            ReturnToDefaultState();
        }

        private void ReturnToDefaultStateOnDashPerformed(Vector3 _)
        {
            ReturnToDefaultState();
        }

        private void ReturnToDefaultState()
        {
            if (_state == State.Default) 
                return;

            _state = State.Default;
            StopAllCoroutines();
            InvokeCooldown();
            _effectsManager.ExitSlowMotion();
        }

        private void PlayKillingSlowMoEffect()
        {
            Time.timeScale = timeScaleDuringKillingEffect;
            this.WaitAndDoCoroutine(killingEffectSlowMotionDuration, () => Time.timeScale = 1);
        }

        private void PlayPlayerDeathSlowMotionEffect()
        {
            Time.timeScale = timeScaleDuringPlayerDeathEffect;
            this.WaitAndDoCoroutine(playerDeathSlowMotionEffectDuration, () => Time.timeScale = 1);
        }
    }
}


