using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ingame.Graphics.VFX
{
    public class ChromaticAbberationController : IVolumeComponentController<ChromaticAberration>
    {
        [SerializeField] [Range(0, 1)] private float startValue;
        [SerializeField] [Range(0, 1)] private float endValue;
        [SerializeField] [Min(0)] private float lerpDuration;

        private float _timeElapsed;

        private void Start()
        {
            _timeElapsed = 0.0f;
        }

        protected override IEnumerator OnModificationRoutine()
        {
            effectToChange.active = true;
            effectToChange.intensity.value = startValue;
            while (_timeElapsed <= lerpDuration)
            {
                effectToChange.intensity.value = Mathf.Lerp(startValue, endValue, _timeElapsed / lerpDuration);
                _timeElapsed += Time.deltaTime / Time.timeScale;
                yield return null;
            }
        }

        public override void DoReset()
        {
            StopAllCoroutines();
            _timeElapsed = 0.0f;
            effectToChange.active = false;
        }
    }
}
