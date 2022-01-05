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
            EffectsManager.Instance.OnSlowMotionEnter += Modify;
            EffectsManager.Instance.OnSlowMotionExit += DoReset;
        }

        private void OnDestroy()
        {
            if(PlayerEventController.Instance == null)
                return;
            EffectsManager.Instance.OnSlowMotionEnter -= Modify;
            EffectsManager.Instance.OnSlowMotionExit -= DoReset;
        }

        protected override IEnumerator OnModificationRoutine()
        {
            if (Camera.main is null)
            {
                Debug.Log("Camera.main is null");
                yield break;
            }

            var mainCamera = Camera.main;
            var timeElapsed = 0.0f;
            effectToChange.active = true;
            effectToChange.intensity.value = startValue;
            while (timeElapsed <= lerpDuration)
            {
                var playerPosition = PlayerEventController.Instance.transform.position;
                var playerPositionOnScreen = mainCamera.WorldToScreenPoint(playerPosition);
                var x = playerPositionOnScreen.x / mainCamera.pixelWidth;
                var y = playerPositionOnScreen.y / mainCamera.pixelHeight;
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
