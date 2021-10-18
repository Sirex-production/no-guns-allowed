using System;
using System.Collections;
using UnityEngine;

namespace Ingame.UI
{
    public class UiOutOfCharges : MonoBehaviour
    {
        [SerializeField] private float animationSpeed;
        [SerializeField] private float messageDuration;

        private CanvasGroup _canvasGroupComponent;

        private void Awake()
        {
            _canvasGroupComponent = GetComponent<CanvasGroup>();
            _canvasGroupComponent.alpha = 0;
        }

        private IEnumerator OutOfChargesRoutine()
        {
            _canvasGroupComponent.alpha = 1;
            UiController.Instance.UiDashesController.HideCharges();

            var timeElapsed = 0.0f;
            var deltaDirection = -1;

            while (timeElapsed <= messageDuration)
            {
                var alpha = _canvasGroupComponent.alpha;
                alpha += animationSpeed * Time.deltaTime * deltaDirection;
                _canvasGroupComponent.alpha = alpha;
                _canvasGroupComponent.alpha = Mathf.Clamp01(alpha);

                if (_canvasGroupComponent.alpha == 0 || Math.Abs(_canvasGroupComponent.alpha - 1) < 0.2f)
                    deltaDirection = -deltaDirection;

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            UiController.Instance.UiDashesController.ShowCharges();
            _canvasGroupComponent.alpha = 0;
        }

        public void TriggerMessage()
        {
            StopAllCoroutines();
            StartCoroutine(OutOfChargesRoutine());
        }
    }
}
