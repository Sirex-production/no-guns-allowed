using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Ingame.Graphics.VFX
{
    public class ColorAdjustmentsController : VolumeComponentController<ColorAdjustments>
    {
        [SerializeField] [Min(0)] private float effectDuration = .05f;

        [Inject] private EffectsManager _effectsManager;

        private void Start()
        {
            _effectsManager.OnEnemyKillEffectPlayed += Modify; 
        }

        private void OnDestroy()
        {
            _effectsManager.OnEnemyKillEffectPlayed -= Modify; 
        }

        protected override IEnumerator OnModificationRoutine()
        {
            effectToChange.active = true;
            yield return new WaitForSeconds(effectDuration);
            effectToChange.active = false;
        }

        public override void Reset()
        {
            effectToChange.active = false;
        }
    }
}