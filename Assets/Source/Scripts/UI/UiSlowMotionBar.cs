using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    [RequireComponent(typeof(Slider))]
    public class UiSlowMotionBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Color minValueColor;
        [SerializeField] private Color maxValueColor;


        private Slider _slider;

        private void Start()
        {
            PlayerEventController.Instance.OnSlowMotionEnter += ShowBar;
            PlayerEventController.Instance.OnSlowMotionExit += HideBar;

            _slider = GetComponent<Slider>();
            _slider.minValue = 0.0f;
            _slider.maxValue = SlowMotionController.Instance.SlowMotionDuration;
            this.SetGameObjectInactive();
        }

        private void Update()
        {
            _slider.value = SlowMotionController.Instance.TimeRemaining;
            fillImage.color = Color.Lerp(minValueColor, maxValueColor, _slider.value / _slider.maxValue);
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnSlowMotionEnter -= ShowBar;
            PlayerEventController.Instance.OnSlowMotionExit -= HideBar;
        }

        public void ShowBar()
        {
            this.SetGameObjectActive();
        }

        public void HideBar()
        {
            this.SetGameObjectInactive();
        }
    }
}
