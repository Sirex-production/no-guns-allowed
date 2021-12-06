using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Support;
using UnityEngine;

namespace Ingame
{
    public class SlowMotionController : MonoSingleton<SlowMotionController>
    {
        [SerializeField] private float slowMotionDuration;

        private enum State
        {
            Default, //No slow-motion, No aiming
            InSlowMotion, //Slow-motion, Aiming
            OutOfSlowMotion //No slow-motion, Aiming
        }
        private State _state;
        private float _timeRemaining;

        public float SlowMotionDuration => slowMotionDuration;
        public float TimeRemaining => _timeRemaining;

        private void Start()
        {
            _state = State.Default;
            _timeRemaining = slowMotionDuration;

            PlayerEventController.Instance.OnAim += CallForSlowMotion;
            PlayerEventController.Instance.OnDashCancelled += ReturnToDefaultState;
            PlayerEventController.Instance.OnDashPerformed += ReturnToDefaultStateOnDashPerformed;
            GameController.Instance.OnLevelRestart += ReturnToDefaultState;
            GameController.Instance.OnLevelEnded += ReturnToDefaultStateOnLevelEnd;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnAim -= CallForSlowMotion;
            PlayerEventController.Instance.OnDashCancelled -= ReturnToDefaultState;
            PlayerEventController.Instance.OnDashPerformed -= ReturnToDefaultStateOnDashPerformed;
            GameController.Instance.OnLevelRestart -= ReturnToDefaultState;
            GameController.Instance.OnLevelEnded -= ReturnToDefaultStateOnLevelEnd;
        }

        private IEnumerator TimerRoutine()
        {
            _timeRemaining = slowMotionDuration;

            while (_timeRemaining >= 0.0f)
            {
                _timeRemaining -= Time.deltaTime / Time.timeScale;
                yield return null;
            }

            if(_state != State.InSlowMotion)
                this.SafeDebug($"State is: {_state}, should be {State.InSlowMotion}", LogType.Error);

            _state = State.OutOfSlowMotion;
            PlayerEventController.Instance.ExitSlowMotion();
        }

        private void CallForSlowMotion(Vector3 _)
        {
            if (_state != State.Default) 
                return;

            _state = State.InSlowMotion;

            InvokeTimer();
            PlayerEventController.Instance.EnterSlowMotion();
        }

        private void InvokeTimer()
        {
            StartCoroutine(TimerRoutine());
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

            StopAllCoroutines();
            _state = State.Default;
            PlayerEventController.Instance.ExitSlowMotion();
        }
    }
}


