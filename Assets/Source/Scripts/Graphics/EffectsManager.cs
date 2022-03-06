using System;
using Extensions;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private float playerDeathEffectDuration = .5f;
            
        public event Action OnSlowMotionEnter;
        public event Action OnSlowMotionExit;
        public event Action OnEnemyKillEffectPlayed;
        public event Action OnPlayerDeathEffectPlayed;
        public event Action OnPlayerDeathEffectStopped;

        [Inject] private GameController _gameController;
        
        public void EnterSlowMotion()
        {
            OnSlowMotionEnter?.Invoke();
        }

        public void ExitSlowMotion()
        {
            OnSlowMotionExit?.Invoke();
        }

        public void PlayKillEnemyEffects()
        {
            OnEnemyKillEffectPlayed?.Invoke();
        }

        public void PlayPlayerDeathEffect()
        {
            OnPlayerDeathEffectPlayed?.Invoke();
            this.WaitAndDoCoroutine(playerDeathEffectDuration, StopPlayerDeathEffect);
        }

        public void StopPlayerDeathEffect()
        {
            OnPlayerDeathEffectStopped?.Invoke();
            _gameController.EndLevel(false);
        }
    }
}

