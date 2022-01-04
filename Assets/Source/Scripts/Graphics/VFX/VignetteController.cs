using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ingame.Graphics.VFX
{
    public class VignetteController : IVolumeComponentController<Vignette>
    {
        [SerializeField] [Range(0, 1)] private float startValue;
        [SerializeField] [Range(0, 1)] private float endValue;
        [SerializeField] [Min(0)] private float lerpDuration;

        private void Start()
        {
            if(PlayerEventController.Instance == null)
                return;
            PlayerEventController.Instance.OnSlowMotionEnter += Modify;
            PlayerEventController.Instance.OnSlowMotionExit += DoReset;
        }

        private void OnDestroy()
        {
            if(PlayerEventController.Instance == null)
                return;
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
                var playerPosition = PlayerEventController.Instance.transform.position;
                var playerPositionOnScreen = Camera.main.WorldToScreenPoint(playerPosition);
                var x = playerPositionOnScreen.x / Camera.main.pixelWidth;
                var y = playerPositionOnScreen.y / Camera.main.pixelHeight;
                effectToChange.center.value = new Vector2(x, y);

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
