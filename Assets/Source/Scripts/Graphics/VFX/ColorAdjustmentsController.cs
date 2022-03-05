using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Ingame.Graphics.VFX
{
    public class ColorAdjustmentsController : VolumeComponentController<ColorAdjustments>
    {
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
            yield return new WaitForSeconds(.05f);
            effectToChange.active = false;
        }

        public override void DoReset()
        {
            effectToChange.active = false;
        }
    }
}