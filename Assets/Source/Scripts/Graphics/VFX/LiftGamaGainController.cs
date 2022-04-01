using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Ingame.Graphics.VFX
{
    public class LiftGamaGainController : VolumeComponentController<LiftGammaGain>
    {
        [SerializeField] [Min(0)] private float lerpSpeed = 5f; 
        [Inject] private EffectsManager _effectsManager;

        private void Start()
        {
            _effectsManager.OnPlayerDeathEffectPlayed += Modify;
            _effectsManager.OnPlayerDeathEffectStopped += Reset;
        }

        private void OnDestroy()
        {
            _effectsManager.OnPlayerDeathEffectPlayed -= Modify;
            _effectsManager.OnPlayerDeathEffectStopped -= Reset;
        }

        protected override IEnumerator OnModificationRoutine()
        {
            var targetTime = effectToChange.lift.value;
            effectToChange.lift.value = Vector4.zero;
            effectToChange.active = true;

            while (effectToChange.lift.value != targetTime)
            {
                effectToChange.lift.value = Vector4.Lerp(effectToChange.lift.value, targetTime, lerpSpeed * Time.deltaTime / Time.timeScale);
                yield return null;
            }
        }

        public override void Reset()
        {
            effectToChange.active = false;
        }
    }
}