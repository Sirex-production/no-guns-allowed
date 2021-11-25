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
