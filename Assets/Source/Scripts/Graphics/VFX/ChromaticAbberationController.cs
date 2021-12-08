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

        private void Start()
        {
            PlayerEventController.Instance.OnSlowMotionEnter += Modify;
            PlayerEventController.Instance.OnSlowMotionExit += DoReset;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnSlowMotionEnter -= Modify;
            PlayerEventController.Instance.OnSlowMotionExit -= DoReset;
        }

        protected override IEnumerator OnModificationRoutine()
        {
            var timeElapsed = 0.0f;
            effectToChange.active = true;
            effectToChange.intensity.value = startValue;
            while (timeElapsed <= lerpDuration)
            {
                effectToChange.intensity.value = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime / Time.timeScale;
                yield return null;
            }
        }

        public override void DoReset()
        {
            StopAllCoroutines();

            effectToChange.active = false;
        }
    }
}
