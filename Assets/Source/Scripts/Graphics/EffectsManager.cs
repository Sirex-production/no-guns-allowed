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
        public event Action<DamageType> OnEnemyKillEffectPlayed;
        public event Action OnPlayerDeathEffectPlayed;
        public event Action OnPlayerDeathEffectStopped;
        public event Action<float> OnEnvironmentShake;
        public event Action OnPlayerAttackEffectPlayed;

        [Inject] private GameController _gameController;
        
        public void EnterSlowMotion()
        {
            OnSlowMotionEnter?.Invoke();
        }

        public void ExitSlowMotion()
        {
            OnSlowMotionExit?.Invoke();
        }

        public void PlayKillEnemyEffects(DamageType damageType)
        {
            OnEnemyKillEffectPlayed?.Invoke(damageType);
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

        public void ShakeEnvironment(float duration)
        {
            duration = Math.Max(0, duration);
            
            OnEnvironmentShake?.Invoke(duration);
        }

        public void PlayPlayerAttackEffect()
        {
            OnPlayerAttackEffectPlayed?.Invoke();
        }
    }
}

